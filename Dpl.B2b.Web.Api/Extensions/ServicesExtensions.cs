using Microsoft.Extensions.DependencyInjection;

namespace Dpl.B2b.Web.Api.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins(
                            // ng serve
                            "http://localhost:4200",
                            "https://localhost:4200",
                            // Dpl.B2b.Web debug
                            "https://localhost:5101",

                            // localhost generice (not sure if this works)
                            "http://localhost",
                            "https://localhost",

                            // actual production url
                            "http://www.contoso.com"
                        )
                        //builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                    );
            });
        }
    }
}
