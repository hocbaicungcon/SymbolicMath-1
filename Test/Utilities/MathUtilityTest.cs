using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Barbar.SymbolicMath.Utilities;
using System.Linq;

namespace Barbar.SymbolicMath.Test.Utilities
{
    [TestClass]
    public class MathUtilityTest
    {
        [TestMethod]
        public void SqrtAsContinuedFraction()
        {
            var factors = MathUtility.SqrtAsContinuedFraction(2);
            Assert.IsTrue(factors.SequenceEqual(new int[] { 1, 2 }));
            factors = MathUtility.SqrtAsContinuedFraction(31);
            Assert.IsTrue(factors.SequenceEqual(new int[] { 5, 1, 1, 3, 5, 3, 1, 1, 10 }));

        }
    }
}
