using Barbar.SymbolicMath.Optimization.Utility;
using Barbar.SymbolicMath.Policies;
using System.Collections.Generic;

namespace Barbar.SymbolicMath.Optimization.Linear
{
    /// <summary>
    /// Solves a linear problem using the "Two-Phase Simplex" method.
    /// The <see cref="SimplexSolver{T, TPolicy}"/> 
    /// supports the following {@link OptimizationData} data provided
    /// as arguments to
    /// <see cref="BaseOptimizer{T}.Optimize"/> 
    /// <list>
    ///   <item>objective function: <see cref="LinearObjectiveFunction{T, TPolicy}"/> - mandatory</item>
    ///   <item>linear constraints <see cref="LinearConstraintSet{T, TPolicy}"/> - mandatory</item>
    ///   <item>type of optimization: <see cref="GoalType"/> - optional, default: <see cref="GoalType.MINIMIZE"/></item>
    ///   <item>whether to allow negative values as solution: <see cref="NonNegativeConstraint"/> - optional, default: true</item>
    ///   <item>pivot selection rule: <see cref="PivotSelectionRule"/> - optional, default <see cref="PivotSelectionRule.DANTZIG"/></item>
    ///   <item>callback for the best solution: <see cref="SolutionCallback{T, TPolicy}"/> - optional</item>
    ///   <item>maximum number of iterations: <see cref="MaxIter"/> - optional, default: <see cref="int.MaxValue"/></item>
    /// </list>
    /// Note: Depending on the problem definition, the default convergence criteria
    /// may be too strict, resulting in <see cref="NoFeasibleSolutionException"/>
    /// In such a case it is advised to adjust these criteria with more appropriate values, e.g.relaxing the epsilon value.
    /// Default convergence criteria:
    /// <list>
    ///   <item>Algorithm convergence: 1e-6 </item>
    ///   <item>Floating - point comparisons: 10 ulp </item>
    ///   <item>Cut - Off value: 1e-10 </item>
    /// </list>
    /// The cut - off value has been introduced to handle the case of very small pivot elements
    /// in the Simplex tableau, as these may lead to numerical instabilities and degeneracy.
    /// Potential pivot elements smaller than this value will be treated as if they were zero
    /// and are thus not considered by the pivot selection mechanism. The default value is safe
    /// for many problems, but may need to be adjusted in case of very small coefficients
    /// used in either the <see cref="Barbar.SymbolicMath.Optimization.Linear.LinearConstraint{T, TPolicy}"/>
    /// or <see cref="Barbar.SymbolicMath.Optimization.Linear.LinearObjectiveFunction{T, TPolicy}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPolicy"></typeparam>
    public class SimplexSolver<T, TPolicy> : LinearOptimizer<T, TPolicy> where TPolicy : IPolicy<T>, new()
    {
        /// <summary>
        /// Default amount of error to accept in floating point comparisons (as ulps).
        /// </summary>
        public const int DEFAULT_ULPS = 10;
        /// <summary>
        ///  Default cut-off value.
        /// </summary>
        public const double DEFAULT_CUT_OFF_DOUBLE = 1e-10;
        /// <summary>
        /// Default amount of error to accept for algorithm convergence.
        /// </summary>
        public const double DEFAULT_EPSILON_DOUBLE = 1.0e-6;
        private T epsilon;
        private int maxUlps;
        private static readonly IPolicy<T> Policy = new TPolicy();
        private T cutOff;
        private PivotSelectionRule pivotSelection;
        private SolutionCallback<T, TPolicy> solutionCallback;

        /// <summary>
        /// Goal type
        /// </summary>
        public GoalType GoalType { get; private set; }

        /// <summary>
        /// Builds a simplex solver with a specified accepted amount of error.
        /// </summary>
        /// <param name="epsilon">Amount of error to accept for algorithm convergence.</param>
        /// <param name="maxUlps">Amount of error to accept in floating point comparisons.</param>
        /// <param name="cutOff">Values smaller than the cutOff are treated as zero.</param>
        /// <param name="goalType"></param>
        public SimplexSolver(T epsilon, int maxUlps, T cutOff, GoalType goalType)
        {
            this.epsilon = epsilon;
            this.maxUlps = maxUlps;
            this.cutOff = cutOff;
            this.pivotSelection = PivotSelectionRule.DANTZIG;
            GoalType = goalType;
        }

        /// <summary>
        /// <inheritdoc />
        /// In addition this method will register the following data:
        /// <list>
        ///  <item><see cref="SolutionCallback{T, TPolicy}"/></item>
        ///  <item><see cref="PivotSelectionRule"/></item>
        /// </list>
        /// </summary>
        /// <param name="optData"></param>
        protected override void ParseOptimizationData(params IOptimizationData[] optData)
        {
            // Allow base class to register its own data.
            base.ParseOptimizationData(optData);

            // reset the callback before parsing
            solutionCallback = null;

            foreach (var data in optData)
            {
                if (data is SolutionCallback<T, TPolicy>)
                {
                    solutionCallback = (SolutionCallback<T, TPolicy>)data;
                    continue;
                }
                if (data is PivotSelectionRule)
                {
                    pivotSelection = (PivotSelectionRule)data;
                    continue;
                }
                if (data is GoalType)
                {
                    GoalType = (GoalType)data;
                }
            }
        }

        /// <summary>
        /// Returns the column with the most negative coefficient in the objective function row.
        /// </summary>
        /// <param name="tableau"></param>
        /// <returns></returns>
        private int? GetPivotColumn(SimplexTableau<T, TPolicy> tableau)
        {
            T minValue = Policy.Zero();
            int? minPos = null;
            for (int i = tableau.NumObjectiveFunctions; i < tableau.Width - 1; i++)
            {
                T entry = tableau.GetEntry(0, i);
                // check if the entry is strictly smaller than the current minimum
                // do not use a ulp/epsilon check
                if (Policy.IsBelowZero(Policy.Sub(entry, minValue)))
                {
                    minValue = entry;
                    minPos = i;

                    // Bland's rule: chose the entering column with the lowest index
                    if (pivotSelection == PivotSelectionRule.BLAND && IsValidPivotColumn(tableau, i))
                    {
                        break;
                    }
                }
            }
            return minPos;
        }

        /// <summary>
        /// Checks whether the given column is valid pivot column, i.e. will result
        /// in a valid pivot row.
        /// When applying Bland's rule to select the pivot column, it may happen that
        /// there is no corresponding pivot row. This method will check if the selected
        /// pivot column will return a valid pivot row.
        /// </summary>
        /// <param name="tableau"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private bool IsValidPivotColumn(SimplexTableau<T, TPolicy> tableau, int col)
        {
            for (int i = tableau.NumObjectiveFunctions; i < tableau.Height; i++)
            {
                T entry = tableau.GetEntry(i, col);

                // do the same check as in getPivotRow
                if (Precision<T, TPolicy>.CompareTo(entry, Policy.Zero(), cutOff) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the row with the minimum ratio as given by the minimum ratio test (MRT).
        /// </summary>
        /// <param name="tableau"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private int? GetPivotRow(SimplexTableau<T, TPolicy> tableau, int col)
        {
            // create a list of all the rows that tie for the lowest score in the minimum ratio test
            List<int?> minRatioPositions = new List<int?>();
            T minRatio = default(T);
            bool minRationUnassigned = true;
            for (int i = tableau.NumObjectiveFunctions; i < tableau.Height; i++)
            {
                T rhs = tableau.GetEntry(i, tableau.Width - 1);
                T entry = tableau.GetEntry(i, col);

                // only consider pivot elements larger than the cutOff threshold
                // selecting others may lead to degeneracy or numerical instabilities
                if (Precision<T, TPolicy>.CompareTo(entry, Policy.Zero(), cutOff) > 0)
                {
                    T ratio = Policy.Abs(Policy.Div(rhs, entry));
                    // check if the entry is strictly equal to the current min ratio
                    // do not use a ulp/epsilon check
                    int cmp;
                    if (minRationUnassigned)
                    {
                        cmp = -1;
                    }
                    else
                    {
                        cmp = Policy.Compare(ratio, minRatio);
                    }
                    if (cmp == 0)
                    {
                        minRatioPositions.Add(i);
                    }
                    else if (cmp < 0)
                    {
                        minRatio = ratio;
                        minRationUnassigned = false;
                        minRatioPositions.Clear();
                        minRatioPositions.Add(i);
                    }
                }
            }

            if (minRatioPositions.Count == 0)
            {
                return null;
            }
            else if (minRatioPositions.Count > 1)
            {
                // there's a degeneracy as indicated by a tie in the minimum ratio test

                // 1. check if there's an artificial variable that can be forced out of the basis
                if (tableau.NumArtificialVariables > 0)
                {
                    foreach (int? row in minRatioPositions)
                    {
                        for (int i = 0; i < tableau.NumArtificialVariables; i++)
                        {
                            int column = i + tableau.ArtificialVariableOffset;
                            T entry = tableau.GetEntry(row.Value, column);
                            if (Precision<T, TPolicy>.Equals(entry, Policy.One(), epsilon) && row.Equals(tableau.GetBasicRow(column)))
                            {
                                return row;
                            }
                        }
                    }
                }

                // 2. apply Bland's rule to prevent cycling:
                //    take the row for which the corresponding basic variable has the smallest index
                //
                // see http://www.stanford.edu/class/msande310/blandrule.pdf
                // see http://en.wikipedia.org/wiki/Bland%27s_rule (not equivalent to the above paper)

                int? minRow = null;
                int minIndex = tableau.Width;
                foreach (int? row in minRatioPositions)
                {
                    int basicVar = tableau.GetBasicVariable(row.Value);
                    if (basicVar < minIndex)
                    {
                        minIndex = basicVar;
                        minRow = row;
                    }
                }
                return minRow;
            }
            return minRatioPositions[0];
        }

        /// <summary>
        /// Runs one iteration of the Simplex method on the given model.
        /// </summary>
        /// <param name="tableau"></param>
        protected void DoIteration(SimplexTableau<T, TPolicy> tableau)
        {

            IncrementIterationCount();

            int? pivotCol = GetPivotColumn(tableau);
            int? pivotRow = GetPivotRow(tableau, pivotCol.Value);
            if (pivotRow == null)
            {
                throw new UnboundedSolutionException();
            }

            tableau.PerformRowOperations(pivotCol.Value, pivotRow.Value);
        }

        /// <summary>
        /// Solves Phase 1 of the Simplex method.
        /// </summary>
        /// <param name="tableau"></param>
        protected void SolvePhase1(SimplexTableau<T, TPolicy> tableau)
        {

            // make sure we're in Phase 1
            if (tableau.NumArtificialVariables == 0)
            {
                return;
            }

            while (!tableau.IsOptimal())
            {
                DoIteration(tableau);
            }

            // if W is not zero then we have no feasible solution
            if (!Precision<T, TPolicy>.Equals(tableau.GetEntry(0, tableau.RhsOffset), Policy.Zero(), epsilon))
            {
                throw new NoFeasibleSolutionException();
            }
        }

        /// <inheritdoc />
        protected override PointValuePair<T> DoOptimize()
        {

            // reset the tableau to indicate a non-feasible solution in case
            // we do not pass phase 1 successfully
            if (solutionCallback != null)
            {
                solutionCallback.Tableau = null;
            }

            var tableau =
               new SimplexTableau<T, TPolicy>(Function,
                                  Constraints,
                                  GoalType,
                                  IsRestrictedToNonNegative,
                                  epsilon,
                                  maxUlps);

            SolvePhase1(tableau);
            tableau.DropPhase1Objective();

            // after phase 1, we are sure to have a feasible solution
            if (solutionCallback != null)
            {
                solutionCallback.Tableau = tableau;
            }

            while (!tableau.IsOptimal())
            {
                DoIteration(tableau);
            }

            // check that the solution respects the nonNegative restriction in case
            // the epsilon/cutOff values are too large for the actual linear problem
            // (e.g. with very small constraint coefficients), the solver might actually
            // find a non-valid solution (with negative coefficients).
            PointValuePair<T> solution = tableau.GetSolution();
            if (IsRestrictedToNonNegative)
            {
                T[] coeff = solution.Point;
                for (int i = 0; i < coeff.Length; i++)
                {
                    if (Precision<T, TPolicy>.CompareTo(coeff[i], Policy.Zero(), epsilon) < 0)
                    {
                        throw new NoFeasibleSolutionException();
                    }
                }
            }
            return solution;
        }
    }
}