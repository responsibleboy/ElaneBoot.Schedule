using ElaneBoot.Schedule.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ElaneBoot.Schedule
{
    internal enum HttpMethod
    {
        Get,
        Post,
        Put,
        Delete
    }

    internal class HttpHelper
    {

        #region MyRegion
        public static string GetStringAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return DoGet(HttpMethod.Get, uri, null, authorizationToken, authorizationMethod);
        }
        public static string DeleteAsync(string uri, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return DoGet(HttpMethod.Delete, uri, null, authorizationToken, authorizationMethod);
        }
        public static string PostAsync(string uri, object item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return DoPost(HttpMethod.Post, uri, item, authorizationToken, authorizationMethod);
        }
        public static string PostFormAsync(string uri, object item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return DoPost(HttpMethod.Post, uri, item, authorizationToken, authorizationMethod, "form");
        }
        public static string PutAsync(string uri, object item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return DoPost(HttpMethod.Put, uri, item, authorizationToken, authorizationMethod);
        }
        public static string PutFormAsync(string uri, object item, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            return DoPost(HttpMethod.Put, uri, item, authorizationToken, authorizationMethod, "form");
        }
        #endregion






        #region MyRegion
        static string charset = "utf-8";
        static int timeout = 5 * 60 * 1000;
        private static string DoPost(HttpMethod method, string url, object item, string authorizationToken, string authorizationMethod, string contentType = "json", bool isEncrypt = false)
        {
            HttpWebRequest req = GetWebRequest(url, method.ToString().ToUpper());
            req.ContentType = contentType == "form" ? "application/x-www-form-urlencoded;charset=" + charset : "application/json;charset=" + charset;


            if (item != null)
            {
                string parameters = null;
                if (contentType == "json")
                {
                    parameters = JsonHelper.Serialize(item);
                }
                else if (contentType == "form")
                {
                    parameters = BuildQuery((Dictionary<string, object>)item);
                }
                byte[] postData = Encoding.GetEncoding(charset).GetBytes(parameters);
                Stream reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();
                reqStream.Dispose();
            }
            else
            {
                byte[] postData = Encoding.GetEncoding(charset).GetBytes("");
                Stream reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();
                reqStream.Dispose();
            }

            //授权token
            if (!string.IsNullOrEmpty(authorizationToken))
            {
                var headers = req.Headers;
                headers["Authorization"] = authorizationMethod + " " + authorizationToken;
                //req.Headers = headers;
            }

            HttpWebResponse rsp = null;
            try
            {
                req.UserAgent = "ElaneClient";
                rsp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                    rsp = wex.Response as HttpWebResponse;
                else
                    throw new ApiException(wex.Message, -1, wex.Status.ToString());
            }

            return GetResponseAsString(rsp);
        }
        private static string DoGet(HttpMethod method, string url, object item, string authorizationToken, string authorizationMethod, string contentType = "json", bool isEncrypt = false)
        {
            if (item != null)
            {
                var parameters = BuildQuery((Dictionary<string, object>)item);
                if (url.Contains("?"))
                {
                    url = url + "&" + parameters;
                }
                else
                {
                    url = url + "?" + parameters;
                }
            }

            HttpWebRequest req = GetWebRequest(url, method.ToString().ToUpper());
            req.ContentType = contentType == "form" ? "application/x-www-form-urlencoded;charset=" + charset : "application/json;charset=" + charset;

            //授权token
            if (!string.IsNullOrEmpty(authorizationToken))
            {
                var headers = req.Headers;
                headers["Authorization"] = authorizationMethod + " " + authorizationToken;
                //req.Headers = headers;
            }

            HttpWebResponse rsp = null;
            try
            {
                req.UserAgent = "ElaneClient";
                rsp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                    rsp = wex.Response as HttpWebResponse;
                else
                    throw new ApiException(wex.Message, -1, wex.Status.ToString());

            }

            return GetResponseAsString(rsp);
        }
        private static string DoFileUpload(HttpMethod method, string url, string fileName, object item, string authorizationToken, string authorizationMethod)
        {
            //1.HttpWebRequest
            HttpWebRequest webRequest = GetWebRequest(url, method.ToString().ToUpper());
            FileInfo fileInfo = new FileInfo(fileName);

            // 边界符
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            // 开始边界符
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            // 结束结束符
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");
            var newLineBytes = Encoding.UTF8.GetBytes("\r\n");
            using (var stream = new MemoryStream())
            {
                // 写入开始边界符
                stream.Write(beginBoundary, 0, beginBoundary.Length);
                // 写入文件
                var fileHeader = "Content-Disposition: form-data; name=\"file\"; filename=\"" + fileInfo.Name + "\"\r\n" + "Content-Type: application/octet-stream\r\n\r\n";
                var fileHeaderBytes = Encoding.UTF8.GetBytes(fileHeader);
                stream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    byte[] byteArray = new byte[fs.Length];
                    fs.Read(byteArray, 0, byteArray.Length);
                    stream.Write(byteArray, 0, byteArray.Length);
                }
                stream.Write(newLineBytes, 0, newLineBytes.Length);
                // 写入字符串
                var keyValue = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
                if (item != null)
                {
                    var parameters = (Dictionary<string, object>)item;
                    foreach (string key in parameters.Keys)
                    {
                        var keyValueBytes = Encoding.UTF8.GetBytes(string.Format(keyValue, key, parameters[key]));
                        stream.Write(beginBoundary, 0, beginBoundary.Length);
                        stream.Write(keyValueBytes, 0, keyValueBytes.Length);
                    }
                }
                // 写入结束边界符
                stream.Write(endBoundary, 0, endBoundary.Length);
                webRequest.ContentLength = stream.Length;
                stream.Position = 0;
                var tempBuffer = new byte[stream.Length];
                stream.Read(tempBuffer, 0, tempBuffer.Length);
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(tempBuffer, 0, tempBuffer.Length);

                    //授权token
                    if (!string.IsNullOrEmpty(authorizationToken))
                    {
                        var headers = webRequest.Headers;
                        headers["Authorization"] = authorizationMethod + " " + authorizationToken;
                    }

                    HttpWebResponse rsp = null;
                    try
                    {
                        webRequest.UserAgent = "ElaneClient";
                        rsp = (HttpWebResponse)webRequest.GetResponse();
                    }
                    catch (WebException wex)
                    {
                        if (wex.Response != null)
                            rsp = wex.Response as HttpWebResponse;
                        else
                            throw new ApiException(wex.Message, -1, wex.Status.ToString());
                    }

                    return GetResponseAsString(rsp);
                }
            }
        }
        private static Stream DoFileDownload(HttpMethod method, string url, object item, string authorizationToken, string authorizationMethod, string contentType = "json", bool isEncrypt = false)
        {
            if (item != null)
            {
                var parameters = BuildQuery((Dictionary<string, object>)item);
                if (url.Contains("?"))
                {
                    url = url + "&" + parameters;
                }
                else
                {
                    url = url + "?" + parameters;
                }
            }

            HttpWebRequest req = GetWebRequest(url, method.ToString().ToUpper());
            req.ContentType = contentType == "form" ? "application/x-www-form-urlencoded;charset=" + charset : "application/json;charset=" + charset;

            //授权token
            if (!string.IsNullOrEmpty(authorizationToken))
            {
                var headers = req.Headers;
                headers["Authorization"] = authorizationMethod + " " + authorizationToken;
                //req.Headers = headers;
            }

            HttpWebResponse rsp = null;
            try
            {
                req.UserAgent = "ElaneClient";
                rsp = (HttpWebResponse)req.GetResponse();
                return rsp.GetResponseStream();
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    rsp = wex.Response as HttpWebResponse;
                    GetResponseAsString(rsp);
                }
                else
                    throw new ApiException(wex.Message, -1, wex.Status.ToString());
            }
            return null;
        }
        private static HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.Timeout = timeout;
            return req;
        }
        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        private static string GetResponseAsString(HttpWebResponse rsp)
        {
            var statusCode = rsp.StatusCode;
            StringBuilder result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, Encoding.GetEncoding(charset));

                // 按字符读取并写入字符串缓冲
                int ch = -1;
                while ((ch = reader.Read()) > -1)
                {
                    // 过滤结束符
                    char c = (char)ch;
                    if (c != '\0')
                    {
                        result.Append(c);
                    }
                }
            }
            finally
            {
                // 释放资源
                if (reader != null) { reader.Close(); reader.Dispose(); }
                if (stream != null) { stream.Close(); stream.Dispose(); }
                if (rsp != null) { rsp.Close(); /*rsp.Dispose();*/ }
            }

            var json = result.ToString();

            if (statusCode != HttpStatusCode.OK)
            {
                if (statusCode == HttpStatusCode.InternalServerError)
                {
                    if (!string.IsNullOrEmpty(json) && json.Contains("__out"))
                    {
                        var r = JsonHelper.Deserialize<Output>(json);
                        throw new ApiException(r.ErrMsg, r.ErrCode, r.Msg);
                    }
                }
                throw new ApiException(statusCode.ToString(), (int)statusCode);
            }
            else
            {
                if (json.Contains("__out"))
                {
                    var r = JsonHelper.Deserialize<Output>(json);
                    if (r.Code == 1)
                    {
                        json = JsonHelper.Serialize(r.Data);
                    }
                    else
                    {
                        throw new ApiException(r.ErrMsg, r.ErrCode, r.Msg);
                    }
                }
            }
            return json;
        }
        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        private static string BuildQuery(Dictionary<string, object> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, object>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                object value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && value != null)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");

                    postData.Append(UrlEncode(value?.ToString()));
                    hasParam = true;
                }
            }

            return postData.ToString();
        }
        /// <summary>
        /// URL编码（暂未实现）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string UrlEncode(string str)
        {
            return str;
            //#if NETSTANDARD2_0
            //            return WebUtility.UrlEncode(str);
            //#elif NET40
            //            return System.Web.HttpUtility.UrlEncode(str);
            //#endif
        }
        #endregion

    }
}
