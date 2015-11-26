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
            var c = Term.Factory.Create(3) + (Term.Factory.Create(3) / Term.Factory.Create(2));
            var baseForm = c.GetBaseForm();
            Assert.AreEqual(new Division(9, 2), baseForm);

            c = (Term.Factory.Create(3) / Term.Factory.Create(2)) + Term.Factory.Create(3);
            baseForm = c.GetBaseForm();
            Assert.AreEqual(new Division(9, 2), baseForm);
        }

        [TestMethod]
        public void SimplifyZero()
        {
            var sqrt = new SquareRoot(new TermInt64(3));
            var c = sqrt + Term.Factory.Create(0);
            var baseForm = c.GetBaseForm();
            Assert.AreEqual(sqrt, baseForm);


            c = new TermInt64(0) + sqrt;
            baseForm = c.GetBaseForm();
            Assert.AreEqual(sqrt, baseForm);
        }

        [TestMethod]
        public void Simplify()
        {
            var c = (SymMathNode)Term.Factory.Create(10) + Term.Factory.Create(5);
            var baseForm = c.GetBaseForm();
            Assert.IsInstanceOfType(baseForm, typeof(Term));
            Assert.AreEqual(baseForm, 15);
        }
    }
}
