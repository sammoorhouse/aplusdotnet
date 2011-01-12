using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;
using Microsoft.Scripting.Hosting;
using AplusCore.Compiler;

namespace AplusCoreUnitTests.Dlr.Assignments
{
    [TestClass]
    public class PickAssignment : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Pick Assignment"), TestMethod]
        public void SimpleIndexing()
        {
            AType expected = AArray.Create(ATypes.AInteger,
                AInteger.Create(0), AInteger.Create(-100), AInteger.Create(0)
            );

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(
                ".a", 
                AArray.Create(ATypes.AInteger,
                    AInteger.Create(0), AInteger.Create(0), AInteger.Create(0)
                )
            );

            this.engine.Execute<AType>("(1 pick a) := -100", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Pick Assignment"), TestMethod]
        public void BoxIndexing()
        {
            AType expected = this.engine.Execute<AType>("(1 2 3; 'hello')");

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(
                ".a",
                this.engine.Execute<AType>("(1 2 3; 4 5 6)")
            );

            this.engine.Execute<AType>("(1 pick a) := 'hello'", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Pick Assignment"), TestMethod]
        public void BoxReplaceIndexing()
        {
            AType expected = this.engine.Execute<AType>("(1 2 3; 'hello')");

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(
                ".a",
                this.engine.Execute<AType>("(1 2 3; (4 5 6; 7 8 9))")
            );

            this.engine.Execute<AType>("(1 pick a) := 'hello'", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Pick Assignment"), TestMethod]
        public void MultiDepthBoxIndexing()
        {
            AType expected = this.engine.Execute<AType>("(1 2 3; ('hello'; 7 8 9))");

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(
                ".a",
                this.engine.Execute<AType>("(1 2 3; (4 5 6; 7 8 9))")
            );

            this.engine.Execute<AType>("((1;0) pick a) := 'hello'", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Pick Assignment"), TestMethod]
        public void OrderOfExecution()
        {
            AType expected = this.engine.Execute<AType>("1 2 3");

            ScriptScope scope = this.engine.CreateScope();

            this.engine.Execute<AType>("(1 pick a) := 2 where a:=1 0 3", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Pick Assignment"), TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void IncorrectChainPicking()
        {
            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(
                ".a",
                AArray.Create(ATypes.AInteger,
                    AInteger.Create(0), AInteger.Create(0), AInteger.Create(0)
                )
            );

            this.engine.Execute<AType>("(0 pick 0 pick a) := -100", scope);
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Pick Assignment"), TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void IncorrectTargetPicking()
        {
            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(
                ".a",
                AArray.Create(ATypes.AInteger,
                    AInteger.Create(0), AInteger.Create(0), AInteger.Create(0)
                )
            );

            this.engine.Execute<AType>("(0 pick a[0 1]) := -100", scope);
        }
    }
}
