namespace Barbar.SymbolicMath.Optimization
{
    /// <summary>
    /// Maximum evaluations
    /// </summary>
    public class MaxEval : IOptimizationData
    {
        /// <summary>
        /// Maximum evaluations
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="value"></param>
        public MaxEval(int value)
        {
            Value = value;
        }
    }
}
