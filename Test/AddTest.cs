using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barbar.SymbolicMath.Test
{
    [TestClass]
    public class AddTest
    {
        [TestMethod]
        public void SimplifyAddToFraction()
        {
            var c = Constant.Factory.Create(3) + (Constant.Factory.Create(3) / Constant.Factory.Create(2));
            var baseForm = c.GetBaseForm();
            Assert.AreEqual(new Division(9, 2), baseForm);

            c = (Constant.Factory.Create(3) / Constant.Factory.Create(2)) + Constant.Factory.Create(3);
            baseForm = c.GetBaseForm();
            Assert.AreEqual(new Division(9, 2), baseForm);
        }

        [TestMethod]
        public void SimplifyZero()
        {
            var sqrt = new SquareRoot(new ConstantInt64(3));
            var c = sqrt + Constant.Factory.Create(0);
            var baseForm = c.GetBaseForm();
            Assert.AreEqual(sqrt, baseForm);


            c = new ConstantInt64(0) + sqrt;
            baseForm = c.GetBaseForm();
            Assert.AreEqual(sqrt, baseForm);
        }

        [TestMethod]
        public void Simplify()
        {
            var c = (SymMathNode)Constant.Factory.Create(10) + Constant.Factory.Create(5);
            var baseForm = c.GetBaseForm();
            Assert.IsInstanceOfType(baseForm, typeof(Constant));
            Assert.AreEqual(baseForm, 15);
        }
    }
}
