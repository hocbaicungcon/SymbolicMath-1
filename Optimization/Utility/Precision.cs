using Barbar.SymbolicMath.Policies;

namespace Barbar.SymbolicMath.Optimization.Utility
{
    /// <summary>
    /// Precision comparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPolicy"></typeparam>
    public static class Precision<T, TPolicy> where TPolicy : IPolicy<T>, new()
    {
        private static IPolicy<T> Policy = new TPolicy();
        
        /// <summary>
        /// True if |x - y| &lt;= eps
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static bool Equals(T x, T y, T eps)
        {
            return Policy.IsBelowZero(Policy.Sub(Policy.Abs(Policy.Sub(y, x)), eps));
        }

        /// <summary>
        /// Compares x and y
        ///  0 - |x - y| &lt;= eps
        /// -1 - x - y &lt; 0
        ///  1 - otherwise
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static int CompareTo(T x, T y, T eps)
        {
            if (Equals(x, y, eps))
            {
                return 0;
            }
            else if (Policy.IsBelowZero(Policy.Sub(x, y)))
            {
                return -1;
            }
            return 1;
        }
    }
}
