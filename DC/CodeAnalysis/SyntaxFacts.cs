using DC.CodeAnalysis.Enums;

namespace DC.CodeAnalysis;

internal static class SyntaxFacts
{
    public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.SlashToken or SyntaxKind.StarToken => 2,
            SyntaxKind.PlusToken or SyntaxKind.MinusToken => 1,
            _ => 0,
        };
    }
}