using System;
using System.Net.Http.Headers;
using System.Net.Mime;
using CardMon.Core.Interfaces.Services;
using CardMon.Core.Helpers;
using CardMon.Infrastructure.Extensions;
using CardMon.Infrastructure.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CardMon.Core.Interfaces.Helpers;
using CardMon.Core.DTOs.Response;
using FluentValidation.AspNetCore;
using CardMon.Infrastructure.Utilities.Filters;

namespace CardMon.API
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
            services.AddCorsService();
            services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; });
            services.AddSwaggerGenService();
            services.AddDbContextService(Configuration);
            services.AddRepositoryManager();
            services.AddServices();
            var configSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(configSection);
            var settings = configSection.Get<AppSettings>();
            services.AddAuthenticationService(settings);
            services.AddHttpClient(settings.Service_Name, x =>
            {
                x.Timeout = new TimeSpan(0, 0, 30);
                x.DefaultRequestHeaders.Clear();
                x.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            });

            services.AddHttpContextAccessor();
            services.ConfigureAutomapper();
            services.ConfigureValidator();
            services.AddControllers(x =>
            {
                x.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));
                x.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
                x.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), StatusCodes.Status403Forbidden));
                x.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), StatusCodes.Status400BadRequest));
                x.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), StatusCodes.Status500InternalServerError));
            }).AddFluentValidation(x =>
            {
                x.DisableDataAnnotationsValidation = true;
            });
            services.AddScoped<ONBUserIdAttribute>();
            services.AddScoped<ONBUserNameAttribute>();
            services.AddScoped<ApiKeyAuthAttribute>();
            services.AddScoped<RequestValidationAttribute>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager loggerManager, IResponseResult responseResult)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler(loggerManager, responseResult);
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseHttpsRedirection();
            app.UseSwaggerConfig();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
