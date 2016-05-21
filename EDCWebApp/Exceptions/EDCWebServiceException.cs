using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http;

namespace EDCWebApp.Exceptions
{
    public enum EDCWebServiceErrorType
    {
        Error,
        Warning
    }
    public class EDCWebServiceException : Exception
    {
        EDCWebServiceErrorType _errorType;
        public EDCWebServiceException()
        {

        }
        public EDCWebServiceException(string message)
            : base(message)
        {

        }
        public EDCWebServiceException(string message, Exception inner)
            :base(message,inner)
        {

        }
        public EDCWebServiceException(string message, EDCWebServiceErrorType type, Exception inner)
            : base(message, inner)
        {
            _errorType = type;
        }
    }

}