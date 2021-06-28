using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackEndAPI.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {

            switch (context.Exception)
            {

                case ArgumentNullException:
                    context.Result = new BadRequestObjectResult(context.Exception.Message);
                    break;

                case InvalidOperationException:
                    context.Result = new NotFoundObjectResult(context.Exception.Message);
                    break;

                case Exception:
                    context.Result = new BadRequestObjectResult(context.Exception.Message);
                    break;

                default:
                    context.Result = new BadRequestObjectResult(context.Exception.Message);
                    break;

            }

        }

    }
}