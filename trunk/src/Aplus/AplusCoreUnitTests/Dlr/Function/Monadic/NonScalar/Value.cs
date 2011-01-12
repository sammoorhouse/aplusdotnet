using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;
using Microsoft.Scripting.Hosting;
using AplusCore.Runtime;

namespace AplusCoreUnitTests.Dlr.Function.Monadic.NonScalar
{
    [TestClass]
    public class Value : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Value"), TestMethod]
        public void ValueUnqualified()
        {
            AType x = AInteger.Create(100);

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(".x", x);

            AType result = this.engine.Execute<AType>("ref `x", scope);

            Assert.AreEqual<AType>(x, result);
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Value"), TestMethod]
        public void ValueQualified()
        {
            AType x = AInteger.Create(100);

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable("Q.x", x);

            AType result = this.engine.Execute<AType>("ref `Q.x", scope);

            Assert.AreEqual<AType>(x, result);
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Value"), TestMethod]
        [ExpectedException(typeof(Error.Value))]
        public void ValueErrorValue()
        {
            // The requested 'x' does not exists
            this.engine.Execute<AType>("ref `x");
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Value"), TestMethod]
        [ExpectedException(typeof(Error.Value))]
        public void ValueErrorQualified()
        {
            // The requested 'a.b.c' is not a qualified name
            this.engine.Execute<AType>("ref `a.b.c");
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Value"), TestMethod]
        [ExpectedException(typeof(Error.Type))]
        public void ValueErrorType()
        {
            this.engine.Execute<AType>("ref 100");
        }

    }
}
