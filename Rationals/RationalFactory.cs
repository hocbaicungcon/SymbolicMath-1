using Barbar.SymbolicMath.Policies;

namespace Barbar.SymbolicMath.Rationals
{
    public static class RationalFactory
    {
        public static Rational<long, Int64Policy> Create(long value)
        {
            return new Rational<long, Int64Policy>(value);
        }

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
