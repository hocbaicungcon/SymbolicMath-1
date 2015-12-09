using Barbar.SymbolicMath.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbar.SymbolicMath.Test.Parser
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void Parse()
        {
            Assert.AreEqual(30, MathParser.Parse("10+20").Evaluate());
            Assert.AreEqual(200, MathParser.Parse("10*20").Evaluate());
            Assert.AreEqual(20, MathParser.Parse("5*2^2").Evaluate());
            Assert.AreEqual(200, MathParser.Parse("2^2*50").Evaluate());
            Assert.AreEqual(205, MathParser.Parse("2^2*50+5").Evaluate());
            Assert.AreEqual(4 * 55, MathParser.Parse("2^2*(50+5)").Evaluate());
        }

    }
}
