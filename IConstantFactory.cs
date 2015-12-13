namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Factory for constructing generic term
    /// </summary>
    public interface IConstantFactory
    {
        /// <summary>
        /// Create instance of term from long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Constant Create(long value);
    }
}
