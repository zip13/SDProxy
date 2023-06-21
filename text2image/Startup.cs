using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NRWebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddMvc().AddJsonOptions(options =>
            {
                //options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                //options.JsonSerializerOptions.Converters.Add(new DateTimeNullConverter());
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddOptions();


            var section = Configuration.GetSection("ApplicationConfiguration");


            //services.AddSingleton<INetPlanService,NetPlanService>();
            //services.AddSingleton<IOMDBService, OMDBService>();
            //services.AddSingleton<ISceneService, SceneService>();
            //services.AddSingleton<IResourceService, ResourceService>();

            services.AddDirectoryBrowser();
            services.AddControllersWithViews();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //// 设置允许所有来源跨域
            //app.UseCors(options =>
            //{
            //    options.WithOrigins("http://localhost:8589", "http://127.0.0.1"); // 允许特定ip跨域
            //    options.AllowAnyHeader();
            //    options.AllowAnyMethod();
            //    //options.AllowAnyOrigin();
            //    options.AllowCredentials();
            //});
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });



        }
    }
}
  
