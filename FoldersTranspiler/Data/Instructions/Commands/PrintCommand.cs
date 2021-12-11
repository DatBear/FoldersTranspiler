using FoldersTranspiler.Data.Instructions.Expressions;
using FoldersTranspiler.Enums;
using FoldersTranspiler.Extensions;

namespace FoldersTranspiler.Data.Instructions.Commands;

public class PrintCommand : BaseCommand {
    public BaseExpression Expression { get; set; }

    public PrintCommand(BaseExpression expression) : base(CommandType.Print)
    {
        Expression = expression;
    }

    public override void Create(string dir)
    {
        Path.Combine(dir, "1").CreateSubFolders((int)Type);
        Expression.Create(Path.Combine(dir, "2"));
    }
}