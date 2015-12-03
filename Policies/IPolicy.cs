namespace Barbar.SymbolicMath.Policies
{
    public interface IPolicy<T>
    {
        /// <summary>
        /// Greatest common divisor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Gcd(T a, T b);
        /// <summary>
        ///  Least common denominator 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Lcd(T a, T b);
        /// <summary>
        /// True if value is exactly one
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        bool IsOne(T a);
        /// <summary>
        /// True if value is zero
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <returns></returns>
        bool IsZero(T a);
        /// <summary>
        /// Division
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Div(T a, T b);
        /// <summary>
        /// True if below zero
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        bool IsBelowZero(T a);
        /// <summary>
        /// Return absolute value of a
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        T Abs(T a);

        /// <summary>
        /// Returns -a
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        T Negate(T a);
        /// <summary>
        /// Multiply
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Mul(T a, T b);

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Add(T a, T b);

        /// <summary>
        /// Subtract
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        T Sub(T a, T b);

        T One();
        T Zero();
    }
}
