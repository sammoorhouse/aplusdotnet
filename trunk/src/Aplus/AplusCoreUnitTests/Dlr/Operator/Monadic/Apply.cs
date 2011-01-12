using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;
using Microsoft.Scripting.Hosting;
using AplusCore.Runtime;

namespace AplusCoreUnitTests.Dlr.Operator.Monadic
{
    [TestClass]
    public class Apply : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Apply"), TestMethod]
        public void ApplyUsePrimitiveScalarFunctionScalar()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("f := <{-}", scope);

            AType expected = AArray.Create(ATypes.AInteger,
                AInteger.Create(-1), AInteger.Create(-2), AInteger.Create(3)
            );

            AType result = this.engine.Execute<AType>("f each 1 2 -3",scope);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Apply"), TestMethod]
        public void ApplyUsePrimitiveFunctionScalarStrand()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("fns := (pi;%;rho)", scope);

            AType expected = AFloat.Create(2 * Math.PI);
            AType result = this.engine.Execute<AType>("fns[0] each 2", scope);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));

            expected = AFloat.Create(0.5);
            result = this.engine.Execute<AType>("fns[1] each 2", scope);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));

            expected = AArray.Create(ATypes.AInteger, AInteger.Create(5));
            result = this.engine.Execute<AType>("fns[2] each iota 5", scope);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Apply"), TestMethod]
        public void ApplyUsePrimitiveNonScalarFunctionScalar()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("f := <{bag}", scope);

            AType expected = AArray.Create(
                ATypes.AInteger,
                AInteger.Create(4),
                AInteger.Create(2)
            );

            AType result = this.engine.Execute<AType>("f each{1 0 0 0 1 0}", scope);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Apply"), TestMethod]
        public void ApplyUseUserDefinedFunctionScalar()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a{b} : b+b", scope);
            this.engine.Execute<AType>("f := <{a}", scope);

            AType expected = AArray.Create(
                ATypes.AInteger,
                AArray.Create(ATypes.AInteger, AInteger.Create(0), AInteger.Create(2)),
                AArray.Create(ATypes.AInteger, AInteger.Create(4), AInteger.Create(6))
            );

            AType result = this.engine.Execute<AType>("(f each){iota 2 2}", scope);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Apply"), TestMethod]
        public void ApplyUseOperatorExpressionScalar()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a := <{(- each)each}", scope);
            AType result = this.engine.Execute<AType>("a each (4;(2;5))", scope);

            AType expected = AArray.Create(
                ATypes.ABox,
                ABox.Create(ABox.Create(AInteger.Create(-4))),
                ABox.Create(
                    AArray.Create(
                        ATypes.ABox,
                        ABox.Create(AInteger.Create(-2)),
                        ABox.Create(AInteger.Create(-5))
                    )
                )
            );

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Apply"), TestMethod]
        [ExpectedException(typeof(Error.Valence))]
        public void ApplyValenceError1()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a{b;c} : b+c", scope);
            this.engine.Execute<AType>("f := <{a}", scope);
            this.engine.Execute<AType>("f each 7 5", scope);
        }

        [TestCategory("DLR"), TestCategory("Monadic"), TestCategory("Apply"), TestMethod]
        [ExpectedException(typeof(Error.Valence))]
        public void ApplyValenceError2()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a := <{/}", scope);
            this.engine.Execute<AType>("a each 7 5", scope);
        }
    }
}
