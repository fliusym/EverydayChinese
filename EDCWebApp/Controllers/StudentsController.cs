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


    }
}
