using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace EDCWebApp.Filters
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public int Port { get; set; }

        public RequireHttpsAttribute()
        {
            Port = 443;
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                var response = new HttpResponseMessage();
                if (request.Method == HttpMethod.Get || request.Method == HttpMethod.Head)
                {
                    var uri = new UriBuilder(request.RequestUri);
                    uri.Scheme = Uri.UriSchemeHttps;
                    uri.Port = this.Port;

                    response.StatusCode = System.Net.HttpStatusCode.Found;
                    response.Headers.Location = uri.Uri;
                }
                else
                {
                    response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                }
            }
            else
            {
                base.OnAuthorization(actionContext);
            }

        }
    }
}