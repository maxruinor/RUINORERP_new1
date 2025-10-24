using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using RUINORERP.UI.Network.Services;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Model.Context;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 图片管理助手类
    /// 提供简化的图片上传、下载、删除功能
    /// </summary>
    public static class ImageManagementHelper
    {
        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="image">要上传的图片</param>
        /// <param name="fileName">文件名</param>
        /// <param name="category">文件分类（如PaymentVoucher、ProductImage等）</param>
        /// <param name="appContext">应用程序上下文</param>
        /// <returns>文件ID，如果上传失败则返回null</returns>
        public static async Task<string> UploadImageAsync(System.Drawing.Image image, string fileName, string category, ApplicationContext appContext)
        {
            try
            {
                if (image == null || string.IsNullOrEmpty(fileName) || appContext == null)
                    return null;

                // 获取文件管理服务
                var fileService = appContext.GetRequiredService<FileManagementService>();
                
                // 将图片转换为字节数组
                byte[] imageBytes = UI.Common.ImageHelper.ImageToByteArray(image);
                
                // 根据分类上传图片
                FileUploadResponse response;
                switch (category.ToLower())
                {
                    case "paymentvoucher":
                        response = await fileService.UploadPaymentVoucherAsync(fileName, imageBytes);
                        break;
                    case "productimage":
                        response = await fileService.UploadProductImageAsync(fileName, imageBytes);
                        break;
                    case "bommanual":
                        response = await fileService.UploadBOMManualAsync(fileName, imageBytes);
                        break;
                    default:
                        // 使用通用上传方法
                        var request = new FileUploadRequest
                        {
                            FileName = fileName,
                            Category = category,
                            FileSize = imageBytes.Length,
                            Data = imageBytes
                        };
                        response = await fileService.UploadFileAsync(request);
                        break;
                }
                
                if (response.IsSuccess)
                {
                    return response.Data.FileId; // 返回文件ID
                }
                else
                {
                    // 记录错误日志
                    System.Diagnostics.Debug.WriteLine($"图片上传失败: {response.ErrorMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // 记录异常日志
                System.Diagnostics.Debug.WriteLine($"图片上传异常: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 下载图片文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="appContext">应用程序上下文</param>
        /// <returns>图片对象，如果下载失败则返回null</returns>
        public static async Task<System.Drawing.Image> DownloadImageAsync(string fileId, ApplicationContext appContext)
        {
            try
            {
                if (string.IsNullOrEmpty(fileId) || appContext == null)
                    return null;

                // 获取文件管理服务
                var fileService = appContext.GetRequiredService<FileManagementService>();
                
                // 创建下载请求
                var request = new FileDownloadRequest
                {
                    FileId = fileId
                };
                
                // 下载文件
                var response = await fileService.DownloadFileAsync(request);
                
                if (response.IsSuccess && response.Data != null)
                {
                    // 将字节数组转换为图片
                    using (var ms = new MemoryStream(response.Data.Data))
                    {
                        return System.Drawing.Image.FromStream(ms);
                    }
                }
                else
                {
                    // 记录错误日志
                    System.Diagnostics.Debug.WriteLine($"图片下载失败: {response.ErrorMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // 记录异常日志
                System.Diagnostics.Debug.WriteLine($"图片下载异常: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 删除图片文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="appContext">应用程序上下文</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public static async Task<bool> DeleteImageAsync(string fileId, ApplicationContext appContext)
        {
            try
            {
                if (string.IsNullOrEmpty(fileId) || appContext == null)
                    return false;

                // 获取文件管理服务
                var fileService = appContext.GetRequiredService<FileManagementService>();
                
                // 创建删除请求
                var request = new FileDeleteRequest
                {
                    FileId = fileId
                };
                
                // 删除文件
                var response = await fileService.DeleteFileAsync(request);
                
                if (response.IsSuccess)
                {
                    return true;
                }
                else
                {
                    // 记录错误日志
                    System.Diagnostics.Debug.WriteLine($"图片删除失败: {response.ErrorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // 记录异常日志
                System.Diagnostics.Debug.WriteLine($"图片删除异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 批量删除图片文件
        /// </summary>
        /// <param name="fileIds">文件ID数组</param>
        /// <param name="appContext">应用程序上下文</param>
        /// <returns>删除成功返回true，否则返回false</returns>
        public static async Task<bool> DeleteImagesAsync(string[] fileIds, ApplicationContext appContext)
        {
            if (fileIds == null || fileIds.Length == 0 || appContext == null)
                return false;

            bool allSuccess = true;
            foreach (var fileId in fileIds)
            {
                if (!string.IsNullOrEmpty(fileId))
                {
                    bool result = await DeleteImageAsync(fileId, appContext);
                    if (!result)
                    {
                        allSuccess = false;
                    }
                }
            }
            
            return allSuccess;
        }
    }
}