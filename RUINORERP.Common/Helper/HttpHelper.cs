using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RUINORERP.Common.Helper
{
    /// <summary>
    /// httpclinet请求方式，请尽量使用IHttpClientFactory方式
    /// </summary>
    public class HttpHelper
    {
        public static async Task<string> DeleteImageAsync(string serviceAddress, string imagePath)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 0, 60);

                // 构建请求
                var request = new HttpRequestMessage(HttpMethod.Delete, serviceAddress);
                // 如果需要在请求中包含图片路径信息，可以通过请求头或请求内容添加
                request.Headers.TryAddWithoutValidation("ImagePath", imagePath);

                // 发送 DELETE 请求
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public static async Task<byte[]> DownloadImageAsync(string serviceAddress)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 0, 60);

                // 发送 GET 请求
                var response = await httpClient.GetAsync(serviceAddress);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<string> UploadImageAsync(string serviceAddress, string imagePath)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 0, 60);

                // 创建 MultipartFormDataContent 对象
                using var content = new MultipartFormDataContent();

                // 读取图片文件并添加到 MultipartFormDataContent
                using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                var byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, byteArray.Length);
                content.Add(new ByteArrayContent(byteArray), "file", Path.GetFileName(imagePath));

                // 发送 POST 请求
                var response = await httpClient.PostAsync(serviceAddress, content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<string> GetAsync(string serviceAddress)
        {
            try
            {
                string result = string.Empty;
                Uri getUrl = new Uri(serviceAddress);
                using var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 0, 60);
                result = await httpClient.GetAsync(serviceAddress).Result.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public static async Task<string> PostAsync(string serviceAddress, string requestJson = null)
        {
            try
            {
                string result = string.Empty;
                Uri postUrl = new Uri(serviceAddress);

                using (HttpContent httpContent = new StringContent(requestJson))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    using var httpClient = new HttpClient();
                    httpClient.Timeout = new TimeSpan(0, 0, 60);
                    result = await httpClient.PostAsync(serviceAddress, httpContent).Result.Content.ReadAsStringAsync();
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }


}
