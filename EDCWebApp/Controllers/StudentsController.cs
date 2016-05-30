using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EDCWebApp.DAL;
using EDCWebApp.Models;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Data.Entity;
using EDCWebApp.Exceptions;
using EDCWebApp.Extensions;
using EDCWebApp.Utilities;

namespace EDCWebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Students")]
    public class StudentsController : ApiController
    {
        private IEDCLoginUserContext db = new EDCLoginUserContext();

        public StudentsController() { }
        public StudentsController(IEDCLoginUserContext context)
        {
            db = context;
        }

        //get student
        [HttpGet]
        [ResponseType(typeof(EDCStudentDTO))]
        public async Task<IHttpActionResult> GetStudent(string id)
        {
            var student = await db.Students.Include(p => p.LearnRequests)
                .Include(p => p.Scenarios)
                .Include(p => p.Words.Select(x => x.Phrases.Select(t => t.Examples)))
                .Where(x => x.StudentName == id).SingleOrDefaultAsync();
            if (student == null)
            {
                var msg = "Couldn't find the student.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }
            try
            {
                var learnRequests = student.LearnRequests != null ? new List<EDCLearnRequestDTO>() : null;
                if (learnRequests != null)
                {
                    foreach (var l in student.LearnRequests)
                    {
                        var dto = db.GenerateDTO(l);
                        learnRequests.Add(dto);
                    }
                }

                var scenarios = student.Scenarios != null ? new List<EDCScenarioContentDTO>() : null;
                if (scenarios != null)
                {
                    foreach (var s in student.Scenarios)
                    {
                        var dto = db.GenerateDTO(s);
                        scenarios.Add(dto);
                    }
                }

                var words = student.Words != null ? new List<EDCWordDTO>() : null;
                if (words != null)
                {
                    foreach (var w in student.Words)
                    {
                        var dto = db.GenerateDTO(w);
                        words.Add(dto);
                    }
                }

                EDCStudentDTO studentDto = new EDCStudentDTO()
                {
                    Name = student.StudentName,
                    LearnRequests = learnRequests,
                    Scenarios = scenarios,
                    Words = words
                };
                return Ok(studentDto);
            }
            catch (Exception e)
            {
                var msg = e.Message;
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }

        }
        //add word
        [Route("~/api/Student/Words")]
        [HttpPut]
        public async Task<IHttpActionResult> PutWord([FromBody]string date)
        {
            var dateObj = DateTime.Parse(date);
            if (dateObj == null)
            {
                var msg = "There is something wrong with the input.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, modelError);
                throw new HttpResponseException(response);
            }
            var d = dateObj.ToShortDateString();
            var word = await db.Words.Where(p => p.Date == d).SingleOrDefaultAsync();
            if (word == null)
            {
                var msg = String.Format("Word of the date {0} can't be find.", d);
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, modelError);
                throw new HttpResponseException(response);
            }
            if (word.StudentName == User.Identity.Name)
            {
                return Ok();
            }
            word.StudentName = User.Identity.Name;
            db.SetEntityModified<EDCWord>(word);
            try
            {
                await db.SaveChangesToDbAsync();
            }
            catch (Exception e)
            {
                var msg = e.Message;
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, modelError);
                throw new HttpResponseException(response);
            }
            return Ok();
        }
        //remove word
        [Route("~/api/Words/{id}")]
        [HttpDelete]
        [Authorize(Roles = "Student")]
        public async Task<IHttpActionResult> DeleteWord(int id)
        {
            var user = await db.Students.Include(p => p.Words)
                .Where(p => p.StudentName == User.Identity.Name).SingleOrDefaultAsync();
            if (user == null)
            {
                var msg = "The user can't be found.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, modelError);
                throw new HttpResponseException(response);
            }

            var word = await db.Words.FindAsync(id);
            if (word == null)
            {
                var msg = "The requested word can't be found.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, modelError);
                throw new HttpResponseException(response);
            }
            try
            {
                user.Words.Remove(word);
                await db.SaveChangesToDbAsync();
            }
            catch (Exception e)
            {
                var msg = e.Message;
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, modelError);
                throw new HttpResponseException(response);
            }
            return Ok(HttpStatusCode.NoContent);
        }

        //add learn request
        [Route("~/api/LearnRequests")]
        [HttpPost]
        public async Task<IHttpActionResult> PostLearnRequest(EDCLearnRequestBindingModel model)
        {
            if (!CheckInputLearnRequest(model))
            {
                var msg = "There is something wrong with the input.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }
            var name = User.Identity.Name;
            var student = await db.Students.Where(p => p.StudentName == name).SingleOrDefaultAsync();
            var learnRequestObjs = new List<EDCLearnRequest>();

            foreach (var l in model.LearnRequests)
            {
                var date = l.Date;
                string shortDate;
                TimeConversionUtils.GetDate(date, out shortDate);
                var dateFromDb = db.LearnRequests.Where(p => p.Date == shortDate).Include(p => p.RegisteredStudents);
                if (dateFromDb.Count() > 0)
                {
                    foreach (var t in l.Times)
                    {
                        string[] times = TimeConversionUtils.GetStartAndEndTime(t);
                        bool needBuildNew = true;
                        EDCLearnRequest existed = null;
                        foreach (var temp in dateFromDb)
                        {
                            if (times[0] == temp.StartTime && times[1] == temp.EndTime)
                            {
                                existed = temp;
                                //if (student != null)
                                //{
                                //    temp.RegisteredStudents.Add(student);

                                //}
                                needBuildNew = false;
                                break;
                            }
                        }
                        if (needBuildNew)
                        {
                            var obj = await GenerateLearnRequest(date, times[0], times[1], name);
                            if (obj != null)
                            {
                                learnRequestObjs.Add(obj);
                            }
                        }
                        else
                        {
                            if (existed != null)
                            {
                                existed.RegisteredStudents.Add(student);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var t in l.Times)
                    {
                        string[] timesTemp = TimeConversionUtils.GetStartAndEndTime(t);
                        var obj = await GenerateLearnRequest(date, timesTemp[0], timesTemp[1], name);
                        if (obj != null)
                        {
                            learnRequestObjs.Add(obj);
                        }
                    }
                }
            }
            if (learnRequestObjs.Count > 0)
            {
                try
                {
                    string teacherName = "xyov.max@gmail.com";
                    var tasks = new List<Task>();
                    foreach (var l in learnRequestObjs)
                    {
                        db.AssignTeacherToLearnRequest(l, teacherName);
                    }

                    foreach (var l in learnRequestObjs)
                    {
                        db.LearnRequests.Add(l);
                    }
                    await db.SaveChangesToDbAsync();
                }
                catch (Exception e)
                {
                    var msg = e.Message;
                    var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                    var response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, modelError);
                    throw new HttpResponseException(response);
                }
            }
            return Ok();
        }

        //edit learn request
        [Route("~/api/LearnRequests/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditLearnRequest(int id, EDCLearnRequestEditBindingModel model)
        {
            if (!CheckInputEditLearnRequest(model))
            {
                var msg = "There is something wrong with input.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }
            var learnRequest = await db.LearnRequests.FindAsync(id);
            if (learnRequest == null)
            {
                var msg = "Can't find the learn request.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, modelError);
                throw new HttpResponseException(response);
            }
            //see if the request new time and date is already existed
            var times = TimeConversionUtils.GetStartAndEndTime(model.Time);
            var startTime = times[0];
            var endTime = times[1];
            var newRequest = await db.LearnRequests.Where(p => p.Date == model.Date
                && p.StartTime == startTime
                && p.EndTime == endTime).SingleOrDefaultAsync();
            if (newRequest != null)
            {
                var msg = "Your requested new date and time are already existed.";
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Warning, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelError);
                throw new HttpResponseException(response);
            }
            learnRequest.Date = model.Date;
            learnRequest.StartTime = times[0];
            learnRequest.EndTime = times[1];
            db.SetEntityModified<EDCLearnRequest>(learnRequest);
            try
            {
                await db.SaveChangesToDbAsync();
            }
            catch (Exception e)
            {
                var msg = e.Message;
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, modelError);
                throw new HttpResponseException(response);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        //remove learn request
        [Route("~/api/LearnRequests/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteLearnRequest(int id)
        {
            var learnRequest = await db.LearnRequests.Include(p => p.RegisteredStudents)
                .Where(i => i.ID == id).SingleOrDefaultAsync();
            string message = "";
            if (learnRequest == null)
            {
                message = "Can't find the learn request.";
                var modelError = EDCExceptionFactory.GenerateHttpError(message, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, modelError);
                throw new HttpResponseException(response);
            }
            var user = await db.Students.FindAsync(User.Identity.Name);
            if (user == null)
            {
                message = "Can't find the user.";
                var modelError = EDCExceptionFactory.GenerateHttpError(message, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, modelError);
                throw new HttpResponseException(response);
            }
            learnRequest.RegisteredStudents.Remove(user);
            if (learnRequest.RegisteredStudents.Count == 0)
            {
                db.LearnRequests.Remove(learnRequest);
            }
            try
            {
                await db.SaveChangesToDbAsync();
            }
            catch (Exception e)
            {
                var msg = e.Message;
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, modelError);
                throw new HttpResponseException(response);
            }
            return Ok();
        }
        #region private helpers
        private bool CheckInputLearnRequest(EDCLearnRequestBindingModel model)
        {
            if (model == null)
            {
                return false;
            }
            var times = model.LearnRequests;
            if (times == null || times.Count == 0)
            {
                return false;
            }
            foreach (var t in times)
            {
                if (t == null)
                {
                    return false;
                }
                var date = t.Date;
                var timesTemp = t.Times;
                if (date == null || timesTemp == null)
                {
                    return false;
                }
                if (date.Length == 0 || timesTemp.Count == 0)
                {
                    return false;
                }
                foreach (var k in timesTemp)
                {
                    if (k == null || k.Length == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private async Task<EDCLearnRequest> GenerateLearnRequest(string date, string startTime, string endTime, string name)
        {
            var learnRequest = new EDCLearnRequest()
            {
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                RegisteredStudents = new List<EDCStudent>()
            };
            var student = await db.Students.Where(p => p.StudentName == name).SingleOrDefaultAsync();
            if (student != null)
            {
                learnRequest.RegisteredStudents.Add(student);
                return learnRequest;
            }
            return null;
        }

        private bool CheckInputEditLearnRequest(EDCLearnRequestEditBindingModel model)
        {
            if (model == null)
            {
                return false;
            }
            if (model.Date == null || model.Time == null
                || model.Date.Length == 0 || model.Time.Length == 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
