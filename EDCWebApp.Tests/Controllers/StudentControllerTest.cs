using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EDCWebApp.Models;
using EDCWebApp.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace EDCWebApp.Tests.Controllers
{
    [TestClass]
    public class StudentControllerTest
    {
        private EDCWebApp.Controllers.StudentsController InitializeTest()
        {
            var context = new TestAppContext();
            var student = TestUtils.GetStudent();
            context.Students.Add(student);


            var controller = new StudentsController(context);
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            return controller;
        }
        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetStudentWithInvalidName()
        {
            var controller = InitializeTest();
            Assert.IsNotNull(controller);
  //          controller.RequestContext.Principal = new TestStudentPrincipal("test@gmail.com");
            try
            {
                
                await controller.GetStudent("test");
            }
            catch (HttpResponseException e)
            {
                var msg = "Couldn't find the student.";
                TestUtils.CheckExceptionMessage(e, msg);
                throw;
            }

        }

        [TestMethod]
     //   [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetStudentWithValidName()
        {
            var controller = InitializeTest();
            Assert.IsNotNull(controller);
            //          controller.RequestContext.Principal = new TestStudentPrincipal("test@gmail.com");


            var result = await controller.GetStudent("test@gmail.com");

            Assert.IsNotNull(result);
            var content = result as OkNegotiatedContentResult<EDCStudentDTO>;
            Assert.IsNotNull(content);
            var obj = content.Content as EDCStudentDTO;
            Assert.IsNotNull(obj);

            Assert.IsTrue(obj.Name == "test@gmail.com");
            Assert.IsNotNull(obj.Words);
            Assert.IsNull(obj.LearnRequests);
        }
    }
}
