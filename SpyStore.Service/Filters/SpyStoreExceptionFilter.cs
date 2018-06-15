using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace SpyStore.Service.Filters
{
    public class SpyStoreExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            string stackTrace = (_isDevelopment) ? context.Exception.StackTrace : string.Empty;
            string message = ex.Message;
            string error = string.Empty;
            IActionResult actionResult;
            if (ex is InvalidQuantityException)
            {
                //Returns a 400
                error = "Invalid quantity request.";
                actionResult = new BadRequestObjectResult(
                    new { Error = error, Message = message, StackTrace = stackTrace });
            }
            else if (ex is DbUpdateConcurrencyException)
            {
                actionResult = new BadRequestObjectResult(
                    new { Error = error, Message = message, StackTrace = stackTrace });
            }
            else
            {
                error = "General Error.";
                actionResult = new ObjectResult(
                    new { Error = error, Message = message, StackTrace = stackTrace })
                {
                    StatusCode = 500
                };
            }
            context.Result = actionResult;
        }
        //Returns a 400
       
        private readonly bool _isDevelopment;
        public SpyStoreExceptionFilter(bool isDevelopment)
        {
            _isDevelopment = isDevelopment;
        }
    }
}