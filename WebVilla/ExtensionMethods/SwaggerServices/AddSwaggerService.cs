using Microsoft.OpenApi.Models;

namespace WebVilla.ExtensionMethods.SwaggerServices
{
    public static class AddSwaggerService
    {
        public static IServiceCollection SwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWt Authorization header using the Bearer scheme \r\n\r\n" +
                                 "Example:\"Bearer 1234asjhbcjhs\" ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Scheme="oauth2",
                            Name="Bearer",
                            In=ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title="Web Villa",
                    Description="Api to manage Villa v1",
                    TermsOfService=new Uri("https://example.com/terms"),
                    Contact=new OpenApiContact
                    {
                       Name="DotnetMastery",
                       Url=new Uri("https://dotnetmastery.com")
                    },
                    License=new OpenApiLicense
                    {
                        Name="Example License",
                        Url=new Uri("https://example.com/license")
                    }
                });
                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2.0",
                    Title = "Web Villa",
                    Description = "Api to manage Villa v2",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "DotnetMastery",
                        Url = new Uri("https://dotnetmastery.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });
            });
            return services;
        }
    }
}
