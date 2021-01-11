using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using QL.Parser;

namespace QL.Core
{
    public class DefaultQLClient : IQLClient
    {
        public const string APP_KEY = "app_key";
        public const string SIGN = "sign";
        public const string SIGN_METHOD = "sign_method";
        public const string FORMAT = "format";
        public const string METHOD = "method";
        public const string TIMESTAMP = "timestamp";
        public const string SDK_VERSION = "sdk_version";
        public const string SESSION = "session";
        public const string SDK_VERSION_VALUE = "ql-sdk-20160128";

        public string Key { get; set; }
        public string Password { get; set; }
        private QLWebClient client_;

        public QLResponseFormat Format { get; set; }

        public string ServerUrl { get; set; }

        public bool UseGZipEncoding { get; set; }

        public DefaultQLClient()
        {
            
            ServerUrl = "http://47.104.147.168:8000/router/rest"; //fg
            //ServerUrl = "http://39.101.175.23:8000/router/rest";
            client_ = new QLWebClient();
            Format = QLResponseFormat.Json;
        }

        public DefaultQLClient(string key, string password)
            : this()
        {
            Key = key;
            Password = password;
        }

        public int Timeout
        {
            get { return client_.Timeout; }
            set { client_.Timeout = value; }
        }

        public int ReadWriteTimeout
        {
            get { return client_.ReadWriteTimeout; }
            set { client_.ReadWriteTimeout = value; }
        }

        #region IQLClient Members

        public T Execute<T>(IQLRequest<T> request) where T : QLResponse
        {
            return Execute<T>(request, null);
        }

        public T Execute<T>(IQLRequest<T> request, string session) where T : QLResponse
        {
            return Execute<T>(request, session, DateTime.Now);
        }

        public T Execute<T>(IQLRequest<T> request, string session, DateTime timestamp) where T : QLResponse
        {
            return DoExecute<T>(request, session, timestamp);
        }

        #endregion

        private T DoExecute<T>(IQLRequest<T> request, string session, DateTime timestamp) where T : QLResponse
        {
            // 提前检查业务参数
            try
            {
                request.Validate();
            }
            catch (QLException e)
            {
                return CreateErrorResponse<T>(e.ErrorCode, e.ErrorMsg);
            }

            // 添加协议级请求参数
            QLDictionary txtParams = new QLDictionary(request.GetParameters());
            txtParams.Add(METHOD, request.GetApiName());
            txtParams.Add(SIGN_METHOD, QLConstants.SIGN_METHOD_MD5);
            txtParams.Add(APP_KEY, Key);
            txtParams.Add(FORMAT, Format.ToString());
            txtParams.Add(SDK_VERSION, SDK_VERSION_VALUE);
            txtParams.Add(TIMESTAMP, timestamp);
            txtParams.Add(SESSION, session);

            // 添加签名参数
            txtParams.Add(SIGN, QLUtil.SignRequestByMd5Method(txtParams, Password));

            // 添加头部参数
            if (this.UseGZipEncoding)
            {
                request.GetHeaderParameters()[QLConstants.ACCEPT_ENCODING] = QLConstants.CONTENT_ENCODING_GZIP;
            }

            try
            {
                string body;
                if (request is IQLUploadRequest<T>) // 是否需要上传文件
                {
                    IQLUploadRequest<T> uRequest = (IQLUploadRequest<T>)request;
                    body = client_.DoPost(
                        ServerUrl,
                        txtParams,
                        uRequest.GetFileParameters(),
                        request.GetHeaderParameters()
                        );
                }
                else
                {
                    body = client_.DoPost(
                        ServerUrl,
                        txtParams,
                        request.GetHeaderParameters()
                        );
                }

                // 解释响应结果
                T rsp;
                if (Format == QLResponseFormat.Xml)
                {
                    IQLParser tp = new QLXmlParser();
                    rsp = tp.Parse<T>(body);
                }
                else
                {
                    IQLParser tp = new QLJsonParser();
                    rsp = tp.Parse<T>(body);
                }

                return rsp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private T CreateErrorResponse<T>(string code, string msg) where T : QLResponse
        {
            T rsp = Activator.CreateInstance<T>();
            rsp.ErrCode = code;
            rsp.ErrMsg = msg;

            if (Format == QLResponseFormat.Xml)
            {
                XmlDocument root = new XmlDocument();
                XmlElement bodyE = root.CreateElement(QLConstants.ERROR_RESPONSE);
                XmlElement codeE = root.CreateElement(QLConstants.ERROR_CODE);
                codeE.InnerText = code;
                bodyE.AppendChild(codeE);
                XmlElement msgE = root.CreateElement(QLConstants.ERROR_MSG);
                msgE.InnerText = msg;
                bodyE.AppendChild(msgE);
                root.AppendChild(bodyE);
                rsp.Body = root.OuterXml;
            }
            else
            {
                rsp.Body = JsonConvert.SerializeObject(
                    new Dictionary<string, object>()
                    {
                        {QLConstants.ERROR_RESPONSE, new Dictionary<string, object>()
                            {
                                {QLConstants.ERROR_CODE, code},
                                {QLConstants.ERROR_MSG, msg},
                            }
                        },
                    });
            }
            return rsp;
        }
    }
}
