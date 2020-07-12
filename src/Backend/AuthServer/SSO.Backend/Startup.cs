using FluentValidation.AspNetCore;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SSO.Backend.Data;
using SSO.Backend.Data.Entities;
using SSO.Backend.Services;
using SSO.BackendIdentityServer;
using SSO.Service.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace SSO.Backend
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

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            //1. Setup entity framework
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString), ServiceLifetime.Transient);
            //2. Setup idetntity
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer(options =>
            {
                options.IssuerUri = Configuration["IssuerUri"];
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<User>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                 {
                     options.ConfigureDbContext = builder =>
                     builder.UseSqlServer(connectionString,
                     sql => sql.MigrationsAssembly(migrationsAssembly));

                     // this enables automatic token cleanup. this is optional.
                     options.EnableTokenCleanup = true;
                     options.TokenCleanupInterval = 30;
                 })
                .AddProfileService<IdentityProfileService>();

            


            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            });

            services.AddControllersWithViews()
                  .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ClientCreateRequestValidator>())
                  .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAuthentication()
               .AddLocalApi("Bearer", option =>
               {
                   option.ExpectedScope = "SSO_API";
               });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy =>
                {
                    policy.AddAuthenticationSchemes("Bearer");
                    policy.RequireAuthenticatedUser();
                });
            });

            services.AddRazorPages(options =>
            {
                options.Conventions.AddAreaFolderRouteModelConvention("Identity", "/Account/", model =>
                {
                    foreach (var selector in model.Selectors)
                    {
                        var attributeRouteModel = selector.AttributeRouteModel;
                        attributeRouteModel.Order = -1;
                        attributeRouteModel.Template = attributeRouteModel.Template.Remove(0, "Identity".Length);
                    }
                });
            });
            services.AddTransient<DbInitializer>();
            services.AddTransient<IEmailSender, EmailSenderService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SSO API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(Configuration["SwaggerAuthorityUrl"] + "/connect/authorize"),
                            Scopes = new Dictionary<string, string> { { "SSO_API", "SSO API" } }
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
                        new List<string>{ "SSO_API" }
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(corsPolicyBuilder =>
            corsPolicyBuilder.WithOrigins(Configuration["AllowOrigin"], "http://localhost:4300")
           .AllowAnyMethod()
           .AllowAnyHeader()
           );

            //InitializeDatabase(app);

            app.UseStaticFiles();

            if (!(Configuration["Https"] == "true"))
            {
                var fordwardedHeaderOptions = new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                };
                fordwardedHeaderOptions.KnownNetworks.Clear();
                fordwardedHeaderOptions.KnownProxies.Clear();

                app.UseForwardedHeaders(fordwardedHeaderOptions);

            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId("swagger_admin");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SSO API V1");
            });

        }

        //Initialize database and seed data: IdentityServer & AspNetIdentity if project can't found database on SqlServer
       /* private void InitializeDatabase(IApplicationBuilder app)
        {
            // Create db User
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            }
            // Add User data
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                dbInitializer.Seed().Wait();
            }
            // Create db and add data AuthServer
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    if (Configuration["Https"] == "true")
                    {
                        foreach (var client in ConfigHttps.GetClients())
                        {
                            context.Clients.Add(client.ToEntity());
                        }
                    }
                    else
                    {
                        foreach (var client in ConfigHttp.GetClients())
                        {
                            context.Clients.Add(client.ToEntity());
                        }
                    }

                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    if (Configuration["Https"] == "true")
                    {
                        foreach (var resource in ConfigHttps.GetIdentityResources())
                        {
                            context.IdentityResources.Add(resource.ToEntity());
                        }
                    }
                    else
                    {
                        foreach (var resource in ConfigHttp.GetIdentityResources())
                        {
                            context.IdentityResources.Add(resource.ToEntity());
                        }
                    }

                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    if (Configuration["Https"] == "true")
                    {
                        foreach (var resource in ConfigHttps.GetApiResources())
                        {
                            context.ApiResources.Add(resource.ToEntity());
                        }
                    }
                    else
                    {

                        foreach (var resource in ConfigHttp.GetApiResources())
                        {
                            context.ApiResources.Add(resource.ToEntity());
                        }
                    }

                    context.SaveChanges();
                }
                if (!context.ApiScopes.Any())
                {
                    if (Configuration["Https"] == "true")
                    {
                        foreach (var resource in ConfigHttps.GetApiScopes())
                        {
                            context.ApiScopes.Add(resource.ToEntity());
                        }
                    }
                    else
                    {
                        foreach (var resource in ConfigHttp.GetApiScopes())
                        {
                            context.ApiScopes.Add(resource.ToEntity());
                        }
                    }
                    context.SaveChanges();
                }
            }
        }*/
    }
}