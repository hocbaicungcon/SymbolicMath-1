using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Base abstract class for symbolic mathematic
    /// </summary>
    public abstract class SymMathNode : IEquatable<SymMathNode>
    {
        private static ITransformationRule[] s_Rules = new ITransformationRule[] { };

        /// <summary>
        /// Return list of possible simplifications
        /// </summary>
        /// <returns></returns>
        public virtual IList<ITransformationRule> GetSimplificationRules()
        {
            return s_Rules;
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            ToString(builder, null);
            return builder.ToString();
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="parent"></param>
        public abstract void ToString(StringBuilder builder, SymMathNode parent);

        /// <summary>
        /// Dump node to MathML
        /// </summary>
        /// <param name="writer"></param>
        public void ToMathML(XmlWriter writer)
        {
            ToMathML(writer, null);
        }

        /// <summary>
        /// Dump node to MathML
        /// </summary>
        /// <param name="writer"></param>
        public abstract void ToMathML(XmlWriter writer, SymMathNode parent);

        /// <summary>
        /// Evaluate expression
        /// </summary>
        /// <returns></returns>
        public abstract double Evaluate();
        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <returns></returns>
        public abstract SymMathNode Clone();
        /// <summary>
        /// Returns true if node equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool Equals(SymMathNode other);
        /// <summary>
        /// Returns new Add(a,b)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Add operator +(SymMathNode a, SymMathNode b)
        {
            return new Add(a, b);
        }
        /// <summary>
        /// Returns new Multiply(a,b)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Multiply operator *(SymMathNode a, SymMathNode b)
        {
            return new Multiply(a, b);
        }

        /// <summary>
        /// Returns new Division(a,b)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Division operator /(SymMathNode a, SymMathNode b)
        {
            return new Division(a, b);
        }

        /// <summary>
        /// Returns new Add(a,new Minus(b))
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Add operator -(SymMathNode a, SymMathNode b)
        {
            return new Add(a, new Minus(b));
        }

        
    }
}