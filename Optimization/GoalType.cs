namespace Barbar.SymbolicMath.Optimization
{
    /// <summary>
    /// Goal type, one of maximize or minimize
    /// </summary>
    public sealed class GoalType : IOptimizationData
    {
        private string m_Value;

        private GoalType(string value)
        {
            m_Value = value;
        }

        /// <summary>
        /// Maximization. 
        /// </summary>
        public static readonly GoalType MAXIMIZE = new GoalType("MAXIMIZE");
        /// <summary>
        /// Minimization.
        /// </summary>
        public static readonly GoalType MINIMIZE = new GoalType("MINIMIZE");
    }
}
