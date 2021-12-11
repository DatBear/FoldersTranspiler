using FoldersTranspiler.Enums;
using FoldersTranspiler.Extensions;

namespace FoldersTranspiler.Data.Instructions.Expressions;

public class BinaryExpression : BaseExpression
{
    protected BaseExpression LeftExpression;
    protected BaseExpression RightExpression;

    public BinaryExpression(ExpressionType type, BaseExpression leftExpression, BaseExpression rightExpression) : base(type)
    {
        LeftExpression = leftExpression;
        RightExpression = rightExpression;
    }

    public override void Create(string dir)
    {
        Path.Combine(dir, "1").CreateSubFolders((int)Type);
        LeftExpression.Create(Path.Combine(dir, "2"));
        RightExpression.Create(Path.Combine(dir, "3"));
    }
}