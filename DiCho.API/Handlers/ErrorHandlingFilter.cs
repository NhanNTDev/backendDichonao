using DiCho.Core.Custom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DiCho.API.Handlers
{
    public class ErrorHandlingFilter : IExceptionFilter
    {
        private readonly ILogger<ErrorHandlingFilter> _logger;

        public ErrorHandlingFilter(ILogger<ErrorHandlingFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.StackTrace);
            if (context.Exception is System.Linq.Dynamic.Core.Exceptions.ParseException || context.Exception is ErrorResponse)
            {
                string message = context.Exception.ToString();
                if (context.Exception.GetType() == typeof(ErrorResponse)) message = ((ErrorResponse)context.Exception).Error.Message;
                context.Result = new ObjectResult(new ErrorResponse(((ErrorResponse)context.Exception).Error.Code, message))
                {
                    StatusCode = ((ErrorResponse)context.Exception).Error.Code,
                };
                context.ExceptionHandled = true;
                return;
            }
#if DEBUG
            global::System.Console.WriteLine(context.Exception.StackTrace);
            global::System.Console.WriteLine(context.Exception.Message);
            context.Result = new ObjectResult(new ErrorResponse((int)HttpStatusCode.InternalServerError, context.Exception.StackTrace))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
            context.ExceptionHandled = true;
#else       
            global::System.Console.WriteLine(context.Exception.StackTrace);
            global::System.Console.WriteLine(context.Exception.Message);
            context.Result = new ObjectResult(new ErrorResponse((int)HttpStatusCode.InternalServerError, "Có lỗi xảy ra trong quá trình xử lý!"))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
            context.ExceptionHandled = true;
#endif

        }
    }
}
