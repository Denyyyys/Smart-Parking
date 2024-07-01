using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ParkingBackEnd.Exceptions;
using System.Text.Json;


namespace ParkingBackEnd.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public ErrorHandlingMiddleware() {}
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(GeneralAPIException e)
            {
                context.Response.StatusCode = e.StatusCode;
                await context.Response.WriteAsync(e.Message);
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                string errorMessage = innerException != null ? innerException.Message : ex.Message;

                // Set the response status code to 500 - Internal Server Error
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(errorMessage);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(e.Message ?? "Something went wrong");
            }

        }
    }
}
