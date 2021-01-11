using System.IO;
using UnityEngine;

public class LuaFileLoader
{
    private string _searchPath;

    public LuaFileLoader(string searchPath)
    {
        _searchPath = searchPath;
    }

    public byte[] LoadFile(ref string filePath)
    {
#if UNITY_EDITOR
        var path = StringUtil.Concat(_searchPath, filePath.Replace('.', '/'), ".lua");
        if (File.Exists(path))
            return File.ReadAllBytes(path);
        return null;
#else
        Debug.LogError(filePath);
        return null;
#endif
    }
}