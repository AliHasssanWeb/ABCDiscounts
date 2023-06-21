using ABC.POS.Domain.DataConfig;
using ABC.POS.Website.Models;
using ABC.Shared.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.POS.Website
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
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSession();
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(opt =>
            {
                opt.LoginPath = "/POSSecurity/POSLogin";
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/POSSecurity/POSLogin";
            });
            services.AddScoped<Globle_Variable>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");

            RequestSender.SetHttpContextAccessor(accessor);
            if (env.IsDevelopment())
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
            app.UseSession();
            app.UseMvc();
             app.UseCookiePolicy();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=POSSecurity}/{action=POSLogin}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
