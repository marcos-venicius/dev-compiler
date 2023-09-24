using DC.CodeAnalysis.Binding;

namespace DC.CodeAnalysis;

internal sealed class Evaluator
{
    private readonly BoundExpression _root;

    public Evaluator(BoundExpression root)
    {
        _root = root;
    }

    public object Evaluate()
    {
        return EvaluateExpression(_root);
    }

    private object EvaluateExpression(BoundExpression node)
    {
        if (node is BoundLiteralExpression number)
            return number.Value;

        if (node is BoundUnaryExpression u)
        {
            var operand = (int)EvaluateExpression(u.Operand);

            return u.OpeartorKind switch
            {
                BoundUnaryOperandKind.Identity => operand,
                BoundUnaryOperandKind.Negation => -operand,
                _ => throw new Exception($"Unexpected unary operator {u.OpeartorKind}"),
            };
        }

        if (node is BoundBinaryExpression binary)
        {
            var left = (int)EvaluateExpression(binary.Left);
            var right = (int)EvaluateExpression(binary.Right);

            return binary.OperatorKind switch
            {
                BoundBinaryOperatorKind.Addition => left + right,
                BoundBinaryOperatorKind.Subtraction => left - right,
                BoundBinaryOperatorKind.Multiplication => left * right,
                BoundBinaryOperatorKind.Division => left / right,
                _ => throw new Exception($"Unexpected binary operator {binary.OperatorKind}")
            };
        }

        throw new Exception($"Unexpected node {node.Kind}");
    }
}
