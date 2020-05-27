using Mega.WhatsAppApi.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mega.WhatsAppApi.Api.Swagger;

namespace Mega.WhatsAppApi.Api
{
    /// <summary>
    /// Api startup class.
    /// </summary>
    public class Startup 
    {
        /// <summary>
        /// Api config.
        /// </summary>
        /// <value>The api config.</value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup (IConfiguration configuration) { Configuration = configuration; }
        
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices (IServiceCollection services) 
        {
            services.AddSession();
            services.StartupSwagger();
            services.AddControllersWithViews (options => 
            {
                options.Filters.Add (typeof (SessionFilter));
                options.Filters.Add (typeof (ExceptionHandler));
            });
        }

        /// <summary>
        /// Configures the application and web host environment.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) 
        {
            if (env.IsDevelopment()) 
            {
                app.UseDeveloperExceptionPage (); 
            }

            app.UseHttpsRedirection ();
            app.UseRouting ();

            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .Build());

            app.UseSession();

            //app.UseMiddleware<LoggingMiddleware>();

            app.UseEndpoints (endpoints => endpoints.MapControllers ());
            
            app.UseSwagger();
            app.UseSwaggerUI (config => 
            {
                config.SwaggerEndpoint ("/swagger/beta/swagger.json", "Mega - Api WhatsApp");
                config.RoutePrefix = "";
            });
        }
    }
}