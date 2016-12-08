using System;

namespace Barbar.SymbolicMath.Parser
{
    /// <summary>
    /// Represent unique mathematic operation
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// String representing operation - such as +, -, *, /, (, sqrt, =, ... etc
        /// </summary>
        public string Operator { get; private set; }
        
        /// <summary>
        /// Precedence
        /// </summary>
        public int Precedence { get; private set; }

        /// <summary>
        /// Associativity
        /// </summary>
        public Associativity Associativity { get; private set; }

        /// <summary>
        /// True if operation is unary
        /// </summary>
        public bool Unary { get; private set; }

        /// <summary>
        /// Evaluates operation
        /// </summary>
        public Func<SymMathNode, SymMathNode, SymMathNode> Evaluation { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="operator"></param>
        /// <param name="precedence"></param>
        /// <param name="associativity"></param>
        /// <param name="unary"></param>
        /// <param name="evaluation"></param>
        public Operation(string @operator, int precedence, Associativity associativity, bool unary, Func<SymMathNode, SymMathNode, SymMathNode> evaluation)
        {
            Operator = @operator;
            Precedence = precedence;
            Associativity = associativity;
            Unary = unary;
            Evaluation = evaluation;
        }
    }
}
