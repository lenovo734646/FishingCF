using QL.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Serialization;

namespace QL.Parser
{
    /// <summary>
    /// QL XML响应通用解释器。
    /// </summary>
    public class QLXmlParser : IQLParser
    {
        private static readonly Regex regex = new Regex("<(\\w+?)[ >]", RegexOptions.Compiled);
        private static readonly ReaderWriterLock rwlock = new ReaderWriterLock();
        private static readonly Dictionary<string, XmlSerializer> parsers = new Dictionary<string, XmlSerializer>();

        #region IQLParser Members

        public T Parse<T>(string body) where T : QLResponse
        {
            Type type = typeof(T);
            string rootTagName = GetRootElement(body);

            string key = type.FullName;
            if (QLConstants.ERROR_RESPONSE.Equals(rootTagName))
            {
                key += ("_" + QLConstants.ERROR_RESPONSE);
            }

            XmlSerializer serializer = null;

            rwlock.AcquireReaderLock(50);
            try
            {
                if (rwlock.IsReaderLockHeld)
                    parsers.TryGetValue(key, out serializer);
            }
            finally
            {
                if (rwlock.IsReaderLockHeld)
                    rwlock.ReleaseReaderLock();
            }

            if (serializer == null)
            {
                XmlAttributes attrs = new XmlAttributes();
                attrs.XmlRoot = new XmlRootAttribute(rootTagName);

                XmlAttributeOverrides xao = new XmlAttributeOverrides();
                xao.Add(type, attrs);

                serializer = new XmlSerializer(type, xao);

                rwlock.AcquireWriterLock(50);
                try
                {
                    if (rwlock.IsWriterLockHeld)
                        parsers[key] = serializer;
                }
                finally
                {
                    if (rwlock.IsWriterLockHeld)
                        rwlock.ReleaseWriterLock();
                }
            }

            object obj = null;
            using (System.IO.Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(body)))
            {
                obj = serializer.Deserialize(stream);
            }

            T rsp = (T)obj;
            if (rsp != null)
            {
                rsp.Body = body;
            }
            return rsp;
        }

        #endregion

        /// <summary>
        /// 获取XML响应的根节点名称
        /// </summary>
        private string GetRootElement(string body)
        {
            Match match = regex.Match(body);
            if (match.Success)
            {
                return match.Groups[1].ToString();
            }
            else
            {
                throw new QLException("Invalid XML response format!");
            }
        }
    }
}
