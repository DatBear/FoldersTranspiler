using FoldersTranspiler.Data.Instructions.Expressions;
using FoldersTranspiler.Enums;
using FoldersTranspiler.Extensions;

namespace FoldersTranspiler.Data.Instructions.Commands;

public class LetCommand : BaseCommand
{
    private readonly int _varNumber;
    private readonly BaseExpression _expression;


    public LetCommand(int varNumber, BaseExpression expression) : base(CommandType.Let)
    {
        _varNumber = varNumber;
        _expression = expression;
    }

    public override void Create(string dir)
    {
        Path.Combine(dir, "1").CreateSubFolders((int)Type);
        Path.Combine(dir, "2").CreateSubFolders(_varNumber);
        _expression.Create(Path.Combine(dir, "3"));
    }
}