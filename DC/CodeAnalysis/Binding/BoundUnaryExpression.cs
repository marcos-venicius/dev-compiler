namespace DC.CodeAnalysis.Binding;

internal sealed class BoundUnaryExpression : BoundExpression
{
    public BoundUnaryExpression(BoundUnaryOperandKind opeartorKind, BoundExpression operand)
    {
        OpeartorKind = opeartorKind;
        Operand = operand;
    }

    public override Type Type => Operand.Type;
    public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;

    public BoundUnaryOperandKind OpeartorKind { get; }
    public BoundExpression Operand { get; }
}
