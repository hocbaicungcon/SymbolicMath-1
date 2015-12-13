namespace Barbar.SymbolicMath.SimplificationRules
{
    internal class MinusMinusRule : TransformationRule<Minus>
    {
        public override SymMathNode Apply(Minus node)
        {
            var minus = node.A as Minus;
            if (minus != null)
            {
                return minus.A;
            }
            return node;
        }

        public override bool IsApplicable(Minus node)
        {
            return node.A is Minus;
        }
    }
}
