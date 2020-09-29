using System;
using System.Collections.Generic;
using System.Text;

namespace ElaneBoot.Schedule
{
    internal class ApiException : Exception
    {
        public ApiException(string errMsg = "操作失败", int? errCode = 4000, string msg = "操作失败", Exception innerExcetion = null) : base(errMsg, innerExcetion)
        {
            ErrMsg = errMsg;
            ErrCode = errCode;
            Msg = msg;
        }

        public string ErrMsg { get; }
        public int? ErrCode { get; }
        public string Msg { get; }
        public string ErrStack { get; set; }
    }


    internal class ApiExceptionHelper
    {
        public static ApiException Resolve(Exception exception)
        {
            if (exception == null) return null;
            if (exception is ApiException) return (ApiException)exception;
            ApiException result = new ApiException();
            if (exception.InnerException != null && exception.InnerException is ApiException)
            {
                result = (ApiException)exception.InnerException;
                result.ErrStack = exception.StackTrace;
            }
            else
            {
                result = new ApiException(exception.Message);
                result.ErrStack = exception.StackTrace;
            }
            return result;
        }
    }

}