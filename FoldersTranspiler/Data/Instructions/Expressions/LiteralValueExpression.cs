using FoldersTranspiler.Enums;
using FoldersTranspiler.Extensions;

namespace FoldersTranspiler.Data.Instructions.Expressions;

public class LiteralValueExpression : BaseExpression
{
    private int _intValue;
    private float _floatValue;
    private string _stringValue;
    private char _charValue;

    private TypeType _type;

    private readonly Dictionary<char, string> _charMap = new()
    {
        { ' ', "space" }
    };

    public LiteralValueExpression(int intValue) : base(ExpressionType.LiteralValue)
    {
        _intValue = intValue;
        _type = TypeType.Int;
    }

    /*
    public LiteralValueExpression(float floatValue) : base(ExpressionType.LiteralValue)
    {
        _floatValue = floatValue;
        _type = TypeType.Float;
    }
    */

    public LiteralValueExpression(string stringValue) : base(ExpressionType.LiteralValue)
    {
        _stringValue = stringValue;
        _type = TypeType.String;
    }

    public LiteralValueExpression(char charValue) : base(ExpressionType.LiteralValue)
    {
        _charValue = charValue;
        _type = TypeType.Char;
    }


    public override void Create(string dir)
    {
        int idx = 1;

        Path.Combine(dir, "1").CreateSubFolders(5);
        Path.Combine(dir, "2").CreateSubFolders((int)_type);
        dir = Path.Combine(dir, "3");

        switch (_type)
        {
            case TypeType.Int:
                var hexString = Convert.ToString(_intValue, 16);
                int intDigits = (int)Math.Floor(Math.Log10(hexString.Length) + 1);
                var value = _intValue;
                idx = hexString.Length;
                while (value > 0)
                {
                    var hexDigitBinary = Convert.ToString(value % 16, 2).PadLeft(4, '0');
                    var paths = hexDigitBinary.Select((x, xIdx) =>
                        Path.Combine(dir, $"{idx}", $"{xIdx + 1}", (x == '1' ? "1" : null) ?? string.Empty));
                    foreach (var path in paths)
                    {
                        Directory.CreateDirectory(path);
                    }

                    value /= 16;
                    idx--;
                }

                break;
            case TypeType.Float:
                throw
                    new NotImplementedException(); //todo implement this, but the spec doesn't specify how to specify floats
                break;
            case TypeType.String:
                int stringDigits = (int)Math.Floor(Math.Log10(_stringValue.Length) + 1);
                foreach (var c in _stringValue)
                {
                    var mappedC = _charMap.ContainsKey(c) ? _charMap[c] : c.ToString();
                    var charDir = Path.Combine(dir, idx.ToString($"D{stringDigits}") + " " + mappedC);
                    Directory.CreateDirectory(charDir);
                    charDir.CreateSubFolders(2);
                    var charDigits =
                        $"{Convert.ToString(c / 16, 2).PadLeft(4, '0')} {Convert.ToString(c % 16, 2).PadLeft(4, '0')}";
                    var paths = charDigits.Split(' ').SelectMany((x, xIdx) => x.Select((d, dIdx) =>
                        Path.Combine(charDir, $"{xIdx + 1}", $"{dIdx + 1}", (d == '1' ? "1" : null) ?? string.Empty)));

                    foreach (var path in paths)
                    {
                        Directory.CreateDirectory(path);
                    }

                    idx++;
                }

                break;
            case TypeType.Char:
                var mappedChar = _charMap.ContainsKey(_charValue) ? _charMap[_charValue] : _charValue.ToString();
                dir.CreateSubFolders(2);
                var charValueDigits =
                    $"{Convert.ToString(_charValue / 16, 2).PadLeft(4, '0')} {Convert.ToString(_charValue % 16, 2).PadLeft(4, '0')}";
                var charValuePaths = charValueDigits.Split(' ').SelectMany((x, xIdx) => x.Select((d, dIdx) =>
                    Path.Combine(dir, $"{xIdx + 1}", $"{dIdx + 1}", (d == '1' ? "1" : null) ?? string.Empty)));

                foreach (var path in charValuePaths)
                {
                    Directory.CreateDirectory(path);
                }

                break;
            default:
                throw new NotImplementedException($"Literal Value Expression {_type} not implemented.");
        }
    }
}