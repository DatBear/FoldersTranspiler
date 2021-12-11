using FoldersTranspiler.Enums;

namespace FoldersTranspiler.Data.Instructions.Expressions;

public class GreaterThanExpression : BinaryExpression
{
    public GreaterThanExpression(BaseExpression leftExpression, BaseExpression rightExpression) : base(ExpressionType.GreaterThan, leftExpression, rightExpression)
    {
    }
}