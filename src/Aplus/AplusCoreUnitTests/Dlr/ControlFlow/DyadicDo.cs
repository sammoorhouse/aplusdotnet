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
    public class DyadicDo : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void SimpleIteration()
        {
            AType expected = AInteger.Create(10);

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a:=0", scope);
            AType result = this.engine.Execute<AType>("10 do { a:=a + 1 }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".a"), "Incorrect result calculated in DO");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void SimpleOneElementArrayIteration()
        {
            AType expected = AInteger.Create(10);

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a:=0", scope);
            AType result = this.engine.Execute<AType>("(1 1 rho 10) do { a:=a + 1 }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".a"), "Incorrect result calculated in DO");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void SimpleToleranceIteration()
        {
            AType expected = AInteger.Create(10);

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a:=0", scope);
            AType result = this.engine.Execute<AType>("10.00000000000001 do { a:=a + 1 }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".a"), "Incorrect result calculated in DO");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void SimpleToleranceRoundingIteration()
        {
            AType expected = AInteger.Create(11);

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a:=0", scope);
            AType result = this.engine.Execute<AType>("10.99999999999999 do { a:=a + 1 }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".a"), "Incorrect result calculated in DO");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void SimpleReverseIteration()
        {
            AType expected = AInteger.Create(10);

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a:=0", scope);
            AType result = this.engine.Execute<AType>("(^10) do { a:=a + 1 }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".a"), "Incorrect result calculated in DO");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void VariableIteration()
        {
            AType expected = AInteger.Create(Enumerable.Range(0, 10).Sum());

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("i:=10; b:=0", scope);
            AType result = this.engine.Execute<AType>("i do { b:=b + i }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".b"), "Incorrect result calculated in DO");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void VariableToleranceRoundingIteration()
        {
            AType expected = AInteger.Create(Enumerable.Range(0, 11).Sum());

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("i:=10.99999999999999; b:=0", scope);
            AType result = this.engine.Execute<AType>("i do { b:=b + i }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".b"), "Incorrect result calculated in DO");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void VariableAssignIteration()
        {
            AType expected = AInteger.Create(Enumerable.Range(0, 20).Sum());

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("b:=0", scope);
            AType result = this.engine.Execute<AType>("(i:=20) do { b:=b + i }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".b"), "Incorrect result calculated in DO");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void VariableIncrementIteration()
        {
            AType expected_i = AInteger.Create(4);
            AType expcetedDoResult = AInteger.Create(13);

            ScriptScope scope = this.engine.CreateScope();
            AType result = this.engine.Execute<AType>("(i:=4) do { i:=10 + i }", scope);

            Assert.AreEqual<AType>(expcetedDoResult, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected_i, scope.GetVariable<AType>(".i"), "Incorrect result calculated in DO");
            Assert.IsTrue(expcetedDoResult.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void ExitIteration()
        {
            AType expected = AInteger.Create(-1);

            ScriptScope scope = this.engine.CreateScope();
            AType result = this.engine.Execute<AType>("(i:=5) do { :=-1; i }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void VariableReverseIteration()
        {
            AType expected = AInteger.Create(Enumerable.Range(0, 10).Sum());

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("i:=10; b:=0", scope);
            AType result = this.engine.Execute<AType>("(^i) do { b:=b + i }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".b"), "Incorrect result calculated in DO");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void VariableAssignReverseIteration()
        {
            AType expected = AInteger.Create(Enumerable.Range(0, 20).Sum());

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("b:=0", scope);
            AType result = this.engine.Execute<AType>("(^i:=20) do { b:=b + i }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".b"), "Incorrect result calculated in DO");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        public void IndexedAssingIteration()
        {
            AType expected = AInteger.Create(10 * 10);

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a:=2 3;b:=0", scope);
            AType result = this.engine.Execute<AType>("(a[0]:=10) do { b:=b + a[0] }", scope);

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned by Do");
            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".b"), "Incorrect result calculated in DO");
            Assert.IsTrue(expected.Type == result.Type);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        [ExpectedException(typeof(Error.Domain))]
        public void SimpleFloatDomainError()
        {
            this.engine.Execute<AType>("1.1 do { 1 }");
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("DyadicDo"), TestMethod]
        [ExpectedException(typeof(Error.Domain))]
        public void VariableFloatDomainError()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a:=2.2", scope);
            this.engine.Execute<AType>("a do { 1 }", scope);
        }

    }
}
