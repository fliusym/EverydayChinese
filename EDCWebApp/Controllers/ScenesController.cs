using EDCWebApp.DAL;
using EDCWebApp.Exceptions;
using EDCWebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using EDCWebApp.Extensions;
using EDCWebApp.Models;


namespace EDCWebApp.Controllers
{
    [RoutePrefix("api/Scenes")]
    public class ScenesController : ApiController
    {
        private IEDCLoginUserContext db = new EDCLoginUserContext();

        public ScenesController() { }
        public ScenesController(IEDCLoginUserContext context)
        {
            db = context;
        }

        [HttpGet]
        [ResponseType(typeof(EDCWebApp.Models.EDCScenarioContentDTO))]
        public async Task<IHttpActionResult> GetScene(string date)
        {
            string d;
            TimeConversionUtils.GetDate(date, out d);
            if (d == null)
            {
                var msg = "The input date is not valid.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }

            var scene = await db.Scenarios
                .Include(p => p.Images.Select(x=>x.Words))
                .Where(p => p.Date == d).SingleOrDefaultAsync();
            if (scene != null)
            {
                var dto = db.GenerateDTO(scene);
                if (dto != null)
                {
                    return Ok(dto);
                }
            }
            var message = string.Format("Can't find the scenario for {0}.", d);
            var error = EDCExceptionFactory.GenerateHttpError(message, EDCWebServiceErrorType.Error, true);
            var httpResponse = Request.CreateErrorResponse(HttpStatusCode.NotFound, error);
            throw new HttpResponseException(httpResponse);
        }

        [HttpPost]
        [Authorize(Roles="Teacher")]
        [Route("~/api/Scenes/Add")]
        public async Task<IHttpActionResult> PostScene(AddSceneBindingModel model)
        {
            if (!ModelState.IsValid )
            {
                var msg = "Something wrong with the input while adding the scenario.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }

            var dateFromDb = await db.Scenarios.Where(p => p.Date == model.Date).SingleOrDefaultAsync();
            if (dateFromDb != null)
            {
                var msg = String.Format("The date {0} already has the word.", model.Date);
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }
            FilterBindingModel(model);
            var images = new List<EDCScenarioImage>();
            if (model.Images != null)
            {
                foreach (var i in model.Images)
                {
                    var words = new List<EDCScenarioWord>();
                    foreach (var w in i.SceneWords)
                    {
                        var word = new EDCScenarioWord
                        {
                            ChineseWord = w.WordChinese,
                            ChineseWordPinyin = w.WordPinyin,
                            ChineseWordAudio = w.WordAudio
                        };
                        words.Add(word);
                        db.ScenarioWords.Add(word);
                    }
                    var image = new EDCScenarioImage
                    {
                        Image = i.ImageSrc,
                        Words = words
                    };
                    images.Add(image);
                    db.ScenarioImages.Add(image);
                }

            }
            var scenario = new EDCScenarioContent
            {
                ThemeChinese = model.TitleChinese,
                ThemeEnglish = model.TitleEnglish,
                Date = model.Date,
                Images = images
            };
            db.Scenarios.Add(scenario);
            try
            {
                await db.SaveChangesToDbAsync();
                return Ok();
            }
            catch (Exception)
            {
                var msg = "Something internal errors occurred while adding the scenario.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, modelError);
                throw new HttpResponseException(response);
            }
        }

        private void FilterBindingModel(AddSceneBindingModel model)
        {
            var finalImage = new List<SceneImageBindingModel>();
            foreach (var i in model.Images)
            {
                if (i.ImageSrc.Length > 0)
                {
                    var validWords = new List<SceneImageWordBindingModel>();
                    foreach (var w in i.SceneWords)
                    {
                        if (w.WordChinese != null && w.WordChinese.Length > 0)
                        {
                            if (w.WordAudio == null || w.WordAudio.Length == 0)
                            {
                                w.WordAudio = w.WordChinese + ".w4a";
                            }
                            if (w.WordPinyin == null || w.WordPinyin.Length == 0)
                            {
                                w.WordPinyin = w.WordChinese + ".png";
                            }
                            validWords.Add(w);
                        }
                    }
                    i.SceneWords = validWords;
                    finalImage.Add(i);
                }
            }
            model.Images = finalImage;
        }

    }
}
