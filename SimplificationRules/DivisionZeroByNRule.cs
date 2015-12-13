namespace Barbar.SymbolicMath.SimplificationRules
{
    internal class DivisionZeroByNRule : TransformationRule<Division>
    {
        public override SymMathNode Apply(Division node)
        {
            return Constant.Factory.Create(0);
        }

        public override bool IsApplicable(Division node)
        {
            return node.A.IfType<Constant>(c => c.IsZero());
        }
    }
}
