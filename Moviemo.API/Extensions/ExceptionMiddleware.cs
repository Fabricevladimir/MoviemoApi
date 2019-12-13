using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moviemo.API.Models;
using System.Net;

namespace Moviemo.API.Extensions
{
    public static class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandler (this IApplicationBuilder app, ILogger<Startup> logger)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    if (feature == null) return;

                    logger.LogError($"Something went wrong: {feature.Error}");

                    var error = new ErrorDetail()
                    {
                        Message = "Internal Server Error.",
                        StatusCode = context.Response.StatusCode
                    };

                    await context.Response.WriteAsync(error.ToString());
                });
            });
        }
    }
}
