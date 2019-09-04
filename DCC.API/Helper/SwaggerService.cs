using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
 
namespace DCC.API.Helper
{
    public static class SwaggerService
    {
     public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
         services.AddSwaggerGen(setupAction => {
                setupAction.SwaggerDoc("DCCOpenAPISpecification", 
                new  Info() {
                    Title = "Dcc API",
                    Version ="1"
                });
              var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
             setupAction.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                setupAction.AddSecurityRequirement(security);
            });
 
            return services;
        }
            public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
                app.UseSwagger();
              app.UseSwaggerUI(setupAction => {
                setupAction.SwaggerEndpoint("/swagger/DCCOpenAPISpecification/swagger.json",
                "Dcc API") ;
              }); 
            return app;
        }
    }
}