using CardMon.Core.Interfaces.Repositories;
using CardMon.Core.Interfaces.Services;
using CardMon.Core.Services;
using CardMon.Core.Helpers;
using CardMon.Infrastructure.Data;
using CardMon.Infrastructure.Data.Manager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;
using CardMon.Core.Interfaces.Helpers;
using AutoMapper;
using CardMon.Core.DTOs.Request;
using CardMon.Core.DTOs.Validators;
using FluentValidation;
using CardMon.Core.Interfaces.ThirdPartyAPI;
using CardMon.Infrastructure.ThirdPartyAPI;

namespace CardMon.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void AddDbContextService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(x =>
            x.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                b => b.MigrationsAssembly("CardMon.Infrastructure")));
        }

        public static void AddRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IResponseResult, ResponseResult>();
            services.AddScoped<IDataSecurityService, DataSecurityService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IDuisService, DuisService>();
            services.AddScoped<IExoSkeletonService, ExoSkeletonService>();
            services.AddScoped<ILoggerManager, LoggerManager>();
        }

        public static void AddCorsService(this IServiceCollection services)
        {
            services.AddCors(x =>
            {
                x.AddDefaultPolicy(x =>
                {
                    x.AllowAnyOrigin();
                    x.AllowAnyHeader();
                    x.AllowAnyMethod();
                });
            });
        }

        public static void ConfigureValidator(this IServiceCollection services)
        {
            services.AddTransient<IValidator<AuthRequest>, AuthRequestValidator>();
            services.AddTransient<IValidator<ResetRequest>, ResetRequestValidator>();
            services.AddTransient<IValidator<CredentialRequest>, CredentialRequestValidator>();
        }

        public static void ConfigureAutomapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            services.AddSingleton(c => config.CreateMapper());
        }

        public static void AddSwaggerGenService(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CardMon API",
                    Version = "v1",
                    Description = "Integration with Duis and exoskeleton admin service APIs"
                });
                // Add JWT authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                x.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, new List<string>{} }
                });
                // Add apikey authentication
                var secureScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "X-Api-Key",
                    Reference = new OpenApiReference
                    {
                        Id = "ApiKey",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                x.AddSecurityDefinition(secureScheme.Reference.Id, secureScheme);
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { secureScheme, new List<string>{} }
                });
            });
        }

        public static void AddAuthenticationService(this IServiceCollection services, AppSettings appsettings)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = appsettings.AuthConfig.ValidIssuer,
                        ValidAudience = appsettings.AuthConfig.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appsettings.AuthConfig.Secret))
                    };
                });
        }

        public static void UseSwaggerConfig(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("../swagger/v1/swagger.json", "CardMon middleware service");
            });
        }

    }
}
