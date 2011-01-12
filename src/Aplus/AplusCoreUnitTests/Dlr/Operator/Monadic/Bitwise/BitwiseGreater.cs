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
    public class BitwiseGreater : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Bitwise Greater"), TestMethod]
        public void BitwiseGreaterVector()
        {
            AType expected = AArray.Create(ATypes.AInteger,
                AInteger.Create(8), AInteger.Create(96), AInteger.Create(96)
            );

            AType result = this.engine.Execute<AType>("10 100 100 bwgt 3 4 5");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Bitwise Greater"), TestMethod]
        [ExpectedException(typeof(Error.Type))]
        public void BitwiseGreaterError()
        {
            this.engine.Execute<AType>("10 100 1000 bwgt 3 4 5.0");
        }
    }
}
