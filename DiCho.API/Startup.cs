using DiCho.API.App_Start;
using DiCho.API.Handlers;
using DiCho.Core.Extension;
using DiCho.DataService.Models;
using DiCho.DataService.Services;
using DiCho.DataService.ViewModels;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiCho.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public static readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder
                        .WithOrigins(GetDomain())
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("verify-otp-mobile-firebase-adminsdk-84ln0-65436d4f03.json"),
            });
            services.ConfigAuthor<AspNetUsers, AspNetRoles, DiChoNaoContext>();
            services.ConfigureSwagger();
            services.AddDbContext<DiChoNaoContext>(opt => opt.UseSqlServer(Configuration["ConnectionStrings:App"]));
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(Configuration.GetSection("Redis").Get<RedisConfiguration>());
            services.ConfigureAutoMapper();
            services.ConfigureDI();
            services.AddControllers();
            services.ConfigureFilter<ErrorHandlingFilter>();
        }

        private string[] GetDomain()
        {
            var domains = Configuration.GetSection("Domain").Get<Dictionary<string, string>>().Select(s => s.Value).ToArray();
            return domains;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseCors(MyAllowSpecificOrigins);
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseFileServer(new FileServerOptions {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Path.Combine(_webHostEnvironment.WebRootPath)))
            });

            var localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizeOptions.Value);

            app.ConfigMigration<DiChoNaoContext>();

            app.UseAuthentication();

            app.ConfigureErrorHandler(env);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.ConfigureSwagger(provider);
        }
    }
}
