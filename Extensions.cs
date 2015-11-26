using System;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns ture if node is of type T and condition evaluate is satisfied
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="evaluate"></param>
        /// <returns></returns>
        public static bool IfType<T>(this SymMathNode node, Func<T, bool> evaluate) where T : class
        {
            var t = node as T;
            if (t != null)
            {
                return evaluate.Invoke(t);
            }
            return false;
        }

        /// <summary>
        /// Returns true if tree is below zero
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsMinusOrNegativeTerm(this SymMathNode node)
        {
            return node is Minus || node.IfType<Term>(t => t.IsNegative());
        }

        /// <summary>
        /// Returns true if node is T1 and node.A is T2 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool Descendands<T1, T2>(this SymMathNode node) where T1 : UnaryOperation where T2 : UnaryOperation
        {
            return (node is T1) && ((T1)node).A is T2;
        }
    }
}