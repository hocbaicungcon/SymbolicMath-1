using System;
using System.Xml;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represent integer value. Class is abstact as can be implemented in two ways
    /// T:TermInt64 (limited to min/max value of Int64) and T:TermBigInteger (no limit)
    /// </summary>
    public abstract class Constant : SymMathNode, IComparable<Constant>
    {
        private static IConstantFactory s_Factory;

        /// <summary>
        /// Default factory for constructing terms. Initial value is null.
        /// Set it to either TermInt64 or TermBigInteger
        /// </summary>
        public static IConstantFactory Factory
        {
            get
            {
                if (s_Factory == null)
                {
                    throw new Exception("Factory is null. Please bootstrap the engine by setting the Factory to ConstantInt64.Factory (if you want to operate in long scope) or to ConstantBigInteger.Factory (if you want to operate in BigInteger scope)");
                }
                return s_Factory;
            }
            set { s_Factory = value; }
        }
        /// <summary>
        /// Returns square root
        /// </summary>
        /// <returns></returns>
        public abstract Constant Sqrt();

        /// <summary>
        /// True if term is zero
        /// </summary>
        /// <returns></returns>
        public abstract bool IsZero();

        /// <summary>
        /// Converts term to Int64
        /// </summary>
        /// <returns></returns>
        public abstract long AsInt64();

        /// <summary>
        /// True if term is one
        /// </summary>
        /// <returns></returns>
        public abstract bool IsOne();

        /// <summary>
        /// Return value equal to 0 - term
        /// </summary>
        /// <returns></returns>
        public abstract Constant Negate();

        /// <summary>
        /// Computes greatest common divisor
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public abstract Constant Gcd(Constant b);

        /// <summary>
        /// Computes greates common divisor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Constant Gcd(Constant a, Constant b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            return a.Gcd(b);
        }

        /// <summary>
        /// Returns this + b
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected abstract Constant AddTerm(Constant b);

        /// <summary>
        /// Returns this * b
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected abstract Constant MultiplyTerm(Constant b);

        /// <summary>
        /// Returns this % b
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected abstract Constant ModTerm(Constant b);

        /// <summary>
        /// Returns this / b
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected abstract Constant DivTerm(Constant b);

        /// <summary>
        /// Returns true if term is below zero
        /// </summary>
        /// <returns>True if term is below zero</returns>
        public abstract bool IsNegative();

        /// <summary>
        /// Compares term to another term
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract int CompareTo(Constant other);

        /// <summary>
        /// Throws an exception - override in child class
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            throw new NotImplementedException("Please override in child class.");
        }

        /// <summary>
        /// Always return 0 - do override in child class
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Returns a + b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Constant operator +(Constant a, Constant b)
        {
            return a.AddTerm(b);
        }

        /// <summary>
        /// Returns a * b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Constant operator *(Constant a, Constant b)
        {
            return a.MultiplyTerm(b);
        }

        /// <summary>
        /// Returns a % b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Constant operator %(Constant a, Constant b)
        {
            return a.ModTerm(b);
        }

        /// <summary>
        /// Returns a / b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SymMathNode operator /(Constant a, Constant b)
        {
            if ((a % b).IsZero())
            {
                return a.DivTerm(b);
            }
            return new Division(a, b);
        }

        /// <summary>
        /// Returns true if a &gt; b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(Constant a, Constant b)
        {
            return a.CompareTo(b) > 0;
        }
        
        /// <summary>
        /// Returns true if a &lt; b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <(Constant a, Constant b)
        {
            return a.CompareTo(b) < 0;
        }

        /// <summary>
        /// Returns true if a == b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator==(Constant a, Constant b)
        {
            if (object.ReferenceEquals(a, null))
            {
                return object.ReferenceEquals(b, null);
            }
            return a.Equals(b);
        }

        /// <summary>
        /// Returns true if a != b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Constant a, Constant b)
        {
            return !(a == b);
        }
    }
}