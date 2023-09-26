using Comma.CodeAnalysis.Enums;

namespace Comma.CodeAnalysis.Binding;

internal sealed class BoundUnaryOperator
{
    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType) : this(syntaxKind, kind, operandType, operandType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        OperandType = operandType;
    }

    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        OperandType = operandType;
        Type = resultType;
    }

    public SyntaxKind SyntaxKind { get; }
    public BoundUnaryOperatorKind Kind { get; }
    public Type OperandType { get; }
    public Type Type { get; }

    private static BoundUnaryOperator[] _operators = {
        new (SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
        new (SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
        new (SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),
    };

    public static BoundUnaryOperator? Bind(SyntaxKind syntaxKind, Type operandKind)
    {
        foreach (var op in _operators)
            if (op.SyntaxKind == syntaxKind && op.OperandType == operandKind)
                return op;

        return null;
    }
}
