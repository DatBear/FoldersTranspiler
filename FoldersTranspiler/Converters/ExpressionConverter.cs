using FoldersTranspiler.Data.Instructions.Commands;
using FoldersTranspiler.Data.Instructions.Expressions;
using FoldersTranspiler.Services;

namespace FoldersTranspiler.Converters;

public class ExpressionConverter
{
    private readonly TranspilerService _transpiler;

    public ExpressionConverter(TranspilerService transpiler)
    {
        _transpiler = transpiler;
    }

    public BaseExpression? ConvertExpression(ExpressionSyntax? syntax)
    {
        if (syntax == null) return null;
        var expression = ConvertIdentifier(syntax as IdentifierNameSyntax);
        expression ??= ConvertLiteral(syntax as LiteralExpressionSyntax);
        expression ??= ConvertBinaryExpression(syntax as BinaryExpressionSyntax);
        return expression;
    }

    public BinaryExpression ConvertBinaryExpression(BinaryExpressionSyntax? syntax)
    {
        if (syntax == null) return null;

        var kind = syntax.Kind();
        var left = ConvertExpression(syntax.Left);
        var right = ConvertExpression(syntax.Right);

        if (left == null || right == null) throw new NotImplementedException($"Unable to convert expression: {syntax}");

        Dictionary<SyntaxKind, Func<BaseExpression, BaseExpression, BinaryExpression>> kindExpressions = new()
        {
            { SyntaxKind.AddExpression,         (l, r) => new AddExpression(l, r) },
            { SyntaxKind.SubtractExpression,    (l, r) => new SubtractExpression(l, r) },
            { SyntaxKind.MultiplyExpression,    (l, r) => new MultiplyExpression(l, r) },
            { SyntaxKind.DivideExpression,      (l, r) => new DivideExpression(l, r) },
            { SyntaxKind.GreaterThanExpression, (l, r) => new GreaterThanExpression(l, r) },
            { SyntaxKind.LessThanExpression,    (l, r) => new LessThanExpression(l, r) },
            { SyntaxKind.EqualsExpression,      (l, r) => new EqualToExpression(l, r) },
        };

        if (!kindExpressions.ContainsKey(syntax.Kind())) throw new NotImplementedException($"Unable to convert expression, unsupported expression type: {syntax}");

        return kindExpressions[syntax.Kind()](left, right);
    }
    
    public BaseExpression? ConvertIdentifier(IdentifierNameSyntax? identifier)
    {
        if (identifier == null) return null;
        var varName = identifier.Identifier.ValueText;
        if (!string.IsNullOrEmpty(varName))
        {
            if (!_transpiler.VariableNames.ContainsKey(varName))
            {
                _transpiler.VariableNames[varName] = _transpiler.VariableNames.Count + 1;
            }

            return new VariableExpression(_transpiler.VariableNames[varName]);
        }

        return null;
    }

    public BaseExpression? ConvertLiteral(LiteralExpressionSyntax? syntax)
    {
        if(syntax == null) return null;
        if (syntax.Token.Value == null) throw new NotImplementedException($"null assignment not possible");

        switch (syntax.Kind())
        {
            case SyntaxKind.StringLiteralExpression:
                return new LiteralValueExpression((string)syntax.Token.Value);
            case SyntaxKind.NumericLiteralExpression:
                return new LiteralValueExpression((int)syntax.Token.Value);
            case SyntaxKind.CharacterLiteralExpression:
                return new LiteralValueExpression((char)syntax.Token.Value);
                break;
        }
        return null;
    }


}