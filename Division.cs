using System;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Represent division operation
    /// </summary>
    public class Division : BinaryOperation
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Division(SymMathNode a, SymMathNode b) : base(a, b)
        {
        }

        /// <summary>
        /// ctor. Term.Factory has to be set otherwise exception is thrown
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Division(long a, long b) : base(a, b)
        {
        }

        /// <summary>
        /// Simplify current expression by one step (if possible)
        /// </summary>
        /// <returns></returns>
        public override SymMathNode Simplify()
        {
            if (A.CanSimplify() || B.CanSimplify())
                return A.Simplify() / B.Simplify();


            var termB = B as Term;
            if (termB != null && termB.IsOne())
            {
                return A;
            }

            var mulA = A as Multiply;
            SymMathNode mulAOther = null;

            Term termN = A as Term;
            if (mulA != null)
            {
                if (mulA.A is Term && !(mulA.B is Term))
                {
                    termN = (Term)mulA.A;
                    mulAOther = mulA.B;
                }
                if (mulA.B is Term && !(mulA.A is Term))
                {
                    termN = (Term)mulA.B;
                    mulAOther = mulA.A;
                }
            }

            Term termD = B as Term;
            var mulB = B as Multiply;
            SymMathNode mulBOther = null;
            if (mulB != null)
            {
                if (mulB.A is Term && !(mulB.B is Term))
                {
                    termD = (Term)mulB.A;
                    mulBOther = mulB.B;
                }
                if (mulB.B is Term && !(mulB.A is Term))
                {
                    termD = (Term)mulB.B;
                    mulBOther = mulB.A;
                }
            }

            if (termN != null && termD != null)
            {
                var gcd = Term.Gcd(termN, termD);
                if (!gcd.IsOne())
                {
                    termN = (Term)(termN / gcd);
                    termD = (Term)(termD / gcd);
                    SymMathNode newA = termN;
                    if (mulAOther != null)
                    {
                        newA = new Multiply(termN, mulAOther);
                    }
                    SymMathNode newB = termD;
                    if (mulBOther != null)
                    {
                        newB = new Multiply(termD, mulBOther);
                    }
                    return new Division(newA, newB);
                }
            }

            var add = B as Add;
            if (add != null && (
                ((add.A.IsMinusOrNegativeTerm() && !add.B.IsMinusOrNegativeTerm()) || (add.B.IsMinusOrNegativeTerm() && !add.A.IsMinusOrNegativeTerm())) &&
               (add.A is SquareRoot || add.A.Descendands<Minus, SquareRoot>() || add.B is SquareRoot || add.B.Descendands<Minus, SquareRoot>())))
            {
                var aSquare = add.A * add.A;
                var bSquare = add.B * add.B;
                if (add.B.IsMinusOrNegativeTerm())
                {
                    return (A * (add.A - add.B)) / ((add.A * add.A) - (add.B * add.B));
                }
                return (A * (add.B - add.A)) / ((add.B * add.B) - (add.A * add.A));
            }


            if (A is Term && B.IfType<Division>(b => b.A is Term && b.B is Term))
            {
                return (A * ((Division)B).B) / ((Division)B).A;
            }

            return base.Simplify();
        }

        /// <summary>
        /// Simplify current expression by one step (if possible)
        /// </summary>
        /// <returns></returns>
        public override bool CanSimplify()
        {
            if (A.CanSimplify() || B.CanSimplify() || B.IfType<Term>(t => t.IsOne()))
            {
                return true;
            }

            var mulA = A as Multiply;

            Term termN = A as Term;
            if (mulA != null)
            {
                if (mulA.A is Term && !(mulA.B is Term))
                {
                    termN = (Term)mulA.A;
                }
                if (mulA.B is Term && !(mulA.A is Term))
                {
                    termN = (Term)mulA.B;
                }
            }

            Term termD = B as Term;
            var mulB = B as Multiply;
            if (mulB != null)
            {
                if (mulB.A is Term && !(mulB.B is Term))
                {
                    termD = (Term)mulB.A;
                }
                if (mulB.B is Term && !(mulB.A is Term))
                {
                    termD = (Term)mulB.B;
                }
            }

            var add = B as Add;
            return
              (termN != null && termD != null && !Term.Gcd(termN, termD).IsOne()) ||
              (add != null &&
              ((add.A.IsMinusOrNegativeTerm() && !add.B.IsMinusOrNegativeTerm()) || (add.B.IsMinusOrNegativeTerm() && !add.A.IsMinusOrNegativeTerm())) &&
               (add.A is SquareRoot || add.A.Descendands<Minus, SquareRoot>() || add.B is SquareRoot || add.B.Descendands<Minus, SquareRoot>())
              ) ||
              (A is Term && B.IfType<Division>(b => b.A is Term && b.B is Term));
        }

        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override BinaryOperation Clone(SymMathNode a, SymMathNode b)
        {
            return new Division(a, b);
        }

        /// <summary>
        /// Dump node to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0}/{1})", A, B);
        }

        /// <summary>
        /// Evaluates current expression tree and returns double
        /// Beware - this can lead to inaccuracies 
        /// </summary>
        /// <returns></returns>
        public override double Evaluate()
        {
            return A.Evaluate() / B.Evaluate();
        }

        /// <summary>
        /// Switch nominator and denominator
        /// </summary>
        /// <returns></returns>
        public Division Revert()
        {
            return new Division(B, A);
        }
    }
}