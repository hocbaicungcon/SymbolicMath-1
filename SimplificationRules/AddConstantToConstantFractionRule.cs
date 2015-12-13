namespace Barbar.SymbolicMath.SimplificationRules
{
    internal sealed class AddConstantToConstantFractionRule : TransformationRule<Add>
    {
        public override SymMathNode Apply(Add add)
        {
            var a = add.A as Constant;
            var b = add.B as Constant;
            var divB = add.B as Division;
            if (divB != null && a != null)
            {
                var divBA = divB.A as Constant;
                var divBB = divB.B as Constant;
                return ((divBB * a) + divB.A) / divB.B;
            }

            var divA = add.A as Division;
            if (divA != null && b != null)
            {
                var divAA = divA.A as Constant;
                var divAB = divA.B as Constant;
                return ((divAB * b) + divA.A) / divA.B;
            }

            return add;
        }

        public override bool IsApplicable(Add add)
        {
            if (add.A is Constant)
            {
                return add.B.IfType<Division>(d => d.A is Constant && d.B is Constant);
            }
            if (add.B is Constant)
            {
                return add.A.IfType<Division>(d => d.A is Constant && d.B is Constant);
            }

            return false;
        }
    }
}
