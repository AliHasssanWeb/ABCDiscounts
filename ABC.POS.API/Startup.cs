using ABC.EFCore.Repository.Edmx;
using ABC.POS.API.Controllers;
using ABC.Shared;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.POS.API
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

            services.AddControllers();
            services.AddEntityFrameworkSqlServer();
            services.AddSwaggerDocument();
            services.AddCors();
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddDbContext<ABCDiscountsContext>(options =>
          options.UseSqlServer(Configuration.GetConnectionString("BloggingDatabase")));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JWTBearer";
                options.DefaultChallengeScheme = "JWTBearer";
            }).AddJwtBearer("JWTBearer", JWToptions =>
            {
                JWToptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = SecurityController.signinKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(5)
                };
            });
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            app.UseAuthentication();
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            //app.UseSwagger();
            ///app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DynamicsServices v1"));
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "images")),
                RequestPath = "/images"
            });
            app.UseRouting();

            app.UseAuthorization();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseCors(x => x.WithOrigins(""));
            app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
