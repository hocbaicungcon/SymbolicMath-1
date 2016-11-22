using System;
using System.Collections.Generic;
using System.Linq;

namespace Barbar.SymbolicMath.Parser
{
    public class MathParser
    {
        private static readonly Operation[] s_Operations = new Operation[]
        {
            new Operation("sqrt", 10, Associativity.Right, true, (a,b) => new SquareRoot(a)),
            new Operation("_", 10, Associativity.Right, true, (a,b) => new Minus(a)),
            new Operation("^", 9, Associativity.Right, false, (a,b) => new Power(a,b)),
            new Operation("*", 8, Associativity.Left, false, (a,b) => a * b),
            new Operation("/", 8, Associativity.Left, false, (a,b) => a / b),
            new Operation("+", 5, Associativity.Left, false, (a,b) => a + b),
            new Operation("-", 5, Associativity.Left, false, (a,b) => new Add(a, new Minus(b))),
            new Operation("(", 1, Associativity.None, false, null),
            new Operation(")", 1, Associativity.None, false, null),
            new Operation("=", 0, Associativity.None, false, (a,b) => new Equality(a,b))
        };

        private static Operation GetOperation(string term, ref int index)
        {
            foreach(var operation in s_Operations)
            {
                if (term.StartsWith(operation.Operator))
                {
                    index += operation.Operator.Length - 1;
                    return operation;
                }
            }
            return null;

        //  return s_Operations.FirstOrDefault(o => o.Operator == ch);
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
            if (op.Operator.StartsWith("("))
            {
                _operations.Push(op);
                return;
            }
            else if (op.Operator.StartsWith(")"))
            {
                while (_nodes.Count > 0 && !_operations.Peek().Operator.StartsWith("("))
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
                if (pop == null || !pop.Operator.StartsWith("("))
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

        static bool IsVariable(char c)
        {
            return c == 'x' || c == 'y' || c == 'z';
        }

        static bool IsSpace(char c)
        {
            return c == ' ';
        }

        SymMathNode ParseConstOrVariable(string term, int index)
        {
            if (IsVariable(term[index]))
            {
                return new Variable(Convert.ToString(term[index]));
            }

            int result = 0;
            for (int i = index; i < term.Length; i++)
            {
                if (!IsDigit(term[i]))
                {
                    return Constant.Factory.Create(result);
                }
                result *= 10;
                result += term[i] - '0';
            }
            return Constant.Factory.Create(result);
        }

        public static SymMathNode Parse(string term)
        {
            return new MathParser().DoParse(term);
        }

        public SymMathNode DoParse(string term)
        {
            // cleanup
            term = term.Replace(" ", "");

            int termStart = -1;
            var startop = new Operation("$", 0, Associativity.None, false, null); /* Dummy operator to mark start */
            var lastop = startop;


            for (var i = 0; i < term.Length; i++)
            {
                Operation op;
                var expr = term[i];
                if (termStart < 0)
                {
                    op = GetOperation(term.Substring(i), ref i);
                    if (op != null)
                    {
                        if (lastop != null && (lastop == startop || !lastop.Operator.StartsWith(")")))
                        {
                            if (op.Operator.StartsWith("-"))
                            {
                                op = GetOperation("_", ref i);
                            }
                            else if (!op.Unary && !op.Operator.StartsWith("("))
                            {
                                throw new Exception("ERROR: Illegal use of binary operator (" + op.Operator + ")");
                            }
                        }
                        Shunt(op);
                        lastop = op;
                        continue;
                    }
                    if (IsDigit(expr) || IsVariable(expr))
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
                    _nodes.Push(ParseConstOrVariable(term, termStart));
                    termStart = -1;
                    lastop = null;
                    continue;
                }
                op = GetOperation(term.Substring(i), ref i);
                if (op != null)
                {
                    _nodes.Push(ParseConstOrVariable(term, termStart));
                    termStart = -1;
                    Shunt(op);
                    lastop = op;
                    continue;
                }
                if (!IsDigit(expr) && !IsVariable(expr))
                {
                    throw new Exception("ERROR: Syntax error");
                }
            }
            if (termStart >= 0)
            {
                _nodes.Push(ParseConstOrVariable(term, termStart));
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