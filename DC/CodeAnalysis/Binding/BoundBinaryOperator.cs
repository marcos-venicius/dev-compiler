using DC.CodeAnalysis.Enums;

namespace DC.CodeAnalysis.Binding;

internal sealed class BoundBinaryOperator
{
    private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type)
    : this(syntaxKind, kind, type, type, type)
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
        ResultType = resultType;
    }

    private static readonly BoundBinaryOperator[] _operators = {
        new (SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int)),
        new (SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int)),
        new (SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(int)),
        new (SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(int)),

        new (SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
        new (SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
    };

    public SyntaxKind SyntaxKind { get; }
    public BoundBinaryOperatorKind Kind { get; }
    public Type LeftType { get; }
    public Type RightType { get; }
    public Type ResultType { get; }

    public static BoundBinaryOperator? Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
    {
        foreach (var op in _operators)
            if (op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType)
                return op;

        return null;
    }
}