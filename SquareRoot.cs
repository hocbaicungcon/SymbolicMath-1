using System;
using System.Text;
using System.Xml;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represent square root
    /// </summary>
    public class SquareRoot : UnaryOperation
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="a"></param>
        public SquareRoot(SymMathNode a) : base(a)
        {
        }

        /// <summary>
        /// ctor. Term.Factory has to be set otherwise exception is thrown
        /// </summary>
        /// <param name="a"></param>
        public SquareRoot(long a) : base(a)
        {
        }

        /// <summary>
        /// Evaluates current expression tree and returns double
        /// Beware - this can lead to inaccuracies 
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            return Math.Sqrt(A.Evaluate());
        }

        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public override UnaryOperation Clone(SymMathNode a)
        {
            return new SquareRoot(a);
        }

        /// <summary>
        /// Dump node to MathML
        /// </summary>
        /// <param name="writer"></param>
        public override void ToMathML(XmlWriter writer, SymMathNode parent)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            writer.WriteStartElement("msqrt");
            A.ToMathML(writer);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="parent"></param>
        public override void ToString(StringBuilder builder, SymMathNode parent)
        {
            builder.Append("Sqrt(");
            A.ToString(builder, this);
            builder.Append(")");
        }
    }
}