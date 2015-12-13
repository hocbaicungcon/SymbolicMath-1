namespace Barbar.SymbolicMath.SimplificationRules
{
    internal sealed class BinaryBaseRule<T> : TransformationRule<T> where T : BinaryOperation
    {
        public override SymMathNode Apply(T node)
        {
            return node.Clone(node.A.Simplify(), node.B.Simplify());
        }

        public override bool IsApplicable(T node)
        {
            return node.A.CanSimplify() || node.B.CanSimplify();
        }
    }
}
