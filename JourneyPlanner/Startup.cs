using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using JourneyPlanner.Services;

namespace JourneyPlanner
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //https://www.c-sharpcorner.com/article/cookie-authentication-in-net-core-3-0/
            services.AddAuthentication("CookieAuthentication")  
                .AddCookie("CookieAuthentication", config =>  
                {  
                    config.Cookie.Name = "UserLoginCookie";  
                    config.LoginPath = "/login";  
                });  
            
            services.AddTransient( _ => new DatabaseService(
                    Configuration.GetConnectionString("DefaultConnection")
                    ));

            services.AddSingleton(Configuration);
            services.AddSingleton<IRealtimeService, RttService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IStationService, StationService>();
            services.AddSingleton<ITimetableService, TimetableService>();

            services.AddControllers();

            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseSpaStaticFiles();
            
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
