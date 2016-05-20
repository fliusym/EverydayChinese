using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RazorEngine;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace EDCWebApp.Controllers
{
    public class HtmlActionResult : IHttpActionResult
    {

        private readonly string _view;
        private readonly object _model;

        public HtmlActionResult(string viewName, object model)
        {
            _view = File.ReadAllText(viewName);
            _model = model;
        }
        public System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var parsedView = Razor.Parse(_view, _model);
            response.Content = new StringContent(parsedView);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
            return Task.FromResult(response);
        }
    }
}
