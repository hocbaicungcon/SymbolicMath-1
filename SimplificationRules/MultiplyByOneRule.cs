namespace Barbar.SymbolicMath.SimplificationRules
{
    internal class MultiplyByOneRule : TransformationRule<Multiply>
    {
        public override SymMathNode Apply(Multiply node)
        {
            var constant = node.A as Constant;
            if (constant != null && constant.IsOne())
            {
                return node.B;
            }
            constant = node.B as Constant;
            if (constant != null && constant.IsOne())
            {
                return node.B;
            }
            return node;
        }

        public override bool IsApplicable(Multiply node)
        {
            return node.A.IfType<Constant>(c => c.IsOne()) || node.B.IfType<Constant>(c => c.IsOne());
        }
    }
}
