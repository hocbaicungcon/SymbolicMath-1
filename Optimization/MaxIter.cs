namespace Barbar.SymbolicMath.Optimization
{
    /// <summary>
    /// Maximum iterations
    /// </summary>
    public class MaxIter : IOptimizationData
    {
        /// <summary>
        /// Maximum iterations
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="value"></param>
        public MaxIter(int value)
        {
            Value = value;
        }
    }
}
