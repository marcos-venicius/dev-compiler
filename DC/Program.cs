using DC.CodeAnalysis;

namespace DC;

public sealed class Program
{
    private static bool _showTree = false;

    public static void Main()
    {

        while (Repl())
            ;
    }

    private static bool Repl()
    {
        Console.Write("> ");

        var line = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(line))
            return true;

        if (line == "#showTree")
        {
            _showTree = !_showTree;

            Console.WriteLine(_showTree ? "Showing parse trees." : "Not showing parse trees.");

            return true;
        } else if (line == "#clear")
        {
            Console.Clear();

            return true;
        } else if (line == "#exit")
        {
            return false;
        }

        var syntaxTree = SyntaxTree.Parse(line);

        if (_showTree)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            PrettyPrint(syntaxTree.Root);
            Console.ResetColor();
        }

        if (syntaxTree.Diagnostics.Any())
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;

            foreach (var diagnostic in syntaxTree.Diagnostics)
                Console.WriteLine(diagnostic);

            Console.ResetColor();
        }
        else
        {
            var evaluator = new Evaluator(syntaxTree.Root);

            var result = evaluator.Evaluate();

            Console.WriteLine(result);
        } 

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