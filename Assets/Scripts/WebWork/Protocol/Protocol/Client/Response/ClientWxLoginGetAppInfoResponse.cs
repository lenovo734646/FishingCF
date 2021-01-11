using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using QL.Core;

namespace QL.Protocol
{
    /// <summary>
    /// QL API: client.wx.login.get.app.info
    /// </summary>
    public class ClientWxLoginGetAppInfoResponse : QLResponse
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        [XmlElement("app_id")]
        public string AppId { get; set; }

        /// <summary>
        /// 应用授权作用域
        /// </summary>
        [XmlElement("scope")]
        public string Scope { get; set; }

    }
}
