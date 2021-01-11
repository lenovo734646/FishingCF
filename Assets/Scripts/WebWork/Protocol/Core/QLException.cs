using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QL.Core
{
    /// <summary>
    /// QL客户端异常。
    /// </summary>
    public class QLException : Exception
    {
        private string code_;
        private string msg_;

        public QLException()
            : base()
        {
        }

        public QLException(string message)
            : base(message)
        {
        }

        public QLException(string code, string msg)
            : base(code + ":" + msg)
        {
            this.code_ = code;
            this.msg_ = msg;
        }

        public string ErrorCode
        {
            get { return this.code_; }
        }

        public string ErrorMsg
        {
            get { return this.msg_; }
        }
    }
}
