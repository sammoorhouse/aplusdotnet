using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Runtime;
using AplusCore.Types;

namespace AplusCoreUnitTests.Dlr
{
    [TestClass]
    public class InvokeFunction : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("InvokeFunction"), TestMethod]
        [ExpectedException(typeof(Error.NonFunction))]
        public void IncorrectTargetCall()
        {
            this.engine.Execute<AType>(" a:=6; a{1}");
        }
    }
}
