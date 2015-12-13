namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Rule for node transformation
    /// </summary>
    public interface ITransformationRule
    {
        /// <summary>
        ///  Applies the transformation rule over node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        SymMathNode Apply(SymMathNode node);
        /// <summary>
        /// True if we can apply the rule
        /// </summary>
        /// <param name="node">Math node</param>
        /// <returns></returns>
        bool IsApplicable(SymMathNode node);
    }
}
