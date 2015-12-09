using System;
using System.Collections.Generic;
using System.Linq;

namespace Barbar.SymbolicMath.Parser
{
    public class MathParser
    {
        private static readonly Operation[] s_Operations = new Operation[]
        {
            new Operation('_', 10, Associativity.Right, true, (a,b) => new Minus(a)),
            new Operation('^', 9, Associativity.Right, false, (a,b) => new Power(a,b)),
            new Operation('*', 8, Associativity.Left, false, (a,b) => a * b),
            new Operation('/', 8, Associativity.Left, false, (a,b) => a / b),
            new Operation('+', 5, Associativity.Left, false, (a,b) => a + b),
            new Operation('-', 5, Associativity.Left, false, (a,b) => new Add(a, new Minus(b))),
            new Operation('(', 0, Associativity.None, false, null),
            new Operation(')', 0, Associativity.None, false, null)
        };

        private static Operation GetOperation(char ch)
        {
            return s_Operations.FirstOrDefault(o => o.Operator == ch);
        }

        Stack<Operation> _operations = new Stack<Operation>();
        Stack<SymMathNode> _nodes = new Stack<SymMathNode>();

        private MathParser()
        {
        }

        void Shunt(Operation op)
        {
            Operation pop;
            SymMathNode n1, n2;
            if (op.Operator == '(')
            {
                _operations.Push(op);
                return;
            }
            else if (op.Operator == ')')
            {
                while (_nodes.Count > 0 && _operations.Peek().Operator != '(')
                {
                    pop = _operations.Pop();
                    n1 = _nodes.Pop();
                    if (pop.Unary) _nodes.Push(pop.Evaluation(n1, null));
                    else
                    {
                        n2 = _nodes.Pop();
                        _nodes.Push(pop.Evaluation(n2, n1));
                    }
                }
                pop = _operations.Pop();
                if (pop == null || pop.Operator != '(')
                {
                    throw new Exception("ERROR: Stack error. No matching \'(\'");
                }
                return;
            }

            if (op.Associativity == Associativity.Right)
            {
                while (_operations.Count > 0 && op.Precedence < _operations.Peek().Precedence)
                {
                    pop = _operations.Pop();
                    n1 = _nodes.Pop();
                    if (pop.Unary) _nodes.Push(pop.Evaluation(n1, null));
                    else
                    {
                        n2 = _nodes.Pop();
                        _nodes.Push(pop.Evaluation(n2, n1));
                    }
                }
            }
            else
            {
                while (_operations.Count > 0 && op.Precedence <= _operations.Peek().Precedence)
                {
                    pop = _operations.Pop();
                    n1 = _nodes.Pop();
                    if (pop.Unary) _nodes.Push(pop.Evaluation(n1, null));
                    else
                    {
                        n2 = _nodes.Pop();
                        _nodes.Push(pop.Evaluation(n2, n1));
                    }
                }
            }
            _operations.Push(op);
        }

        static bool IsDigit(char c)
        {
            int digit = c - '0';
            return digit >= 0 && digit <= 9;
        }

        static bool IsSpace(char c)
        {
            return c == ' ';
        }

        int Atoi(string term, int index)
        {
            int result = 0;
            for (int i = index; i < term.Length; i++)
            {
                if (!IsDigit(term[i]))
                {
                    return result;
                }
                result *= 10;
                result += term[i] - '0';
            }
            return result;
        }

        public static SymMathNode Parse(string term)
        {
            return new MathParser().DoParse(term);
        }

        public SymMathNode DoParse(string term)
        {
            int termStart = -1;
            var startop = new Operation('X', 0, Associativity.None, false, null); /* Dummy operator to mark start */
            var lastop = startop;


            for (var i = 0; i < term.Length; i++)
            {
                Operation op;
                var expr = term[i];
                if (termStart < 0)
                {
                    op = GetOperation(expr);
                    if (op != null)
                    {
                        if (lastop != null && (lastop == startop || lastop.Operator != ')'))
                        {
                            if (op.Operator == '-')
                            {
                                op = GetOperation('_');
                            }
                            else if (op.Operator != '(')
                            {
                                throw new Exception("ERROR: Illegal use of binary operator (" + op.Operator + ")");
                            }
                        }
                        Shunt(op);
                        lastop = op;
                        continue;
                    }
                    if (IsDigit(expr))
                    {
                        termStart = i;
                        continue;
                    }
                    if (!IsSpace(expr))
                    {
                        throw new Exception("ERROR: Syntax error");
                    }
                    continue;
                }
                if (IsSpace(expr))
                {
                    _nodes.Push(Term.Factory.Create(Atoi(term, termStart)));
                    termStart = -1;
                    lastop = null;
                    continue;
                }
                op = GetOperation(expr);
                if (op != null)
                {
                    _nodes.Push(Term.Factory.Create(Atoi(term, termStart)));
                    termStart = -1;
                    Shunt(op);
                    lastop = op;
                    continue;
                }
                if (!IsDigit(expr))
                {
                    throw new Exception("ERROR: Syntax error");
                }
            }
            if (termStart >= 0)
            {
                _nodes.Push(Term.Factory.Create(Atoi(term, termStart)));
            }

            while (_operations.Count > 0)
            {
                var op = _operations.Pop();
                var n1 = _nodes.Pop();
                if (op.Unary) _nodes.Push(op.Evaluation(n1, null));
                else
                {
                    var n2 = _nodes.Pop();
                    _nodes.Push(op.Evaluation(n2, n1));
                }
            }
            if (_nodes.Count != 1)
            {
                throw new Exception("ERROR: Number stack has % d elements after evaluation.Should be 1.");
            }
            return _nodes.Pop();
        }
    }
}