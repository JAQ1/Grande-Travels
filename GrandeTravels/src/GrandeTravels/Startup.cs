using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using GrandeTravels.Services;
using GrandeTravels.Models;
using System.Reflection;

namespace GrandeTravels
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
                
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc();

            services.AddScoped<IRepository<Profile>, BaseRepository<Profile>>();
            services.AddScoped<IRepository<Package>, BaseRepository<Package>>();
            services.AddScoped<IRepository<Booking>, BaseRepository<Booking>>();
            services.AddScoped<IRepository<Feedback>, BaseRepository<Feedback>>();
            services.AddScoped<IRepository<ShoppingCart>, BaseRepository<ShoppingCart>>();
            services.AddScoped<IRepository<ShoppingCartPackage>, BaseRepository<ShoppingCartPackage>>();
            

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddIdentity<User, IdentityRole>
                (
                config =>
                {
                    //FIX LATER

                    //config.Password.RequireNonAlphanumeric = false;
                    //config.Password.RequiredLength = 4;
                    //config.Password.RequireDigit = false;
                    //config.Cookies.ApplicationCookie.AccessDeniedPath = "/Account/AccessDenied";
                    //config.SignIn.RequireConfirmedEmail = true;
                }

                ).AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();

            services.AddDbContext<MyDbContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"]
            });

            app.UseGoogleAuthentication(new GoogleOptions()
            {
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"]
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
