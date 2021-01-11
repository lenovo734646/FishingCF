using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaBundleLoader
{
    private static readonly char[] key = "Secret".ToCharArray();

    private readonly string _searchPath;
    private readonly Context _context;

    public LuaBundleLoader(string searchPath, Context context)
    {
        _searchPath = searchPath;
        _context = context;
    }

    public byte[] LoadFile(ref string filepath)
    {
        var assetPath = StringUtil.Concat(_searchPath, "/", filepath.Replace(".", "/"), ".lua.bytes");
        var textAsset = _context.Loader.LoadAsset<TextAsset>(assetPath, false);
        if (textAsset == null)
            return null;

        var bytes = Decrypt(textAsset.bytes);
        Resources.UnloadAsset(textAsset);

        return bytes;
    }

    public static byte[] Encrypt(byte[] bytes)
    {
        var len = key.Length;
        for (int i = 0; i < bytes.Length; i++)
        {
            var j = i % len;
            bytes[i] ^= (byte)key[j];
        }
        return bytes;
    }

    public static byte[] Decrypt(byte[] bytes)
    {
        return Encrypt(bytes);
    }
}
