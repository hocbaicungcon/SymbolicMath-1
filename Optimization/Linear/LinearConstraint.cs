using Barbar.SymbolicMath.Policies;
using Barbar.SymbolicMath.Vectors;

namespace Barbar.SymbolicMath.Optimization.Linear
{
    /// <summary>
    /// A linear constraint for a linear optimization problem.
    /// A linear constraint has one of the forms:
    /// <list type="bullet">
    /// <item>c1x1 + ... c nx n = v </item>
    /// <item>c1x1 + ... c nx n &lt;= v</item>
    /// <item>c1x1 + ... c nx n >= v</item>
    /// <item>l1x1 + ... l nx n + l cst = r1x1 + ...r nx n + r cst</item>
    /// <item>l1x1 + ... l nx n + l cst &lt;= r1x1 + ... r nx n + r cst</item>
    /// <item>l1x1 + ... l nx n + l cst >= r1x1 + ... r nx n + r cst</item>    
    /// </list>
    /// The ci, li or ri are the coefficients of the constraints, the xi
    /// are the coordinates of the current point and v is the value of the constraint.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPolicy"></typeparam>
    public class LinearConstraint<T, TPolicy> where TPolicy : IPolicy<T>, new()
    {
        /// <summary>
        /// Policy - usually one of <see cref="DoublePolicy"/> or <see cref="RationalPolicy{T, TPolicy}"/>
        /// </summary>
        public static IPolicy<T> Policy = new TPolicy();

        /// <summary>
        /// Build a constraint involving a single linear equation.
        /// </summary>
        /// <param name="coefficients">The coefficients of the constraint (left hand side)</param>
        /// <param name="relationship">The type of (in)equality used in the constraint</param>
        /// <param name="value">The value of the constraint (right hand side)</param>
        public LinearConstraint(T[] coefficients,
                                 Relationship relationship,
                                 T value) : this(new Vector<T, TPolicy>(coefficients), relationship, value)
        {
        }

        /// <summary>
        /// Build a constraint involving a single linear equation.
        /// </summary>
        /// <param name="coefficients">The coefficients of the constraint (left hand side)</param>
        /// <param name="relationship">The type of (in)equality used in the constraint</param>
        /// <param name="value">The value of the constraint (right hand side)</param>
        public LinearConstraint(Vector<T, TPolicy> coefficients,
                                 Relationship relationship,
                                 T value)
        {
            Coefficients = coefficients;
            Relationship = relationship;
            Value = value;
        }

        /// <summary>
        /// Build a constraint involving two linear equations.
        /// </summary>
        /// <param name="lhsCoefficients">The coefficients of the linear expression on the left hand side of the constraint</param>
        /// <param name="lhsConstant">The constant term of the linear expression on the left hand side of the constraint</param>
        /// <param name="relationship">The type of (in)equality used in the constraint</param>
        /// <param name="rhsCoefficients">The coefficients of the linear expression on the right hand side of the constraint</param>
        /// <param name="rhsConstant">The constant term of the linear expression on the right hand side of the constraint</param>
        public LinearConstraint(T[] lhsCoefficients, T lhsConstant,
                                 Relationship relationship,
                                 T[] rhsCoefficients, T rhsConstant)
        {
            T[] sub = new T[lhsCoefficients.Length];
            for (int i = 0; i < sub.Length; ++i)
            {
                sub[i] = Policy.Sub(lhsCoefficients[i], rhsCoefficients[i]);
            }
            Coefficients = new Vector<T, TPolicy>(sub, false);
            Relationship = relationship;
            Value = Policy.Sub(rhsConstant, lhsConstant);
        }

        /// <summary>
        /// Build a constraint involving two linear equations.
        /// </summary>
        /// <param name="lhsCoefficients">The coefficients of the linear expression on the left hand side of the constraint</param>
        /// <param name="lhsConstant">The constant term of the linear expression on the left hand side of the constraint</param>
        /// <param name="relationship">The type of (in)equality used in the constraint</param>
        /// <param name="rhsCoefficients">The coefficients of the linear expression on the right hand side of the constraint</param>
        /// <param name="rhsConstant">The constant term of the linear expression on the right hand side of the constraint</param>
        public LinearConstraint(Vector<T, TPolicy> lhsCoefficients, T lhsConstant,
                                 Relationship relationship,
                                 Vector<T, TPolicy> rhsCoefficients, T rhsConstant)
        {
            Coefficients = lhsCoefficients - rhsCoefficients;
            Relationship = relationship;
            Value = Policy.Sub(rhsConstant, lhsConstant);
        }

        /// <summary>
        /// Gets the coefficients of the constraint (left hand side).
        /// </summary>
        public Vector<T, TPolicy> Coefficients { get; private set; }

        /// <summary>
        ///  Gets the relationship between left and right hand sides.
        /// </summary>
        public Relationship Relationship { get; private set; }

        /// <summary>
        /// Gets the value of the constraint (right hand side).
        /// </summary>
        public T Value { get; private set; }

        /// <inheritDoc/>
        public override bool Equals(object other)
        {
            if (this == other)
            {
                return true;
            }
            if (other is LinearConstraint<T, TPolicy>)
            {
                var rhs = (LinearConstraint<T, TPolicy>)other;
                return Relationship == rhs.Relationship && Policy.Equals(Value, rhs.Value) &&
                    Coefficients.Equals(rhs.Coefficients);
            }
            return false;
        }

        /// <inheritDoc/>
        public override int GetHashCode()
        {
            return Relationship.GetHashCode() ^ Value.GetHashCode() ^ Coefficients.GetHashCode();
        }
    }
}