using FoldersTranspiler.Data.Instructions.Expressions;
using FoldersTranspiler.Enums;
using FoldersTranspiler.Extensions;

namespace FoldersTranspiler.Data.Instructions.Commands;

public class WhileCommand : BaseCommand
{
    private readonly BaseExpression _expression;
    private readonly List<BaseCommand> _commands;

    public WhileCommand(BaseExpression expression, List<BaseCommand> commands) : base(CommandType.While)
    {
        _expression = expression;
        _commands = commands;
    }

    public override void Create(string dir)
    {
        Path.Combine(dir, "1").CreateSubFolders((int)Type);
        _expression.Create(Path.Combine(dir, "2"));
        var idx = 1;
        foreach (var command in _commands)
        {
            command.Create(Path.Combine(dir, "3", $"{idx++}"));
        }
    }
}