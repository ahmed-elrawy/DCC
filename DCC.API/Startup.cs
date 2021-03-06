﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DCC.API.Data;
using DCC.API.Helper;
using DCC.API.Hubs;
using DCC.API.Model;
using DCC.API.Repository;
using DCC.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace DCC.API
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

            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            // services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                      .AddJwtBearer(options =>
                      {
                          options.TokenValidationParameters = new TokenValidationParameters
                          {
                              ValidateIssuerSigningKey = true,
                              IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                                  .GetBytes(Configuration.GetSection("AppSetting:Token").Value)),
                              ValidateIssuer = false,
                              ValidateAudience = false
                          };
                      });
                services.AddAuthorization(opt => {
                    opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin") );

                });

            services.AddMvc(opt => {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
           .AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddCors();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            Mapper.Reset();
            services.AddAutoMapper();
            services.AddSwaggerDocumentation();
            services.AddTransient<Seed>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IDccRepository, DccRepository>();
            services.AddScoped<IDiagnosisRepository, DiagnosisRepository>();

            services.AddScoped<LogUserActivity>();
            services.AddScoped<IWebSocketService, SignalRWebSocketService>();

            services.AddSignalR();

            services.AddCors(options =>
                 {
                     options.AddPolicy("CorsPolicy",
                         builders => builders.WithOrigins("http://localhost:4200")
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials());
                 });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                        {
                            builder.Run(async context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                                var error = context.Features.Get<IExceptionHandlerFeature>();
                                if (error != null)
                                {
                                    context.Response.AddApplicationError(error.Error.Message);
                                    await context.Response.WriteAsync(error.Error.Message);
                                }
                            });
                        });              //  app.UseHsts();
            }

            if (env.IsDevelopment())
            {


            }
            app.UseAuthentication();
            // app.UseHttpsRedirection();
            app.UseSwaggerDocumentation();
            app.UsePathBase("/swagger/index.html");
            app.UseCors("CorsPolicy");

            app.UseSignalR(c =>
            {
                c.MapHub<ChatHub>("/chats");
            });
            app.UseMvc();

            seeder.Initialize();
        }
    }
}
