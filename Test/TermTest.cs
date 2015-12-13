using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barbar.SymbolicMath.Test
{
    [TestClass]
    public class TermTest
    {
        [TestMethod]
        public void Clone()
        {
            var term = Constant.Factory.Create(1);
            Assert.IsInstanceOfType(term.Clone(), typeof(ConstantInt64));
            Assert.AreEqual(((Constant)term.Clone()).AsInt64(), 1);
        }

        [TestMethod]
        public void CompareTo()
        {
            var term1 = Constant.Factory.Create(1);
            var term2 = Constant.Factory.Create(2);
            Assert.IsTrue(term1.CompareTo(term2) < 0);
            Assert.IsTrue(term2.CompareTo(term1) > 0);
        }

        [TestMethod]
        public void Evaluate()
        {
            var term1 = Constant.Factory.Create(1);
            Assert.AreEqual(term1.Evaluate(), 1.0);
        }

        [TestMethod]
        public void Gcd()
        {
            var a = Constant.Factory.Create(15);
            var b = Constant.Factory.Create(3);
            var c = a.Gcd(b).AsInt64();
            Assert.AreEqual(c, 3);
            a = Constant.Factory.Create(36);
            b = Constant.Factory.Create(26);
            c = a.Gcd(b).AsInt64();
            Assert.AreEqual(c, 2);
            a = Constant.Factory.Create(7);
            b = Constant.Factory.Create(5);
            c = a.Gcd(b).AsInt64();
            Assert.AreEqual(c, 1);
        }

        [TestMethod]
        public void IsNegative()
        {
            Assert.IsTrue(Constant.Factory.Create(-1).IsMinusOrNegativeTerm());
        }
    }
}
