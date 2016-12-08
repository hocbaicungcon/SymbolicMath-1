using System;
using System.Text;
using System.Xml;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represents A^B operation
    /// </summary>
    public class Power : BinaryOperation
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Power(SymMathNode a, SymMathNode b) : base(a, b)
        {
        }

        /// <inheritdoc />
        public override double Evaluate()
        {
            return Math.Pow(A.Evaluate(), B.Evaluate());
        }

        /// <inheritdoc />
        public override BinaryOperation Clone(SymMathNode a, SymMathNode b)
        {
            return new Power(a, b);
        }

        /// <summary>
        /// Dump node to MathML
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="parent"></param>
        public override void ToMathML(XmlWriter writer, SymMathNode parent)
        {
            writer.WriteStartElement("msup");
            writer.WriteStartElement("mrow");
            A.ToMathML(writer, this);
            writer.WriteEndElement();
            writer.WriteStartElement("mrow");
            B.ToMathML(writer, this);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="parent"></param>
        public override void ToString(StringBuilder builder, SymMathNode parent)
        {
            A.ToString(builder, this);
            builder.Append("^");
            B.ToString(builder, this);
        }
    }
}
