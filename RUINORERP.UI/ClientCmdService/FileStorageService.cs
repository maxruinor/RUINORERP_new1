﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using CacheManager.Core;
using StackExchange.Redis;
using RUINORERP.PacketSpec.Models;
using System.Configuration;
using Microsoft.Extensions.Logging;
using TransInstruction;


namespace RUINORERP.UI.ClientCmdService
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _storageRoot;
        private readonly ICacheManager<object> _cacheManager;
        private readonly IDatabase _redisDb;
        private readonly ILogger<FileStorageService> _logger;

        public FileStorageService(
            ICacheManager<object> cacheManager,
            IConnectionMultiplexer redisConnection,
            ILogger<FileStorageService> logger)
        {
            _storageRoot = ConfigurationManager.AppSettings["FileStorageRoot"] ?? @"G:\FileStorage";
            _cacheManager = cacheManager;
            _redisDb = redisConnection.GetDatabase();
            _logger = logger;

            // 确保存储目录存在
            EnsureDirectories();
        }

        private void EnsureDirectories()
        {
            var directories = new[] { "Images/Expenses", "Images/Products", "Images/Payments", "Documents/Manuals", "Documents/Templates", "Temp" };

            foreach (var dir in directories)
            {
                Directory.CreateDirectory(Path.Combine(_storageRoot, dir));
            }

            _logger.LogInformation("文件存储目录已初始化");
        }

        public async Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request)
        {
            try
            {
                // 生成唯一文件名
                var fileExt = Path.GetExtension(request.FileName);
                var fileName = $"{Guid.NewGuid()}{fileExt}";
                var categoryPath = GetCategoryPath(request.Category);
                var filePath = Path.Combine(_storageRoot, categoryPath, fileName);

                // 处理分块上传
                if (request.TotalChunks > 1)
                {
                    return await HandleChunkedUpload(request, filePath);
                }

                // 单文件上传
                //await File.WriteAllBytesAsync(filePath, request.Data);
                // 单文件上传 - 使用异步写入
                await WriteAllBytesAsync(filePath, request.Data);

                // 缓存文件信息
                var fileInfo = new
                {
                    OriginalName = request.FileName,
                    StoredName = fileName,
                    Category = request.Category,
                    Size = request.Data.Length,
                    UploadTime = DateTime.UtcNow
                };

                var cacheKey = $"file:{fileName}";
                await _redisDb.StringSetAsync(cacheKey, JsonConvert.SerializeObject(fileInfo));

                return new FileUploadResponse
                {
                    Success = true,
                    FileId = fileName,
                    Message = "文件上传成功"
                };
            }
            catch (Exception ex)
            {
                return new FileUploadResponse
                {
                    Success = false,
                    Message = $"文件上传失败: {ex.Message}"
                };
            }
        }


        public async Task<FileDeleteResponse> DeleteFileAsync(FileDeleteRequest request)
        {
            try
            {
                // 获取文件信息以确定存储路径
                var fileInfo = await GetFileInfoFromCacheAsync(request.FileId);
                if (fileInfo == null)
                {
                    return new FileDeleteResponse
                    {
                        Success = false,
                        Message = "文件不存在或已删除"
                    };
                }

                // 确定文件路径
                var categoryPath = GetCategoryPath(fileInfo.Category);
                var filePath = Path.Combine(_storageRoot, categoryPath, request.FileId);

                // 删除物理文件
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("已删除文件: {FilePath}", filePath);
                }

                // 从Redis缓存中删除文件信息
                var cacheKey = $"file:{request.FileId}";
                await _redisDb.KeyDeleteAsync(cacheKey);

                // 从分类文件列表中移除
                var categoryKey = $"files:{fileInfo.Category}";
                await _redisDb.SetRemoveAsync(categoryKey, request.FileId);

                // 清除内存缓存
                _cacheManager.Remove($"file_data:{request.FileId}");

                return new FileDeleteResponse
                {
                    Success = true,
                    Message = "文件删除成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除文件失败: {FileId}", request.FileId);
                return new FileDeleteResponse
                {
                    Success = false,
                    Message = $"文件删除失败: {ex.Message}"
                };
            }
        }

        public async Task<FileInfoResponse> GetFileInfoAsync(FileInfoRequest request)
        {
            try
            {
                // 从缓存获取文件信息
                var fileInfo = await GetFileInfoFromCacheAsync(request.FileId);
                if (fileInfo == null)
                {
                    // 检查文件是否存在（可能缓存已过期但文件仍在）
                    var filePath = await FindFileInStorageAsync(request.FileId);
                    if (string.IsNullOrEmpty(filePath))
                    {
                        return new FileInfoResponse
                        {
                            Success = false,
                            Message = "文件不存在"
                        };
                    }

                    // 重新构建文件信息
                    fileInfo = await RebuildFileInfoAsync(request.FileId, filePath);
                }

                return new FileInfoResponse
                {
                    Success = true,
                    FileInfo = fileInfo,
                    Message = "获取文件信息成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取文件信息失败: {FileId}", request.FileId);
                return new FileInfoResponse
                {
                    Success = false,
                    Message = $"获取文件信息失败: {ex.Message}"
                };
            }
        }

        public async Task<FileListResponse> ListFilesAsync(FileListRequest request)
        {
            try
            {
                var files = new List<UploadFileInfo>();

                // 如果指定了分类，只获取该分类的文件
                if (!string.IsNullOrEmpty(request.Category))
                {
                    var categoryKey = $"files:{request.Category}";
                    var fileIds = await _redisDb.SetMembersAsync(categoryKey);

                    foreach (var fileId in fileIds)
                    {
                        var fileInfo = await GetFileInfoFromCacheAsync(fileId.ToString());
                        if (fileInfo != null &&
                            (string.IsNullOrEmpty(request.Pattern) ||
                             fileInfo.OriginalName.Contains(request.Pattern)))
                        {
                            files.Add(fileInfo);
                        }
                    }
                }
                else
                {
                    // 获取所有分类的文件
                    var categories = new[] { "Expenses", "Products", "Payments", "Manuals", "Templates" };

                    foreach (var category in categories)
                    {
                        var categoryKey = $"files:{category}";
                        var fileIds = await _redisDb.SetMembersAsync(categoryKey);

                        foreach (var fileId in fileIds)
                        {
                            var fileInfo = await GetFileInfoFromCacheAsync(fileId.ToString());
                            if (fileInfo != null &&
                                (string.IsNullOrEmpty(request.Pattern) ||
                                 fileInfo.OriginalName.Contains(request.Pattern)))
                            {
                                files.Add(fileInfo);
                            }
                        }
                    }
                }

                // 应用分页
                if (request.PageSize > 0)
                {
                    files = files
                        .Skip((request.PageIndex - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToList();
                }

                return new FileListResponse
                {
                    Success = true,
                    Files = files,
                    TotalCount = files.Count,
                    Message = "获取文件列表成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取文件列表失败");
                return new FileListResponse
                {
                    Success = false,
                    Message = $"获取文件列表失败: {ex.Message}"
                };
            }
        }

        #region 辅助方法
        private async Task WriteAllBytesAsync(string filePath, byte[] data)
        {
            using (var fileStream = new FileStream(
                filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                useAsync: true))
            {
                await fileStream.WriteAsync(data, 0, data.Length);
            }
        }
        // 异步读取文件
        private async Task<byte[]> ReadAllBytesAsync(string filePath)

        {
            using (var fileStream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true))
            {
                byte[] bytes = new byte[fileStream.Length];
                int numBytesToRead = (int)fileStream.Length;
                int numBytesRead = 0;

                while (numBytesToRead > 0)
                {
                    int n = await fileStream.ReadAsync(bytes, numBytesRead, numBytesToRead);
                    if (n == 0) break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }

                return bytes;
            }
        }
        private async Task<UploadFileInfo> GetFileInfoFromCacheAsync(string fileId)
        {
            try
            {
                var cacheKey = $"file:{fileId}";
                var fileInfoJson = await _redisDb.StringGetAsync(cacheKey);

                if (fileInfoJson.IsNullOrEmpty)
                    return null;

                return JsonConvert.DeserializeObject<UploadFileInfo>(fileInfoJson);
            }
            catch
            {
                return null;
            }
        }

        private async Task<string> FindFileInStorageAsync(string fileId)
        {
            // 在所有分类目录中查找文件
            var categories = new[] { "Images/Expenses", "Images/Products", "Images/Payments", "Documents/Manuals", "Documents/Templates" };

            foreach (var category in categories)
            {
                var filePath = Path.Combine(_storageRoot, category, fileId);
                if (File.Exists(filePath))
                    return filePath;
            }

            return null;
        }

        private async Task<UploadFileInfo> RebuildFileInfoAsync(string fileId, string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                var category = DetermineCategoryFromPath(filePath);

                var fileInfoObj = new UploadFileInfo
                {
                    FileId = fileId,
                    OriginalName = fileId, // 无法恢复原始名称，使用文件ID
                    Category = category,
                    Size = fileInfo.Length,
                    UploadTime = fileInfo.LastWriteTimeUtc,
                    LastModified = fileInfo.LastWriteTimeUtc
                };

                // 重新缓存文件信息
                var cacheKey = $"file:{fileId}";
                await _redisDb.StringSetAsync(cacheKey, JsonConvert.SerializeObject(fileInfoObj));

                // 添加到分类文件列表
                var categoryKey = $"files:{category}";
                await _redisDb.SetAddAsync(categoryKey, fileId);

                return fileInfoObj;
            }
            catch
            {
                return null;
            }
        }

        private string DetermineCategoryFromPath(string filePath)
        {
            if (filePath.Contains("Expenses")) return "Expenses";
            if (filePath.Contains("Products")) return "Products";
            if (filePath.Contains("Payments")) return "Payments";
            if (filePath.Contains("Manuals")) return "Manuals";
            if (filePath.Contains("Templates")) return "Templates";
            return "Temp";
        }

        private string GetCategoryPath(string category)
        {
            return category switch
            {
                "Expenses" => "Images/Expenses",
                "Products" => "Images/Products",
                "Payments" => "Images/Paymens",
                "Manuals" => "Documents/Manuals",
                "Templates" => "Documents/Templates",
                _ => "Temp"
            };
        }

        

        #endregion



        private async Task<FileUploadResponse> HandleChunkedUpload(FileUploadRequest request, string filePath)
        {
            // 使用Redis跟踪分块上传状态
            var uploadKey = $"upload:{request.FileName}";

            // 如果是第一块，初始化上传状态
            if (request.ChunkIndex == 0)
            {
                var uploadInfo = new
                {
                    FileName = request.FileName,
                    TotalChunks = request.TotalChunks,
                    StartTime = DateTime.UtcNow
                };

                await _redisDb.StringSetAsync(uploadKey, JsonConvert.SerializeObject(uploadInfo));
            }

            // 保存分块
            var chunkPath = $"{filePath}.part{request.ChunkIndex}";
            await WriteAllBytesAsync(chunkPath, request.Data);

            // 记录已上传分块
            await _redisDb.SetAddAsync($"{uploadKey}:chunks", request.ChunkIndex);

            // 检查是否所有分块都已上传
            var uploadedChunks = await _redisDb.SetLengthAsync($"{uploadKey}:chunks");
            if (uploadedChunks == request.TotalChunks)
            {
                // 合并所有分块
                await MergeChunks(filePath, request.TotalChunks);

                // 清理上传状态
                await _redisDb.KeyDeleteAsync(uploadKey);
                await _redisDb.KeyDeleteAsync($"{uploadKey}:chunks");

                return new FileUploadResponse
                {
                    Success = true,
                    FileId = Path.GetFileName(filePath),
                    Message = "文件上传成功"
                };
            }

            return new FileUploadResponse
            {
                Success = true,
                Message = $"分块 {request.ChunkIndex + 1}/{request.TotalChunks} 上传成功"
            };
        }

        private async Task MergeChunks(string targetPath, int totalChunks)
        {
            using (var targetStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
            {
                for (int i = 0; i < totalChunks; i++)
                {
                    var chunkPath = $"{targetPath}.part{i}";
                    using (var chunkStream = new FileStream(chunkPath, FileMode.Open, FileAccess.Read))
                    {
                        await chunkStream.CopyToAsync(targetStream);
                    }
                    File.Delete(chunkPath);
                }
            }
        }

        public async Task<FileDownloadResponse> DownloadFileAsync(FileDownloadRequest request)
        {
            try
            {
                var categoryPath = GetCategoryPath(request.Category);
                var filePath = Path.Combine(_storageRoot, categoryPath, request.FileId);

                if (!File.Exists(filePath))
                {
                    return new FileDownloadResponse
                    {
                        Success = false,
                        Message = "文件不存在"
                    };
                }

                var fileData = await ReadAllBytesAsync(filePath);

                // 从缓存获取原始文件名
                var cacheKey = $"file:{request.FileId}";
                var fileInfoJson = await _redisDb.StringGetAsync(cacheKey);

                string originalName = request.FileId;
                if (!fileInfoJson.IsNullOrEmpty)
                {
                    var fileInfo = JsonConvert.DeserializeObject<dynamic>(fileInfoJson);
                    originalName = fileInfo.OriginalName;
                }

                return new FileDownloadResponse
                {
                    Success = true,
                    FileName = originalName,
                    Data = fileData,
                    Message = "文件下载成功"
                };
            }
            catch (Exception ex)
            {
                return new FileDownloadResponse
                {
                    Success = false,
                    Message = $"文件下载失败: {ex.Message}"
                };
            }
        }

       

        // 实现其他方法: DeleteFileAsync, GetFileInfoAsync, ListFilesAsync
    }
}
