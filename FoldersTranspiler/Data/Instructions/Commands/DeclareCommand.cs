using FoldersTranspiler.Enums;
using FoldersTranspiler.Extensions;

namespace FoldersTranspiler.Data.Instructions.Commands;

public class DeclareCommand : BaseCommand
{
    private int _variableNum;
    private TypeType _variableType;


    public DeclareCommand(int variableNum, TypeType variableType) : base(CommandType.Declare)
    {
        _variableNum = variableNum;
        _variableType = variableType;
    }

    public override void Create(string dir)
    {
        Path.Combine(dir, "1").CreateSubFolders((int)Type);
        Path.Combine(dir, "2").CreateSubFolders((int)_variableType);
        Path.Combine(dir, "3").CreateSubFolders(_variableNum);
    }
}