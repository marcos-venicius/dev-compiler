﻿using DC.CodeAnalysis.Enums;

namespace DC.CodeAnalysis;

public sealed class Evaluator
{
    private readonly ExpressionSyntax _root;

    public Evaluator(ExpressionSyntax root)
    {
        _root = root;
    }

    public int Evaluate()
    {
        return EvaluateExpression(_root);
    }

    private int EvaluateExpression(ExpressionSyntax node)
    {
        if (node is LiteralExpressionSyntax number)
            return (int)number.LiteralToken.Value!;

        if (node is UnaryExpressionSyntax u)
        {
            var operand = EvaluateExpression(u.Operand);

            if (u.OperatorToken.Kind == SyntaxKind.PlusToken)
                return operand;
            else if (u.OperatorToken.Kind == SyntaxKind.MinusToken)
                return -operand;

            throw new Exception($"Unexpected unary operator {u.OperatorToken.Kind}");
        }

        if (node is BinaryExpressionSyntax binary)
        {
            var left = EvaluateExpression(binary.Left);
            var right = EvaluateExpression(binary.Right);

            var result = binary.OperatorToken.Kind switch
            {
                SyntaxKind.PlusToken => left + right,
                SyntaxKind.MinusToken => left - right,
                SyntaxKind.StarToken => left * right,
                SyntaxKind.SlashToken => left / right,
                _ => throw new Exception($"Unexpected binary operator {binary.OperatorToken.Kind}")
            };

            return result;
        }

        if (node is ParenthesizedExpressionSyntax p)
            return EvaluateExpression(p.Expression);

        throw new Exception($"Unexpected node {node.Kind}");
    }
}
