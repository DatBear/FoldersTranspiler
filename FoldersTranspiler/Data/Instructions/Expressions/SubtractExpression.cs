using FoldersTranspiler.Enums;
using FoldersTranspiler.Extensions;

namespace FoldersTranspiler.Data.Instructions.Expressions;

public class SubtractExpression : BinaryExpression
{
    public SubtractExpression(BaseExpression leftExpression, BaseExpression rightExpression) : base(ExpressionType.Subtract, leftExpression, rightExpression)
    {
    }
}