using Barbar.SymbolicMath.Policies;

namespace Barbar.SymbolicMath.Rationals
{
    /// <summary>
    /// Factory for quick-creation of rationals
    /// </summary>
    public static class RationalFactory
    {
        /// <summary>
        /// Creates rational from integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Rational<long, Int64Policy> Create(long value)
        {
            return new Rational<long, Int64Policy>(value);
        }

        /// <summary>
        /// Creates array of rationals from array of integers
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Rational<long, Int64Policy>[] Create(long[] values)
        {
            var result = new Rational<long, Int64Policy>[values.Length];
            for(var i = 0; i < values.Length; i++)
            {
                result[i] = new Rational<long, Int64Policy>(values[i]);
            }
            return result;
        }
    }
}
