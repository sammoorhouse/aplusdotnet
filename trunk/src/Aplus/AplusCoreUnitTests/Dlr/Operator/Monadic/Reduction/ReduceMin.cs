using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;

namespace AplusCoreUnitTests.Dlr.Operator.Monadic.Reduction
{
    [TestClass]
    public class ReduceMin : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMin"), TestMethod]
        public void ReduceMinIntegerVector()
        {
            AType expected = AInteger.Create(2);

            AType result = this.engine.Execute<AType>("min/ 4 5 65 3 2 122");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMin"), TestMethod]
        public void ReduceMinFloatMatrix()
        {
            AType expected = AArray.Create(
                ATypes.AFloat,
                AFloat.Create(4.43),
                AFloat.Create(45.6),
                AFloat.Create(34.44)
            );

            AType result = this.engine.Execute<AType>(" min/ 3 3 rho 4.43 77.5 34.44 23.3 45.6 67.4 34.5 87.3 43.2");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMin"), TestMethod]
        public void ReduceMinNull()
        {
            AType expected = AFloat.Create(Double.PositiveInfinity);

            AType result = this.engine.Execute<AType>("min/ ()");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMin"), TestMethod]
        public void ReduceMinCharacterArray()
        {
            AType expected = AArray.Create(
                ATypes.AFloat,
                AArray.Create(ATypes.AFloat, AFloat.Create(Double.PositiveInfinity), AFloat.Create(Double.PositiveInfinity)),
                AArray.Create(ATypes.AFloat, AFloat.Create(Double.PositiveInfinity), AFloat.Create(Double.PositiveInfinity))
            );

            AType result = this.engine.Execute<AType>("min/ 0 2 2 rho 'a'");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }
    }
}
