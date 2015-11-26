using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Barbar.SymbolicMath.Test
{
    [TestClass]
    public class Bootstrapper
    {
        [AssemblyInitialize]
        public static void Bootstrap(TestContext context)
        {
            Term.Factory = TermInt64.Factory;
        }

    }
}
