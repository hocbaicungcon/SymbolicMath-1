namespace Barbar.SymbolicMath.SimplificationRules
{
    internal class MultiplySquareRootRule : TransformationRule<Multiply>
    {
        public override SymMathNode Apply(Multiply node)
        {
            var sqra = node.A as SquareRoot;
            var sqrb = node.B as SquareRoot;
            if (sqra != null && sqrb != null && sqra.A.Equals(sqrb.A))
            {
                return sqra.A;
            }
            return node;
        }

        public override bool IsApplicable(Multiply node)
        {
            var sqra = node.A as SquareRoot;
            var sqrb = node.B as SquareRoot;
            return (sqra != null && sqrb != null && sqra.A.Equals(sqrb.A));
        }
    }
}
