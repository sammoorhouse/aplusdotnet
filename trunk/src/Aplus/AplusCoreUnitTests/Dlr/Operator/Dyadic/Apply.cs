using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Scripting.Hosting;
using AplusCore.Types;
using AplusCore.Runtime;

namespace AplusCoreUnitTests.Dlr.Operator.Dyadic
{
    [TestClass]
    public class Apply : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Dyadic"), TestCategory("Apply"), TestMethod]
        public void ApplyUsePrimitiveScalarFunctionScalar()
        {
            AType expected = AArray.Create(ATypes.AInteger,
                AInteger.Create(4), AInteger.Create(4), AInteger.Create(1)
            );

            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("f := <{+}", scope);
            AType result = this.engine.Execute<AType>("3 2 4 f each 1 2 -3", scope);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Dyadic"), TestCategory("Apply"), TestMethod]
        public void ApplyUsePrimitiveFunctionScalarStrand()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("fns := (*;%;rho)", scope);

            AType expected = AArray.Create(
                ATypes.AInteger,
                AInteger.Create(8),
                AInteger.Create(12)
            );

            AType result = this.engine.Execute<AType>("2 3 fns[0] each 4", scope);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));

            expected = AArray.Create(
                ATypes.AFloat,
                AFloat.Create(0.5),
                AFloat.Create(0.75)
            );

            result = this.engine.Execute<AType>("2 3 fns[1] each 4", scope);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));

            expected = AArray.Create(
                ATypes.AInteger,
                AArray.Create(ATypes.AInteger, AInteger.Create(4), AInteger.Create(4), AInteger.Create(4)),
                AArray.Create(ATypes.AInteger, AInteger.Create(4), AInteger.Create(4), AInteger.Create(4))
            );

            result = this.engine.Execute<AType>("2 3 fns[2] each 4", scope);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Dyadic"), TestCategory("Apply"), TestMethod]
        public void ApplyUsePrimitiveNonScalarFunctionScalar()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("f := <{pick}", scope);

            AType expected = AInteger.Create(20);

            AType result = this.engine.Execute<AType>("`was f each (`this`was`that;(10;20;30))", scope);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Dyadic"), TestCategory("Apply"), TestMethod]
        public void ApplyUseUserDefinedFunctionScalar()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a{b;c} : (b+c)%2", scope);
            this.engine.Execute<AType>("f := <{a}", scope);

            AType expected = AArray.Create(
                ATypes.AFloat,
                AArray.Create(ATypes.AFloat, AFloat.Create(6), AFloat.Create(3)),
                AArray.Create(ATypes.AFloat, AFloat.Create(3), AFloat.Create(5.5))
            );

            AType result = this.engine.Execute<AType>("(f each){2 2 rho 4 5 2 5; 2 2 rho 8 1 4 6}", scope);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Dyadic"), TestCategory("Apply"), TestMethod]
        public void ApplyUseOperatorExpressionScalar()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a := <{(+ each)each}", scope);
            AType result = this.engine.Execute<AType>("(2;(3;5)) a each (6;(3;8))", scope);

            AType expected = AArray.Create(
                ATypes.ABox,
                ABox.Create(ABox.Create(AInteger.Create(8))),
                ABox.Create(
                    AArray.Create(
                        ATypes.ABox,
                        ABox.Create(AInteger.Create(6)),
                        ABox.Create(AInteger.Create(13))
                    )
                )
            );

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Dyadic"), TestCategory("Apply"), TestMethod]
        [ExpectedException(typeof(Error.Valence))]
        public void ApplyValenceError1()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a{b} : b+b", scope);
            this.engine.Execute<AType>("f := <{a}", scope);
            this.engine.Execute<AType>("3 f each 7 5", scope);
        }

        [TestCategory("DLR"), TestCategory("Dyadic"), TestCategory("Apply"), TestMethod]
        [ExpectedException(typeof(Error.Valence))]
        public void ApplyValenceError2()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a := <{rtack}", scope);
            this.engine.Execute<AType>("3 a each 5", scope);
        }
    }
}
