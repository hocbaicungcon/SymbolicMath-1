using System;

namespace Barbar.SymbolicMath.Optimization.Linear
{
    /// <summary>
    /// Exception to be thrown when two dimensions differ.
    /// </summary>
    public class DimensionMismatchException : Exception
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dimensionsExpected"></param>
        /// <param name="dimensions"></param>
        public DimensionMismatchException(int dimensionsExpected, int dimensions) : base(string.Format("Expected {0} dimensions, got {1}.", dimensionsExpected, dimensions))
        {
        }
    }
}
