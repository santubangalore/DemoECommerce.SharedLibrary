
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using eCommerce.SharedLibrary.Logs;
namespace eCommerce.SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {

            string message = "Sorry, internal server error occurred. Kindly try again.";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {

                await next(context);
                if (context.Response.StatusCode == (int)StatusCodes.Status429TooManyRequests)
                {
                    message = "Too many requests. Please try again later.";
                    title = "Too Many Requests";
                    statusCode = (int)HttpStatusCode.TooManyRequests;
                    await ModifyHeder(context, message, title, statusCode);
                }
                if(context.Response.StatusCode == (int)StatusCodes.Status401Unauthorized)
                {
                    message = "Unauthorized. Please authenticate and try again.";
                    title = "Unauthorized";
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    await ModifyHeder(context, message, title, statusCode);
                }
                if (context.Response.StatusCode == (int)StatusCodes.Status403Forbidden)
                {
                    message = "Forbidden. You do not have permission to access this resource.";
                    title = "Forbidden";
                    statusCode = (int)HttpStatusCode.Forbidden;
                    await ModifyHeder(context, message, title, statusCode);
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                // Handle specific exceptions , request timeout, 408 error;
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    message = "Request timeout. Please try again.";
                    title = "Request Timeout";
                    statusCode = (int)HttpStatusCode.RequestTimeout;
                }
                await ModifyHeder(context, message, title, statusCode);

            }
        }

        private async Task ModifyHeder(HttpContext context, string message, string title, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var response = new { Title = title, Message = message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Title = title,
                Detail = message,
                Status = statusCode
            }),CancellationToken.None);
        }
    }
}
