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
            Constant.Factory = ConstantInt64.Factory;
        }

    }
}
