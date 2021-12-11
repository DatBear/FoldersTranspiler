namespace FoldersTranspiler.Extensions;

public static class StringExtensions
{
    public static void CreateSubFolders(this string dir, int count)
    {
        Directory.CreateDirectory(dir);
        for (var i = 1; i <= count; i++)
        {
            Directory.CreateDirectory(Path.Combine(dir, i.ToString()));
        }
    }
}