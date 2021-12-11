using FoldersTranspiler.Enums;

namespace FoldersTranspiler.Data.Instructions.Commands;

public abstract class BaseCommand
{
    public CommandType Type { get; set; }

    public BaseCommand(CommandType type)
    {
        Type = type;
    }

    public abstract void Create(string dir);
}