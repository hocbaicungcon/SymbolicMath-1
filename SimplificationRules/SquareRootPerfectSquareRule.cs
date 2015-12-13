namespace Barbar.SymbolicMath.SimplificationRules
{
    internal sealed class SquareRootPerfectSquareRule : TransformationRule<SquareRoot>
    {
        public override SymMathNode Apply(SquareRoot node)
        {
            var term = node.A as Constant;
            if (term != null)
            {
                var sqrt = term.Sqrt();
                if (sqrt * sqrt == term)
                {
                    return sqrt;
                }
            }

            return node;
        }

        public override bool IsApplicable(SquareRoot node)
        {
            var term = node.A as Constant;
            if (term != null)
            {
                var sqrt = term.Sqrt();
                if (sqrt * sqrt == term)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
