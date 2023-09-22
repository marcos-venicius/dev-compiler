namespace DC;

public sealed class Program
{
    public static void Main()
    {

        while (Repl())
            ;
    }

    private static bool Repl()
    {
        Console.Write("> ");

        var line = Console.ReadLine() ?? "";

        var parser = new Parser(line);

        var expression = parser.Parse();

        Console.ForegroundColor = ConsoleColor.DarkGray;

        PrettyPrint(expression);

        Console.ResetColor();

        return true;
    }

    private static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
    {
        // └── 
        // ├──
        // │

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

        indent += isLast ? "    " : "│   ";

        var lastChild = node.GetChildren().LastOrDefault();

        foreach (var child in node.GetChildren())
            PrettyPrint(child, indent, child == lastChild);
    }
}