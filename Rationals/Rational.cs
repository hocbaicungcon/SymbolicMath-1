using Barbar.SymbolicMath.Policies;
using System;

namespace Barbar.SymbolicMath.Rationals
{
    /// <summary>
    /// Represent rational number in form of fraction
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPolicy"></typeparam>
    public struct Rational<T,TPolicy> where TPolicy : IPolicy<T>, new()
    {
        private T m_Numerator;
        private T m_Denominator;

        /// <summary>
        /// Default singleton
        /// </summary>
        public static readonly IPolicy<T> Policy = new TPolicy();

        /// <summary>
        /// Numerator
        /// </summary>
        public T Numerator { get { return m_Numerator; } }
        /// <summary>
        /// Denominator
        /// </summary>
        public T Denominator { get { return m_Denominator; } }

        /// <summary>
        /// ctor, denominator will be set to one
        /// </summary>
        /// <param name="numerator"></param>
        public Rational(T numerator) : this(numerator, Policy.One())
        {
        }
        
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        public Rational(T numerator, T denominator)
        {
            m_Numerator = numerator;
            m_Denominator = denominator;
        }


        /// <summary>
        /// Convert fraction to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Policy.IsOne(Denominator))
            {
                return Convert.ToString(Numerator);
            }
            return string.Format("{0}/{1}", Numerator, Denominator);
        }

        /// <summary>
        /// True if numerator is zero
        /// </summary>
        /// <returns></returns>
        public bool IsZero()
        {
            return Policy.IsZero(Numerator);
        }

        /// <summary>
        /// True if denominator is zero
        /// </summary>
        /// <returns></returns>
        public bool IsNan()
        {
            return Policy.IsZero(Denominator);
        }

        /// <summary>
        /// Normalize according to folowing rules
        ///   a) if there is a minus sign it will remain only in numerator
        ///   b) if there is common divisor, both numerator and denominator are divided by this common divisor
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Multiply and normalize
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Rational<T, TPolicy> operator *(Rational<T, TPolicy> a, Rational<T, TPolicy> b)
        {
            return new Rational<T, TPolicy>(Policy.Mul(a.Numerator, b.Numerator), Policy.Mul(a.Denominator, b.Denominator)).Normalize();
        }

        /// <summary>
        /// Add and normalize
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Subtract and normalize
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Divide and normalize. Division by zero will throw an exception
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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
