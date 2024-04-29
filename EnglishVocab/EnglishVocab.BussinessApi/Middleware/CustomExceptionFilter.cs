using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using EnglishVocab.Application.Models;

namespace EnglishVocab.BussinessApi.Middleware
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An error occurred");

            var serviceRepose = new ServiceResponse()
            {
                Success = false,
                UserMessage = context.Exception.Message,
                SystemMessage = context.Exception.StackTrace?.ToString()
            };
            context.Result = new JsonResult(serviceRepose);

            context.ExceptionHandled = true;
        }
    }
}
