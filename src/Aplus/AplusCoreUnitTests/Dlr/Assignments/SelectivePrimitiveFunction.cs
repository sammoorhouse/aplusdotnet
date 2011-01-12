using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Scripting.Hosting;
using AplusCore.Types;
using AplusCore.Runtime;

namespace AplusCoreUnitTests.Dlr.Assignments
{
    [TestClass]
    public class SelectivePrimitiveFunction : AbstractTest
    {

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Selective with Primitive Function"), TestMethod]
        public void DiagonalAssignment()
        {
            AType expected = AArray.Create(ATypes.AInteger,
                AArray.Create(ATypes.AInteger, AInteger.Create(1), AInteger.Create(0)),
                AArray.Create(ATypes.AInteger, AInteger.Create(0), AInteger.Create(2))
            );

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(".a", 
                 AArray.Create(ATypes.AInteger,
                    AArray.Create(ATypes.AInteger, AInteger.Create(0), AInteger.Create(0)),
                    AArray.Create(ATypes.AInteger, AInteger.Create(0), AInteger.Create(0))
                )
            );

            this.engine.Execute<AType>("(0 0 flip a):=1 2", scope);

            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".a"));
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Selective with Primitive Function"), TestMethod]
        public void MonadicFunctionAssignment()
        {
            AType expected = this.engine.Execute<AType>("3 3 3 rho -3");

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(".e", this.engine.Execute<AType>("3 3 3 rho 0"));

            this.engine.Execute<AType>("(!e) := -3", scope);

            Assert.AreEqual<AType>(expected, scope.GetVariable<AType>(".e"));
        }

        /// <summary>
        /// This should not run according to the Language Reference. well sort of...
        /// </summary>
        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Selective with Primitive Function"), TestMethod]
        public void NonAllowedFunction()
        {
            AType expected_a = AInteger.Create(0);
            AType expected_b = AArray.Create(ATypes.AInteger, AInteger.Create(-300), AInteger.Create(200));
            

            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(".a", AInteger.Create(0));
            scope.SetVariable(".b", AArray.Create(ATypes.AInteger, AInteger.Create(200), AInteger.Create(200)));

            this.engine.Execute<AType>("(a where b):=-300", scope);

            Assert.AreEqual<AType>(expected_a, scope.GetVariable<AType>(".a"));
            Assert.AreEqual<AType>(expected_b, scope.GetVariable<AType>(".b"));
        }

        /// <summary>
        /// This case demonstrates that if on the left side of assignment the functions recives different parameters.
        /// As seen on the 93 page of the Language Reference.
        /// </summary>
        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Selective with Primitive Function"), TestMethod]
        [ExpectedException(typeof(Error.Index))]
        public void IndexError()
        {
            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(".a", AInteger.Create(0));
            scope.SetVariable(".b", AInteger.Create(0));
            this.engine.Execute<AType>("(a where b):=100", scope);
        }
    }
}
