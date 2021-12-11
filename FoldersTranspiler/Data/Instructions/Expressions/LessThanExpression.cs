using FoldersTranspiler.Enums;

namespace FoldersTranspiler.Data.Instructions.Expressions;

public class LessThanExpression : BinaryExpression
{
    public LessThanExpression(BaseExpression leftExpression, BaseExpression rightExpression) : base(ExpressionType.LessThan, leftExpression, rightExpression)
    {
    }
}