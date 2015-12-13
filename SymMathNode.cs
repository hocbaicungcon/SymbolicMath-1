using System;
using System.Collections.Generic;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Base abstract class for symbolic mathematic
    /// </summary>
    public abstract class SymMathNode : IEquatable<SymMathNode>
    {
        private static ITransformationRule[] s_Rules = new ITransformationRule[] { };

        /// <summary>
        /// Return list of possible simplifications
        /// </summary>
        /// <returns></returns>
        public virtual IList<ITransformationRule> GetSimplificationRules()
        {
            return s_Rules;
        }
        

        public abstract double Evaluate();
        /// <summary>
        /// Clones current node (deep-copy)
        /// </summary>
        /// <returns></returns>
        public abstract SymMathNode Clone();
        /*
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
        }*/

        /// <summary>
        /// Returns true if node equal to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool Equals(SymMathNode other);

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