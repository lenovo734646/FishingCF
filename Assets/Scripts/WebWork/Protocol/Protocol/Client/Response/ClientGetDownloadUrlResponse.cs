using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using QL.Core;

namespace QL.Protocol
{
    /// <summary>
    /// QL API: client.get.download.url
    /// </summary>
    public class ClientGetDownloadUrlResponse : QLResponse
    {
        /// <summary>
        /// 阿里云oss区域Url地址
        /// </summary>
        [XmlElement("oss_url")]
        public string OssUrl { get; set; }

        /// <summary>
        /// 客户端配置表md5值
        /// </summary>
        [XmlElement("client_config_pkg_md5")]
        public string ClientConfigPkgMd5 { get; set; }

    }
}
