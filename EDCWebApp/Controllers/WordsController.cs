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
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.BadRequest);
                throw exception;
            }
            string d;
            TimeConversionUtils.GetDate(date, out d);
            if (d == null)
            {
                var msg = "The input date is not valid.";
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.BadRequest);
                throw exception;
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
            var error = "There is something wrong when getting word.";
            var webException = EDCExceptionFactory.CreateEDCWebServiceException(error, EDCWebServiceErrorType.Error);
            throw webException;
        }
        
    }
}
