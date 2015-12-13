namespace Barbar.SymbolicMath.SimplificationRules
{
    internal sealed class AddZeroRule : TransformationRule<Add>
    {
        public override SymMathNode Apply(Add add)
        {
            var ca = add.A as Constant;
            if (ca != null && ca.IsZero())
            {
                return add.B;
            }

            var cb = add.B as Constant;
            if (cb != null && cb.IsZero())
            {
                return add.A;
            }

            return add;
        }

        public override bool IsApplicable(Add node)
        {
            return node.A.IfType<Constant>(t => t.IsZero()) ||
                node.B.IfType<Constant>(t => t.IsZero());
        }
    }
}
