using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QL.Core
{
    /// <summary>
    /// QL请求接口。
    /// </summary>
    public interface IQLRequest<T> where T : QLResponse
    {
        /// <summary>
        /// 获取QL请求的API名称。
        /// </summary>
        string GetApiName();

        /// <summary>
        /// 获取自定义HTTP请求头参数。
        /// </summary>
        IDictionary<string, string> GetHeaderParameters();

        /// <summary>
        /// 获取所有的Key-Value形式的文本请求参数字典。
        /// </summary>
        IDictionary<string, string> GetParameters();

        /// <summary>
        /// 客户端参数检查，减少服务端无效调用。
        /// </summary>
        void Validate();
    }
}
