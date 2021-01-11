using System;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using XLua;

namespace XLuaExtension
{
    public class DownloadLuaBehaviour : BaseLuaBehaviour<DownloadLuaBehaviour>
    {
        private Action<LuaTable, float> luaOnProgress;
        private Action<LuaTable, bool, int, string> luaOnDone;

        private UnityWebRequest _request;
        private string m_filePath;
        private int m_firstBytePos = 0;
        private int m_downloaded = 0;
        private int m_totalSize = 0;
        private int m_contentLength = 0;
        private int m_dataLength = 0;

        public override void Init()
        {
            self.Get("OnProgress", out luaOnProgress);
            self.Get("OnDone", out luaOnDone);
        }

        public UnityWebRequest GetRawRequest()
        {
            return _request;
        }

        public void StartDownload(string url, string path, int firstBytePos = -1, int streamFragmentSize = 0, int totalSize = 0)
        {
            Clear();

            m_totalSize = totalSize;
            m_filePath = path;

            StartCoroutine(_Start(url, firstBytePos, streamFragmentSize));
        }

        public void StopDownload()
        {
            StopAllCoroutines();

            if (_request == null)
                return;

            _request.Abort();
            _request.Dispose();
            _request = null;
        }

        public void Clear()
        {
            StopDownload();

            m_filePath = null;
            m_firstBytePos = 0;
            m_downloaded = 0;
            m_totalSize = 0;
            m_contentLength = 0;
            m_dataLength = 0;
        }

        private IEnumerator _Start(string url, int firstBytePos, int streamFragmentSize)
        {
            _request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            _request.disposeDownloadHandlerOnDispose = true;
            m_firstBytePos = firstBytePos;
            if (m_firstBytePos < 0)
            {
                _request.downloadHandler = new DownloadHandlerBuffer();

                if (luaOnProgress != null)
                {
                    _request.SendWebRequest();
                    yield return _OnProgress();
                }
                else
                {
                    yield return _request.SendWebRequest();
                }

                OnDone(false);
            }
            else
            {
                _request.chunkedTransfer = true;
                if (m_firstBytePos > 0)
                {
                    _request.SetRequestHeader("Range", "bytes=" + firstBytePos + "-");
                }

                var downloadHandler = new DownloadHandler(DownloadCallback, streamFragmentSize);
                downloadHandler.ContentLengthHandler = ReceiveContentLength;
                _request.downloadHandler = downloadHandler;

                yield return _request.SendWebRequest();

                OnDone(true, (int)HTTPStates.Finished);
            }
        }

        private IEnumerator _OnProgress()
        {
            while (!_request.isDone)
            {
                yield return null;

                if (luaOnProgress != null)
                    luaOnProgress(self, _request.downloadProgress);
            }
        }

        private void OnDone(bool isBreakpoint, int custom_code = -1)
        {
            var result = !_request.isNetworkError && string.IsNullOrEmpty(_request.error)
                && (_request.responseCode == 200 || _request.responseCode == 206);

            if (result && !isBreakpoint)
            {
                string dir = Path.GetDirectoryName(m_filePath);

                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (!string.IsNullOrEmpty(m_filePath))
                    File.WriteAllBytes(m_filePath, _request.downloadHandler.data);
            }

            var error_string = _request.error;
            var responseCode = (int)_request.responseCode;

            _request.Dispose();
            _request = null;

            if (luaOnDone != null)
                luaOnDone(self, result, custom_code >= 0 ? custom_code : responseCode, error_string);
        }

        private void ReceiveContentLength(int contentLength)
        {
            m_contentLength = contentLength;
        }

        private void DownloadCallback(byte[] data, int dataLength)
        {
            if (_request == null)
                return;

            using (FileStream fs = new FileStream(m_filePath, FileMode.Append))
            {
                fs.Write(data, 0, dataLength);
            }

            m_dataLength = dataLength;
            m_downloaded += dataLength;

            float totalSize = Math.Max(m_totalSize, m_contentLength + m_firstBytePos);
            if (luaOnProgress != null)
                luaOnProgress(self, (m_firstBytePos + m_downloaded) / totalSize);
            if (luaOnDone != null)
                luaOnDone(self, true, (int)HTTPStates.Processing, null);
        }

        public int GetDownloadLength()
        {
            if (_request == null)
                return -1;

            return m_dataLength;
        }

        private enum HTTPStates
        {
            Processing = 0,
            Finished = 1,
        }

        private class DownloadHandler : DownloadHandlerScript
        {
            private readonly Action<byte[], int> dataHandler;

            public DownloadHandler(Action<byte[], int> dataHandler, int bytesLength) : base(new byte[bytesLength])
            {
                this.dataHandler = dataHandler;
            }

            public Action<int> ContentLengthHandler { private get; set; }

            protected override bool ReceiveData(byte[] data, int dataLength)
            {
                if (data == null || data.Length < 1)
                    return false;

                if (dataHandler != null)
                    dataHandler(data, dataLength);

                return true;
            }

            protected override void ReceiveContentLength(int contentLength)
            {
                if (ContentLengthHandler != null)
                    ContentLengthHandler(contentLength);
            }
        }
    }
}

