using FoldersTranspiler.Enums;
using FoldersTranspiler.Services;

namespace FoldersTranspiler.Converters;

public class TypeConverter
{
    private readonly TranspilerService _transpiler;

    public TypeConverter(TranspilerService transpiler)
    {
        _transpiler = transpiler;
    }

    public TypeType? Convert(VariableDeclarationSyntax? syntax)
    {
        if (syntax == null) return null;
        return Convert(syntax.Type);
    }

    public TypeType? Convert(TypeSyntax? syntax)
    {
        if (syntax == null) return null;
        return Convert(syntax as PredefinedTypeSyntax);
    }

    private TypeType? Convert(PredefinedTypeSyntax? syntax)
    {
        if (syntax == null) return null;
        switch (syntax.Keyword.ValueText)
        {
            case "string":
                return TypeType.String;
            case "int":
                return TypeType.Int;
            case "char":
                return TypeType.Char;
            default:
                throw new NotSupportedException($"Unsupported type: {syntax.Keyword.ValueText}");
        }
    }
}