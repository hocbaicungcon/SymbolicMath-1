namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Factory for constructing generic term
    /// </summary>
    public interface ITermFactory
    {
        /// <summary>
        /// Create instance of term from long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Term Create(long value);
    }
}
