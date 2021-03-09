using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Dpl.B2b.Web.Api.Authorization.Requirements
{
    public class CreateOrderRequirement : IAuthorizationRequirement
    {
    }

    public class CreateOrderAuthorizationHandler : AuthorizationHandler<CreateOrderRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationDataService _authorizationData;

        public CreateOrderAuthorizationHandler(IHttpContextAccessor httpContextAccessor, IAuthorizationDataService authorizationData)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizationData = authorizationData;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CreateOrderRequirement requirement)
        {
            //if (context.User.HasClaim(claim =>
            //    claim.Type == "EmployeeNumber" && claim.Issuer == request.AirlineName))
            //{
            //    context.Succeed(requirement);
            //}

            var routeData = _httpContextAccessor.HttpContext.GetRouteData();
            var areaName = routeData?.Values["area"];
            var controllerName = routeData?.Values["controller"];
            var actionName = routeData?.Values["action"];
            var orderId = routeData.Values["id"];

            //// might be that parameter values areactually in route values
            //var id1 = _httpContextAccessor.HttpContext.GetRouteValue("id");

            //// or check if its available in the items collection
            //var id2 = _httpContextAccessor.HttpContext.Items["id"] as string;


            // the below code shows how to safely read the body of a request without affecting downstream middleware (as usually streams can only be read once)
            string body = null;
            var request = _httpContextAccessor.HttpContext.Request;
            request.EnableBuffering();
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                body = await reader.ReadToEndAsync();
            }
            request.Body.Position = 0;

            context.Succeed(requirement);

            return;
        }
    }
}
