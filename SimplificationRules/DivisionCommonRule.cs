namespace Barbar.SymbolicMath.SimplificationRules
{
    internal class DivisionCommonRule : TransformationRule<Division>
    {
        public override SymMathNode Apply(Division node)
        {
            var mulA = node.A as Multiply;
            SymMathNode mulAOther = null;

            Constant termN = node.A as Constant;
            if (mulA != null)
            {
                if (mulA.A is Constant && !(mulA.B is Constant))
                {
                    termN = (Constant)mulA.A;
                    mulAOther = mulA.B;
                }
                if (mulA.B is Constant && !(mulA.A is Constant))
                {
                    termN = (Constant)mulA.B;
                    mulAOther = mulA.A;
                }
            }

            Constant termD = node.B as Constant;
            var mulB = node.B as Multiply;
            SymMathNode mulBOther = null;
            if (mulB != null)
            {
                if (mulB.A is Constant && !(mulB.B is Constant))
                {
                    termD = (Constant)mulB.A;
                    mulBOther = mulB.B;
                }
                if (mulB.B is Constant && !(mulB.A is Constant))
                {
                    termD = (Constant)mulB.B;
                    mulBOther = mulB.A;
                }
            }

            if (termN != null && termD != null)
            {
                var gcd = Constant.Gcd(termN, termD);
                if (!gcd.IsOne())
                {
                    termN = (Constant)(termN / gcd);
                    termD = (Constant)(termD / gcd);
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
            return node;
        }

        public override bool IsApplicable(Division node)
        {
            var mulA = node.A as Multiply;
            SymMathNode mulAOther = null;

            Constant termN = node.A as Constant;
            if (mulA != null)
            {
                if (mulA.A is Constant && !(mulA.B is Constant))
                {
                    termN = (Constant)mulA.A;
                    mulAOther = mulA.B;
                }
                if (mulA.B is Constant && !(mulA.A is Constant))
                {
                    termN = (Constant)mulA.B;
                    mulAOther = mulA.A;
                }
            }

            Constant termD = node.B as Constant;
            var mulB = node.B as Multiply;
            SymMathNode mulBOther = null;
            if (mulB != null)
            {
                if (mulB.A is Constant && !(mulB.B is Constant))
                {
                    termD = (Constant)mulB.A;
                    mulBOther = mulB.B;
                }
                if (mulB.B is Constant && !(mulB.A is Constant))
                {
                    termD = (Constant)mulB.B;
                    mulBOther = mulB.A;
                }
            }

            if (termN != null && termD != null)
            {
                var gcd = Constant.Gcd(termN, termD);
                if (!gcd.IsOne())
                {
                    termN = (Constant)(termN / gcd);
                    termD = (Constant)(termD / gcd);
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
                    return true;
                }
            }
            return false;
        }
    }
}
