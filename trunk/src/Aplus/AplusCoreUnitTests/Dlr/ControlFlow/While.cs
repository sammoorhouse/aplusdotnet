using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;
using Microsoft.Scripting.Hosting;
using AplusCore.Runtime;

namespace AplusCoreUnitTests.Dlr.ControlFlow
{
    [TestClass]
    public class While : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("While"), TestMethod]
        public void SimpleWhile()
        {
            AType expected = AInteger.Create(14);

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a:=1", scope);
            AType result = this.engine.Execute<AType>("while a { a:=0; i:=14 }", scope);

            Assert.AreEqual<AType>(expected, result, "Returned value is invalid");
            Assert.AreEqual<AType>(AInteger.Create(0), scope.GetVariable<AType>(".a"),
                "Condition not modified outside the block");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".i"),
                "Variable not created outside the block");

        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("While"), TestMethod]
        public void SimpleFalseWhile()
        {
            AType result = this.engine.Execute<AType>("while 0 { i:=14 }");

            Assert.IsTrue(result.Type == ATypes.ANull);
            
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("While"), TestMethod]
        [ExpectedException(typeof(Error.Domain))]
        public void DomainErrorMaxInt()
        {
            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(".a", AFloat.Create(Int32.MaxValue));
            this.engine.Execute<AType>("while a { a:=a + 1.0 }", scope);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("While"), TestMethod]
        [ExpectedException(typeof(Error.Domain))]
        public void DomainErrorIterative()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a:=1", scope);
            this.engine.Execute<AType>("while a { a:=0.1 }", scope);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("While"), TestMethod]
        [ExpectedException(typeof(Error.Domain))]
        public void DomainErrorFloat()
        {
            this.engine.Execute<AType>("while 1.1 { 'zero' }");
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("While"), TestMethod]
        [ExpectedException(typeof(Error.Domain))]
        public void DomainErrorCharacter()
        {
            this.engine.Execute<AType>("while 'zero' { 'zero' }");
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("While"), TestMethod]
        [ExpectedException(typeof(Error.Domain))]
        public void DomainErrorSymbol()
        {
            this.engine.Execute<AType>("while `sym { 'zero' }");
        }
    }
}
