namespace Barbar.SymbolicMath.SimplificationRules
{
    internal class DivisionSquareRootInDenominatorRule : TransformationRule<Division>
    {
        public override SymMathNode Apply(Division node)
        {
            var add = node.B as Add;
            if (add != null && (
                ((add.A.IsMinusOrNegativeTerm() && !add.B.IsMinusOrNegativeTerm()) || (add.B.IsMinusOrNegativeTerm() && !add.A.IsMinusOrNegativeTerm())) &&
               (add.A is SquareRoot || add.A.Descendands<Minus, SquareRoot>() || add.B is SquareRoot || add.B.Descendands<Minus, SquareRoot>())))
            {
                var aSquare = add.A * add.A;
                var bSquare = add.B * add.B;
                if (add.B.IsMinusOrNegativeTerm())
                {
                    return (node.A * (add.A - add.B)) / ((add.A * add.A) - (add.B * add.B));
                }
                return (node.A * (add.B - add.A)) / ((add.B * add.B) - (add.A * add.A));
            }
            return node;
        }

        public override bool IsApplicable(Division node)
        {
            var add = node.B as Add;
            if (add != null && (
                ((add.A.IsMinusOrNegativeTerm() && !add.B.IsMinusOrNegativeTerm()) || (add.B.IsMinusOrNegativeTerm() && !add.A.IsMinusOrNegativeTerm())) &&
               (add.A is SquareRoot || add.A.Descendands<Minus, SquareRoot>() || add.B is SquareRoot || add.B.Descendands<Minus, SquareRoot>())))
            {
                return true;
            }
            return false;
        }
    }
}
