using System.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Mega.WhatsAppApi.Api.Filters;
using Mega.WhatsAppApi.Api.Swagger.Examples;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Mega.WhatsAppApi.Api.Swagger;

namespace Mega.WhatsAppApi.Api
{
    public class Startup 
    {
        public IConfiguration Configuration { get; }

        public Startup (IConfiguration configuration) { Configuration = configuration; }
        
        public void ConfigureServices (IServiceCollection services) 
        {
            services.AddSession();

            services.StartupSwagger();
            services.AddControllersWithViews (options => 
            {
                options.Filters.Add (typeof (SessaoFilter));
                options.Filters.Add (typeof (ExceptionHandler));
            });
        }

        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) 
        {
            if (env.IsDevelopment ()) 
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