using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Barbar.SymbolicMath.Utilities
{
    /// <summary>
    /// Mathematical utilites
    /// </summary>
    public static class MathUtility
    {
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

        /// <summary>
        /// Computes partition function
        /// </summary>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static long Partition(int stop)
        {
            var p = new List<int> { 1 };

            int n = 1;
            while (true)
            {
                int i = 0;
                int penta = 1;
                p.Add(0);

                while (penta <= n)
                {
                    int sign = (i % 4 > 1) ? -1 : 1;
                    p[n] += sign * p[n - penta];
                    i++;

                    int j = (i % 2 == 0) ? i / 2 + 1 : -(i / 2 + 1);
                    penta = j * (3 * j - 1) / 2;
                }

                if (n == stop)
                {
                    return p[n];
                }

                n++;
            }
        }

        /// <summary>
        /// Calculates square root of big integer
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static BigInteger Sqrt(BigInteger n)
        {
            if (n == 0) return 0;
            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                var root = BigInteger.One << (bitLength / 2);

                while (!IsSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private static bool IsSqrt(BigInteger n, BigInteger root)
        {
            var lowerBound = root * root;
            var upperBound = (root + 1) * (root + 1);

            return n >= lowerBound && n < upperBound;
        }

        public static void GetPermutations<T>(T[] list, ICollection<T[]> perms)
        {
            int x = list.Length - 1;
            GetPermutations(list, 0, x, perms);
        }

        private static void GetPermutations<T>(T[] list, int k, int m, ICollection<T[]> perms)
        {
            if (k == m)
            {
                perms.Add((T[])list.Clone());
                return;
            }
            for (int i = k; i <= m; i++)
            {
                Swap(ref list[k], ref list[i]);
                GetPermutations(list, k + 1, m, perms);
                Swap(ref list[k], ref list[i]);
            }
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            if (!EqualityComparer<T>.Default.Equals(a, b))
            {
                T xchg = a;
                a = b;
                b = xchg;
            }
        }
    }
}
