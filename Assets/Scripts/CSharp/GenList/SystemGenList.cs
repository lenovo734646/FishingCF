using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

namespace XLuaExtension
{
    public static class SytemGenList
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(Directory),
            typeof(File),
            typeof(Path),
            typeof(Dictionary<int,string>),
            typeof(Dictionary<int,int>),
            typeof(DateTime),
            typeof(Array),
            typeof(SystemInfo),
            typeof(NetBinaryReaderProxy),
            typeof(NetBinaryWriterProxy),
        };

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {
            new List<string>(){ "System.IO.Directory", "CreateDirectory",
                "System.String", "System.Security.AccessControl.DirectorySecurity" },
            new List<string>(){ "System.IO.Directory", "SetAccessControl",
                "System.String", "System.Security.AccessControl.DirectorySecurity" },
            new List<string>(){ "System.IO.Directory", "GetAccessControl",
                "System.String" },
            new List<string>(){ "System.IO.Directory", "GetAccessControl",
                "System.String", "System.Security.AccessControl.AccessControlSections" },
            new List<string>(){ "System.IO.File", "Create", "System.String",
                "System.Int32", "System.IO.FileOptions" },
            new List<string>(){ "System.IO.File", "Create", "System.String",
                "System.Int32", "System.IO.FileOptions", "System.Security.AccessControl.FileSecurity" },
            new List<string>(){ "System.IO.File", "GetAccessControl", "System.String" },
            new List<string>(){ "System.IO.File", "GetAccessControl", "System.String",
                "System.Security.AccessControl.AccessControlSections" },
            new List<string>(){ "System.IO.File", "SetAccessControl", "System.String",
                "System.Security.AccessControl.FileSecurity" },
        };
    }
}

