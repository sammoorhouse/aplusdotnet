using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Scripting.Hosting;
using AplusCore.Types;

namespace AplusCoreUnitTests.Dlr.Assignments
{
    [TestClass]
    public class ValueInContext : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Value"), TestMethod]
        public void ValueAssignment()
        {
            AType expected = AInteger.Create(-1);

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(".a", AInteger.Create(100));

            this.engine.Execute<AType>("(ref `a) := -1", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Value"), TestMethod]
        public void ValueAssignmentNoVariable()
        {
            AType expected = AInteger.Create(-1);

            ScriptScope scope = this.engine.CreateScope();

            this.engine.Execute<AType>("(ref `a) := -1", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Value"), TestMethod]
        public void ValueAssignmentToSillyVariable()
        {
            AType expected = AInteger.Create(-1);

            ScriptScope scope = this.engine.CreateScope();

            this.engine.Execute<AType>("(ref `0) := -1", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".0"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Value In Context"), TestMethod]
        public void ValueInContextAssignmentToSillyVariable()
        {
            AType expected = AInteger.Create(-1);

            ScriptScope scope = this.engine.CreateScope();

            this.engine.Execute<AType>("(`0 ref `0) := -1", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>("0.0"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Value In Context"), TestMethod]
        public void ValueinContextAssignment()
        {
            AType expected = AInteger.Create(-1);

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable("X.a", AInteger.Create(100));

            this.engine.Execute<AType>("(`X ref `a) := -1", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>("X.a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Value In Context"), TestMethod]
        public void ValueinContextAssignmentNoVariable()
        {
            AType expected = AInteger.Create(-1);

            ScriptScope scope = this.engine.CreateScope();
            //scope.SetVariable("X.a", AInteger.Create(100));

            this.engine.Execute<AType>("(`X ref `a) := -1", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>("X.a"), "Incorrect value assigned");
        }
    }
}
