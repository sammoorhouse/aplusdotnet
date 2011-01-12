using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;
using AplusCore.Runtime;

namespace AplusCoreUnitTests.Dlr.ControlFlow
{
    [TestClass]
    public class If : AbstractTest
    {

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("If"), TestMethod]
        public void IfTrue()
        {
            AType expected = Helpers.BuildString("ok");
            AType value = this.engine.Execute<AType>("if 1 'ok'");

            Assert.AreEqual<AType>(expected, value, "Incorrect value returned");
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("If"), TestMethod]
        public void IfTrueWholeNumber()
        {
            AType expected = Helpers.BuildString("ok");
            AType value = this.engine.Execute<AType>("if 1.0000000000001 'ok'");

            Assert.AreEqual<AType>(expected, value, "Incorrect value returned");
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("If"), TestMethod]
        public void IfTrueArray()
        {
            AType expected = Helpers.BuildString("ok");
            AType value = this.engine.Execute<AType>("if (1+iota 1) 'ok'");

            Assert.AreEqual<AType>(expected, value, "Incorrect value returned");
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("If"), TestMethod]
        [ExpectedException(typeof(Error.Domain))]
        public void IfTrueInvalidArray()
        {
            AType value = this.engine.Execute<AType>("if (1+iota 10) 'ok'");
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("If"), TestMethod]
        [ExpectedException(typeof(Error.Domain))]
        public void IfTrueInvalidWholeNumber()
        {
            AType value = this.engine.Execute<AType>("if 1.000000000001 'ok'");
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("If"), TestMethod]
        [ExpectedException(typeof(Error.Domain))]
        public void IfDomainErrorVector()
        {
            // Because a vector is supplied we expect a Domain error
            this.engine.Execute<AType>("if 1 200");
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("If"), TestMethod]
        public void IfFalse()
        {
            AType value = this.engine.Execute<AType>("if 0 'ok'");

            Assert.IsTrue(value.Type == ATypes.ANull, "Incorrect value returned");
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("If"), TestMethod]
        public void IfElseFalse()
        {
            AType expected = Helpers.BuildString("fail");
            AType value = this.engine.Execute<AType>("if 0 'ok' else 'fail'");

            Assert.AreEqual<AType>(expected, value, "Incorrect value returned");
        }
    }
}
