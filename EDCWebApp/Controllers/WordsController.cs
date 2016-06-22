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
                //return BadRequest();
            }

            var word = await db.Words
                    .Include(p => p.Phrases.Select(x => x.Examples))
                    .Include(p => p.Slangs)
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

        [Authorize(Roles="Teacher")]
        [HttpPost]
        [Route("~/api/Words/Add")]
   //     [ResponseType(typeof(EDCWordDTO))]
        public async Task<IHttpActionResult> PostWord(AddWordBindingModel word)
        {
            if (!ModelState.IsValid)
            {
                var msg = "Something wrong with the input while adding the word.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }
            //to see if the word is already there
            var wordFromDb = await db.Words.Where(p => p.Character == word.Character).SingleOrDefaultAsync();
            if (wordFromDb != null)
            {
                return Ok();
            }
            //see if the date is the same
            var dateFromDb = await db.Words.Where(p => p.Date == word.Date).SingleOrDefaultAsync();
            if (dateFromDb != null)
            {
                var msg = String.Format("The date {0} already has the word.", word.Date);
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }
            var edcWord = new EDCWord()
            {
                Audio = word.Audio,
                Pinyin = word.Pinyin,
                Character = word.Character,
                BasicMeanings = word.BasicMeanings,
                Date = word.Date,
                Explanation = word.Explanation
            };
            if (word.Phrases != null && word.Phrases.Count() > 0)
            {
                var phrases = new List<EDCPhrase>();
                var examples = new List<EDCPhraseExample>();
                foreach (var p in word.Phrases)
                {
                    if(p.English == null || p.English.Length == 0
                        || p.Chinese == null || p.Chinese.Length == 0)
                    {
                        continue;
                    }
                    var phraseExamples = new List<EDCPhraseExample>();
                    foreach (var e in p.Examples)
                    {
                        phraseExamples.Add(new EDCPhraseExample
                        {
                            Englisgh = e.English,
                            Chinese = e.Chinese
                        });
                    }
                    phrases.Add(new EDCPhrase
                    {
                        English = p.English,
                        Chinese = p.Chinese,
                        Pinyin = p.Pinyin,
                        Examples = phraseExamples
                    });
                    examples.AddRange(phraseExamples);
                }
                edcWord.Phrases = phrases;
                foreach (var e in examples)
                {
                    db.PhraseExamples.Add(e);
                }
                foreach (var p in phrases)
                {
                    db.Phrases.Add(p);
                }
            }
            if (word.Slangs != null && word.Slangs.Count() > 0)
            {
                var slangs = new List<EDCSlang>();
                foreach (var q in word.Slangs)
                {
                    if(q.SlangChinese == null || q.SlangChinese.Length == 0
                        ||q.SlangEnglish == null || q.SlangEnglish.Length ==0
                        || q.SlangExampleChinese == null || q.SlangExampleChinese.Length == 0
                        || q.SlangExampleEnglish == null || q.SlangExampleEnglish.Length == 0)
                    {
                        continue;
                    }
                    slangs.Add(new EDCSlang
                    {
                        SlangChinese = q.SlangChinese,
                        SlangEnglish = q.SlangEnglish,
                        SlangExampleEnglish = q.SlangExampleEnglish,
                        SlangExampleChinese = q.SlangExampleChinese
                    });
                }
                edcWord.Slangs = slangs;
                foreach (var q in slangs)
                {
                    db.Slangs.Add(q);
                }
            }
            
            db.Words.Add(edcWord);
            try
            {
                await db.SaveChangesToDbAsync();
                return Ok();
            }
            catch (Exception)
            {
                var msg = "Something internal errors occurred while adding the word.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, modelError);
                throw new HttpResponseException(response);
            }
            //var wordDto = db.GenerateDTO(edcWord);
            //return CreatedAtRoute("DefaultApi", new { id = edcWord.ID }, wordDto);
        }
    }
}
