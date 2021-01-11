using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class AssetUtil
{
    public static string ReadFile(string path)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string header = Application.streamingAssetsPath + "/";
        if (path.StartsWith(header))
        {
            try
            {
                object[] args = { path.Substring(header.Length) };
                AndroidJavaClass jc = new AndroidJavaClass("com.");
                IntPtr methodID = AndroidJNIHelper.GetMethodID<byte[]>(jc.GetRawClass(), "readAsset", args, true);
                jvalue[] array = AndroidJNIHelper.CreateJNIArgArray(args);
                try
                {
                    IntPtr array2 = AndroidJNI.CallStaticObjectMethod(jc.GetRawClass(), methodID, array);
                    if (array2 != IntPtr.Zero)
                    {
                        byte[] ret = AndroidJNIHelper.ConvertFromJNIArray<byte[]>(array2);
                        return Encoding.UTF8.GetString(ret);
                    }
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(args, array);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return null;
        }
#endif
        return File.ReadAllText(path);
    }
}