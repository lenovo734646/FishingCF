using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QL.Core
{
    /// <summary>
    /// 常量类
    /// </summary>
    public sealed class QLConstants
    {
        /// <summary>
        /// 时间格式
        /// </summary>
        public const string DATE_FORMAT = "yyyy-MM-dd";
        /// <summary>
        /// 时间格式
        /// </summary>
        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 时间格式精确到毫秒
        /// </summary>
        public const string DATE_TIME_MS_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";
        /// <summary>
        /// 签名方式
        /// </summary>
        public const string SIGN_METHOD_MD5 = "md5";
        /// <summary>
        /// 签名字段
        /// </summary>
        public const string SIGN = "sign";
        /// <summary>
        /// 日志记录的换行标识
        /// </summary>
        public const string LOG_SPLIT = "\r\n";
        /// <summary>
        /// Http请求头参数
        /// </summary>
        public const string ACCEPT_ENCODING = "Accept-Encoding";
        /// <summary>
        /// Http请求头参数
        /// </summary>
        public const string CONTENT_ENCODING = "Content-Encoding";
        /// <summary>
        /// 压缩方式
        /// </summary>
        public const string CONTENT_ENCODING_GZIP = "gzip";
        /// <summary>
        /// 错误回应根节点名称
        /// </summary>
        public const string ERROR_RESPONSE = "error_response";
        /// <summary>
        /// 错误码字段名
        /// </summary>
        public const string ERROR_CODE = "code";
        /// <summary>
        /// 错误描述字段名称
        /// </summary>
        public const string ERROR_MSG = "msg";
        /// <summary>
        /// 回应格式
        /// </summary>
        public const string RESPONSE_FORMAT = "Xml";
    }
}
