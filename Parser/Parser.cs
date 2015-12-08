using System;
using System.Collections.Generic;
using System.Linq;

namespace Barbar.SymbolicMath.Parser
{
    public class MathParser
    {
        static int eval_uminus(int a1, int a2)
        {
            return -a1;
        }
        static int eval_exp(int a1, int a2)
        {
            return a2 < 0 ? 0 : (a2 == 0 ? 1 : a1 * eval_exp(a1, a2 - 1));
        }
        static int eval_mul(int a1, int a2)
        {
            return a1 * a2;
        }
        static int eval_div(int a1, int a2)
        {
            if (a2 == 0)
            {
                throw new InvalidOperationException("Division by zero.");
            }
            return a1 / a2;
        }

        static int eval_mod(int a1, int a2)
        {
            if (a2 == 0)
            {
                throw new InvalidOperationException("Division by zero.");
            }
            return a1 % a2;
        }
        static int eval_add(int a1, int a2)
        {
            return a1 + a2;
        }
        static int eval_sub(int a1, int a2)
        {
            return a1 - a2;
        }

        private static readonly Operation[] s_Operations = new Operation[]
        {
            new Operation('_', 10, Associativity.Right, true, eval_uminus),
            new Operation('^', 9, Associativity.Right, false, eval_exp),
            new Operation('*', 8, Associativity.Left, false, eval_mul),
            new Operation('/', 8, Associativity.Left, false, eval_div),
            new Operation('%', 8, Associativity.Left, false, eval_mod),
            new Operation('+', 5, Associativity.Left, false, eval_add),
            new Operation('-', 5, Associativity.Left, false, eval_sub),
            new Operation('(', 0, Associativity.None, false, null),
            new Operation(')', 0, Associativity.None, false, null)
        };

        private static Operation GetOperation(char ch)
        {
            return s_Operations.FirstOrDefault(o => o.Operator == ch);
        }

        Stack<Operation> operations = new Stack<Operation>();
        Stack<int> numstack = new Stack<int>();


        void Shunt(Operation op)
        {
            Operation pop;
            int n1, n2;
            if (op.Operator == '(')
            {
                operations.Push(op);
                return;
            }
            else if (op.Operator == ')')
            {
                while (numstack.Count > 0 && operations.Peek().Operator != '(')
                {
                    pop = operations.Pop();
                    n1 = numstack.Pop();
                    if (pop.Unary) numstack.Push(pop.Evaluation(n1, 0));
                    else
                    {
                        n2 = numstack.Pop();
                        numstack.Push(pop.Evaluation(n2, n1));
                    }
                }
                pop = operations.Pop();
                if (pop == null || pop.Operator != '(')
                {
                    throw new Exception("ERROR: Stack error. No matching \'(\'");
                }
                return;
            }

            if (op.Associativity == Associativity.Right)
            {
                while (operations.Count > 0 && op.Precedence < operations.Peek().Precedence)
                {
                    pop = operations.Pop();
                    n1 = numstack.Pop();
                    if (pop.Unary) numstack.Push(pop.Evaluation(n1, 0));
                    else
                    {
                        n2 = numstack.Pop();
                        numstack.Push(pop.Evaluation(n2, n1));
                    }
                }
            }
            else
            {
                while (operations.Count > 0 && op.Precedence <= operations.Peek().Precedence)
                {
                    pop = operations.Pop();
                    n1 = numstack.Pop();
                    if (pop.Unary) numstack.Push(pop.Evaluation(n1, 0));
                    else
                    {
                        n2 = numstack.Pop();
                        numstack.Push(pop.Evaluation(n2, n1));
                    }
                }
            }
            operations.Push(op);
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

        public int Parse(string term)
        {
            int termStart = -1;
            Operation startop = new Operation('X', 0, Associativity.None, false, null); /* Dummy operator to mark start */
            Operation lastop = startop;


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
                    numstack.Push(Atoi(term, termStart));
                    termStart = -1;
                    lastop = null;
                    continue;
                }
                op = GetOperation(expr);
                if (op != null)
                {
                    numstack.Push(Atoi(term, termStart));
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
                numstack.Push(Atoi(term, termStart));
            }

            while (operations.Count > 0)
            {
                var op = operations.Pop();
                int n1 = numstack.Pop();
                if (op.Unary) numstack.Push(op.Evaluation(n1, 0));
                else
                {
                    int n2 = numstack.Pop();
                    numstack.Push(op.Evaluation(n2, n1));
                }
            }
            if (numstack.Count != 1)
            {
                throw new Exception("ERROR: Number stack has % d elements after evaluation.Should be 1.");
            }
            return numstack.Pop();
        }
    }
}