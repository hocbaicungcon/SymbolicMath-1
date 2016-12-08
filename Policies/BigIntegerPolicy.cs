using System;
using System.Numerics;

namespace Barbar.SymbolicMath.Policies
{
    /// <summary>
    /// Implements numeric policy for <see cref="BigInteger"/> 
    /// </summary>
    public sealed class BigIntegerPolicy : IPolicy<BigInteger>
    {
        /// <summary>
        /// Default singleton
        /// </summary>
        public static readonly IPolicy<BigInteger> Instance = new BigIntegerPolicy();

        /// <inheritdoc />
        public BigInteger Abs(BigInteger a)
        {
            return BigInteger.Abs(a);
        }

        /// <inheritdoc />
        public BigInteger Add(BigInteger a, BigInteger b)
        {
            return a + b;
        }

        /// <inheritdoc />
        public BigInteger Div(BigInteger a, BigInteger b)
        {
            return a / b;
        }

        /// <inheritdoc />
        public bool Equals(BigInteger a, BigInteger b)
        {
            return a.Equals(b);
        }

        /// <inheritdoc />
        public BigInteger Gcd(BigInteger a, BigInteger b)
        {
            return BigInteger.GreatestCommonDivisor(a, b);
        }

        /// <inheritdoc />
        public bool IsBelowZero(BigInteger a)
        {
            return a < 0;
        }

        /// <inheritdoc />
        public bool IsOne(BigInteger a)
        {
            return a.IsOne;
        }

        /// <inheritdoc />
        public bool IsZero(BigInteger a)
        {
            return a.IsZero;
        }

        /// <inheritdoc />
        public BigInteger Lcd(BigInteger a, BigInteger b)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public BigInteger Mul(BigInteger a, BigInteger b)
        {
            return a * b;
        }

        /// <inheritdoc />
        public BigInteger Negate(BigInteger a)
        {
            return -a;
        }

        /// <inheritdoc />
        public BigInteger One()
        {
            return BigInteger.One;
        }

        /// <inheritdoc />
        public BigInteger Sqrt(BigInteger n)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public BigInteger Sub(BigInteger a, BigInteger b)
        {
            return a - b;
        }

        /// <inheritdoc />
        public BigInteger Zero()
        {
            return BigInteger.Zero;
        }

        /// <inheritdoc />
        public int Compare(BigInteger x, BigInteger y)
        {
            return BigInteger.Compare(x, y);
        }
    }
}
