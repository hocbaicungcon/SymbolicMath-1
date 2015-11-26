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
            var term = Term.Factory.Create(1);
            Assert.IsInstanceOfType(term.Clone(), typeof(TermInt64));
            Assert.AreEqual(((Term)term.Clone()).AsInt64(), 1);
        }

        [TestMethod]
        public void CompareTo()
        {
            var term1 = Term.Factory.Create(1);
            var term2 = Term.Factory.Create(2);
            Assert.IsTrue(term1.CompareTo(term2) < 0);
            Assert.IsTrue(term2.CompareTo(term1) > 0);
        }

        [TestMethod]
        public void Evaluate()
        {
            var term1 = Term.Factory.Create(1);
            Assert.AreEqual(term1.Evaluate(), 1.0);
        }

        [TestMethod]
        public void Gcd()
        {
            var a = Term.Factory.Create(15);
            var b = Term.Factory.Create(3);
            var c = a.Gcd(b).AsInt64();
            Assert.AreEqual(c, 3);
            a = Term.Factory.Create(36);
            b = Term.Factory.Create(26);
            c = a.Gcd(b).AsInt64();
            Assert.AreEqual(c, 2);
            a = Term.Factory.Create(7);
            b = Term.Factory.Create(5);
            c = a.Gcd(b).AsInt64();
            Assert.AreEqual(c, 1);
        }

        [TestMethod]
        public void IsNegative()
        {
            Assert.IsTrue(Term.Factory.Create(-1).IsMinusOrNegativeTerm());
        }
    }
}
