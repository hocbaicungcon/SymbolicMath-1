using System;

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
        /// Dump node to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Sqrt({0})", A);
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
    }
}