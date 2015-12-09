using System;

namespace Barbar.SymbolicMath
{
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

        public override double Evaluate()
        {
            return Math.Pow(A.Evaluate(), B.Evaluate());
        }

        protected override BinaryOperation Clone(SymMathNode a, SymMathNode b)
        {
            return new Power(a, b);
        }
    }
}
