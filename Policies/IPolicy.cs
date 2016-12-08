using System.Collections.Generic;
using System.Numerics;

namespace Barbar.SymbolicMath.Policies
{
    /// <summary>
    /// Abstraction of numeric policy. For some algorithms double precision is not enough - so you need to use rationals instead.
    /// Also in some cases you're fine with <see cref="int.MaxValue"/> or <see cref="long.MaxValue" /> but for other cases
    /// you need arbitary integer size - so you need to switch to <see cref="BigInteger"/>
    /// The policy pattern is designed to abstract from such problems.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPolicy<T> : IComparer<T>
    {
        /// <summary>
        /// Greatest common divisor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Gcd(T a, T b);

        /// <summary>
        ///  Least common denominator 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Lcd(T a, T b);

        /// <summary>
        /// True if value is exactly one
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        bool IsOne(T a);

        /// <summary>
        /// True if value is zero
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        bool IsZero(T a);

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Div(T a, T b);

        /// <summary>
        /// True if below zero
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        bool IsBelowZero(T a);

        /// <summary>
        /// Return absolute value of a
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        T Abs(T a);

        /// <summary>
        /// Returns -a
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        T Negate(T a);
        /// <summary>
        /// Multiply
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Mul(T a, T b);

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Add(T a, T b);

        /// <summary>
        /// Subtract
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Sub(T a, T b);
        /// <summary>
        /// Return one
        /// </summary>
        /// <returns></returns>
        T One();
        /// <summary>
        /// Return zero
        /// </summary>
        /// <returns></returns>
        T Zero();

        /// <summary>
        /// Calculates square root
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        T Sqrt(T n);

        /// <summary>
        /// Equality test
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        bool Equals(T a, T b);
    }
}
