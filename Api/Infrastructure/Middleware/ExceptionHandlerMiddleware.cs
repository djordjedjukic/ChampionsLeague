using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Infrastructure.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
            //this.logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var aggregateException = exception as AggregateException;
            var e = aggregateException != null ? aggregateException.Flatten().InnerExceptions.FirstOrDefault() : exception;

            switch (e)
            {
                case ValidationException ve:
                    return WriteExceptionAsync(context,
                        HttpStatusCode.BadRequest,
                        ve.Errors.Select(x => x.ErrorMessage));
                case UnauthorizedAccessException uae:
                    return WriteExceptionAsync(context,
                        HttpStatusCode.Unauthorized,
                        uae.Message);
            }

            return WriteExceptionAsync(context, HttpStatusCode.InternalServerError, e.Message);
        }

        public Task WriteExceptionAsync(HttpContext context, HttpStatusCode code, object error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;
            return response.WriteAsync(JsonConvert.SerializeObject(error));
        }
    }
}
