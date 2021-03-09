using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Dpl.B2b.Web.Api.Authorization;

namespace Dpl.B2b.Web.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        //private static void Samples(IApplicationBuilder app)
        //{
        //    // NO change to response
        //    app.Use(async (context, next) =>
        //    {
        //        // Do work that doesn't write to the Response.
        //        await next.Invoke();
        //        // Do logging or other work that doesn't write to the Response.
        //    });

        //    // change response
        //    app.Run(async context =>
        //    {
        //        await context.Response.WriteAsync("Hello from 2nd delegate.");
        //    });
        //}

        public static void UsePopulateSecurityClaims(this IApplicationBuilder app)
        {
            app.UseMiddleware<DynamicClaimsMiddleware>(Options.Create<CustomClaimsOptions>(new CustomClaimsOptions()));
        }
    }
}
