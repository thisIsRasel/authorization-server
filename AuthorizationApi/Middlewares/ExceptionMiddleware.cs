using Application.Models;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.Middlewares
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next.Invoke(httpContext);
            }
            catch(InvalidRequestException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                await httpContext.Response.WriteAsync(new ErrorDetails
                {
                    Error = "invalid_request",
                    Message = ex.Message
                }.ToString());
            }
            catch(UnauthorizedException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await httpContext.Response.WriteAsync(new ErrorDetails
                {
                    Error = "unauthorized",
                    Message = ex.Message
                }.ToString());
            }
            catch(InvalidRefreshTokenException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                await httpContext.Response.WriteAsync(new ErrorDetails
                {
                    Error = "invalid_refresh_token",
                    Message = ex.Message
                }.ToString());
            }
            catch(ExpiredRefreshTokenException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                await httpContext.Response.WriteAsync(new ErrorDetails
                {
                    Error = "expired_refresh_token",
                    Message = ex.Message
                }.ToString());
            }
            catch(Exception ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await httpContext.Response.WriteAsync(new ErrorDetails
                {
                    Error = "internal_server_error",
                    Message = ex.Message
                }.ToString());
            }
        }
    }
}
