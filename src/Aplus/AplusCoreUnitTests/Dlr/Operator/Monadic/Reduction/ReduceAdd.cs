using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;
using AplusCore.Runtime;

namespace AplusCoreUnitTests.Dlr.Operator.Monadic.Reduction
{
    [TestClass]
    public class ReduceAdd : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceAdd"), TestMethod]
        public void ReduceAddIntegerNull()
        {
            AType expected = AInteger.Create(0);

            AType result = this.engine.Execute<AType>("+/ `int ? ()");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceAdd"), TestMethod]
        public void ReduceAddIntegerVector()
        {
            AType expected = AInteger.Create(42);

            AType result = this.engine.Execute<AType>("+/ 4 3 2 8 5 8 9 3");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceAdd"), TestMethod]
        public void ReduceAddIntegerMatrix()
        {
            AType expected = AArray.Create(
                ATypes.AInteger,
                AInteger.Create(30),
                AInteger.Create(36)
            );

            AType result = this.engine.Execute<AType>("+/ iota 6 2");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceAdd"), TestMethod]
        public void ReduceAddNull()
        {
            AType expected = AInteger.Create(0);

            AType result = this.engine.Execute<AType>("+/ ()");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceAdd"), TestMethod]
        public void ReduceAddFloatMatrix()
        {
            AType expected = AArray.Create(
                ATypes.AFloat,
                AArray.Create(ATypes.AFloat, AFloat.Create(0), AFloat.Create(0)),
                AArray.Create(ATypes.AFloat, AFloat.Create(0), AFloat.Create(0))
            );

            AType result = this.engine.Execute<AType>("+/ 0 2 2 rho 4.5");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("ReduceAdd"), TestMethod]
        [ExpectedException(typeof(Error.Type))]
        public void ReduceAddTypeError()
        {
            AType result = this.engine.Execute<AType>("+/ `sym");
        }
    }
}
