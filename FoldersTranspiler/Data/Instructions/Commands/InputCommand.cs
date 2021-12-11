using FoldersTranspiler.Enums;
using FoldersTranspiler.Extensions;

namespace FoldersTranspiler.Data.Instructions.Commands;

public class InputCommand : BaseCommand
{
    private readonly int _varNumber;

    public InputCommand(int varNumber) : base(CommandType.Input)
    {
        _varNumber = varNumber;
    }

    public override void Create(string dir)
    {
        Path.Combine(dir, "1").CreateSubFolders((int)Type);
        Path.Combine(dir, "2").CreateSubFolders(_varNumber);
    }
}