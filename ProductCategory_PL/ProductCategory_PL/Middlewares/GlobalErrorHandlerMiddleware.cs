using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductCatalog_BLL.Helpers.Exceptions;
using System.Net;

namespace ProductCategory_PL.Middlewares
{
    public class GlobalErrorHandlerMiddleware
    {
        RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;

        public GlobalErrorHandlerMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                string message = "Error Occured";
                int errorCode = (int)HttpStatusCode.InternalServerError ;

                if (ex is BusinessException businessException)
                {
                    message = businessException.Message;
                    errorCode = businessException.ErrorCode;
                }
                else
                {
                    _logger.LogError(ex, ex.Message);
                }
                var result = new ObjectResult(message)
                {
                    StatusCode = errorCode
                   
                };
             

                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}
