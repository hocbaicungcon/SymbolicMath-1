namespace Barbar.SymbolicMath.Optimization.Linear
{

    /// <summary>
    /// Pivot selection rule to the use for a Simplex solver.
    /// </summary>
    public class PivotSelectionRule : IOptimizationData
    {
        private string m_Value;

        private PivotSelectionRule(string value)
        {
            m_Value = value;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return m_Value;
        }

        /// <summary>
        /// The classical rule, the variable with the most negative coefficient in the objective function row 
        /// will be chosen as entering variable.
        /// </summary>
        public static readonly PivotSelectionRule DANTZIG = new PivotSelectionRule("DANTZIG");

        /// <summary>
        /// The first variable with a negative coefficient in the objective function row will be chosen as entering variable.
        /// This rule guarantees to prevent cycles, but may take longer to find an optimal solution.
        /// </summary>
        public static readonly PivotSelectionRule BLAND = new PivotSelectionRule("BLAND");
    }
}