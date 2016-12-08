using System;

namespace Barbar.SymbolicMath.Policies
{
    /// <summary>
    /// Implements numeric policy for <see cref="double"/> 
    /// </summary>    
    public sealed class DoublePolicy : IPolicy<double>
    {
        /// <inheritdoc />
        public double Abs(double a)
        {
            return Math.Abs(a);
        }

        /// <inheritdoc />
        public double Add(double a, double b)
        {
            return a + b;
        }

        /// <inheritdoc />
        public double Div(double a, double b)
        {
            return a / b;
        }

        /// <inheritdoc />
        public bool Equals(double a, double b)
        {
            return a == b;
        }

        /// <inheritdoc />
        public double Gcd(double a, double b)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool IsBelowZero(double a)
        {
            return a < 0;
        }

        /// <inheritdoc />
        public bool IsOne(double a)
        {
            return a == 1;
        }

        /// <inheritdoc />
        public bool IsZero(double a)
        {
            return a == 0;
        }

        /// <inheritdoc />
        public double Lcd(double a, double b)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public double Mul(double a, double b)
        {
            return a * b;
        }

        /// <inheritdoc />
        public double Negate(double a)
        {
            return -a;
        }

        /// <inheritdoc />
        public double One()
        {
            return 1;
        }

        /// <inheritdoc />
        public double Sqrt(double n)
        {
            return Math.Sqrt(n);
        }

        /// <inheritdoc />
        public double Sub(double a, double b)
        {
            return a - b;
        }

        /// <inheritdoc />
        public double Zero()
        {
            return 0;
        }

        /// <inheritdoc />
        public int Compare(double x, double y)
        {
            if (x > y)
            {
                return 1;
            }
            if (x < y)
            {
                return -1;
            }
            return 0;
        }
    }
}
