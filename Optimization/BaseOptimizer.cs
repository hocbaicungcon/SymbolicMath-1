using Barbar.SymbolicMath.Optimization.Utility;
using System;

namespace Barbar.SymbolicMath.Optimization
{
    /// <summary>
    /// Base class for implementing optimizers.
    /// It contains the boiler-plate code for counting the number of evaluations
    /// of the objective function and the number of iterations of the algorithm,
    /// and storing the convergence checker.
    /// It is not a "user" class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseOptimizer<T>
    {
        /// Evaluations counter
        protected readonly Incrementor evaluations;
        /// Iterations counter
        protected readonly Incrementor iterations;
        /// Convergence checker
        private readonly ConvergenceChecker<T> checker;


        /// <param name="checker">Convergence checker.</param>
        protected BaseOptimizer(ConvergenceChecker<T> checker) : this(checker, 0, int.MaxValue)
        {
        }

        /// <param name="checker">Convergence checker.</param>
        /// <param name="maxEval">Maximum number of objective function evaluations.</param>
        /// <param name="maxIter">Maximum number of algorithm iterations.</param>
        protected BaseOptimizer(ConvergenceChecker<T> checker, int maxEval, int maxIter)
        {
            this.checker = checker;

            evaluations = new Incrementor(maxEval, (e) => { throw new Exception(e.ToString()); });
            iterations = new Incrementor(maxIter, (e) => { throw new Exception(e.ToString()); });
        }

        /// <summary>
        /// Gets the maximal number of function evaluations.
        /// </summary>
        /// <value>return the maximal number of function evaluations</value>
        public int MaxEvaluations { get { return evaluations.MaximalCount; } }

        /// <summary>
        /// Gets the number of evaluations of the objective function.
        /// The number of evaluations corresponds to the last call to the
        /// Optimize method. It is 0 if the method has not been called yet.
        /// </summary>
        /// <value>the number of evaluations of the objective function.</value>
        public int Evaluations { get { return evaluations.Count; } }

        /// <summary>
        /// Gets the maximal number of iterations.
        /// </summary>
        public int MaxIterations { get { return iterations.MaximalCount; } }

        /// <summary>
        /// Gets the number of iterations performed by the algorithm.
        /// The number iterations corresponds to the last call to the
        /// Optimize method. It is 0 if the method has not been called yet.
        /// </summary>
        public int Iterations { get { return iterations.Count; } }

        /// <summary>
        /// Gets the convergence checker.
        /// </summary>
        public ConvergenceChecker<T> ConvergenceChecker { get { return checker; } }



        /// <summary>
        /// Stores data and performs the optimization.
        /// The list of parameters is open-ended so that sub-classes can extend it
        /// with arguments specific to their concrete implementations.
        /// When the method is called multiple times, instance data is overwritten
        /// only when actually present in the list of arguments: when not specified,
        /// data set in a previous call is retained(and thus is optional in
        /// subsequent calls).
        /// Important note: Subclasses must override
        /// <see cref="ParseOptimizationData"/> if they need to register
        /// their own options; but then, they must also call
        /// base.ParseOptimizationData(optData) within that method.
        /// </summary>
        /// <param name="optData">
        /// This method will register the following data:
        /// <list>
        /// <item><see cref="MaxEval"/></item>
        /// <item><see cref="MaxIter"/></item>
        /// </list>
        /// </param>
        /// <returns>a point/value pair that satisfies the convergence criteria.</returns>
        public T Optimize(params IOptimizationData[] optData)
        {
            // Parse options.
            ParseOptimizationData(optData);

            // Reset counters.
            evaluations.ResetCount();
            iterations.ResetCount();
            // Perform optimization.
            return DoOptimize();
        }

        /// <summary>
        /// Performs the optimization.
        /// </summary>
        /// <returns>a point/value pair that satisfies the convergence criteria.</returns>
        public T Optimize()
        {
            // Reset counters.
            evaluations.ResetCount();
            iterations.ResetCount();
            // Perform optimization.
            return DoOptimize();
        }

        /// <summary>
        /// Performs the bulk of the optimization algorithm.
        /// </summary>
        /// <returns>the point/value pair giving the optimal value of the objective function.</returns>
        protected abstract T DoOptimize();

        /// <summary>
        /// Increment the evaluation count.
        /// </summary>
        protected void IncrementEvaluationCount()
        {
            evaluations.IncrementCount();
        }

        /// <summary>
        /// Increment the iteration count.
        /// </summary>
        protected void IncrementIterationCount()
        {
            iterations.IncrementCount();
        }

        /// <summary>
        /// Scans the list of (required and optional) optimization data that characterize the problem.
        /// </summary>
        /// <param name="optData">
        /// Optimization data.
        /// The following data will be looked for:
        /// <list>
        ///   <item><see cref="MaxEval" /></item>
        ///   <item><see cref="MaxIter" /></item>
        /// </list>
        /// </param>
        protected virtual void ParseOptimizationData(params IOptimizationData[] optData)
        {
            // The existing values (as set by the previous call) are reused if
            // not provided in the argument list.
            foreach (var data in optData)
            {
                if (data is MaxEval)
                {
                    evaluations.MaximalCount = ((MaxEval)data).Value;
                    continue;
                }
                if (data is MaxIter)
                {
                    iterations.MaximalCount = ((MaxIter)data).Value;
                    continue;
                }
            }
        }
    }
}
