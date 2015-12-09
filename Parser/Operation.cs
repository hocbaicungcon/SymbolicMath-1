using System;

namespace Barbar.SymbolicMath.Parser
{
    public class Operation
    {
        public char Operator { get; private set; }
        public int Precedence { get; private set; }
        public Associativity Associativity { get; private set; }
        public bool Unary { get; private set; }
        public Func<SymMathNode, SymMathNode, SymMathNode> Evaluation { get; private set; }

        public Operation(char @operator, int precedence, Associativity associativity, bool unary, Func<SymMathNode, SymMathNode, SymMathNode> evaluation)
        {
            Operator = @operator;
            Precedence = precedence;
            Associativity = associativity;
            Unary = unary;
            Evaluation = evaluation;
        }
    }
}
