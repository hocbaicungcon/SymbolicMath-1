using System;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Base abstract class for symbolic mathematic
    /// </summary>
    public abstract class SymMathNode
    {
        /// <summary>
        /// Returns true if this tree is equal to node tree
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public abstract bool SymbolicEquality(SymMathNode node);
        /// <summary>
        /// Simplify current expression by one step (if possible)
        /// </summary>
        /// <returns></returns>
        public abstract SymMathNode Simplify();
        /// <summary>
        /// True if current expression can be simplified
        /// </summary>
        /// <returns></returns>
        public abstract bool CanSimplify();
        /// <summary>
        /// Evaluates current expression tree and returns double
        /// Beware - this can lead to inaccuracies 
        /// </summary>
        /// <returns></returns>
        public abstract double Evaluate();
        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <returns></returns>
        public abstract SymMathNode Clone();
        /// <summary>
        /// Reduces expression to basic form
        /// </summary>
        /// <returns></returns>
        public SymMathNode GetBaseForm()
        {
            var clone = this;
            while(clone.CanSimplify())
            {
                clone = clone.Simplify();
            }
            return clone;
        }
        /// <summary>
        /// Returns new Add(a,b)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Add operator +(SymMathNode a, SymMathNode b)
        {
            return new Add(a, b);
        }

        /// <summary>
        /// Returns new Multiply(a,b)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Multiply operator *(SymMathNode a, SymMathNode b)
        {
            return new Multiply(a, b);
        }

        /// <summary>
        /// Returns new Division(a,b)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Division operator /(SymMathNode a, SymMathNode b)
        {
            return new Division(a, b);
        }

        /// <summary>
        /// Returns new Add(a,new Minus(b))
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Add operator -(SymMathNode a, SymMathNode b)
        {
            return new Add(a, new Minus(b));
        }
    }
}