using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NRWebSite.Models
{
public class ApplicationConfiguration
    {
        public ApplicationConfiguration()
        {
            width = 768;
            height = 512;
            steps = 25;
            negative_prompt = "";
            sdurl = "http://127.0.0.1:7860/sdapi/v1/txt2img";
        }
        #region 属性成员

        /// <summary>
        /// 文件上传路径
        /// </summary>
        public int width { get; set; }
        public int height { get; set; }
        public int steps {get;set; }
        public string negative_prompt { get; set; }
        public string sdurl { get; set; }
        #endregion


    }
}