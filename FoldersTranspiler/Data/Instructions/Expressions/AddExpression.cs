using FoldersTranspiler.Enums;
using FoldersTranspiler.Extensions;

namespace FoldersTranspiler.Data.Instructions.Expressions;

public class AddExpression : BinaryExpression
{
    public AddExpression(BaseExpression leftExpression, BaseExpression rightExpression) : base(ExpressionType.Add, leftExpression, rightExpression)
    {
    }
}