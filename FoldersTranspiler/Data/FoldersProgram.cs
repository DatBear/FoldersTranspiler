using FoldersTranspiler.Data.Instructions.Commands;

namespace FoldersTranspiler.Data;

public class FoldersProgram
{
    private List<BaseCommand> _commands;
    public FoldersProgram(List<BaseCommand> commands)
    {
        _commands = commands;
    }

    public void Create(string dir)
    {
        var idx = 1;
        foreach (var command in _commands)
        {
            command.Create(Path.Combine(dir, idx.ToString()));
            idx++;
        }
    }
}