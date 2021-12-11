using FoldersTranspiler.Enums;

namespace FoldersTranspiler.Data.Instructions.Expressions;

public class DivideExpression : BinaryExpression
{
    public DivideExpression(BaseExpression leftExpression, BaseExpression rightExpression) : base(ExpressionType.Divide, leftExpression, rightExpression)
    {
    }
}