using System;
using System.Collections.Generic;
using QL.Core;

namespace QL.Protocol
{
    /// <summary>
    /// QL API: client.get.download.url
    /// </summary>
    public class ClientGetDownloadUrlRequest : BaseQLRequest<QL.Protocol.ClientGetDownloadUrlResponse>
    {
        /// <summary>
        /// 区服Id
        /// </summary>
        public long ZoneId { get; set; }

        #region IQLRequest Members

        public override string GetApiName()
        {
            return "client.get.download.url";
        }

        public override IDictionary<string, string> GetParameters()
        {
            QLDictionary parameters = new QLDictionary();
            parameters.Add("zone_id", this.ZoneId);
            return parameters;
        }

        public override void Validate()
        {
        }
        #endregion
    }
}
