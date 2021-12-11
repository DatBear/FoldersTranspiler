using FoldersTranspiler.Enums;

namespace FoldersTranspiler.Data.Instructions.Expressions;

public class EqualToExpression : BinaryExpression
{
    public EqualToExpression(BaseExpression leftExpression, BaseExpression rightExpression) : base(ExpressionType.EqualTo, leftExpression, rightExpression)
    {
    }
}