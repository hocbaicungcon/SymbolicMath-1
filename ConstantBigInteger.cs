using System;
using System.Numerics;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Implements term with BigInteger as background storage
    /// Do not mix TermInt64 with TermBigInteger - choose one type and use it
    /// </summary>
    public class ConstantBigInteger : Constant
    {
        private BigInteger m_Value;
        /// <summary>
        /// Factory for constructing terms (will return TermBigInteger)
        /// </summary>
        public static readonly new IConstantFactory Factory = new TermBigIntegerFactory();

        private class TermBigIntegerFactory : IConstantFactory
        {
            public Constant Create(long value)
            {
                return new ConstantBigInteger(value);
            }
        }

        private static BigInteger Evaluate(Constant term)
        {
            if (term == null)
            {
                throw new ArgumentNullException("term");
            }
            var otherBig = term as ConstantBigInteger;
            if (otherBig == null)
            {
                throw new ArgumentException("Can only evaluate TermBigInteger", "term");
            }
            return otherBig.m_Value;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="value"></param>
        public ConstantBigInteger(long value)
        {
            m_Value = value;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="value"></param>
        public ConstantBigInteger(BigInteger value)
        {
            m_Value = value;
        }

        /// <summary>
        /// Returns value as Int64, beware - this can overflow for large values
        /// </summary>
        /// <returns></returns>
        public override long AsInt64()
        {
            return (long)m_Value;
        }

        /// <summary>
        /// Clon
        /// </summary>
        /// <returns></returns>
        public override SymMathNode Clone()
        {
            return new ConstantBigInteger(m_Value);
        }

        /// <summary>
        /// Compares term to another term
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override int CompareTo(Constant other)
        {
            return BigInteger.Compare(m_Value, Evaluate(other));
        }

        /// <summary>
        /// Evaluates current expression tree and returns double
        /// Beware - this can lead to inaccuracies 
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            return (double)m_Value;
        }

        /// <summary>
        /// Computes greatest common divisor
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override Constant Gcd(Constant other)
        {
            return new ConstantBigInteger(BigInteger.GreatestCommonDivisor(m_Value, Evaluate(other)));
        }

        /// <summary>
        /// True if term is one
        /// </summary>
        /// <returns></returns>
        public override bool IsOne()
        {
            return m_Value.IsOne;
        }

        /// <summary>
        /// True if term is zero
        /// </summary>
        /// <returns></returns>
        public override bool IsZero()
        {
            return m_Value.IsZero;
        }

        /// <summary>
        /// Return value equal to 0 - term
        /// </summary>
        /// <returns></returns>
        public override Constant Negate()
        {
            return new ConstantBigInteger(-m_Value);
        }

        /// <summary>
        ///  Returns square root
        /// </summary>
        /// <returns></returns>
        public override Constant Sqrt()
        {
            // TODO: proper implementation - i.e. Newton method
            return new ConstantBigInteger((BigInteger)Math.Sqrt((double)m_Value));
        }

        /// <summary>
        /// Returns true if this tree is equal to node tree
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override bool Equals(SymMathNode node)
        {
            var term = node as ConstantBigInteger;
            if (term != null)
            {
                return m_Value == term.m_Value;
            }
            return false;
        }

        /// <summary>
        /// Returns this + b
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Constant AddTerm(Constant b)
        {
            return new ConstantBigInteger(m_Value + Evaluate(b));
        }

        /// <summary>
        /// Returns this / b
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Constant DivTerm(Constant b)
        {
            return new ConstantBigInteger(m_Value / Evaluate(b));
        }

        /// <summary>
        /// Returns this % b
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Constant ModTerm(Constant b)
        {
            return new ConstantBigInteger(m_Value % Evaluate(b));
        }

        /// <summary>
        /// Returns this * b
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Constant MultiplyTerm(Constant b)
        {
            return new ConstantBigInteger(m_Value * Evaluate(b));
        }

        /// <summary>
        /// True if expression is equal to obj
        /// </summary>
        /// <param name="obj">TermBigInteger</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var term = obj as ConstantBigInteger;
            if (term != null)
            {
                return term.m_Value == m_Value;
            }
            return false;
        }

        /// <summary>
        /// Computes hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_Value.GetHashCode();
        }

        /// <summary>
        /// True if value is below zero
        /// </summary>
        /// <returns></returns>
        public override bool IsNegative()
        {
            return m_Value < BigInteger.Zero;
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_Value.ToString();
        }
    }
}
