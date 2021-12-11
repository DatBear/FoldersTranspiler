using FoldersTranspiler.Enums;

namespace FoldersTranspiler.Data.Instructions.Expressions;

public abstract class BaseExpression
{
    public ExpressionType Type { get; set; }

    public BaseExpression(ExpressionType type)
    {
        Type = type;
    }

    public abstract void Create(string dir);
}