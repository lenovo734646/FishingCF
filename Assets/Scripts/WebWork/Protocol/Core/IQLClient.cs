using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QL.Core
{
    /// <summary>
    /// QL客户端。
    /// </summary>
    public interface IQLClient
    {
        QLResponseFormat Format { get; set; }

        string ServerUrl { get; set; }

        bool UseGZipEncoding { get; set; }

        int Timeout { get; set; }

        int ReadWriteTimeout { get; set; }

        /// <summary>
        /// 执行QL公开API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">具体的QL API请求</param>
        /// <returns>领域对象</returns>
        T Execute<T>(IQLRequest<T> request) where T : QLResponse;

        /// <summary>
        /// 执行QL隐私API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">具体的QL API请求</param>
        /// <param name="session">用户会话码</param>
        /// <returns>领域对象</returns>
        T Execute<T>(IQLRequest<T> request, string session) where T : QLResponse;

        /// <summary>
        /// 执行QL隐私API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">具体的QL API请求</param>
        /// <param name="session">用户会话码</param>
        /// <param name="timestamp">请求时间戳</param>
        /// <returns>领域对象</returns>
        T Execute<T>(IQLRequest<T> request, string session, DateTime timestamp) where T : QLResponse;
    }
}
