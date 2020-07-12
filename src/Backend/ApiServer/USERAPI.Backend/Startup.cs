using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using USERAPI.Backend.Services;

namespace USERAPI.Backend
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

            services.AddHttpClient("BackendApi").ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                //if (environment == Environments.Development)
                //{
                //    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                //}


                handler.ServerCertificateCustomValidationCallback += (message, cert, chain, errors) => { return true; };
                return handler;

                
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });

            services.AddControllersWithViews();

            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;

            })
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = Configuration["Authority"];
                options.ApiSecret = "secret";
                options.ApiName = "USER_API";
                options.ApiName = "SSO_API";
                options.RequireHttpsMetadata = bool.Parse(Configuration["RequireHttpsMetadata"]);
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("myPolicy", builder =>
                {
                    // require scope1
                    builder.RequireScope("USER_API");
                    builder.RequireScope("SSO_API");
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "USER API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(Configuration["SwaggerAuthorityUrl"] + "/connect/authorize"),
                            Scopes = new Dictionary<string, string> { { "SSO_API", "SSO API" }, { "USER_API", "USER API" } }
                        },
                    },
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>{ "USER_API", "SSO_API" }
                    }
                });
            });

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            //Declare DI containers
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserApiClient, UserApiClient>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSession();

            if (Configuration["Https"] == "true")
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseCors(corsPolicyBuilder =>
            corsPolicyBuilder.WithOrigins(Configuration["AllowOrigins"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            );
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId("swagger_user_manager");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SSO API V1");
            });
        }
    }
}
