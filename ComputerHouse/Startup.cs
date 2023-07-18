using ComputerHouse.Data;
using ComputerHouse.Services;
using ComputerHouse.Services.Background;
using ComputerHouse.Settings;
using ComputerHouse.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ComputerHouse
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
            // Configure options/settings
            services.Configure<AzureBlobOptions>(Configuration.GetSection(AzureBlobOptions.SectionName));
            services.Configure<MailOptions>(Configuration.GetSection(MailOptions.SectionName));

            // Configure Cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add application services.
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IGlobalSettingsService, GlobalSettingsService>();

            // Configure background task queues and recurring jobs.
            services.AddHostedService<QueuedHostedService>();
            services.AddHostedService<DownloadCleanupService>();

            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromHours(1);
                option.Cookie.HttpOnly = true;
                //options.Cookie.IsEssential=true;
            });

            // Can configure autovalidate antiforgeryToken for controller and views.

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseCookiePolicy();

            //To use Identity, and should be after app.UseStaticFiles
            app.UseAuthentication();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
