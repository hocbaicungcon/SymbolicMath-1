using System;

namespace Barbar.SymbolicMath.Policies
{
    /// <summary>
    /// Implements policy for long type
    /// </summary>
    public sealed class Int64Policy : IPolicy<long>
    {
        public static readonly IPolicy<long> Instance = new Int64Policy();

        /// <summary>
        /// Calculates greatest common divisor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public long Gcd(long a, long b)
        {
            if (a <= 0)
            {
                throw new ArgumentOutOfRangeException("a");
            }
            if (b <= 0)
            {
                throw new ArgumentOutOfRangeException("b");
            }

            int d = 0;
            while (((a & 1) == 0) && ((b & 1) == 0))
            {
                a = a >> 1;
                b = b >> 1;
                d++;
            }
            while (a != b)
            {
                if ((a & 1) == 0)
                {
                    a = a >> 1;
                    continue;
                }
                if ((b & 1) == 0)
                {
                    b = b >> 1;
                    continue;
                }
                if (a > b)
                {
                    a = (a - b) >> 1;
                }
                else
                {
                    b = (b - a) >> 1;
                }
            }
            return (1 << d) * a;
        }

        public long Lcd(long a, long b)
        {
            long num1, num2;
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (int i = 1; i <= num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                {
                    return i * num1;
                }
            }
            return num2;
        }

        public bool IsOne(long a)
        {
            return a == 1;
        }

        public bool IsZero(long a)
        {
            return a == 0;
        }

        public long Div(long a, long b)
        {
            return a / b;
        }

        public long Mul(long a, long b)
        {
            return a * b;
        }

        public long Add(long a, long b)
        {
            return a + b;
        }

        public long Sub(long a, long b)
        {
            return a - b;
        }

        public bool IsBelowZero(long a)
        {
            return a < 0;
        }

        public long Negate(long a)
        {
            return -a;
        }
        public long Abs(long n)
        {
            return n < 0 ? -n : n;
        }

        public long One()
        {
            return 1L;
        }

        public long Zero()
        {
            return 0L;
        }

        public long Sqrt(long n)
        {
            return (int)Math.Sqrt(n);
        }

        /*BIGINT below:
            if (n == 0) return 0;
            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(Math.Log(n, 2)));
                var root = 1L << (bitLength / 2);

                while (!IsSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private static bool IsSqrt(long n, long root)
        {
            var lowerBound = root * root;
            var upperBound = (root + 1) * (root + 1);

            return n >= lowerBound && n < upperBound;
        }*/
    }
}
