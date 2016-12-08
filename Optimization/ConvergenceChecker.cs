namespace Barbar.SymbolicMath.Optimization
{
    ///<summary>
    /// Check if the optimization algorithm has converged.
    /// </summary>   
    public delegate bool ConvergenceChecker<T>(int iteration, T previous, T current);
}