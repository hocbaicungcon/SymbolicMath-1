namespace Barbar.SymbolicMath.SimplificationRules
{
    internal sealed class UnaryBaseRule<T> : TransformationRule<T> where T : UnaryOperation
    {
        public override SymMathNode Apply(T node)
        {
            return node.Clone(node.A.Simplify());
        }

        public override bool IsApplicable(T node)
        {
            return node.A.CanSimplify();
        }
    }
}
