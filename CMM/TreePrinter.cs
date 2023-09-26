using CMM.CodeAnalysis.Syntax;

namespace CMM;

internal sealed class TreePrinter
{
    private readonly string _tab = "    ";

    public void Print(SyntaxNode root)
    {
        PrettyPrint(root);
    }

    private void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
    {
        var marker = isLast ? "└── " : "├── ";

        Console.Write(indent);
        Console.Write(marker);
        Console.Write(node.Kind);

        if (node is SyntaxToken token && token.Value is not null)
        {
            Console.Write(" ");
            Console.Write(token.Value);
        }

        Console.WriteLine();

        indent += isLast ? _tab : "│  ";

        var lastChild = node.GetChildren().LastOrDefault();

        foreach (var child in node.GetChildren())
            PrettyPrint(child, indent, child == lastChild);
    }
}
