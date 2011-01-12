using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;
using AplusCore.Runtime;

namespace AplusCoreUnitTests.Dlr.Operator.Monadic.Bitwise
{
    [TestClass]
    public class BitwiseLessEqual : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Bitwise Less Equal"), TestMethod]
        public void BitwiseLessEqualVector()
        {
            AType expected = AArray.Create(ATypes.AInteger,
                AInteger.Create(-9), AInteger.Create(-97), AInteger.Create(-97)
            );

            AType result = this.engine.Execute<AType>("10 100 100 bwle 3 4 5");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Bitwise Less Equal"), TestMethod]
        [ExpectedException(typeof(Error.Type))]
        public void Error()
        {
            this.engine.Execute<AType>("10 100 1000 bwle 3 4 5.0");
        }
    }
}
