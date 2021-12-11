using FoldersTranspiler.Converters;
using FoldersTranspiler.Data;
using FoldersTranspiler.Data.Instructions.Commands;
using FoldersTranspiler.Data.Instructions.Expressions;
using FoldersTranspiler.Enums;
using FoldersTranspiler.Services;


public class Program
{
    public static void Main(string[] args)
    {
        if (!AreArgsValid(args))
        {
            OutputHelpText();
            return;
        }

        var inputFile = args[0];
        var outputFolder = args[1];
        var programText = File.ReadAllText(inputFile);
        
        SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);
        CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

        var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
        var mainMethod = methods.FirstOrDefault(x => x.Identifier.ValueText == "Main");
        var argsParam = mainMethod?.ParameterList.Parameters.FirstOrDefault()?.Type;
        var isValidEntryPoint = argsParam != null && argsParam.IsKind(SyntaxKind.ArrayType) && (((ArrayTypeSyntax)argsParam).ElementType as PredefinedTypeSyntax)?.Keyword.ValueText == "string";

        if (mainMethod?.Body == null || !isValidEntryPoint)
        {
            Console.WriteLine($"Error: Invalid program - Could not find a valid entry point...");
            return;
        }

        var transpiler = new TranspilerService();
        try
        {
            var program = transpiler.CreateProgram(outputFolder, mainMethod.Body.Statements);
            if (program != null)
            {
                Console.WriteLine($"Successfully transpiled {inputFile} to {outputFolder}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: Uncaught exception {e}");
        }
    }

    private static void OutputHelpText()
    {
        Console.WriteLine($"Usage: ");
        Console.WriteLine($"FoldersTranspiler inputFile outputFolder");
        Console.WriteLine($"\tinputFile\t\tpath to input .cs file, e.g. HelloWorld.cs");
        Console.WriteLine($"\toutputFolder\t\tpath to output Folders program to");
    }

    private static bool AreArgsValid(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine($"Error: incorrect number of arguments...");
            return false;
        }

        var inputFile = args[0];
        var outputFolder = args[1];

        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Error: could not find inputFile.");
            return false;
        }

        try
        {
            Directory.CreateDirectory(outputFolder);
        }
        catch
        {
            Console.WriteLine($"Error: unable to create directory {outputFolder}");
            return false;
        }
        
        return true;
    }
}




