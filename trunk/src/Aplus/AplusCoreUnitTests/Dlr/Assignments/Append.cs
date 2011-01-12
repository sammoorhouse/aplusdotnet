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
    public class Append : AbstractTest
    {
        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Append"), TestMethod]
        public void VectorAppendScalar()
        {
            AType expected = AArray.FromIntegerList(new int[] { 1, 2, 3 });

            ScriptScope scope = this.engine.CreateScope();

            scope.SetVariable(".a", AArray.FromIntegerList(new int[] { 1, 2 }));

            this.engine.Execute<AType>("a[,] := 3", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Append"), TestMethod]
        public void EmptyVectorAppendVector()
        {
            AType expected = AArray.FromIntegerList(new int[] { 3, 4 });

            ScriptScope scope = this.engine.CreateScope();

            this.engine.Execute<AType>("a:=0 rho 0; a[,] := 3 4", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Append"), TestMethod]
        public void VectorAppendVector()
        {
            AType expected = AArray.FromIntegerList(new int[] { 1, 2, 3, 4});

            ScriptScope scope = this.engine.CreateScope();

            scope.SetVariable(".a", AArray.FromIntegerList(new int[] { 1, 2 }));

            this.engine.Execute<AType>("a[,] := 3 4", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Append"), TestMethod]
        public void MatrixAppendScalar()
        {
            AType expected = AArray.Create(ATypes.AInteger,
                AArray.FromIntegerList(new int[] { 0, 0, 0 }),
                AArray.FromIntegerList(new int[] { 0, 0, 0 }),
                AArray.FromIntegerList(new int[] { 3, 3, 3 })
            );

            ScriptScope scope = this.engine.CreateScope();

            scope.SetVariable(".a",
                AArray.Create(ATypes.AInteger,
                    AArray.FromIntegerList(new int[] { 0, 0, 0 }),
                    AArray.FromIntegerList(new int[] { 0, 0, 0 })
                )
            );

            this.engine.Execute<AType>("a[,] := 3", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Append"), TestMethod]
        [ExpectedException(typeof(Error.Length))]
        public void AppendLengthError()
        {
            ScriptScope scope = this.engine.CreateScope();

            scope.SetVariable(".a",
                AArray.Create(ATypes.AInteger,
                    AArray.FromIntegerList(new int[] { 0, 0, 0 }),
                    AArray.FromIntegerList(new int[] { 0, 0, 0 })
                )
            );

            this.engine.Execute<AType>("a[,] := 3 4", scope);
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Append"), TestMethod]
        public void ComplexAppendScalar()
        {
            AType expected = AArray.Create(ATypes.ABox,
                ABox.Create(AArray.FromIntegerList(new int[] { 0, 0, -3 })),
                ABox.Create(AArray.FromIntegerList(new int[] { 2, 2 }))
            );

            ScriptScope scope = this.engine.CreateScope();

            scope.SetVariable(".a",
                AArray.Create(ATypes.ABox,
                    ABox.Create(AArray.FromIntegerList(new int[] { 0, 0 })),
                    ABox.Create(AArray.FromIntegerList(new int[] { 2, 2 }))
                )
            );

            this.engine.Execute<AType>("(0 pick a)[,] := -3", scope);

            Assert.AreEqual(expected, scope.GetVariable<AType>(".a"), "Incorrect value assigned");
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Append"), TestMethod]
        [ExpectedException(typeof(Error.Rank))]
        public void AppendScalarError()
        {
            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(".a", AInteger.Create(1));
            this.engine.Execute<AType>("a[,] := 3", scope);
        }

        [TestCategory("DLR"), TestCategory("Assign"), TestCategory("Append"), TestMethod]
        [ExpectedException(typeof(Error.Type))]
        public void AppendTypeError()
        {
            ScriptScope scope = this.engine.CreateScope();
            scope.SetVariable(".a", AArray.FromIntegerList(new int[] { 1, 2 }));
            this.engine.Execute<AType>("a[,] := 'd'", scope);
        }
    }
}
