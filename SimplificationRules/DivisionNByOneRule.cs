namespace Barbar.SymbolicMath.SimplificationRules
{
    internal class DivisionNByOneRule : TransformationRule<Division>
    {
        public override SymMathNode Apply(Division node)
        {
            if (IsApplicable(node))
            {
                return node.A;
            }
            return node;
        }

        public override bool IsApplicable(Division node)
        {
            return node.B.IfType<Constant>(c => c.IsOne());
        }
    }
}
