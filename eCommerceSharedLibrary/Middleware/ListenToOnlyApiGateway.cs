using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.SharedLibrary.Middleware
{
    public class ListenToOnlyApiGateway (RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var signedHeaders = context.Request.Headers["Api-Gateway"];

            //null means, the request is not coming from the API Gateway, so we will return 503 service unavailable.
            if (signedHeaders.FirstOrDefault() is null) { 
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry, service unavailable.");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
