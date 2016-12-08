using System.Collections.Generic;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Rule for node transformation
    /// </summary>
    public abstract class TransformationRule<T> : ITransformationRule where T : SymMathNode
    {
        /// <summary>
        /// Get nodes different from node
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        protected static IEnumerable<SymMathNode> GetDifferentNodes<TNode>(TNode node) where TNode : BinaryOperation
        {
            var stack = new Stack<TNode>();
            stack.Push(node);
            while (stack.Count > 0)
            {
                node = stack.Pop();
                var aa = node.A as TNode;
                if (aa != null)
                {
                    stack.Push(aa);
                }
                else
                {
                    yield return node.A;
                }

                var ab = node.B as TNode;
                if (ab != null)
                {
                    stack.Push(ab);
                }
                else
                {
                    yield return node.B;
                }
            }
        }
        
        /// <summary>
        /// Applies the transformation rule over node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public SymMathNode Apply(SymMathNode node)
        {
            return Apply((T)node);
        }

        /// <summary>
        /// True if we can apply the rule
        /// </summary>
        /// <param name="node">Math node</param>
        /// <returns></returns>
        public bool IsApplicable(SymMathNode node)
        {
            return IsApplicable((T)node);
        }

        /// <summary>
        /// Applies the transformation rule over node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public abstract SymMathNode Apply(T node);

        /// <summary>
        /// True if we can apply the rule
        /// </summary>
        /// <param name="node">Math node</param>
        /// <returns></returns>
        public abstract bool IsApplicable(T node);
    }
}
