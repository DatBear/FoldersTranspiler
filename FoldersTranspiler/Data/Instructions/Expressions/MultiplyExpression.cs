using FoldersTranspiler.Enums;

namespace FoldersTranspiler.Data.Instructions.Expressions;

public class MultiplyExpression : BinaryExpression
{
    public MultiplyExpression(BaseExpression leftExpression, BaseExpression rightExpression) : base(ExpressionType.Multiply, leftExpression, rightExpression)
    {
    }
}