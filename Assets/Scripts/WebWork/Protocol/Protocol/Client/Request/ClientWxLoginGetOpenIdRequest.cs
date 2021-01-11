using System;
using System.Collections.Generic;
using QL.Core;

namespace QL.Protocol
{
    /// <summary>
    /// QL API: client.wx.login.get.open.id
    /// </summary>
    public class ClientWxLoginGetOpenIdRequest : BaseQLRequest<QL.Protocol.ClientWxLoginGetOpenIdResponse>
    {
        /// <summary>
        /// 区服Id
        /// </summary>
        public long ZoneId { get; set; }

        /// <summary>
        /// 微信授权成功后返回的code串
        /// </summary>
        public string Code { get; set; }

        #region IQLRequest Members

        public override string GetApiName()
        {
            return "client.wx.login.get.open.id";
        }

        public override IDictionary<string, string> GetParameters()
        {
            QLDictionary parameters = new QLDictionary();
            parameters.Add("zone_id", this.ZoneId);
            parameters.Add("code", this.Code);
            return parameters;
        }

        public override void Validate()
        {
            QLRequestValidator.ValidateRequired("code", this.Code);
        }
        #endregion
    }
}
