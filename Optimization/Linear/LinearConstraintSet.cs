using Barbar.SymbolicMath.Policies;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barbar.SymbolicMath.Optimization.Linear
{
    /// <summary>
    /// Class that represents a set of <see cref="LinearConstraint{T, TPolicy}"/> 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPolicy"></typeparam>
    public class LinearConstraintSet<T, TPolicy> : IOptimizationData where TPolicy : IPolicy<T>, new()
    {
        private ISet<LinearConstraint<T, TPolicy>> linearConstraints = new HashSet<LinearConstraint<T, TPolicy>>();

        /// <summary>
        /// Creates a set containing the given constraints.
        /// </summary>
        /// <param name="constraints">Constraints.</param>
        public LinearConstraintSet(params LinearConstraint<T, TPolicy>[] constraints)
        {
            foreach (var c in constraints)
            {
                linearConstraints.Add(c);
            }
        }

        /// <summary>
        /// Creates a set containing the given constraints.
        /// </summary>
        /// <param name="constraints">Constraints.</param>
        public LinearConstraintSet(Collection<LinearConstraint<T, TPolicy>> constraints)
        {
            foreach (var c in constraints)
            {
                linearConstraints.Add(c);
            }
        }

        /// <summary>
        /// Gets the set of linear constraints.
        /// </summary>
        public ICollection<LinearConstraint<T, TPolicy>> Constraints { get { return new ReadOnlyCollection<LinearConstraint<T, TPolicy>>(linearConstraints.ToList()); } }
    }
}