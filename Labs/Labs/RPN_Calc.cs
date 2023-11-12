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
            if (char.IsDigit(c))
            {
                number += c;
                continue;
            }
            if (number != "")
            {
                tokens.Add(new Number(double.Parse(number)));
                number = "";
            }
            if (c == '(' || c == ')' || c == '+' || c == '-' || c == '*' || c == '/') tokens.Add(new Operation(c));
            if (c == '(')
            {
                var par = ((Parenthesis)token);
                if (par != null && par.Type == TokenType.PARENTHESIS && par == '(')
                {
                    tokens.Add(new Parenthesis(')'));
                }
                else
                {
                    tokens.Add(new Parenthesis('('));
                }
            }
        }
        if (number != "") tokens.Add(new Number(double.Parse(number)));
        return tokens;
    }

    static List<Token> ToPostfix(List<Token> tokens)
    {
        var output = new List<Token>();
        var stack = new Stack<Token>();
        foreach (var token in tokens)
        {
            if (token.Type == TokenType.NUMBER) output.Add(token);
            else if (token.Type == TokenType.OPERATION)
            {
                while (stack.Count != 0 && stack.Peek().Type == TokenType.OPERATION)
                {
                    output.Add(stack.Pop());
                }
                stack.Push(token);
            }
            else if (token.Type == TokenType.PARENTHESIS && ((Parenthesis)token).Par == '(')
            {
                stack.Push(token);
            }
            else
            {
                while (stack.Peek().Type != TokenType.PARENTHESIS) output.Add(stack.Pop());
                stack.Pop();
            }
        }
        while (stack.Count != 0) output.Add(stack.Pop());
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
            else
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
        return stack.Pop();
    }
}