using ElaneBoot.Schedule.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace ElaneBoot.Schedule.Filters
{
    public class ApiActionFilter : IActionFilter
    {
        private readonly ILogger<ApiActionFilter> _logger;

        public ApiActionFilter(ILogger<ApiActionFilter> logger)
        {
            _logger = logger;
        }

        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is RedirectResult) return;
            if (!context.HttpContext.Request.Path.Value.ToLower().StartsWith("/schedule/")) return;

            var exp = context.Exception;
            if (exp == null)
            {
                object outputData = null;
                bool isOutput = false;
                if (context.Result is ObjectResult objectResult)
                {
                    isOutput = true;
                    outputData = objectResult.Value;
                }
                else if (context.Result is EmptyResult)
                {
                    isOutput = true;
                    outputData = null;
                }
                else
                {
                    //其他返回结果
                }

                if (isOutput)
                {
                    #region 封装出参
                    var output = new Output()
                    {
                        Code = 1,
                        Msg = "操作成功",
                        Data = outputData
                    };
                    var result = new ObjectResult(output);
                    result.StatusCode = (int)HttpStatusCode.OK;
                    context.Result = result;
                    #endregion 封装出参
                }
            }
            else
            {
                //异常
                var exp1 = ApiExceptionHelper.Resolve(exp);
                Output output = new Output()
                {
                    Code = 0,
                    Msg = exp1.Msg,
                    ErrCode = exp1.ErrCode,
                    ErrMsg = exp1.ErrMsg,
                };
                var result = new ObjectResult(output);
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = result;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}