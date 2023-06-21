using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NRWebSite.Models;
using Microsoft.Extensions.Options;
namespace NRWebSite.Controllers
{
    public class ImageController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private ApplicationConfiguration _appcfg;
        private string imagepath;
        public ImageController(IWebHostEnvironment hostingEnvironment, IOptions<ApplicationConfiguration> appcfg )
        {
            _hostingEnvironment = hostingEnvironment;
            imagepath = Path.Combine(_hostingEnvironment.WebRootPath, "image");
            System.IO.Directory.CreateDirectory(_hostingEnvironment.WebRootPath);
            System.IO.Directory.CreateDirectory(imagepath);
            _appcfg = appcfg.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        private  Byte[] GetFileFormLoction(string prompt)
        {
            var filename = Path.Combine(imagepath, prompt+".png");
            if(System.IO.File.Exists(filename))
            {
                return System.IO.File.ReadAllBytes(filename);
            }
            else
            {
                return null;
            }
        }

        private void  WriteFileFormLoction(string prompt, Byte[] data)
        {
            var filename = Path.Combine(imagepath, prompt + ".png");
            System.IO.File.WriteAllBytes(filename, data);
  
        }
      
        [HttpGet]
        [Route("image/text2image/{prompt}")]
        public async Task<IActionResult> text2image(string prompt)
        {
            var image = GetFileFormLoction(prompt);
            if (image != null)
                return File(image, "image/png");
            var url = _appcfg.sdurl;

            // 要发送的POST参数
            var postData = new TextParam
            {
                prompt = prompt,
                negative_prompt = _appcfg.negative_prompt,
                //sampler_index = "DPM++ SDE",
                // seed = -1,
                steps = _appcfg.steps,
                width = _appcfg.width,
                height = _appcfg.height,
                //cfg_scale = 8
            };

            // 将postData对象序列化为JSON
            string jsonContent = JsonConvert.SerializeObject(postData);

            // 创建HttpClient实例并设置请求头
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // 将JSON内容作为HttpContent发送
                using (HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
                using (HttpResponseMessage response = await client.PostAsync(url, content))
                {
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容，并将其反序列化为dynamic对象
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseBody);
                    var base64Image = result.images[0];
                    // 接下来可以处理结果对象
                    //Console.WriteLine($"Result: {result}");
                    //Console.ReadLine();

                    try
                    {
                        // 解码base64编码的图片字符串
                        byte[] imageBytes = Convert.FromBase64String(base64Image.Value);
                        WriteFileFormLoction(prompt, imageBytes);
                        return File(imageBytes, "image/png");

                    }
                    catch (Exception ex)
                    {
                        // 如果在处理图像时发生了错误，则返回错误响应
                        return BadRequest(new { Message = $"Error processing image: {ex.Message}" });
                    }
                }
            }


        }

    }
}
