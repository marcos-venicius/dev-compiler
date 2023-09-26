using Comma.CodeAnalysis;
using Comma.CodeAnalysis.Syntax;

namespace CMM;

internal static class Program
{
    private static bool _showTree = false;

    private readonly static string _banner = """
 _____     ______     __   __      ______     ______     __    __     ______   __     __         ______     ______    
/\  __-.  /\  ___\   /\ \ / /     /\  ___\   /\  __ \   /\ "-./  \   /\  == \ /\ \   /\ \       /\  ___\   /\  == \   
\ \ \/\ \ \ \  __\   \ \ \'/      \ \ \____  \ \ \/\ \  \ \ \-./\ \  \ \  _-/ \ \ \  \ \ \____  \ \  __\   \ \  __<   
 \ \____-  \ \_____\  \ \__|       \ \_____\  \ \_____\  \ \_\ \ \_\  \ \_\    \ \_\  \ \_____\  \ \_____\  \ \_\ \_\ 
  \/____/   \/_____/   \/_/         \/_____/   \/_____/   \/_/  \/_/   \/_/     \/_/   \/_____/   \/_____/   \/_/ /_/
""";

    public static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(_banner);
        Console.WriteLine();
        Console.ResetColor();

        var treePrinter = new TreePrinter();

        while (Repl(treePrinter))
            ;
    }

    private static bool Repl(TreePrinter treePrinter)
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
        }
        else if (line == "#clear")
        {
            Console.Clear();

            return true;
        }
        else if (line == "#exit")
        {
            return false;
        }

        var syntaxTree = SyntaxTree.Parse(line);

        var binder = new Compilation(syntaxTree);

        var evaluationResult = binder.Evaluate();

        if (_showTree)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            treePrinter.Print(syntaxTree.Root);
            Console.ResetColor();
        }

        if (evaluationResult.Diagnostics.Any())
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;

            foreach (var diagnostic in evaluationResult.Diagnostics)
                Console.WriteLine(diagnostic);

            Console.ResetColor();
        }
        else
            Console.WriteLine(evaluationResult.Value);

        return true;
    }
}