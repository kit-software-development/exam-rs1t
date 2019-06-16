using System;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(o =>
                    {
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
            app.UseFileServer();
            app.UseMvc();
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql();
            services.AddDbContext<CryptoWalletDbContext>(options =>
            {
                var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production"
                    ? Configuration.GetConnectionString("MyDbConnection")
                    : Configuration["ConnectionString:CryptoWalletDbContext"];
                Console.WriteLine("Connection string!!!: " + connectionString);
                options.UseNpgsql(connectionString);
            });

            services.BuildServiceProvider().GetService<CryptoWalletDbContext>().Database.Migrate();
        }
    }
}