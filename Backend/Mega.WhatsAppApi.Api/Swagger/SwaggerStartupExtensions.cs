using System;
using System.Collections.Generic;
using System.IO;
using Mega.WhatsAppApi.Api.Swagger.Examples;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Mega.WhatsAppApi.Api.Swagger
{
    public static class SwaggerStartupExtensions
    {
        public static void StartupSwagger(this IServiceCollection services)
        {
            services.AddSwaggerExamplesFromAssemblyOf<MensagemExample> ();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("beta", new OpenApiInfo { Title = "Mega - WhatsApp API", Version = "beta" });
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                {
                    Name = "TokenCliente",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Token do cliente informado no header(cabeçalho) da requisição",
                    Scheme = "ApiKeyScheme"
                });

                var key = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement { { key, new List<string> () } };
                c.AddSecurityRequirement(requirement);
                c.ExampleFilters();

                var xmlPath1 = Path.Combine(AppContext.BaseDirectory, "Mega.WhatsAppApi.Dominio.xml");
                var xmlPath2 = Path.Combine(AppContext.BaseDirectory, "Mega.WhatsAppApi.Infraestrutura.xml");
                var xmlPath3 = Path.Combine(AppContext.BaseDirectory, "Mega.WhatsAppApi.Api.xml");

                c.IncludeXmlComments(xmlPath1);
                c.IncludeXmlComments(xmlPath2);
                c.IncludeXmlComments(xmlPath3);
            });
        }   
    }
}