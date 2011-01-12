using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;
using Microsoft.Scripting.Hosting;

namespace AplusCoreUnitTests.Dlr.Operator.Dyadic.InnerProduct
{
    [TestClass]
    public class IPAddMultiply : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Inner Product"), TestCategory("IP Add Multiply"), TestMethod]
        public void Add2Matrix()
        {
            AType expected = AArray.Create(ATypes.AFloat,
                AArray.Create(ATypes.AFloat, AFloat.Create(2), AFloat.Create(3)),
                AArray.Create(ATypes.AFloat, AFloat.Create(6), AFloat.Create(11))
            );

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(
                ".a", 
                AArray.Create(ATypes.AInteger,
                    AArray.Create(ATypes.AInteger, AInteger.Create(0), AInteger.Create(1)),
                    AArray.Create(ATypes.AInteger, AInteger.Create(2), AInteger.Create(3))
                )
            );

            AType result = this.engine.Execute<AType>("a +.* a", scope);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Inner Product"), TestCategory("IP Add Multiply"), TestMethod]
        public void Add2Arrays()
        {
            AType expected = this.engine.Execute<AType>("`float ? 2 3 4 rho 60 63 66 69 72 75 78 81 84 87 90 93 168 180 192 204 216 228 240 252 264 276 288 300");

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(".y", this.engine.Execute<AType>("iota 2 3"));
            scope.SetVariable(".x", this.engine.Execute<AType>("iota 3 3 4"));

            AType result = this.engine.Execute<AType>("y +.* x", scope);

            Assert.AreEqual(expected, result);
            Assert.AreEqual(InfoResult.OK, result.CompareInfos(expected));
        }

        [TestCategory("DLR"), TestCategory("Dyadic"), TestCategory("Rank"), TestMethod]
        public void InnerProductVsRank()
        {
            ScriptScope scope = this.engine.CreateScope();
            this.engine.Execute<AType>("a pdt b : +/ a * b", scope);
            this.engine.Execute<AType>("y InnerProduct x : y (pdt @ 1 1 0)(-1 rot iota rho rho x) flip x", scope);

            AType expected = AArray.Create(ATypes.AInteger,
                AArray.Create(ATypes.AInteger, AInteger.Create(20), AInteger.Create(23), AInteger.Create(26), AInteger.Create(29)),
                AArray.Create(ATypes.AInteger, AInteger.Create(56), AInteger.Create(68), AInteger.Create(80), AInteger.Create(92))
            );

            AType result_rank = this.engine.Execute<AType>("InnerProduct{iota 2 3; iota 3 4}", scope);
            AType result_ip = this.engine.Execute<AType>("(iota 2 3) +.* iota 3 4", scope);

            Assert.AreEqual(expected, result_rank);
            Assert.AreEqual(InfoResult.OK, result_rank.CompareInfos(expected));

            expected.ConvertToFloat(); // inner product always returns float

            Assert.AreEqual(expected, result_ip);
            Assert.AreEqual(InfoResult.OK, result_ip.CompareInfos(expected));
        }
    }
}
