using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace EDCWebApp.Exceptions
{
    public static class EDCExceptionFactory
    {

        public static HttpError GenerateHttpError(string msg, EDCWebServiceErrorType type, bool includeErrorDetail)
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("Message", msg);
            modelState.AddModelError("ErrorType", type.ToString());
            return new HttpError(modelState, includeErrorDetail);
        }

    }
}