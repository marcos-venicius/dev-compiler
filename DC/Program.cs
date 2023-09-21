using DC.Enums;

namespace DC;

public sealed class Program
{
    public static void Main()
    {

        while (Repl())
            ;
            
    }

    public static bool Repl()
    {
        Console.Write("> ");

        var line = Console.ReadLine() ?? "";

        var lexer = new Lexer(line);

        while (true)
        {
            var token = lexer.NextToken();

            if (token.Kind == SyntaxKind.EndOfFileToken)
                break;

            Console.Write($"{token.Kind}: '{token.Text}'");

            if (token.Value is not null)
                Console.Write($" {token.Value}");

            Console.WriteLine();
        }

        return true;
    }
}