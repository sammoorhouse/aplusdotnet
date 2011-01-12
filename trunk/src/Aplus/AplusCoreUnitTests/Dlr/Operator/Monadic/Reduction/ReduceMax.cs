using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;

namespace AplusCoreUnitTests.Dlr.Operator.Monadic.Reduction
{
    [TestClass]
    public class ReduceMax : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMax"), TestMethod]
        public void ReduceMaxIntegerVector()
        {
            AType expected = AInteger.Create(122);

            AType result = this.engine.Execute<AType>("max/ 4 5 65 3 2 122");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMax"), TestMethod]
        public void ReduceMaxFloatMatrix()
        {
            AType expected = AArray.Create(
                ATypes.AFloat,
                AFloat.Create(34.5),
                AFloat.Create(87.3),
                AFloat.Create(67.4)
            );

            AType result = this.engine.Execute<AType>(" max/ 3 3 rho 4.43 77.5 34.44 23.3 45.6 67.4 34.5 87.3 43.2");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMax"), TestMethod]
        public void ReduceMaxNull()
        {
            AType expected = AFloat.Create(Double.NegativeInfinity);

            AType result = this.engine.Execute<AType>("max/ ()");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceMax"), TestMethod]
        public void ReduceMaxCharacterArray()
        {
            AType expected = AArray.Create(
                ATypes.AFloat,
                AArray.Create(ATypes.AFloat, AFloat.Create(Double.NegativeInfinity), AFloat.Create(Double.NegativeInfinity)),
                AArray.Create(ATypes.AFloat, AFloat.Create(Double.NegativeInfinity), AFloat.Create(Double.NegativeInfinity))
            );

            AType result = this.engine.Execute<AType>("max/ 0 2 2 rho 'a'");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }
    }
}
