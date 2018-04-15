using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EFLogging;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using CsEvaluator.Database.SqlHelper;
using CsEvaluator.Engine;
using CsEvaluator.Repository.Interfaces;

namespace CsEvaluator
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("config.json") //?
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddLogging();

            services.AddDbContext<CsEvaluatorContext>();

            services.AddSingleton(Configuration);

            services.AddSingleton<IEvaluator, PAWEvaluator>();

            services.AddSingleton<IAppRepository, AppRepository>();

            services.AddTransient<DbInitializer>();

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DbInitializer dbInit)
        {
            loggerFactory.AddProvider(new MyLoggerProvider());

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/App/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=App}/{action=Index}/{id?}");
            });

            dbInit.Initialize().Wait();
        }
    }
}
