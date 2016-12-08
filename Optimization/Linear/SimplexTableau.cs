using Barbar.SymbolicMath.Optimization.Utility;
using Barbar.SymbolicMath.Policies;
using Barbar.SymbolicMath.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Barbar.SymbolicMath.Optimization.Linear
{
    /// <summary>
    /// A tableau for use in the Simplex method.
    /// </summary>
    /// <example>
    /// Example:
    ///  W |  Z |  x1 |  x2 |  x- | s1 |  s2 |  a1 |  RHS
    /// ---------------------------------------------------
    /// -1    0    0     0     0     0     0     1     0   &lt;= phase 1 objective
    ///  0    1   -15   -10    0     0     0     0     0   &lt;= phase 2 objective
    ///  0    0    1     0     0     1     0     0     2   &lt;= constraint 1
    ///  0    0    0     1     0     0     1     0     3   &lt;= constraint 2
    ///  0    0    1     1     0     0     0     1     4   &lt;= constraint 3
    ///  
    /// W: Phase 1 objective function
    /// Z: Phase 2 objective function
    /// x1 &amp; x2: Decision variables
    /// x-: Extra decision variable to allow for negative values
    /// s1 &amp; s2: Slack/Surplus variables
    /// a1: Artificial variable
    /// RHS: Right hand side
    /// </example>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPolicy"></typeparam>
    public class SimplexTableau<T, TPolicy> where TPolicy : IPolicy<T>, new()
    {
        private static string NEGATIVE_VAR_COLUMN_LABEL = "x-";
        private LinearObjectiveFunction<T, TPolicy> f;
        private List<LinearConstraint<T, TPolicy>> constraints;
        private bool restrictToNonNegative;
        private List<string> columnLabels = new List<string>();
        private Matrix<T, TPolicy> tableau;
        private int numDecisionVariables;
        private int numSlackVariables;
        private int numArtificialVariables;
        private T epsilon;
        private int maxUlps;
        private int[] basicVariables;
        private int[] basicRows;
        private static readonly IPolicy<T> Policy = new TPolicy();

        /// <summary>
        /// Goal type
        /// </summary>
        public GoalType GoalType { get; private set; }

        /// <summary>
        /// Builds a tableau for a linear problem.
        /// </summary>
        /// <param name="f">Linear objective function.</param>
        /// <param name="constraints">Linear constraints.</param>
        /// <param name="goalType">Optimization goal</param>
        /// <param name="restrictToNonNegative">Whether to restrict the variables to non-negative values.</param>
        /// <param name="epsilon">Amount of error to accept when checking for optimality.</param>
        public SimplexTableau(LinearObjectiveFunction<T, TPolicy> f,
                        ICollection<LinearConstraint<T, TPolicy>> constraints,
                        GoalType goalType,
                        bool restrictToNonNegative,
                        T epsilon) : this(f, constraints, goalType, restrictToNonNegative, epsilon, SimplexSolver<T, TPolicy>.DEFAULT_ULPS)
        {
        }

        /// <summary>
        /// Build a tableau for a linear problem.
        /// </summary>
        /// <param name="f">linear objective function</param>
        /// <param name="constraints">linear constraints</param>
        /// <param name="goalType">type of optimization goal</param>
        /// <param name="restrictToNonNegative">whether to restrict the variables to non-negative values</param>
        /// <param name="epsilon">amount of error to accept when checking for optimality</param>
        /// <param name="maxUlps">amount of error to accept in floating point comparisons</param>
        public SimplexTableau(LinearObjectiveFunction<T, TPolicy> f,
                        ICollection<LinearConstraint<T, TPolicy>> constraints,
                        GoalType goalType,
                        bool restrictToNonNegative,
                        T epsilon,
                        int maxUlps)
        {
            CheckDimensions(f, constraints);
            this.f = f;
            this.constraints = NormalizeConstraints(constraints);
            this.restrictToNonNegative = restrictToNonNegative;
            this.epsilon = epsilon;
            this.maxUlps = maxUlps;
            this.numDecisionVariables = f.Coefficients.Count + (restrictToNonNegative ? 0 : 1);
            this.numSlackVariables = ConstraintTypeCounts(Relationship.LEQ) +
                                          ConstraintTypeCounts(Relationship.GEQ);
            this.numArtificialVariables = ConstraintTypeCounts(Relationship.EQ) +
                                          ConstraintTypeCounts(Relationship.GEQ);
            this.tableau = CreateTableau(goalType == GoalType.MAXIMIZE);
            // initialize the basic variables for phase 1:
            //   we know that only slack or artificial variables can be basic
            InitializeBasicVariables(SlackVariableOffset);
            InitializeColumnLabels();
            GoalType = goalType;
        }

       
        private void CheckDimensions(LinearObjectiveFunction<T, TPolicy> objectiveFunction,
                                      ICollection<LinearConstraint<T, TPolicy>> c)
        {
            int dimension = objectiveFunction.Coefficients.Count;
            foreach (LinearConstraint<T, TPolicy> constraint in c)
            {
                int constraintDimension = constraint.Coefficients.Count;
                if (constraintDimension != dimension)
                {
                    throw new DimensionMismatchException(constraintDimension, dimension);
                }
            }
        }

        /// <summary>
        /// Initialize the labels for the columns.
        /// </summary>
        protected void InitializeColumnLabels()
        {
            if (NumObjectiveFunctions == 2)
            {
                columnLabels.Add("W");
            }
            columnLabels.Add("Z");
            for (int i = 0; i < OriginalNumDecisionVariables; i++)
            {
                columnLabels.Add("x" + i);
            }
            if (!restrictToNonNegative)
            {
                columnLabels.Add(NEGATIVE_VAR_COLUMN_LABEL);
            }
            for (int i = 0; i < NumSlackVariables; i++)
            {
                columnLabels.Add("s" + i);
            }
            for (int i = 0; i < NumArtificialVariables; i++)
            {
                columnLabels.Add("a" + i);
            }
            columnLabels.Add("RHS");
        }

        /// <summary>
        /// Create the tableau by itself.
        /// </summary>
        /// <param name="maximize">if true, goal is to maximize the objective function</param>
        /// <returns>created tableau</returns>
        protected Matrix<T, TPolicy> CreateTableau(bool maximize)
        {

            // create a matrix of the correct size
            int width = numDecisionVariables + numSlackVariables +
            numArtificialVariables + NumObjectiveFunctions + 1; // + 1 is for RHS
            int height = constraints.Count + NumObjectiveFunctions;
            var matrix = new Matrix<T, TPolicy>(width, height);

            var minusOne = Policy.Sub(Policy.Zero(), Policy.One());

            // initialize the objective function rows
            if (NumObjectiveFunctions == 2)
            {
                matrix[0, 0] = minusOne;
            }

            int zIndex = (NumObjectiveFunctions == 1) ? 0 : 1;
            matrix[zIndex, zIndex] = maximize ? Policy.One() : minusOne;

            Vector<T, TPolicy> objectiveCoefficients = maximize ? f.Coefficients * minusOne : f.Coefficients;
            var rowData = matrix.GetRow(zIndex);
            CopyArray(objectiveCoefficients.Data, rowData);
            matrix.SetRow(zIndex, rowData);
            matrix[width - 1, zIndex] = maximize ? f.ConstantTerm : Policy.Mul(minusOne, f.ConstantTerm);

            if (!restrictToNonNegative)
            {
                matrix[SlackVariableOffset - 1, zIndex] = InvertedCoefficientSum(objectiveCoefficients);
            }

            // initialize the constraint rows
            int slackVar = 0;
            int artificialVar = 0;
            for (int i = 0; i < constraints.Count; i++)
            {
                var constraint = constraints[i];
                int row = NumObjectiveFunctions + i;

                rowData = matrix.GetRow(row);
                // decision variable coefficients
                CopyArray(constraint.Coefficients.Data, rowData);
                matrix.SetRow(row, rowData);
                // x-
                if (!restrictToNonNegative)
                {
                    matrix[SlackVariableOffset - 1, row] =
                                    InvertedCoefficientSum(constraint.Coefficients);
                }

                // RHS
                matrix[width - 1, row] = constraint.Value;

                // slack variables
                if (constraint.Relationship == Relationship.LEQ)
                {
                    matrix[SlackVariableOffset + slackVar++, row] = Policy.One();  // slack
                }
                else if (constraint.Relationship == Relationship.GEQ)
                {
                    matrix[SlackVariableOffset + slackVar++, row] = minusOne; // excess
                }

                // artificial variables
                if ((constraint.Relationship == Relationship.EQ) ||
                    (constraint.Relationship == Relationship.GEQ))
                {
                    matrix[ArtificialVariableOffset + artificialVar, 0] = Policy.One();
                    matrix[ArtificialVariableOffset + artificialVar++, row] = Policy.One();
                    matrix.SetRowVector(0, matrix.GetRowVector(0) - matrix.GetRowVector(row));
                }
            }

            return matrix;
        }

        /// <summary>
        /// Get new versions of the constraints which have positive right hand sides.
        /// </summary>
        /// <param name="originalConstraints">original (not normalized) constraints</param>
        /// <returns>new versions of the constraints</returns>

        public List<LinearConstraint<T, TPolicy>> NormalizeConstraints(ICollection<LinearConstraint<T, TPolicy>> originalConstraints)
        {
            var normalized = new List<LinearConstraint<T, TPolicy>>(originalConstraints.Count);
            foreach (var constraint in originalConstraints)
            {
                normalized.Add(Normalize(constraint));
            }
            return normalized;
        }

        /// <summary>
        /// Get a new equation equivalent to this one with a positive right hand side.
        /// </summary>
        /// <param name="constraint">reference constraint</param>
        /// <returns>new equation</returns>
        private LinearConstraint<T, TPolicy> Normalize(LinearConstraint<T, TPolicy> constraint)
        {
            if (LinearConstraint<T, TPolicy>.Policy.IsBelowZero(constraint.Value))
            {
                var minusOne = Policy.Sub(Policy.Zero(), Policy.One());
                return new LinearConstraint<T, TPolicy>(constraint.Coefficients * minusOne,
                                            constraint.Relationship.oppositeRelationship(),
                                            Policy.Mul(minusOne, constraint.Value));
            }
            return new LinearConstraint<T, TPolicy>(constraint.Coefficients,
                                        constraint.Relationship, constraint.Value);
        }


        /// <summary>
        /// Get the number of objective functions in this tableau.
        /// </summary>
        public int NumObjectiveFunctions { get { return this.numArtificialVariables > 0 ? 2 : 1; } }

        /// <summary>
        /// Get a count of constraints corresponding to a specified relationship.
        /// </summary>
        /// <param name="relationship">relationship to count</param>
        /// <returns>number of constraint with the specified relationship</returns>
        private int ConstraintTypeCounts(Relationship relationship)
        {
            int count = 0;
            foreach (LinearConstraint<T, TPolicy> constraint in constraints)
            {
                if (constraint.Relationship == relationship)
                {
                    ++count;
                }
            }
            return count;
        }

        /// <summary>
        /// Get the -1 times the sum of all coefficients in the given array.
        /// </summary>
        /// <param name="coefficients">coefficients to sum</param>
        /// <returns>the -1 times the sum of all coefficients in the given array.</returns>
        protected static T InvertedCoefficientSum(Vector<T, TPolicy> coefficients)
        {
            T sum = Policy.Zero();
            foreach (T coefficient in coefficients)
            {
                sum = Policy.Sub(sum, coefficient);
            }
            return sum;
        }

        /// <summary>
        /// Checks whether the given column is basic.
        /// </summary>
        /// <param name="col">index of the column to check</param>
        /// <returns>the row that the variable is basic in.  null if the column is not basic</returns>
        public int? GetBasicRow(int col)
        {
            int row = basicVariables[col];
            return row == -1 ? null : (int?)row;
        }

        /// <summary>
        /// Returns the variable that is basic in this row.
        /// </summary>
        /// <param name="row">the index of the row to check</param>
        /// <returns>the variable that is basic for this row.</returns>
        public int GetBasicVariable(int row)
        {
            return basicRows[row];
        }

        /// <summary>
        /// Initializes the basic variable / row mapping.
        /// </summary>
        /// <param name="startColumn">startColumn the column to start</param>
        private void InitializeBasicVariables(int startColumn)
        {
            basicVariables = new int[Width - 1];
            basicRows = new int[Height];

            for (var i = 0; i < basicVariables.Length; i++)
            {
                basicVariables[i] = -1;
            }

            for (int i = startColumn; i < Width - 1; i++)
            {
                int? row = FindBasicRow(i);
                if (row != null)
                {
                    basicVariables[i] = row.Value;
                    basicRows[row.Value] = i;
                }
            }
        }

        /// <summary>
        ///  Returns the row in which the given column is basic.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private int? FindBasicRow(int col)
        {
            int? row = null;
            for (int i = 0; i < Height; i++)
            {
                T entry = GetEntry(i, col);
                if (Policy.Equals(entry, Policy.One()) && (row == null))
                {
                    row = i;
                }
                else if (!Policy.Equals(entry, Policy.Zero()))
                {
                    return null;
                }
            }
            return row;
        }

        /// <summary>
        /// Removes the phase 1 objective function, positive cost non-artificial variables,
        /// and the non-basic artificial variables from this tableau.
        /// </summary>
        public void DropPhase1Objective()
        {
            if (NumObjectiveFunctions == 1)
            {
                return;
            }

            ISet<int?> columnsToDrop = new SortedSet<int?>();
            columnsToDrop.Add(0);

            // positive cost non-artificial variables
            for (int i = NumObjectiveFunctions; i < ArtificialVariableOffset; i++)
            {
                T entry = GetEntry(0, i);
                if (Precision<T, TPolicy>.CompareTo(entry, Policy.Zero(), epsilon) > 0)
                {
                    columnsToDrop.Add(i);
                }
            }

            // non-basic artificial variables
            for (int i = 0; i < NumArtificialVariables; i++)
            {
                int col = i + ArtificialVariableOffset;
                if (GetBasicRow(col) == null)
                {
                    columnsToDrop.Add(col);
                }
            }

            T[][] matrix = new T[Height - 1][];
            for (var i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new T[Width - columnsToDrop.Count];
            }

            for (int i = 1; i < Height; i++)
            {
                int col = 0;
                for (int j = 0; j < Width; j++)
                {
                    if (!columnsToDrop.Contains(j))
                    {
                        matrix[i - 1][col++] = GetEntry(i, j);
                    }
                }
            }

            // remove the columns in reverse order so the indices are correct
            int?[] drop = columnsToDrop.ToArray();
            for (int i = drop.Length - 1; i >= 0; i--)
            {
                columnLabels.RemoveAt((int)drop[i]);
            }

            this.tableau = Matrix<T, TPolicy>.CreateMatrixRowIndexFirst(matrix);
            this.numArtificialVariables = 0;
            // need to update the basic variable mappings as row/columns have been dropped
            InitializeBasicVariables(NumObjectiveFunctions);
        }

        private void CopyArray(T[] src, T[] dest)
        {
            //System.arraycopy(src, 0, dest, getNumObjectiveFunctions(), src.Length);
            Array.Copy(src, 0, dest, NumObjectiveFunctions, src.Length);

        }

        /// <summary>
        /// Returns whether the problem is at an optimal state.
        /// </summary>
        /// <returns></returns>
        public bool IsOptimal()
        {
            T[] objectiveFunctionRow = tableau.GetRow(0);
            int end = RhsOffset;
            for (int i = NumObjectiveFunctions; i < end; i++)
            {
                T entry = objectiveFunctionRow[i];
                if (Precision<T, TPolicy>.CompareTo(entry, Policy.Zero(), epsilon) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get the current solution.
        /// </summary>
        /// <returns></returns>
        public PointValuePair<T> GetSolution()
        {
            int negativeVarColumn = columnLabels.IndexOf(NEGATIVE_VAR_COLUMN_LABEL);
            int? negativeVarBasicRow = negativeVarColumn > 0 ? GetBasicRow(negativeVarColumn) : null;
            T mostNegative = negativeVarBasicRow == null ? Policy.Zero() : GetEntry(negativeVarBasicRow.Value, RhsOffset);

            var usedBasicRows = new HashSet<int?>();
            T[] coefficients = new T[OriginalNumDecisionVariables];
            for (int i = 0; i < coefficients.Length; i++)
            {
                int colIndex = columnLabels.IndexOf("x" + i);
                if (colIndex < 0)
                {
                    coefficients[i] = Policy.Zero();
                    continue;
                }
                int? basicRow = GetBasicRow(colIndex);
                if (basicRow != null && basicRow == 0)
                {
                    // if the basic row is found to be the objective function row
                    // set the coefficient to 0 -> this case handles unconstrained
                    // variables that are still part of the objective function
                    coefficients[i] = Policy.Zero();
                }
                else if (usedBasicRows.Contains(basicRow))
                {
                    // if multiple variables can take a given value
                    // then we choose the first and set the rest equal to 0
                    coefficients[i] = Policy.Sub(Policy.Zero(), (restrictToNonNegative ? Policy.Zero() : mostNegative));
                }
                else
                {
                    usedBasicRows.Add(basicRow);
                    coefficients[i] =
                        Policy.Sub((basicRow == null ? Policy.Zero() : GetEntry(basicRow.Value, RhsOffset)),
                        (restrictToNonNegative ? Policy.Zero() : mostNegative));
                }
            }
            return new PointValuePair<T>(coefficients, f.Value(coefficients));
        }

        /// <summary>
        /// Perform the row operations of the simplex algorithm with the selected pivot column and row.
        /// </summary>
        /// <param name="pivotCol">the pivot column</param>
        /// <param name="pivotRow">the pivot row</param>
        public void PerformRowOperations(int pivotCol, int pivotRow)
        {
            // set the pivot element to 1
            T pivotVal = GetEntry(pivotRow, pivotCol);
            DivideRow(pivotRow, pivotVal);

            // set the rest of the pivot column to 0
            for (int i = 0; i < Height; i++)
            {
                if (i != pivotRow)
                {
                    T multiplier = GetEntry(i, pivotCol);
                    if (!Policy.IsZero(multiplier))
                    {
                        SubtractRow(i, pivotRow, multiplier);
                    }
                }
            }

            // update the basic variable mappings
            int previousBasicVariable = GetBasicVariable(pivotRow);
            basicVariables[previousBasicVariable] = -1;
            basicVariables[pivotCol] = pivotRow;
            basicRows[pivotRow] = pivotCol;
        }

        /// <summary>
        /// Divides one row by a given divisor.
        /// After application of this operation, the following will hold:
        /// dividendRow = dividendRow / divisor
        /// </summary>
        /// <param name="dividendRowIndex">index of the row</param>
        /// <param name="divisor">value of the divisor</param>
        protected void DivideRow(int dividendRowIndex, T divisor)
        {
            T[] dividendRow = tableau.GetRow(dividendRowIndex);
            for (int j = 0; j < Width; j++)
            {
                dividendRow[j] = Policy.Div(dividendRow[j], divisor);
            }
            tableau.SetRow(dividendRowIndex, dividendRow);
        }

        /// <summary>
        /// Subtracts a multiple of one row from another.
        /// After application of this operation, the following will hold:
        /// minuendRow = minuendRow - multiple * subtrahendRow
        /// </summary>
        /// <param name="minuendRowIndex">row index</param>
        /// <param name="subtrahendRowIndex">row index</param>
        /// <param name="multiplier">multiplication factor</param>
        protected void SubtractRow(int minuendRowIndex, int subtrahendRowIndex, T multiplier)
        {
            T[] minuendRow = tableau.GetRow(minuendRowIndex);
            T[] subtrahendRow = tableau.GetRow(subtrahendRowIndex);
            for (int i = 0; i < Width; i++)
            {
                minuendRow[i] = Policy.Sub(minuendRow[i], Policy.Mul(subtrahendRow[i], multiplier));
            }
            tableau.SetRow(minuendRowIndex, minuendRow);
        }

        /// <summary>
        /// Get the width of the tableau.
        /// </summary>
        public int Width { get { return tableau.Width; } }

        /// <summary>
        /// Get the height of the tableau.
        /// </summary>
        public int Height { get { return tableau.Height; } }

        /// <summary>
        /// Get an entry of the tableau.
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="column">column index</param>
        /// <returns>entry at (row, column)</returns>
        public T GetEntry(int row, int column)
        {
            return tableau[column, row];
        }

        /// <summary>
        /// Get the offset of the first slack variable.
        /// </summary>
        protected int SlackVariableOffset { get { return NumObjectiveFunctions + numDecisionVariables; } }

        /// <summary>
        /// Get the offset of the first artificial variable.
        /// </summary>
        public int ArtificialVariableOffset
        {
            get { return NumObjectiveFunctions + numDecisionVariables + numSlackVariables; }
        }

        /// <summary>
        /// Get the offset of the right hand side.
        /// </summary>
        public int RhsOffset { get { return Width - 1; } }

        /// <summary>
        /// Get the number of decision variables.
        /// If variables are not restricted to positive values, this will include 1 extra decision variable to represent
        /// the absolute value of the most negative variable.
        /// </summary>
        protected int NumDecisionVariables { get { return numDecisionVariables; } }

        /// <summary>
        /// Get the original number of decision variables.
        /// </summary>
        protected int OriginalNumDecisionVariables { get { return f.Coefficients.Count; } }

        /// <summary>
        /// Get the number of slack variables.
        /// </summary>
        protected int NumSlackVariables { get { return numSlackVariables; } }


        /// <summary>
        /// Get the number of artificial variables.
        /// </summary>
        public int NumArtificialVariables { get { return numArtificialVariables; } }

        /// <inheritdoc />
        public override bool Equals(object other)
        {

            if (this == other)
            {
                return true;
            }

            if (other is SimplexTableau<T, TPolicy>)
            {
                var rhs = (SimplexTableau<T, TPolicy>)other;
                return (restrictToNonNegative == rhs.restrictToNonNegative) &&
                       (numDecisionVariables == rhs.numDecisionVariables) &&
                       (numSlackVariables == rhs.numSlackVariables) &&
                       (numArtificialVariables == rhs.numArtificialVariables) &&
                       Policy.Equals(epsilon, rhs.epsilon) &&
                       (maxUlps == rhs.maxUlps) &&
                       f.Equals(rhs.f) &&
                       constraints.Equals(rhs.constraints) &&
                       tableau.Equals(rhs.tableau);
            }
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return restrictToNonNegative.GetHashCode() ^
                   numDecisionVariables ^
                   numSlackVariables ^
                   numArtificialVariables ^
                   epsilon.GetHashCode() ^
                   maxUlps ^
                   f.GetHashCode() ^
                   constraints.GetHashCode() ^
                   tableau.GetHashCode();
        }
    }
}