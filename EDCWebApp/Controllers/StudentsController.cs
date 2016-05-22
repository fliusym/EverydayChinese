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
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.NotFound);
                throw exception;
            }
            var learnRequests = new List<EDCLearnRequestDTO>();
            foreach (var l in student.LearnRequests)
            {
                var dto = db.GenerateDTO(l);
                learnRequests.Add(dto);
            }
            var scenarios = new List<EDCScenarioContentDTO>();
            foreach (var s in student.Scenarios)
            {
                var dto = db.GenerateDTO(s);
                scenarios.Add(dto);
            }
            var words = new List<EDCWordDTO>();
            foreach (var w in student.Words)
            {
                var dto = db.GenerateDTO(w);
                words.Add(dto);
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

        //remove word
        [Route("{name}/Words/{id}")]
        [HttpDelete]
        [Authorize(Roles="Student")]
        public async Task<IHttpActionResult> RemoveWord(string name, int id)
        {
            var user = await db.Students.Include(p => p.Words)
                .Where(p => p.StudentName == name).SingleOrDefaultAsync();
            if (user == null)
            {
                var msg = "The user can't be found.";
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.NotFound);
                throw exception;
            }
            try
            {
                var word = await db.Words.FindAsync(id);
                if (word == null)
                {
                    var msg = "The requested word can't be found.";
                    var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.NotFound);
                    throw exception;
                }
                user.Words.Remove(word);
                await db.SaveChangesToDbAsync();
            }
            catch (Exception e)
            {
                var msg = e.Message;
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.InternalServerError);
                throw exception;
            }
            return Ok(HttpStatusCode.NoContent);
        }

        //add learn request
        [Route("{name}/LearnRequests/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddLearnRequest(string name, EDCLearnRequestBindingModel model)
        {
            if (name == null || name.Length == 0 || !CheckInputLearnRequest(model))
            {
                var msg = "There is something wrong with the input.";
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.BadRequest);
                throw exception;
            }
            var learnRequestObjs = new List<EDCLearnRequest>();

            foreach (var l in model.DateAndTimes)
            {
                var date = l.Date;
                string shortDate;
                TimeConversionUtils.GetDate(date,out shortDate);
                var dateFromDb = db.LearnRequests.Where(p => p.Date == shortDate);
                if (dateFromDb.Count() > 0)
                {
                    foreach (var t in l.Times)
                    {
                        string[] times = TimeConversionUtils.GetStartAndEndTime(t);
                        bool needBuildNew = true;
                        foreach (var temp in dateFromDb)
                        {
                            if (times[0] == temp.StartTime && times[1] == temp.EndTime)
                            {
                                var student = await db.Students.Where(p => p.StudentName == name).SingleOrDefaultAsync();
                                if (student != null)
                                {
                                    temp.RegisteredStudents.Add(student);
                                }
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
                        tasks.Add(db.AssignTeacherToLearnRequest(l, teacherName));
                    }
                    await Task.WhenAll(tasks);
                    foreach (var l in learnRequestObjs)
                    {
                        db.LearnRequests.Add(l);
                    }
                    await db.SaveChangesToDbAsync();
                }
                catch (Exception e)
                {
                    var msg = e.Message;
                    var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.InternalServerError);
                    throw exception;
                } 
            }
            return Ok();
        }

        //edit learn request
        [Route("{name}/LearnRequests/Edit/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditLearnRequest(string name, int id, EDCLearnRequestEditBindingModel model)
        {
            if (name == null || name.Length == 0 || !CheckInputEditLearnRequest(model))
            {
                var msg = "There is something wrong with input.";
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.BadRequest);
                throw exception;
            }
            var learnRequest = await db.LearnRequests.FindAsync(id);
            if (learnRequest == null)
            {
                var msg = "Can't find the learn request.";
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.BadRequest);
                throw exception;
            }
            //see if the request new time and date is already existed
            var times = TimeConversionUtils.GetStartAndEndTime(model.Time);
            var newRequest = await db.LearnRequests.Where(p => p.Date == model.Date
                && p.StartTime == times[0]
                && p.EndTime == times[1]).SingleOrDefaultAsync();
            if (newRequest != null)
            {
                var msg = "Your requested new date and time are already existed.";
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Warning);
                throw exception;
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
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.InternalServerError);
                throw exception;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        //remove learn request
        [Route("{name}/LearnRequests/Delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteLearnRequest(string name, int id)
        {
            var user = await db.Students.FindAsync(name);
            var learnRequest = await db.LearnRequests.FindAsync(id);
            string message = "";
            if (user == null)
            {
                message = string.Format("Can't find the user {0}", name);
            }
            else if (learnRequest == null)
            {
                message = "Can't find the learn request.";
            }
            if (user == null || learnRequest == null)
            {
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(message, EDCWebServiceErrorType.Error, HttpStatusCode.NotFound);
                throw exception;
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
                var exception = EDCExceptionFactory.CreateEDCWebServiceException(msg, EDCWebServiceErrorType.Error, HttpStatusCode.InternalServerError);
                throw exception;
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
            var userName = model.StudentName;
            if (userName == null || userName.Length == 0)
            {
                return false;
            }
            var times = model.DateAndTimes;
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
