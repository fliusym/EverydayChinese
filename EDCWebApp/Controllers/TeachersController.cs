﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EDCWebApp.Models;
using EDCWebApp.DAL;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Data.Entity;
using EDCWebApp.Exceptions;
using EDCWebApp.Extensions;

namespace EDCWebApp.Controllers
{
    [Authorize(Roles="Teacher")]
    [RoutePrefix("api/Teachers")]
    public class TeachersController : ApiController
    {
        private IEDCLoginUserContext db = new EDCLoginUserContext();
        public TeachersController() { }
        public TeachersController(IEDCLoginUserContext context)
        {
            db = context;
        }

        //get the teacher
        [ResponseType(typeof(EDCTeacherDTO))]
        public async Task<IHttpActionResult> GetTeacher(string id)
        {
            var teacher = await db.Teachers.Include(p => p.LearnRequests.Select(x => x.RegisteredStudents))
                .SingleOrDefaultAsync(p => p.TeacherName == id);
            if (teacher == null)
            {
                var msg = string.Format("Can't find the teacher {0}", id);
                var modelError = EDCExceptionFactory.GenerateHttpError(msg, EDCWebServiceErrorType.Error, true);
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, modelError);
                throw new HttpResponseException(response);
            }
            var learnRequestDtos = new List<EDCLearnRequestDTO>();
            foreach (var i in teacher.LearnRequests)
            {
                var dto = db.GenerateDTO(i);
                learnRequestDtos.Add(dto);
            }
            return Ok(new EDCTeacherDTO()
            {
                TeacherName = id,
                LearnRequests = learnRequestDtos
            });
        }
    }
}
