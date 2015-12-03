using Barbar.SymbolicMath.Policies;
using System;

namespace Barbar.SymbolicMath.Rationals
{
    public struct Rational<T,TPolicy> where TPolicy : IPolicy<T>, new()
    {
        private T n;
        private T d;
        private static readonly TPolicy Policy = new TPolicy();

        public T Numerator { get { return n; } }
        public T Denominator { get { return d; } }

        public Rational(T numerator, T denominator)
        {
            this.n = numerator;
            this.d = denominator;
        }

        public override string ToString()
        {
            if (Policy.IsOne(Denominator))
            {
                return Convert.ToString(Numerator);
            }
            return string.Format("{0}/{1}", Numerator, Denominator);
        }

        public Rational<T, TPolicy> Normalize()
        {
            if (Policy.IsOne(Denominator))
            {
                return this;
            }

             if (Policy.IsZero(Numerator))
            {
                return new Rational<T, TPolicy>(Policy.Zero(), Policy.One());
            }

            var n = Numerator;
            var d = Denominator;
            if ((Policy.IsBelowZero(n) && Policy.IsBelowZero(d)) || Policy.IsBelowZero(d))
            {
                n = Policy.Negate(n);
                d = Policy.Negate(d);
            }

            var gcd = Policy.Gcd(Policy.Abs(n), d);
            if (!Policy.IsOne(gcd))
            {
                n = Policy.Div(n, gcd);
                d = Policy.Div(d, gcd);
            }
            return new Rational<T, TPolicy>(n, d);
        }

        public static Rational<T, TPolicy> operator *(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            return new Rational<T, TPolicy>(Policy.Mul(a.Numerator, b.Numerator), Policy.Mul(a.Denominator, b.Denominator)).Normalize();
        }

        public static Rational<T, TPolicy> operator +(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            if (Policy.IsZero(a.Numerator))
                return b;
            if (Policy.IsZero(b.Numerator))
                return a;

            if (Policy.IsOne(a.Denominator) && Policy.IsOne(b.Denominator))
            {
                return new Rational<T, TPolicy>(Policy.Add(a.Numerator, b.Numerator), a.Denominator);
            }

            var d = Policy.Lcd(a.Denominator, b.Denominator);
            var an = Policy.Mul(a.Numerator, (Policy.Div(d, a.Denominator)));
            var bn = Policy.Mul(b.Numerator, (Policy.Div(d, b.Denominator)));
            return new Rational<T, TPolicy>(Policy.Add(an, bn), d).Normalize();
        }

        public static Rational<T, TPolicy> operator -(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            if (Policy.IsZero(a.Numerator))
                return b;
            if (Policy.IsZero(b.Numerator))
                return a;

            if (Policy.IsOne(a.Denominator) && Policy.IsOne(b.Denominator))
            {
                return new Rational<T, TPolicy>(Policy.Sub(a.Numerator, b.Numerator), a.Denominator);
            }

            var d = Policy.Lcd(a.Denominator, b.Denominator);
            var an = Policy.Mul(a.Numerator, (Policy.Div(d, a.Denominator)));
            var bn = Policy.Mul(b.Numerator, (Policy.Div(d, b.Denominator)));
            return new Rational<T, TPolicy>(Policy.Sub(an, bn), d).Normalize();
        }

        public static Rational<T, TPolicy> operator /(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            if (Policy.IsZero(a.Numerator))
                return a;
            if (Policy.IsZero(b.Numerator))
                throw new ArgumentException("Division by zero.", "b");

            var n = Policy.Mul(a.Numerator, b.Denominator);
            var d = Policy.Mul(a.Denominator, b.Numerator);

            return new Rational<T, TPolicy>(n, d).Normalize();
        }
    }
}
