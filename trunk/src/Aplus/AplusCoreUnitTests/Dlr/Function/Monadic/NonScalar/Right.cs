using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;

namespace AplusCoreUnitTests.Dlr.Function.Monadic.NonScalar
{
    [TestClass]
    public class Right : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Right"), TestMethod]
        public void Null()
        {
            AType expected = AArray.Create(ATypes.AInteger, AInteger.Create(1), AInteger.Create(2));
            AType result = this.engine.Execute<AType>("rtack 1 2");

            Assert.AreEqual<AType>(expected, result, "Incorrect result returned");
        }

    }
}
