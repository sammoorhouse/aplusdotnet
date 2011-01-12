using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;

namespace AplusCoreUnitTests.Dlr.Function.Dyadic.Scalar
{
    [TestClass]
    public class Max : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Dyadic"), TestCategory("Max"), TestMethod]
        public void MaxVector2Vector()
        {
            AType expected = AArray.Create(
                ATypes.AInteger,
                AInteger.Create(3),
                AInteger.Create(7),
                AInteger.Create(10)
            );
            AType result = this.engine.Execute<AType>("2 5 8 max 3 7 10");

            Assert.AreEqual(expected, result);
        }
    }
}
