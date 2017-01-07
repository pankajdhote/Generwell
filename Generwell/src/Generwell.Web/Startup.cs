using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Generwell.Core.Model;
using Generwell.Modules.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.Management;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Modules.Management.PictureManagement;
using System.Collections.Generic;
using Generwell.Modules.Management.FacilityManagement;

namespace Generwell.Web
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
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            // Add MVC services to the services container.
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = "Session";
            });

            var appSettings = Configuration.GetSection("ApplicationSettings");
            services.Configure<AppSettingsModel>(appSettings);

            services.AddAutoMapper(); // <-- This is the line you add.
            //added for depenedency injection
            services.AddSingleton<IGenerwellServices, GenerwellServices>();
            services.AddSingleton<IWellManagement, WellManagement>();
            services.AddSingleton<ITaskManagement, TaskManagement>();
            services.AddSingleton<IGenerwellManagement, GenerwellManagement>();
            services.AddSingleton<IPictureManagement, PictureManagement>();
            services.AddSingleton<List<WellModel>>();
            services.AddSingleton<WellModel>();
            services.AddSingleton<List<MapModel>>();
            services.AddSingleton<List<WellLineReportModel>>();
            services.AddSingleton<List<FilterModel>>();
            services.AddSingleton<LineReportsModel>();
            services.AddSingleton<TaskDetailsModel>();
            services.AddSingleton<List<TaskModel>>();
            services.AddSingleton<List<DictionaryModel>>();
            services.AddSingleton<List<ContactInformationModel>>();
            services.AddSingleton<PictureModel>();
            services.AddSingleton<AlbumModel>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IFacilityManagement, FacilityManagement>();
            services.AddSingleton<FacilityModel>();
            services.AddSingleton<List<FacilityModel>>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSession();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                AuthenticationScheme = "MyCookieMiddlewareInstance",
                LoginPath = new PathString("/Accounts/Login/"),
                AccessDeniedPath = new PathString("/Accounts/Login/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Accounts}/{action=Login}/{id?}");
            });
        }
    }
}
