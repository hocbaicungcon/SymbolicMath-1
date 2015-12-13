using System;
using System.Numerics;

namespace Barbar.SymbolicMath.Policies
{
    public class BigIntegerPolicy : IPolicy<BigInteger>
    {
        public static readonly IPolicy<BigInteger> Instance = new BigIntegerPolicy();

        public BigInteger Abs(BigInteger a)
        {
            return BigInteger.Abs(a);
        }

        public BigInteger Add(BigInteger a, BigInteger b)
        {
            return a + b;
        }

        public BigInteger Div(BigInteger a, BigInteger b)
        {
            return a / b;
        }

        public BigInteger Gcd(BigInteger a, BigInteger b)
        {
            return BigInteger.GreatestCommonDivisor(a, b);
        }

        public bool IsBelowZero(BigInteger a)
        {
            return a < 0;
        }

        public bool IsOne(BigInteger a)
        {
            return a.IsOne;
        }

        public bool IsZero(BigInteger a)
        {
            return a.IsZero;
        }

        public BigInteger Lcd(BigInteger a, BigInteger b)
        {
            throw new NotImplementedException();
        }

        public BigInteger Mul(BigInteger a, BigInteger b)
        {
            return a * b;
        }

        public BigInteger Negate(BigInteger a)
        {
            return -a;
        }

        public BigInteger One()
        {
            return BigInteger.One;
        }

        public BigInteger Sqrt(BigInteger n)
        {
            throw new NotImplementedException();
        }

        public BigInteger Sub(BigInteger a, BigInteger b)
        {
            return a - b;
        }

        public BigInteger Zero()
        {
            return BigInteger.Zero;
        }
    }
}
