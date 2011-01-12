using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;

namespace AplusCoreUnitTests.Dlr.Operator.Monadic.Reduction
{
    [TestClass]
    public class ReduceMultiply : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMultiply"), TestMethod]
        public void ReduceMultiplyIntegerVector()
        {
            AType expected = AInteger.Create(960);

            AType result = this.engine.Execute<AType>("*/ 4 3 2 8 5");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMultiply"), TestMethod]
        public void ReduceMultiplyIntegerMatrix()
        {
            AType expected = AArray.Create(
                ATypes.AInteger,
                AInteger.Create(160),
                AInteger.Create(72)
            );

            AType result = this.engine.Execute<AType>("*/ 3 2 rho 5 6 8 2 4 6");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMultiply"), TestMethod]
        public void ReduceMultiplyNull()
        {
            AType expected = AInteger.Create(1);

            AType result = this.engine.Execute<AType>("*/ ()");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMultiply"), TestMethod]
        public void ReduceMultiplyFloatMatrix()
        {
            AType expected = AArray.Create(
                ATypes.AFloat,
                AArray.Create(ATypes.AFloat, AFloat.Create(1), AFloat.Create(1)),
                AArray.Create(ATypes.AFloat, AFloat.Create(1), AFloat.Create(1))
            );

            AType result = this.engine.Execute<AType>("*/ 0 2 2 rho 4.5");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }
    }
}
