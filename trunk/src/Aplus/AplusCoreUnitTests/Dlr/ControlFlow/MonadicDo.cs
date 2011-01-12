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
    public class MonadicDo : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("MonadicDo"), TestMethod]
        public void MonadicDoNoError()
        {
            AType expected = Helpers.BuildStrand(
                new AType[] {
                    AInteger.Create(100),
                    AInteger.Create(0)
                }
            );

            AType result = this.engine.Execute<AType>("do { 100 }");

            Assert.AreEqual<AType>(expected, result);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("MonadicDo"), TestMethod]
        public void MonadicDoExit()
        {
            AType expected = Helpers.BuildStrand(
                new AType[] {
                    AInteger.Create(-1),
                    AInteger.Create(0)
                }
            );

            AType result = this.engine.Execute<AType>("do { :=-1; 8}");

            Assert.AreEqual<AType>(expected, result);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("MonadicDo"), TestMethod]
        public void MonadicDoInnerExit()
        {
            AType expected = Helpers.BuildStrand(
                new AType[] {
                    AInteger.Create(8),
                    AInteger.Create(0)
                }
            );

            AType result = this.engine.Execute<AType>("do { do{ :=-1; 3}; 8 }");

            Assert.AreEqual<AType>(expected, result);
        }

        [TestCategory("DLR"), TestCategory("ControlFlow"), TestCategory("MonadicDo"), TestMethod]
        public void MonadicDoValueError()
        {
            AType expected = Helpers.BuildStrand(
                new AType[] {
                    Helpers.BuildString(ErrorType.Value.ToString()),
                    AInteger.Create((int)ErrorType.Value)
                }
            );

            AType result = this.engine.Execute<AType>("do { no_such_variable }");

            Assert.AreEqual<AType>(expected, result);
        }
    }
}
