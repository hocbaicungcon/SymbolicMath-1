using Barbar.SymbolicMath.Policies;

namespace Barbar.SymbolicMath.Optimization.Linear
{

    /// <summary>
    /// A callback object that can be provided to a linear optimizer to keep track of the best solution found.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPolicy"></typeparam>
    public class SolutionCallback<T, TPolicy> where TPolicy : IPolicy<T>, new()
    {
        private SimplexTableau<T, TPolicy> tableau;

        /// <summary>
        /// Set the simplex tableau used during the optimization once a feasible solution has been found.
        /// </summary>       
        /// <value>tableau the simplex tableau containing a feasible solution</value>
        public SimplexTableau<T, TPolicy> Tableau
        {
            set { tableau = value; }
        }

        /// <summary>
        /// Retrieve the best solution found so far.
        /// Note: the returned solution may not be optimal, e.g. in case
        /// the optimizer did reach the iteration limits.
        /// </summary>
        /// <value>
        /// the best solution found so far by the optimizer, or null if no feasible solution could be found
        /// </value>
        public PointValuePair<T> Solution
        {
            get { return tableau != null ? tableau.GetSolution() : null; }
        }

        /// <summary>
        /// Returns if the found solution is optimal.
        /// </summary>
        public bool IsSolutionOptimal
        {
            get { return tableau != null ? tableau.IsOptimal() : false; }
        }
    }
}