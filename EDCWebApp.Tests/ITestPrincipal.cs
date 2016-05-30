using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EDCWebApp.Tests
{
    interface ITestPrincipal : IPrincipal
    {
        
    }
    interface ITestIdentity : IIdentity
    {

    }
    class TestGenericIdentity : GenericIdentity
    {
        public TestGenericIdentity(string name)
            : base(name)
        {

        }
        public override bool IsAuthenticated
        {
            get
            {
                return false;
            }
        }
        public override string Name
        {
            get
            {
                return base.Name;
            }
        }
    }
    class TestStudentPrincipal : ITestPrincipal
    {
        public IIdentity Identity { get; private set; }
        public TestStudentPrincipal(string userName)
        {
            this.Identity = new TestGenericIdentity(userName);
        }
        public bool IsInRole(string role)
        {
            if(role == "Student")
            {
                return true;
            }
            return false;
        }
    }
}
