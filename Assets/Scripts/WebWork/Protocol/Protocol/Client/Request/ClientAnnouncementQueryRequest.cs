using System;
using System.Collections.Generic;
using QL.Core;

namespace QL.Protocol
{
    /// <summary>
    /// QL API: client.announcement.query
    /// </summary>
    public class ClientAnnouncementQueryRequest : BaseQLRequest<QL.Protocol.ClientAnnouncementQueryResponse>
    {
        /// <summary>
        /// 区服Id
        /// </summary>
        public long ZoneId { get; set; }

        /// <summary>
        /// 请求哪个语言的公告？CN代表中文
        /// </summary>
        public string Language { get; set; }

        #region IQLRequest Members

        public override string GetApiName()
        {
            return "client.announcement.query";
        }

        public override IDictionary<string, string> GetParameters()
        {
            QLDictionary parameters = new QLDictionary();
            parameters.Add("zone_id", this.ZoneId);
            parameters.Add("language", this.Language);
            return parameters;
        }

        public override void Validate()
        {
            QLRequestValidator.ValidateRequired("language", this.Language);
        }
        #endregion
    }
}
