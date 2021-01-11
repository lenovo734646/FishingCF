using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QL.Core
{
    /// <summary>
    /// 基础QL请求类，存放一些通用的请求参数。
    /// </summary>
    public abstract class BaseQLRequest<T> : IQLRequest<T> where T : QLResponse
    {
        /// <summary>
        /// HTTP请求头参数
        /// </summary>
        public QLDictionary HeaderParams { get; set; }

        /// <summary>
        /// 添加请求头信息
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void AddHeaderParameter(string key, string value)
        {
            GetHeaderParameters().Add(key, value);
        }

        /// <summary>
        /// 获取自定义HTTP请求头参数。
        /// </summary>
        /// <returns>请求头列表</returns>
        public IDictionary<string, string> GetHeaderParameters()
        {
            if (this.HeaderParams == null)
            {
                this.HeaderParams = new QLDictionary();
            }
            return this.HeaderParams;
        }

        /// <summary>
        /// 获取QL请求的API名称。
        /// </summary>
        /// <returns></returns>
        public abstract string GetApiName();

        /// <summary>
        /// 客户端参数检查，减少服务端无效调用。
        /// </summary>
        public abstract void Validate();

        /// <summary>
        /// 获取所有的Key-Value形式的文本请求参数字典。
        /// </summary>
        /// <returns>请求参数列表</returns>
        public abstract IDictionary<string, string> GetParameters();
    }
}
