# SDProxy
stable diffusion api proxy

# 直接运行
编译好的文件在v1目录下
运行 v2下的text2image.exe
text2image.exe会代理访问 http://127.0.0.1:7860/sdapi/v1/txt2img

格式：
http://127.0.0.1:5002/image/text2image/{提示词}

例子：
http://127.0.0.1:5002/image/text2image/1girl

## 配置文件
修改v1目录下的appsettings.json

"ApplicationConfiguration": {
	"width": 768, //生成图片的宽
	"height": 512,//生成图片的高
	"steps": 25, //生成的步数
	"negative_prompt": "NSFW", //方向提示词
	"sdurl": "http://127.0.0.1:7860/sdapi/v1/txt2img" //SD的链接
},
"urls": "http://127.0.0.1:5002;http://0.0.0.0:5002" //代理服务发布的端口

# 编译
使用net5.0框架 在当前目录执行dotnet publish