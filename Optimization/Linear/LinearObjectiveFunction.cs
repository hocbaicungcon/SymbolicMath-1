using Barbar.SymbolicMath.Policies;
using Barbar.SymbolicMath.Vectors;

namespace Barbar.SymbolicMath.Optimization.Linear
{
    /// <summary>
    /// An objective function for a linear optimization problem.
    /// A linear objective function has one the form:
    /// c1x1 + ... cnxn + d
    /// The ci and d are the coefficients of the equation,
    /// the xi are the coordinates of the current point.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPolicy"></typeparam>
    public class LinearObjectiveFunction<T, TPolicy> : IOptimizationData where TPolicy : IPolicy<T>, new()
    {
        private static readonly IPolicy<T> Policy = new TPolicy();

        /// <param name="coefficients">Coefficients for the linear equation being optimized.</param>
        /// <param name="constantTerm">Constant term of the linear equation.</param>
        public LinearObjectiveFunction(T[] coefficients, T constantTerm) : this(new Vector<T, TPolicy>(coefficients), constantTerm)
        {
        }

        /// <param name="coefficients">Coefficients for the linear equation being optimized.</param>
        /// <param name="constantTerm">Constant term of the linear equation.</param>
        public LinearObjectiveFunction(Vector<T, TPolicy> coefficients, T constantTerm)
        {
            Coefficients = coefficients;
            ConstantTerm = constantTerm;
        }

        /// <summary>
        /// Gets the coefficients of the linear equation being optimized.
        /// </summary>
        public Vector<T, TPolicy> Coefficients { get; private set; }

        /// <summary>
        /// Gets the constant of the linear equation being optimized.
        /// </summary>
        public T ConstantTerm { get; private set; }

        /// <summary>
        /// Computes the value of the linear equation at the current point.
        /// </summary>
        /// <param name="point">Point at which linear equation must be evaluated.</param>
        /// <returns>the value of the linear equation at the current point.</returns>
        public T Value(T[] point)
        {
            return Value(new Vector<T, TPolicy>(point, false));
        }

        /// <summary>
        /// Computes the value of the linear equation at the current point.
        /// </summary>
        /// <param name="point">Point at which linear equation must be evaluated.</param>
        /// <returns>the value of the linear equation at the current point.</returns>
        public T Value(Vector<T, TPolicy> point)
        {
            return Policy.Add(Coefficients.DotProduct(point), ConstantTerm);
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if (this == other)
            {
                return true;
            }
            if (other is LinearObjectiveFunction<T, TPolicy>)
            {
                var rhs = (LinearObjectiveFunction<T, TPolicy>)other;
                return Policy.Equals(ConstantTerm, rhs.ConstantTerm) && Coefficients.Equals(rhs.Coefficients);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ConstantTerm.GetHashCode() ^ Coefficients.GetHashCode();
        }
    }
}