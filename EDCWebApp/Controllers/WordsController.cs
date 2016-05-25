using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EDCWebApp.DAL;
using System.Threading.Tasks;
using System.Web.Http.Description;
using EDCWebApp.Exceptions;
using System.Data.Entity;
using EDCWebApp.Models;
using EDCWebApp.Extensions;
using EDCWebApp.Utilities;
using System.Web.Http.ModelBinding;

namespace EDCWebApp.Controllers
{
    [RoutePrefix("api/Words")]
    public class WordsController : ApiController
    {
        private IEDCLoginUserContext db = new EDCLoginUserContext();

        public WordsController() { }
        public WordsController(IEDCLoginUserContext context)
        {
            db = context;
        }
        [HttpGet]
        [ResponseType(typeof(EDCWebApp.Models.EDCWordDTO))]
        public async Task<IHttpActionResult> GetWord(string date)
        {
            if (date == null || date.Length == 0)
            {

                var msg = "The input date is empty.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }
            string d;
            TimeConversionUtils.GetDate(date, out d);
            if (d == null)
            {
                var msg = "The input date is not valid.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }

            var word = await db.Words
                    .Include(p => p.Phrases.Select(x => x.Examples))
                    .Include(p => p.Quotes)
                    .Where(p => p.Date == d).SingleOrDefaultAsync();
            if (word != null)
            {
                EDCWordDTO wordDto = db.GenerateDTO(word);
                if (wordDto != null)
                {
                    return Ok(wordDto);
                }
            }
            var message = string.Format("Can't find the word for {0}.", d);
            var error = EDCExceptionFactory.GenerateHttpError(message, EDCWebServiceErrorType.Error, true);
            var httpResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, error);
            throw new HttpResponseException(httpResponse);
        }

    }
}
