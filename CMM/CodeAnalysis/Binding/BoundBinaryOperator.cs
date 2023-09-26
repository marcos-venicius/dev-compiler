using CMM.CodeAnalysis.Enums;

namespace CMM.CodeAnalysis.Binding;

internal sealed class BoundBinaryOperator
{
    private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type)
    : this(syntaxKind, kind, type, type, type)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
    }

    private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type operandType, Type resultType)
    : this(syntaxKind, kind, operandType, operandType, resultType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
    }

    private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        LeftType = leftType;
        RightType = rightType;
        Type = resultType;
    }

    private static readonly BoundBinaryOperator[] _operators = {
        new (SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int)),
        new (SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int)),
        new (SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(int)),
        new (SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(int)),

        new (SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equals, typeof(int), typeof(bool)),
        new (SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, typeof(int), typeof(bool)),

        new (SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
        new (SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),

        new (SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equals, typeof(bool)),
        new (SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, typeof(bool)),
    };

    public SyntaxKind SyntaxKind { get; }
    public BoundBinaryOperatorKind Kind { get; }
    public Type LeftType { get; }
    public Type RightType { get; }
    public Type Type { get; }

    public static BoundBinaryOperator? Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
    {
        foreach (var op in _operators)
            if (op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType)
                return op;

        return null;
    }
}
