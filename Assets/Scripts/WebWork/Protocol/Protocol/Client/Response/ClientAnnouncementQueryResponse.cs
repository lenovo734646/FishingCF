using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using QL.Core;

namespace QL.Protocol
{
    /// <summary>
    /// QL API: client.announcement.query
    /// </summary>
    public class ClientAnnouncementQueryResponse : QLResponse
    {
        /// <summary>
        /// 公告标题
        /// </summary>
        [XmlElement("announcement_title")]
        public string AnnouncementTitle { get; set; }

        /// <summary>
        /// 公告内容
        /// </summary>
        [XmlElement("announcement_content")]
        public string AnnouncementContent { get; set; }

        /// <summary>
        /// 公告发布时间
        /// </summary>
        [XmlElement("announcement_create_time")]
        public string AnnouncementCreateTime { get; set; }

    }
}
