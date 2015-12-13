namespace Barbar.SymbolicMath.SimplificationRules
{
    internal class MinusConstantRule : TransformationRule<Minus>
    {
        public override SymMathNode Apply(Minus node)
        {
            var term = node.A as Constant;
            if (term != null)
            {
                return term.Negate();
            }

            return node;
        }

        public override bool IsApplicable(Minus node)
        {
            return node.A is Constant;
        }
    }
}
