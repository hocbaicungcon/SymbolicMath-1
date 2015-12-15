using Barbar.SymbolicMath.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbar.SymbolicMath.Test
{
    [TestClass]
    public class EqualityTest
    {
        [TestMethod]
        public void Parse()
        {
            var node = MathParser.Parse("3+3=2*3");
            node = MathParser.Parse("sqrt(7) * sqrt(3+4) = 7");
        }
    }
}
