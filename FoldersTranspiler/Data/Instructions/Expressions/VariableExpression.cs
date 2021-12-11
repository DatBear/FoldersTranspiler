using FoldersTranspiler.Enums;
using FoldersTranspiler.Extensions;

namespace FoldersTranspiler.Data.Instructions.Expressions;

public class VariableExpression : BaseExpression
{
    private int _variableNum;
    public VariableExpression(int variableNum) : base(ExpressionType.Variable)
    {
        _variableNum = variableNum;
    }

    public override void Create(string dir)
    {
        Path.Combine(dir, "1").CreateSubFolders((int)Type);
        Path.Combine(dir, "2").CreateSubFolders(_variableNum);
    }
}