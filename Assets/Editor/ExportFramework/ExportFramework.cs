using System.IO;
using UnityEditor;
using UnityEngine;

public class ExportFramework
{
    public const string FrameworkFilePath = "Assets/Editor/ExportFramework/Framework.txt";

    [MenuItem("Tools/Framework/Check")]
    public static void Check()
    {
        string[] lines = File.ReadAllLines(FrameworkFilePath);
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            if (!Directory.Exists(line) && !File.Exists(line))
                Debug.LogError(line + " not exist!");
        }
    }

    [MenuItem("Tools/Framework/Export")]
    public static void Export()
    {
        string[] lines = File.ReadAllLines(FrameworkFilePath);
        AssetDatabase.ExportPackage(lines, "MyFramework.unitypackage",
            ExportPackageOptions.Recurse |
            ExportPackageOptions.IncludeDependencies |
            ExportPackageOptions.Interactive);
    }
}
