using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;
using Microsoft.Scripting.Hosting;

namespace AplusCoreUnitTests.Dlr
{
    [TestClass]
    public class SystemCommand : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("System Command"), TestMethod]
        public void ContextTest()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a:=5\n$cx X\na:=6", scope);

            Assert.IsTrue(scope.ContainsVariable(".a"), "Variable not found");
            Assert.AreEqual(AInteger.Create(5), scope.GetVariable<AType>(".a"));

            Assert.IsTrue(scope.ContainsVariable("X.a"), "Variable not found");
            Assert.AreEqual(AInteger.Create(6), scope.GetVariable<AType>("X.a"));
        }
    }
}
