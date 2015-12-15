using Barbar.SymbolicMath.Policies;
using Barbar.SymbolicMath.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Implements term with long as background storage
    /// Do not mix TermInt64 with TermBigInteger - choose one type and use it
    /// </summary>
    public class ConstantInt64 : Constant
    {
        private long m_Value;
        /// <summary>
        /// Factory for constructing terms (will return TermInt64)
        /// </summary>
        public static readonly new IConstantFactory Factory = new TermInt64Factory();

        private class TermInt64Factory : IConstantFactory
        {
            public Constant Create(long value)
            {
                return new ConstantInt64(value);
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="value"></param>
        public ConstantInt64(long value)
        {
            m_Value = value;
        }

        /// <summary>
        /// Returns value as Int64, this is guaranteed not to overflow
        /// </summary>
        /// <returns></returns>
        public override long AsInt64()
        {
            return m_Value;
        }

        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <returns></returns>
        public override SymMathNode Clone()
        {
            return new ConstantInt64(m_Value);
        }

        private static long Evaluate(Constant term)
        {
            if (term == null)
            {
                throw new ArgumentNullException("term");
            }
            var other64 = term as ConstantInt64;
            if (other64 == null)
            {
                throw new ArgumentException("Can only evaluate TermInt64", "term");
            }
            return other64.m_Value;
        }

        /// <summary>
        /// Compares term to another term
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override int CompareTo(Constant other)
        {
            return Comparer<long>.Default.Compare(m_Value, Evaluate(other));
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
            return new ConstantInt64(Int64Policy.Instance.Gcd(m_Value, Evaluate(other)));
        }

        /// <summary>
        /// True if term is one
        /// </summary>
        /// <returns></returns>
        public override bool IsOne()
        {
            return m_Value == 1;
        }

        /// <summary>
        /// True if term is zero
        /// </summary>
        /// <returns></returns>
        public override bool IsZero()
        {
            return m_Value == 0;
        }

        /// <summary>
        /// Return value equal to 0 - term
        /// </summary>
        /// <returns></returns>
        public override Constant Negate()
        {
            return new ConstantInt64(-m_Value);
        }

        /// <summary>
        /// Returns square root
        /// </summary>
        /// <returns></returns>
        public override Constant Sqrt()
        {
            return new ConstantInt64((long)Math.Sqrt((double)m_Value));
        }

        /// <summary>
        /// Returns true if this tree is equal to node tree
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override bool Equals(SymMathNode node)
        {
            var term = node as ConstantInt64;
            if (term != null)
            {
                return m_Value == term.m_Value;
            }
            return false;
        }

        /// <summary>
        /// Returns this + other
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected override Constant AddTerm(Constant other)
        {
            return new ConstantInt64(m_Value + Evaluate(other));
        }

        /// <summary>
        /// Returns this / other
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected override Constant DivTerm(Constant other)
        {
            return new ConstantInt64(m_Value / Evaluate(other));
        }

        /// <summary>
        /// Returns this % other
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected override Constant ModTerm(Constant other)
        {
            return new ConstantInt64(m_Value % Evaluate(other));
        }

        /// <summary>
        /// Returns this * other
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected override Constant MultiplyTerm(Constant other)
        {
            return new ConstantInt64(m_Value * Evaluate(other));
        }

        /// <summary>
        /// True if this is equal to obj
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var term = obj as ConstantInt64;
            if (term != null)
            {
                return term.m_Value == m_Value;
            }
            if (obj is long)
            {
                return m_Value == (long)obj;
            }
            if (obj is int)
            {
                return m_Value == (int)obj;
            }
            return false;
        }

        /// <summary>
        /// Returns hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)m_Value;
        }

        /// <summary>
        /// True if value is below zero
        /// </summary>
        /// <returns></returns>
        public override bool IsNegative()
        {
            return m_Value < 0;
        }

        /// <summary>
        /// Dump node to MathML
        /// </summary>
        /// <param name="writer"></param>
        public override void ToMathML(XmlWriter writer, SymMathNode parent)
        {
            if (m_Value < 0)
            {
                writer.WriteElementString("mo", "-");
                writer.WriteElementString("mn", Convert.ToString(-m_Value));
            }
            else
            {
                writer.WriteElementString("mn", Convert.ToString(m_Value));
            }
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="parent"></param>
        public override void ToString(StringBuilder builder, SymMathNode parent)
        {
            builder.Append(m_Value);
        }
    }
}
