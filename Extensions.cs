using System;

namespace Barbar.SymbolicMath
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// If any simplification rule exits, simplify given node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool CanSimplify(this SymMathNode node)
        {
            foreach (var rule in node.GetSimplificationRules())
            {
                if (rule.IsApplicable(node))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// If any simplification rule exits, simplify given node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static SymMathNode Simplify(this SymMathNode node)
        {
            foreach(var rule in node.GetSimplificationRules())
            {
                if (rule.IsApplicable(node))
                {
                    return rule.Apply(node);
                }
            }
            return node;
        }

        /// <summary>
        /// Applies all simplification rules, until none is applicable
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static SymMathNode GetBaseForm(this SymMathNode node)
        {
            var result = node;
            while (true)
            {
                bool ruleFound = false;
                foreach (var rule in result.GetSimplificationRules())
                {
                    if (rule.IsApplicable(result))
                    {
                        result = rule.Apply(result);
                        ruleFound = true;
                        break;
                    }
                }
                if (!ruleFound)
                {
                    return result;
                }
            }
        }


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
            return node is Minus || node.IfType<Constant>(t => t.IsNegative());
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