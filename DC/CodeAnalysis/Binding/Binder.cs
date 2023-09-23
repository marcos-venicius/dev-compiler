using DC.CodeAnalysis.Enums;
using DC.CodeAnalysis.Syntax;

namespace DC.CodeAnalysis.Binding;

internal sealed class Binder
{
    private readonly List<string> _diagnostics = new();

    public IEnumerable<string> Diagnostics => _diagnostics;

    public BoundExpression BindExpression(ExpressionSyntax syntax)
    {
        return syntax.Kind switch
        {
            SyntaxKind.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax)syntax),
            SyntaxKind.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax)syntax),
            SyntaxKind.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax)syntax),
            _ => throw new Exception($"Unexpected syntax {syntax.Kind}"),
        };
    }

    private static BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
    {
        var value = syntax.LiteralToken.Value as int? ?? 0;

        return new BoundLiteralExpression(value);
    }

    private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
    {
        var boundOperand = BindExpression(syntax.Operand);
        var boundOperatorKind = BindUnaryOperatorKind(syntax.OperatorToken.Kind, boundOperand.Type);

        if (boundOperatorKind is null)
        {
            _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}");

            return boundOperand;
        }

        return new BoundUnaryExpression(boundOperatorKind.Value, boundOperand);
    }

    private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
    {
        var boundLeft = BindExpression(syntax.Left);
        var boundRight = BindExpression(syntax.Right);
        var boundOperatorKind = BindBinaryOperatorKind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

        if (boundOperatorKind is null)
        {
            _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}");

            return  boundLeft;
        }

        return new BoundBinaryExpression(boundLeft, boundOperatorKind.Value, boundRight);
    }

    private static BoundUnaryOperandKind? BindUnaryOperatorKind(SyntaxKind kind, Type operandType)
    {
        if (operandType != typeof(int)) return null;

        return kind switch
        {
            SyntaxKind.PlusToken => BoundUnaryOperandKind.Identity,
            SyntaxKind.MinusToken => BoundUnaryOperandKind.Negation,
            _ => throw new Exception($"Unexpected unary operator {kind}"),
        };
    }

    private static BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType)
    {
        if (leftType != typeof(int) || rightType != typeof(int))
            return null;

        return kind switch
        {
            SyntaxKind.PlusToken => BoundBinaryOperatorKind.Addition,
            SyntaxKind.MinusToken => BoundBinaryOperatorKind.Subtraction,
            SyntaxKind.StarToken => BoundBinaryOperatorKind.Multiplication,
            SyntaxKind.SlashToken => BoundBinaryOperatorKind.Division,
            _ => throw new Exception($"Unexpected binary operator {kind}"),
        };
    }
}