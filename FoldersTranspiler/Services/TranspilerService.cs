using FoldersTranspiler.Converters;
using FoldersTranspiler.Data;
using FoldersTranspiler.Data.Instructions.Commands;

namespace FoldersTranspiler.Services;

public class TranspilerService
{
    public readonly Dictionary<string, int> VariableNames = new();

    public CommandConverter CommandConverter { get; set; }
    public ExpressionConverter ExpressionConverter { get; set; }
    public TypeConverter TypeConverter { get; set; }

    public TranspilerService()
    {
        CommandConverter = new(this);
        ExpressionConverter = new(this);
        TypeConverter = new(this);
    }

    public int DeclareVariable(string name)
    {
        VariableNames[name] = VariableNames.ContainsKey(name) ? VariableNames[name] : VariableNames.Count + 1;
        return VariableNames[name];
    }

    public int? GetVariableNumber(string? name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        return VariableNames.ContainsKey(name) ? VariableNames[name] : null;
    }

    public FoldersProgram CreateProgram(string dir, SyntaxList<StatementSyntax> statements)
    {
        var programCommands = CommandConverter.ConvertStatements(statements);

        if (programCommands.Any())
        {
            var program = new FoldersProgram(programCommands);
            program.Create(dir);
            return program;
        }

        return null;
    }
}