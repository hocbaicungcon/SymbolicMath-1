namespace Barbar.SymbolicMath.Parser
{
    /// <summary>
    /// Operation associativity
    /// </summary>
    public enum Associativity : int
    {
        /// <summary>
        /// No associativity
        /// </summary>
        None = 0,
        /// <summary>
        /// Left associativity (like *, /, +, -)
        /// </summary>
        Left = 1,

        /// <summary>
        /// Righ associativity (like ^ (power) or sqrt)
        /// </summary>
        Right = 2
    }
}
