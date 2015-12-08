using System;

namespace Barbar.SymbolicMath.Parser
{
    public class Operation
    {
        public char Operator { get; private set; }
        public int Precedence { get; private set; }
        public Associativity Associativity { get; private set; }
        public bool Unary { get; private set; }
        public Func<int, int, int> Evaluation { get; private set; }

        public Operation(char @operator, int precedence, Associativity associativity, bool unary, Func<int, int, int> evaluation)
        {
            Operator = @operator;
            Precedence = precedence;
            Associativity = associativity;
            Unary = unary;
            Evaluation = evaluation;
        }
    }
}
