#region License
// Copyright © 2018 Darko Jurić
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Net;
using System.Text;

namespace SimpleHttp
{
    /// <summary>
    /// Class containing HTTP response extensions.
    /// </summary>
    public static partial class ResponseExtensions
    {
        #region Response extensions (with)

        //partly according to: https://williambert.online/2013/06/allow-cors-with-localhost-in-chrome/
        /// <summary>
        /// Sets response headers to enable CORS.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <returns>Modified HTTP response.</returns>
        public static HttpListenerResponse WithCORS(this HttpListenerResponse response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response), "Response must not be null.");

            response.WithHeader("Access-Control-Allow-Origin", "*");
            response.WithHeader("Access-Control-Allow-Headers", "Cache-Control, Pragma, Accept, Origin, Authorization, Content-Type, X-Requested-With");
            response.WithHeader("Access-Control-Allow-Methods", "GET, POST");
            response.WithHeader("Access-Control-Allow-Credentials", "true");

            return response;
        }

        /// <summary>
        /// Sets the content-type for the response.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="contentType">HTTP content-type.</param>
        /// <returns>Modified HTTP response.</returns>
        public static HttpListenerResponse WithContentType(this HttpListenerResponse response, string contentType)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (contentType == null)
                throw new ArgumentNullException(nameof(contentType));

            response.ContentType = contentType;
            return response;
        }

        /// <summary>
        /// Sets the specified header for the response.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="name">Header name.</param>
        /// <param name="value">Header value.</param>
        /// <returns>Modified HTTP response.</returns>
        public static HttpListenerResponse WithHeader(this HttpListenerResponse response, string name, string value)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(name));

            switch (name)
            {
                case "content-length":
                    Int32.TryParse(value, out int vInt);
                    response.ContentLength64 = vInt;
                    break;
                case "content-type":
                    response.ContentType = value;
                    break;
                case "keep-alive":
                    Boolean.TryParse(value, out bool vBool);
                    response.KeepAlive = vBool;
                    break;
                case "transfer-encoding":
                    if (value.Contains("chunked")) throw new ArgumentException(nameof(name), "Use 'SendChunked' property instead.");
                    else response.Headers[name] = value;
                    break;
                default:
                    response.Headers[name] = value;
                    break;
            }

            return response;
        }

        /// <summary>
        /// Sets the status code for the response.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="statusCode">HTTP status code.</param>
        /// <returns>Modified HTTP response.</returns>
        public static HttpListenerResponse WithCode(this HttpListenerResponse response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            response.StatusCode = (int)statusCode;
            return response;
        }

        /// <summary>
        /// Sets the cookie for the response.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="name">Cookie name.</param>
        /// <param name="value">Cookie value.</param>
        /// <returns>Modified HTTP response.</returns>
        public static HttpListenerResponse WithCookie(this HttpListenerResponse response, string name, string value)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            response.Cookies.Add(new Cookie(name, value));
            return response;
        }

        /// <summary>
        /// Sets the cookie for the response.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="name">Cookie name.</param>
        /// <param name="value">Cookie value.</param>
        /// <param name="expires">Cookie expiration date (UTC).</param>
        /// <returns>Modified HTTP response.</returns>
        public static HttpListenerResponse WithCookie(this HttpListenerResponse response, string name, string value, DateTime expires)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            response.Cookies.Add(new Cookie { Name = name, Value = value, Expires = expires });
            return response;
        }

        /// <summary>
        /// Sets the cookie for the response.
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="cookie">Cookie.</param>
        /// <returns>Modified HTTP response.</returns>
        public static HttpListenerResponse WithCookie(this HttpListenerResponse response, Cookie cookie)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (cookie == null)
                throw new ArgumentNullException(nameof(cookie));

            response.Cookies.Add(cookie);
            return response;
        }

        #endregion

        #region Response extensions (As)

        /// <summary>
        /// Writes the specified data to the response.
        /// <para>Response is closed and can not be longer modified.</para>
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="txt">Text data to write.</param>
        /// <param name="mime">Mime type.</param>
        public static void AsText(this HttpListenerResponse response, string txt, string mime = "text/html")
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (txt == null)
                throw new ArgumentNullException(nameof(txt));

            if (mime == null)
                throw new ArgumentNullException(nameof(mime));
            try
            {
                var data = Encoding.ASCII.GetBytes(txt);
                response.ContentLength64 = data.Length;
                if (response.StatusCode == 404)
                {
                    mime = "text/html";
                    Console.WriteLine("404: " + response.StatusDescription);
                }
                response.ContentType = mime;
                response.OutputStream.Write(data, 0, data.Length);
            }
            catch (ArgumentException ex)
            {
                // 处理异常，例如记录日志或设置错误响应
                Console.WriteLine($"mime类型:{mime},不对;txt：{txt} " + ex.Message);
                // 根据需要设置适当的HTTP响应状态码
                response.StatusCode = 400; // Bad Request
            }

            /*
             * 以下是一些常见的 MIME 类型：

text/html：HTML 文档
text/plain：纯文本
text/css：CSS 文件
application/javascript：JavaScript 文件
image/jpeg：JPEG 图像
image/png：PNG 图像
image/gif：GIF 图像
application/pdf：PDF 文件
application/json：JSON 数据
application/xml：XML 数据
multipart/form-data：用于表单数据的多部分类型
             */
        }

        /// <summary>
        /// Builds a redirect response.
        /// <para>Response is closed and can not be longer modified.</para>
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="url">A new location (URL).</param>
        public static void AsRedirect(this HttpListenerResponse response, string url)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (url == null)
                throw new ArgumentNullException(nameof(url));


            response.StatusCode = (int)HttpStatusCode.Redirect;
            response.RedirectLocation = url;
            response.Close();
        }

        #endregion
    }
}
