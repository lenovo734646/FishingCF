using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QL.Core
{
    public class QLFileItem
    {
        private string FileName;
        private string MimeType;
        private byte[] Content;
        private FileInfo fileinfo_;

        /// <summary>
        /// 基于本地文件的构造器。
        /// </summary>
        /// <param name="fileInfo">本地文件</param>
        public QLFileItem(FileInfo fileinfo)
        {
            if (fileinfo == null || !fileinfo.Exists)
            {
                throw new ArgumentException("fileInfo is null or not exists!");
            }
            this.fileinfo_ = fileinfo;
        }

        /// <summary>
        /// 基于本地文件全路径的构造器。
        /// </summary>
        /// <param name="filePath">本地文件全路径</param>
        public QLFileItem(string filepath)
            : this(new FileInfo(filepath))
        { }

        /// <summary>
        /// 基于文件名和字节流的构造器。
        /// </summary>
        /// <param name="fileName">文件名称（服务端持久化字节流到磁盘时的文件名）</param>
        /// <param name="content">文件字节流</param>
        public QLFileItem(string filename, byte[] content)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException("fileName");
            if (content == null || content.Length == 0) throw new ArgumentNullException("content");

            this.FileName = filename;
            this.Content = content;
        }

        /// <summary>
        /// 基于文件名、字节流和媒体类型的构造器。
        /// </summary>
        /// <param name="fileName">文件名（服务端持久化字节流到磁盘时的文件名）</param>
        /// <param name="content">文件字节流</param>
        /// <param name="mimeType">媒体类型</param>
        public QLFileItem(string filename, byte[] content, string mimetype)
            : this(filename, content)
        {
            if (string.IsNullOrEmpty(mimetype)) throw new ArgumentNullException("mimeType");
            this.MimeType = mimetype;
        }

        public string GetFileName()
        {
            if (this.FileName == null && this.fileinfo_ != null && this.fileinfo_.Exists)
            {
                this.FileName = this.fileinfo_.FullName;
            }
            return this.FileName;
        }

        public string GetMimeType()
        {
            if (this.MimeType == null)
            {
                this.MimeType = QLUtil.GetMimeType(GetContent());
            }
            return this.MimeType;
        }

        public byte[] GetContent()
        {
            if (this.Content == null && this.fileinfo_ != null && this.fileinfo_.Exists)
            {
                using (System.IO.Stream fs = this.fileinfo_.OpenRead())
                {
                    this.Content = new byte[fs.Length];
                    fs.Read(Content, 0, Content.Length);
                }
            }

            return this.Content;
        }
    }
}
