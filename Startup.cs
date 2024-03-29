﻿using System;
using System.Threading.Tasks;
using CryptoWallet.Database;
using CryptoWallet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoWallet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IWalletsService, WalletsService>();
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(o =>
                    {
                        o.Cookie.HttpOnly = false;
                        o.Events.OnRedirectToLogin = ctx =>
                        {
                            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        };
                    });
            ConfigureDatabase(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseCors(x => x.WithOrigins("http://localhost:8000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseFileServer();
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint("/swagger/v1/swagger.json", "Crypto Wallet API V1");
               });
            app.UseMvc();
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql();
            services.AddDbContext<CryptoWalletDbContext>(options =>
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                options.UseNpgsql(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
                    ? Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_MyDbConnection")
                    : Configuration["ConnectionString:CryptoWalletDbContext"]);
            });
            services.BuildServiceProvider().GetService<CryptoWalletDbContext>().Database.Migrate();

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Crypto Wallet HTTP API",
                    Version = "v1",
                    Description = "",
                    TermsOfService = "Terms Of Service"
                });
            });
        }
    }
}