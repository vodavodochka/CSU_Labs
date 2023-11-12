using System;
using System.Collections.Generic;
using System.Linq;

enum TokenType
{
    NUMBER,
    OPERATION,
    PARENTHESIS
}

class Token
{
    public TokenType Type;
}

class Number : Token
{
    public double Value;

    public Number(double value)
    {
        this.Value = value;
        this.Type = TokenType.NUMBER;
    }
}

class Operation : Token
{
    public char Op;

    public Operation(char op)
    {
        this.Op = op;
        this.Type = TokenType.OPERATION;
    }
}

class Parenthesis : Token
{
    public char Par;

    public Parenthesis(char par)
    {
        this.Par = par;
        this.Type = TokenType.PARENTHESIS;
    }
}

class RPN_Calc
{
    static void Main(string[] args)
    {
        Console.Write("Enter an expression: ");
        string expression = Console.ReadLine();
        var tokens = Tokenize(expression);
        var postfix = ToPostfix(tokens);
        double result = Evaluate(postfix);
        Console.WriteLine($"Result: {result}");
    }

    static List<Token> Tokenize(string expression)
    {
        var tokens = new List<Token>();
        var number = "";
        foreach (var c in expression)
        {
            if (char.IsDigit(c) || c == ',')
            {
                number += c;
                continue;
            }
            if (number != "")
            {
                tokens.Add(new Number(double.Parse(number)));
                number = "";
            }
            if (c == '+' || c == '-' || c == '*' || c == '/')
            {
                tokens.Add(new Operation(c));
            }
            else if (c == '(' || c == ')')
            {
                tokens.Add(new Parenthesis(c));
            }
        }
        if (number != "") tokens.Add(new Number(double.Parse(number)));
        return tokens;
    }

    static int GetPrecedence(Operation op)
    {
        switch (op.Op)
        {
            case '+':
            case '-':
                return 1;
            case '*':
            case '/':
                return 2;
            default:
                throw new ArgumentException("Unknown operator");
        }
    }
    static List<Token> ToPostfix(List<Token> tokens)
    {
        var output = new List<Token>();
        var stack = new Stack<Token>();
        foreach (var token in tokens)
        {
            if (token.Type == TokenType.NUMBER)
            {
                output.Add(token);
            }
            else if (token.Type == TokenType.OPERATION)
            {
                Operation o1 = (Operation)token;

                while (stack.Count != 0 && stack.Peek().Type == TokenType.OPERATION)
                {
                    Operation o2 = (Operation)stack.Peek();

                    if (GetPrecedence(o1) <= GetPrecedence(o2))
                    {
                        output.Add(stack.Pop());
                    }
                    else
                    {
                        break;
                    }
                }
                stack.Push(token);
            }
            else if (token.Type == TokenType.PARENTHESIS)
            {
                Parenthesis pat = (Parenthesis)token;

                if (pat.Par == '(')
                {
                    stack.Push(token);
                }
                else if (pat.Par == ')')
                {
                    while (stack.Count != 0 && stack.Peek().Type != TokenType.PARENTHESIS)
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Pop(); // Pop the '('
                }
            }
        }
        while (stack.Count != 0)
        {
            output.Add(stack.Pop());
        }
        return output;
    }

    static double Evaluate(List<Token> tokens)
    {
        var stack = new Stack<double>();
        foreach (var token in tokens)
        {
            if (token.Type == TokenType.NUMBER)
            {
                stack.Push(((Number)token).Value);
            }
            else if (token.Type == TokenType.OPERATION)
            {
                var b = stack.Pop();
                var a = stack.Pop();
                var op = ((Operation)token).Op;
                if (op == '+') stack.Push(a + b);
                else if (op == '-') stack.Push(a - b);
                else if (op == '*') stack.Push(a * b);
                else stack.Push(a / b);
            }
        }
        if (stack.Count != 1)
        {
            throw new InvalidOperationException("Invalid expression");
        }
        return stack.Pop();
    }
}