using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace EDCWebApp.Exceptions
{
    public static class ExceptionFactory
    {

        public static EDCWebServiceException CreateEDCWebServiceException(string message, EDCWebServiceErrorType type)
        {
            var inner = new HttpResponseException(HttpStatusCode.InternalServerError);
            return new EDCWebServiceException(message, type, inner);
        }
        public static EDCWebServiceException CreateEDCWebServiceException(string message, EDCWebServiceErrorType type, HttpStatusCode code)
        {
            var inner = new HttpResponseException(code);
            return new EDCWebServiceException(message, type, inner);
        }
    }
}