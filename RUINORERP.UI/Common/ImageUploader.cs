using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public class ImageUploader
    {
        public static async Task UploadImageAsync(string filePath, string url)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    // 添加图片文件
                    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        content.Add(new StreamContent(stream), "file", Path.GetFileName(filePath));
                    }

                    // 可选：添加其他表单字段
                    content.Add(new StringContent("value"), "key");

                    try
                    {
                        // 发送 POST 请求
                        var response = await client.PostAsync(url, content);
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Image uploaded successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Error uploading image.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                    }
                }
            }
        }
    }
}

