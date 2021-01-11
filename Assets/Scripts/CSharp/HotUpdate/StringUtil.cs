using System.Text;

public class StringUtil
{
    private static readonly StringBuilder _sb = new StringBuilder();

    public static string Concat(string param1, string param2)
    {
        _sb.Length = 0;
        _sb.Append(param1);
        _sb.Append(param2);

        var ret = _sb.ToString();
        _sb.Length = 0;
        return ret;
    }

    public static string Concat(string param1, string param2, string param3)
    {
        _sb.Length = 0;
        _sb.Append(param1);
        _sb.Append(param2);
        _sb.Append(param3);

        var ret = _sb.ToString();
        _sb.Length = 0;
        return ret;
    }

    public static string Concat(string param1, string param2, string param3, string param4)
    {
        _sb.Length = 0;
        _sb.Append(param1);
        _sb.Append(param2);
        _sb.Append(param3);
        _sb.Append(param4);

        var ret = _sb.ToString();
        _sb.Length = 0;
        return ret;
    }
}