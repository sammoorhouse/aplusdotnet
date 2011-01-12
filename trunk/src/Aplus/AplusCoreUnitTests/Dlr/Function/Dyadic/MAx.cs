using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;

namespace AplusCoreUnitTests.Dlr.Function.Dyadic
{
    [TestClass]
    public class Max : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Dyadic"), TestCategory("Max"), TestMethod]
        public void MaxVector2Vector()
        {
            AType expected = new AArray(
                ATypes.AInteger,
                new AInteger(3),
                new AInteger(7),
                new AInteger(10)
            );
            AType result = this.engine.Execute<AType>("2 5 8 max 3 7 10");

            Assert.AreEqual(expected, result);
        }
    }
}
