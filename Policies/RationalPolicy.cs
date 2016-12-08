using Barbar.SymbolicMath.Rationals;
using System;

namespace Barbar.SymbolicMath.Policies
{
    /// <summary>
    /// Implements numeric policy for <see cref="Rational{T, TPolicy}" /> 
    /// </summary> 
    public sealed class RationalPolicy<T, TPolicy> : IPolicy<Rational<T, TPolicy>> where TPolicy : IPolicy<T>, new()
    {
        /// <inheritdoc />
        public Rational<T, TPolicy> Abs(Rational<T, TPolicy> a)
        {
            var policy = Rational<T, TPolicy>.Policy;
            return new Rational<T, TPolicy>(policy.Abs(a.Numerator), policy.Abs(a.Denominator));
        }

        /// <inheritdoc />
        public Rational<T, TPolicy> Add(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            return a + b;
        }

        /// <inheritdoc />
        public int Compare(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            var policy = Rational<T, TPolicy>.Policy;
            var na = a.Normalize();
            var nb = b.Normalize();
            
            if (policy.IsOne(na.Denominator) && policy.IsOne(nb.Denominator))
            {
                return policy.Compare(na.Numerator, nb.Numerator);
            }

            var d = policy.Lcd(na.Denominator, nb.Denominator);
            var an = policy.Mul(na.Numerator, policy.Div(d, na.Denominator));
            var bn = policy.Mul(nb.Numerator, policy.Div(d, nb.Denominator));

            return policy.Compare(an, bn);
        }

        /// <inheritdoc />
        public Rational<T, TPolicy> Div(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            return a / b;
        }

        /// <inheritdoc />
        public bool Equals(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            var na = a.Normalize();
            var nb = b.Normalize();
            var policy = Rational<T, TPolicy>.Policy;
            return policy.Equals(na.Numerator, nb.Numerator) && policy.Equals(na.Denominator, nb.Denominator);
        }

        /// <inheritdoc />
        public Rational<T, TPolicy> Gcd(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool IsBelowZero(Rational<T, TPolicy> a)
        {
            var policy = Rational<T, TPolicy>.Policy;
            return policy.IsBelowZero(a.Numerator) ^ policy.IsBelowZero(a.Denominator);
        }

        /// <inheritdoc />
        public bool IsOne(Rational<T, TPolicy> a)
        {
            var policy = Rational<T, TPolicy>.Policy;
            return !policy.IsZero(a.Denominator) && policy.Equals(a.Denominator, a.Numerator);
        }

        /// <inheritdoc />
        public bool IsZero(Rational<T, TPolicy> a)
        {
            var policy = Rational<T, TPolicy>.Policy;
            return policy.IsZero(a.Numerator) && !policy.IsZero(a.Denominator);
        }

        /// <inheritdoc />
        public Rational<T, TPolicy> Lcd(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Rational<T, TPolicy> Mul(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            return a * b;
        }

        /// <inheritdoc />
        public Rational<T, TPolicy> Negate(Rational<T, TPolicy> a)
        {
            var policy = Rational<T, TPolicy>.Policy;
            return new Rational<T, TPolicy>(policy.Negate(a.Numerator), a.Denominator);
        }

        /// <inheritdoc />
        public Rational<T, TPolicy> One()
        {
            var policy = Rational<T, TPolicy>.Policy;
            return new Rational<T, TPolicy>(policy.One(), policy.One());
        }

        /// <inheritdoc />
        public Rational<T, TPolicy> Sqrt(Rational<T, TPolicy> n)
        {
            var policy = Rational<T, TPolicy>.Policy;
            return new Rational<T, TPolicy>(policy.Sqrt(n.Numerator), policy.Sqrt(n.Denominator));
        }

        /// <inheritdoc />
        public Rational<T, TPolicy> Sub(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            return a - b;
        }

        /// <inheritdoc />
        public Rational<T, TPolicy> Zero()
        {
            var policy = Rational<T, TPolicy>.Policy;
            return new Rational<T, TPolicy>(policy.Zero(), policy.One());
        }
    }
}
