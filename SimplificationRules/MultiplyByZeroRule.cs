namespace Barbar.SymbolicMath.SimplificationRules
{
    internal class MultiplyByZeroRule : TransformationRule<Multiply>
    {
        public override SymMathNode Apply(Multiply node)
        {
            var constant = node.A as Constant;
            if (constant != null && constant.IsZero())
            {
                return Constant.Factory.Create(0);
            }
            constant = node.B as Constant;
            if (constant != null && constant.IsZero())
            {
                return Constant.Factory.Create(0);
            }
            return node;
        }

        public override bool IsApplicable(Multiply node)
        {
            return node.A.IfType<Constant>(c => c.IsZero()) || node.B.IfType<Constant>(c => c.IsZero());
        }
    }
}
