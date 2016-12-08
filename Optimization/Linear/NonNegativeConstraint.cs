namespace Barbar.SymbolicMath.Optimization.Linear
{
    /// <summary>
    /// A constraint for a linear optimization problem indicating whether all variables must be restricted to non-negative values.
    /// </summary>
    public class NonNegativeConstraint : IOptimizationData
    {
        /// <param name="isRestrictedToNonNegative">
        /// If true, all the variables must be positive.
        /// </param>
        public NonNegativeConstraint(bool isRestrictedToNonNegative)
        {
            IsRestrictedToNonNegative = isRestrictedToNonNegative;
        }
        
        /// <summary>
        /// Indicates whether all the variables must be restricted to non-negative values.
        /// </summary>
        public bool IsRestrictedToNonNegative { get; private set; }
    }
}