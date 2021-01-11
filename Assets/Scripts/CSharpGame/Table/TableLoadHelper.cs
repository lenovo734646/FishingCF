using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class TableLoadHelper
{
    public static TableLoadConfig config;
    private static Dictionary<string, TableFieldMapping> dataTypeMappings_;

    public static readonly string LocalPackageFileName = "config.pkg";
    private static string localPackageMd5_;

    class TableLoadTask
    {
        public string Url { get; set; }
        public string RemoteMd5 { get; set; }
        public Action Callback { get; set; }
    }
    private static List<TableLoadTask> taskList_;

    private static Dictionary<string, string> tableMd5Cache_;

    private static Dictionary<string, string> dicConfig;

    static TableLoadHelper()
    {
        config = new TableLoadConfig();
        dataTypeMappings_ = new Dictionary<string, TableFieldMapping>();
        localPackageMd5_ = string.Empty;
        taskList_ = new List<TableLoadTask>();
        tableMd5Cache_ = new Dictionary<string, string>();

    }

    /// <summary>
    /// 根据本地数据包加载配置文件，优先加载本地package文件
    /// 如果不存在则尝试从包内Resource目录下加载json文件
    /// </summary>
    public static void LoadFromLocalPackage()
    {
        Dictionary<string, string> vFiles = null;

        string path = $"{Application.dataPath}/{LocalPackageFileName}";
        if (File.Exists(path))
        {
            byte[] content = File.ReadAllBytes(path);
            localPackageMd5_ = _mdContent(content);
            vFiles = _parsePackageContent(content);
        }

        _loadJsonImpl(vFiles, false);
    }

    private static Dictionary<string, string> _parsePackageContent(byte[] fileContent)
    {
        Dictionary<string, string> vFiles = new Dictionary<string, string>();
        try
        {
            byte[] content = _processGZipDecode(fileContent);
            MemoryStream ms = new MemoryStream(content);
            BinaryReader br = new BinaryReader(ms);

            // ************ PKG FILE FORMAT ************
            // ZIP-BEG
            //
            // version: 
            //          version         1byte   cur:1
            // [REPEAT]:
            //          tableName       nbytes  utf8string
            //          tableContent    nbytes  utf8string
            // ZIP-END

            //version
            byte version = br.ReadByte();
            if (version == 1)
            {
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    var key = _readUtf8String(br);
                    var val = _readUtf8String(br);
                    vFiles[key] = val;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"加载config.pkg失败：{ex.Message}");
        }
        return vFiles;
    }

    /// <summary>
    /// 根据远程网络数据加载配置表
    /// </summary>
    public static void LoadFromNet(string ossUrl, string remoteMd5, Action callback = null)
    {
        if (string.IsNullOrEmpty(ossUrl) || string.IsNullOrEmpty(remoteMd5) || localPackageMd5_.Equals(remoteMd5, StringComparison.CurrentCultureIgnoreCase))
        {
            callback?.Invoke();
            return;
        }
            
        var url = $"{ossUrl}/{SysDefines.ZoneId}/Table/Data/config.pkg";
        bool isEmpty = taskList_.Count == 0;
        //添加到队列
        taskList_.Add(new TableLoadTask()
        {
            Url = url,
            RemoteMd5 = remoteMd5,
            Callback = callback,
        });
        if (isEmpty)
            _asyncLoadFromNet();
    }

    private static async void _asyncLoadFromNet()
    {
        if (taskList_.Count == 0)
            return;

        var task = taskList_.First();

        byte[] content = null;
        string tmpMd5 = string.Empty;
        var errmsg = string.Empty;

        var savePath = $"{Application.dataPath}/{LocalPackageFileName}";

        await Task.Run(() =>
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(task.Url) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();

                var ms = new MemoryStream();

                byte[] bArr = new byte[4096];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    ms.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }

                content = ms.ToArray();
                tmpMd5 = _mdContent(content);
                
                File.WriteAllBytes(savePath, content);
            }
            catch (System.Exception ex)
            {
                errmsg = ex.Message;
            }
        });

        //从队列移除
        taskList_.RemoveAt(0);

        do
        {
            if (!string.IsNullOrEmpty(errmsg))
            {
                Debug.LogError($"下载配置文件失败 URL:{task.Url} msg:{errmsg}");
                break;
            }

            if (!task.RemoteMd5.Equals(tmpMd5, StringComparison.CurrentCultureIgnoreCase))
            {
                Debug.LogError($"下载到的config.pkg文件md5与远程不一致！ URL:{task.Url} remote:{task.RemoteMd5} local:{tmpMd5}");
                break;
            }

            localPackageMd5_ = tmpMd5;
            var vFiles = _parsePackageContent(content);
            dicConfig = vFiles;
            if (vFiles != null && vFiles.Count > 0)
                _loadJsonImpl(vFiles, true);

        } while (false);

        //执行回调
        task.Callback?.Invoke();

        //递归继续做任务
        _asyncLoadFromNet();
    }

    /// <summary>
    /// 加载json配置文件，初始化相关内存对象
    /// <param name="onlyLoadFromNet">是否仅加载网络存在的配置表</param>
    /// </summary>
    public static void _loadJsonImpl(Dictionary<string, string> vFiles, bool onlyLoadFromNet)
    {
        foreach (var t in config.TableHelperTypeList)
        {
#if LOCAL_JSON
            vFiles = null;
#endif
            var tableName = t.GetField("TableName").GetValue(null) as string;
            var tableType = t.GetField("TableType").GetValue(null) as Type;
            
            string fileContent = null;
            if (vFiles != null)
                vFiles.TryGetValue(tableName, out fileContent);
            if (string.IsNullOrEmpty(fileContent) && onlyLoadFromNet)
                continue;

            if (fileContent == null)
                fileContent = Resources.Load<TextAsset>(config.LocalJsonDirectory + tableName).text;

            string tmpMd5 = _mdString(fileContent);
            if (tmpMd5 == _getCacheMd5ForTable(tableName))
                continue;

            _setCacheMd5ForTable(tableName, tmpMd5);

            var lst = _loadJsonFromContent(fileContent, tableType);

            var method = t.GetMethod("LoadData");
            method.Invoke(null, new object[] { lst });
        }
    }

    private static List<object> _loadJsonFromInnerPath(string filename, Type dataType)
    {
        return _loadJsonFromContent(Resources.Load<TextAsset>(config.LocalJsonDirectory + filename).text, dataType);
    }

    private static List<object> _loadJsonFromContent(string content, Type dataType)
    {
        var arr = JsonConvert.DeserializeObject(content) as JArray;
        return arr.Select(a => _createObjectByJToken(a, dataType)).ToList();
    }

    private static object _createObjectByJToken(JToken token, Type dataType)
    {
        TableFieldMapping map = null;
        if (!dataTypeMappings_.TryGetValue(dataType.FullName, out map))
        {
            map = new TableFieldMapping(dataType);
            dataTypeMappings_[dataType.FullName] = map;
        }

        var r = Activator.CreateInstance(dataType);

        foreach (var prop in map.PropertiesMap.Values)
        {
            JToken t = token[prop.ItemName];
            if (t == null || t.Type == JTokenType.None || t.Type == JTokenType.Null)
                continue;

            try
            {
                _setObjectPropertyValue(r, prop, t);
            }
            catch
            {
            }
        }

        return r;
    }

    private static void _setObjectPropertyValue(object obj, TableFieldProperty prop, JToken token)
    {
        object val = null;
        if (prop.ItemType == typeof(int))
        {
            val = token.ToObject<int>();
        }
        else if (prop.ItemType == typeof(long))
        {
            val = token.ToObject<long>();
        }
        else if (prop.ItemType == typeof(double))
        {
            val = token.ToObject<double>();
        }
        else if (prop.ItemType == typeof(JArray))
        {
            val = JsonConvert.DeserializeObject(token.ToObject<string>()) as JArray;
        }
        else if (prop.ItemType == typeof(string))
        {
            val = token.ToObject<string>();
        }
        prop.Method.Invoke(obj, new object[] { val });
    }


    /// <summary>
    /// GZip解压缩
    /// </summary>
    /// <param name="content">GZip压缩过的内容</param>
    /// <returns></returns>
    private static byte[] _processGZipDecode(byte[] content)
    {
        var wms = new MemoryStream();

        var ms = new MemoryStream(content, 0, content.Length);
        ms.Seek(0, SeekOrigin.Begin);
        var zip = new GZipStream(ms, CompressionMode.Decompress, true);

        var buf = new byte[4096];
        int n;
        while ((n = zip.Read(buf, 0, buf.Length)) != 0)
        {
            wms.Write(buf, 0, n);
        }

        zip.Close();
        ms.Close();

        var r = new byte[wms.Length];
        Array.Copy(wms.GetBuffer(), r, wms.Length);
        return r;
    }

    /// <summary>
    /// 从二进制流中读取utf8字符串
    /// </summary>
    /// <param name="br"></param>
    /// <returns></returns>
    private static string _readUtf8String(BinaryReader br)
    {
        MemoryStream ms = new MemoryStream();
        while (true)
        {
            byte a = br.ReadByte();
            if (a == 0)
                break;
            ms.WriteByte(a);
        }
        return Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
    }

    private static string _mdContent(byte[] content)
    {
        MD5 md5 = MD5.Create();
        byte[] bytes = md5.ComputeHash(content);
        
        StringBuilder result = new StringBuilder();
        foreach (byte v in bytes)
            result.Append(v.ToString("X2"));

        return result.ToString();
    }

    private static string _mdString(string str)
    {
        return _mdContent(Encoding.UTF8.GetBytes(str));
    }

    private static string _getCacheMd5ForTable(string tableName)
    {
        string r = string.Empty;
        lock (tableMd5Cache_)
        {
            tableMd5Cache_.TryGetValue(tableName, out r);
        }
        return r;
    }

    private static void _setCacheMd5ForTable(string tableName, string md5)
    {
        lock (tableMd5Cache_)
        {
            tableMd5Cache_[tableName] = md5;
        }
    }

    public static string LoadConfigByTableName(string fileName) {
        string file = string.Empty;
        if (dicConfig.ContainsKey(fileName))
            file = dicConfig[fileName];
        else
            Debug.LogError("LoadConfigByTableName  file Not Found : " + fileName);
        return file;
    }
}
