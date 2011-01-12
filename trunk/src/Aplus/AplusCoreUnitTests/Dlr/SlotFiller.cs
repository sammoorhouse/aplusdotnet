using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AplusCore.Types;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Hosting;
using AplusCore.Runtime;

namespace AplusCoreUnitTests.Dlr
{
    [TestClass]
    public class SlotFiller : AbstractTest
    {
        AType issf = AFunc.Create(
            "_issf",
            (Func<AplusEnvironment, AType, AType>)((scope, x) =>
            {
                if (x.IsArray)
                {
                    return AInteger.Create(x.IsSlotFiller() ? 1 : 0);
                }
                return AInteger.Create(0);
            }),
            2,
            "checks if x is a slotfiller"
        );

        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFiller1()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            AType expected = AInteger.Create(1);
            AType result = this.engine.Execute<AType>("_issf{(`sym;<4)}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFiller2()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            AType expected = AInteger.Create(1);
            AType result = this.engine.Execute<AType>("_issf{(`small`medium`large`super;(16;32;64;72))}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        /*[TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFiller3()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            AType expected = AInteger.Create(1);
            AType result = this.engine.Execute<AType>("_issf{(`a`b`c;(10;(`x`y;(100;200));(`e`f;((`g`h;(1000;2000));'A+'))))}", scriptscope);

            Assert.AreEqual(expected, result);
        }*/

        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFiller4()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            this.engine.Execute<AType>("a{b}: b+b", scriptscope);

            AType expected = AInteger.Create(1);
            AType result = this.engine.Execute<AType>("_issf{(`a;<{a})}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFiller5()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);
            this.engine.Execute<AType>("a{b}: b+b", scriptscope);
            this.engine.Execute<AType>("b{a}: a*a", scriptscope);

            AType expected = AInteger.Create(1);
            AType result = this.engine.Execute<AType>("_issf{(`a`b;(a;b))}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFiller6()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);
            this.engine.Execute<AType>("a{b}: b+b", scriptscope);
            this.engine.Execute<AType>("b{a}: a*a", scriptscope);

            AType expected = AInteger.Create(1);
            AType result = this.engine.Execute<AType>("_issf{(`a`b`c;(a;b;<{+}))}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        //The input is not Array box.
        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFillerSimpleArray()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            AType expected = AInteger.Create(0);
            AType result = this.engine.Execute<AType>("_issf{`small`medium`large`super}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        //The keys and values lenght different.
        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFillerDifferentLength1()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            AType expected = AInteger.Create(0);
            AType result = this.engine.Execute<AType>("_issf{(`small;(16;32;64;72))}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        //The keys and values lenght different.
        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFillerDifferentLength2()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            AType expected = AInteger.Create(0);
            AType result = this.engine.Execute<AType>("_issf{(`small`medium`large`super;(64;72))}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        //The keys are not symbol consant.
        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFillerKeysNotSymbolConstant()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            AType expected = AInteger.Create(0);
            AType result = this.engine.Execute<AType>("_issf{(2 3 1 4;(16;32;64;72))}", scriptscope);

            Assert.AreEqual(expected, result);
        }
        //The keys rank > 1.
        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFillerKeysMatrix()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            AType expected = AInteger.Create(0);
            AType result = this.engine.Execute<AType>("_issf{(4 1 rho `small`medium`large`super;(16;32;64;72))}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        //The keys contains same key.
        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFillerSameKeys()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            AType expected = AInteger.Create(0);
            AType result = this.engine.Execute<AType>("_issf{(`small`medium`large`super`large;(16;32;64;72;64))}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFillerValueIntegerList()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            AType expected = AInteger.Create(0);
            AType result = this.engine.Execute<AType>("_issf{(`x`y`z; 32 64 72)}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFillerValueUserDefinedAndPrimitiveFunctions()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);
            this.engine.Execute<AType>("a{b}: b+b", scriptscope);
            this.engine.Execute<AType>("b{a}: a*a", scriptscope);

            AType expected = AInteger.Create(0);
            AType result = this.engine.Execute<AType>("_issf{(`a`c`b;(a;+;b))}", scriptscope);

            Assert.AreEqual(expected, result);
        }

        [TestCategory("DLR"), TestCategory("SystemFunction"), TestCategory("SlotFiller"), TestMethod]
        public void SlotFillerFunctionScalarKey()
        {
            ScriptScope scriptscope = this.engine.CreateScope();
            scriptscope.SetVariable("_issf", issf);

            AType expected = AInteger.Create(0);
            AType result = this.engine.Execute<AType>("_issf{(`a ,<{+};(3;5))}", scriptscope);

            Assert.AreEqual(expected, result);
        }
    }
}
