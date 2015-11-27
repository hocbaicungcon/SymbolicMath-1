using System;
using System.Collections.Generic;
using System.Linq;

namespace Barbar.SymbolicMath.Utilities
{
    /// <summary>
    /// Mathematical utilites
    /// </summary>
    public static class MathUtility
    {
        /// <summary>
        /// Calculates greatest common divisor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long Gcd(long a, long b)
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

        /// <summary>
        /// Gets factors of square root of number, expressed 
        /// as continued fraction. Stops after recurring period is reached.
        /// For example 2 yields { 1,2 }, 3 yields { 1, 1, 2 }, etc.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static IList<int> SqrtAsContinuedFraction(int number)
        {
            var a = (long)Math.Sqrt(number);
            if (a * a == number)
            {
                a--;
            }
            var lbound = a;

            var nodes = new List<SymMathNode>();
            var aFactors = new List<int>();

            var term = (SymMathNode)(Term.Factory.Create(1) / (new SquareRoot(number) - Term.Factory.Create(a)));
            while (true)
            {
                term = term.GetBaseForm();
                aFactors.Add((int)a);
                if (nodes.Any(n => n.SymbolicEquality(term)))
                {
                    return aFactors;
                }
                nodes.Add(term.Clone());
                var division = term as Division;
                Add add = term as Add;
                long d = 1;
                if (division != null)
                {
                    add = (Add)division.A;
                    d = ((Term)division.B).AsInt64();
                }
                long z = lbound + ((Term)add.B).AsInt64();
                while (z % d != 0)
                {
                    z--;
                }
                a = z / d;
                term = (Term.Factory.Create(d) / (new SquareRoot(number) + Term.Factory.Create((((Term)add.B).AsInt64())) - Term.Factory.Create(z)));
            }
        }

        /// <summary>
        /// Calculates totien function (phi) from fist element to size
        /// </summary>
        /// <param name="size"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public static int[] Totient(int size, out long sum)
        {
            int[] phi = new int[size];
            sum = 0;

            phi[1] = 1;
            for (int i = 2; i < size; i++)
            {
                if (phi[i] == 0)
                {
                    phi[i] = i - 1;
                    for (int j = 2; i * j < size; j++)
                    {
                        if (phi[j] == 0)
                            continue;

                        int q = j;
                        int f = i - 1;
                        while (q % i == 0)
                        {
                            f *= i;
                            q /= i;
                        }
                        phi[i * j] = f * phi[q];
                    }
                }
                sum += phi[i];
            }
            return phi;
        }

        /// <summary>
        /// Naive primality test
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsPrime(long n)
        {
            if (n < 2)
                return false;
            if (n < 4)
                return true;
            if ((n & 1) == 0 || n % 3 == 0)
                return false;
            long i = 5;
            while (i * i <= n)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
                i = i + 6;
            }
            return true;
        }
    }
}
