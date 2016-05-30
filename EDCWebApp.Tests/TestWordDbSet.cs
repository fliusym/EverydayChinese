using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDCWebApp.Models;

namespace EDCWebApp.Tests
{
    class TestWordDbSet : TestDbSet<EDCWord>
    {
        public override EDCWord Find(params object[] keyValues)
        {
            return this.SingleOrDefault(p => p.ID == (int)keyValues.Single());
        }
    }
    class TestStudentDbSet : TestDbSet<EDCStudent>
    {

    }
}
