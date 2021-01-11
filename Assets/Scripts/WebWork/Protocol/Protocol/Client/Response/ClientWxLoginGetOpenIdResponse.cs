using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using QL.Core;

namespace QL.Protocol
{
    /// <summary>
    /// QL API: client.wx.login.get.open.id
    /// </summary>
    public class ClientWxLoginGetOpenIdResponse : QLResponse
    {
        /// <summary>
        /// 授权用户唯一标识
        /// </summary>
        [XmlElement("open_id")]
        public string OpenId { get; set; }

    }
}
