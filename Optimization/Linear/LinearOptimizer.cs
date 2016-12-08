using Barbar.SymbolicMath.Policies;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barbar.SymbolicMath.Optimization.Linear
{
    /// <summary>
    /// Base class for implementing linear optimizers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPolicy"></typeparam>
    public abstract class LinearOptimizer<T, TPolicy> : BaseOptimizer<PointValuePair<T>> where TPolicy : IPolicy<T>, new()
    {
        private ICollection<LinearConstraint<T, TPolicy>> linearConstraints;

        /// <summary>
        /// true if the variables are restricted to non-negative values.
        /// </summary>
        protected bool IsRestrictedToNonNegative { get; private set; }

        /// <value>
        /// the optimization function
        /// </value>
        protected LinearObjectiveFunction<T, TPolicy> Function { get; private set; }


        /// <summary>
        /// the optimization constraints
        /// </summary>
        protected ICollection<LinearConstraint<T, TPolicy>> Constraints
        {
            get
            {
                return new ReadOnlyCollection<LinearConstraint<T, TPolicy>>(linearConstraints.ToList());
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public LinearOptimizer() : base(null)
        {
        }

        /// <summary>
        /// Scans the list of (required and optional) optimization data that
        /// characterize the problem.
        /// </summary>
        /// <param name="optData">
        /// The following data will be looked for:
        /// <list>
        ///   <item><see cref="LinearObjectiveFunction{T, TPolicy}"/></item>
        ///   <item><see cref="LinearConstraintSet{T, TPolicy}"/></item>
        ///   <item><see cref="NonNegativeConstraint" /></item>
        /// </list>
        /// </param>
        protected override void ParseOptimizationData(params IOptimizationData[] optData)
        {
            // Allow base class to register its own data.
            base.ParseOptimizationData(optData);

            // The existing values (as set by the previous call) are reused if
            // not provided in the argument list.
            foreach (var data in optData)
            {
                if (data is LinearObjectiveFunction<T, TPolicy>)
                {
                    Function = (LinearObjectiveFunction<T, TPolicy>)data;
                    continue;
                }
                if (data is LinearConstraintSet<T, TPolicy>)
                {
                    linearConstraints = ((LinearConstraintSet<T, TPolicy>)data).Constraints;
                    continue;
                }
                if (data is NonNegativeConstraint)
                {
                    IsRestrictedToNonNegative = ((NonNegativeConstraint)data).IsRestrictedToNonNegative;
                    continue;
                }
            }
        }
    }
}