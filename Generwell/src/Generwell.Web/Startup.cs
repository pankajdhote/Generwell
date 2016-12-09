using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Generwell.Modules.Model;
using Generwell.Modules.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Elmah.Io.AspNetCore;
using Elmah.Io.AspNetCore.ExceptionFormatters;

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

            //services.AddElm( /* options HERE */ options =>
            //{
            //    options.Path = new PathString("/elm");
            //    options.Filter = (name, level) => level >= LogLevel.Error;
            //});

            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = "Session";
            });

            //added for depenedency injection
            services.AddSingleton<IGenerwellServices, GenerwellServices>();
            var appSettings = Configuration.GetSection("ApplicationSettings");
            services.Configure<AppSettingsModel>(appSettings);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSession();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //app.UseElmahIo(
            //"API_KEY",
            //new Guid("LOG_ID"),
            //new ElmahIoSettings
            //    {
            //        ExceptionFormatter = new DefaultExceptionFormatter()
            //    });

            //app.Map("/Micotan", map =>
            //{
            //    app.UseElmPage();
            //    app.UseElmCapture();
            //    //app.UseIISPlatformHandler();
            //    app.UseDeveloperExceptionPage();
            //    app.UseMvcWithDefaultRoute();
            //});

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
