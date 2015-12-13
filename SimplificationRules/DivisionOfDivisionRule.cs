namespace Barbar.SymbolicMath.SimplificationRules
{
    internal class DivisionOfDivisionRule : TransformationRule<Division>
    {
        public override SymMathNode Apply(Division node)
        {
            if (node.B is Division)
            {
                return (node.A * ((Division)node.B).B) / ((Division)node.B).A;
            }
            return node;
        }

        public override bool IsApplicable(Division node)
        {
            return node.B is Division;
        }
    }
}
