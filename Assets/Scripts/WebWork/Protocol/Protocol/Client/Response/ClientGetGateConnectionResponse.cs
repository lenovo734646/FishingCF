using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using QL.Core;

namespace QL.Protocol
{
    /// <summary>
    /// QL API: client.get.gate.connection
    /// </summary>
    public class ClientGetGateConnectionResponse : QLResponse
    {
        /// <summary>
        /// ip地址
        /// </summary>
        [XmlElement("ip")]
        public string Ip { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        [XmlElement("port")]
        public long Port { get; set; }

    }
}
