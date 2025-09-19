using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.Server.Network.Interfaces.Services;
using StackExchange.Redis;
using FileInfo = RUINORERP.PacketSpec.Models.Requests.FileInfo;

namespace RUINORERP.Server.Network.CoreServices
{
    /// <summary>
    /// 文件存储服务实现 - 提供分布式文件存储功能
    /// 迁移自: RUINORERP.PacketSpec.Services.FileStorageService
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        private readonly string _storageRoot;
        private readonly ICacheManager<object> _cacheManager;
        private readonly IDatabase _redisDb;
        private readonly ILogger<FileStorageService> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="redisConnection">Redis连接</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="configuration">配置</param>
        public FileStorageService(
            ICacheManager<object> cacheManager,
            IConnectionMultiplexer redisConnection,
            ILogger<FileStorageService> logger,
            IConfiguration configuration)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _redisDb = redisConnection?.GetDatabase() ?? throw new ArgumentNullException(nameof(redisConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            // 获取存储根目录配置
            _storageRoot = _configuration["FileStorage:Root"] ?? @"G:\FileStorage";

            // 确保存储目录存在
            EnsureDirectories();
        }

        /// <summary>
        /// 确保必要的目录结构存在
        /// </summary>
        private void EnsureDirectories()
        {
            var directories = new[] 
            { 
                "Images/Expenses", 
                "Images/Products", 
                "Images/Payments", 
                "Documents/Manuals", 
                "Documents/Templates", 
                "Temp" 
            };

            foreach (var dir in directories)
            {
                Directory.CreateDirectory(Path.Combine(_storageRoot, dir));
            }

            _logger.LogInformation("文件存储目录已初始化");
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="request">文件列表请求参数</param>
        /// <returns>文件列表响应</returns>
        public async Task<FileListResponse> GetFileListAsync(FileListRequest request)
        {
            try
            {
                var files = new List<FileInfo>();

                // 如果指定了分类，只获取该分类的文件
                if (!string.IsNullOrEmpty(request.Category))
                {
                    await GetFilesByCategory(request.Category, files, request.Pattern);
                }
                else
                {
                    // 获取所有分类的文件
                    var categories = new[] { "Expenses", "Products", "Payments", "Manuals", "Templates" };

                    foreach (var category in categories)
                    {
                        await GetFilesByCategory(category, files, request.Pattern);
                    }
                }

                // 按修改时间排序（最新的在前）
                files = files.OrderByDescending(f => f.LastModified).ToList();

                // 获取总数（用于分页计算）
                int totalCount = files.Count;

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
                    TotalCount = totalCount,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalPages = request.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / request.PageSize) : 1,
                    Message = $"找到 {totalCount} 个文件"
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

        /// <summary>
        /// 列出文件（Redis缓存版本）
        /// </summary>
        /// <param name="request">文件列表请求参数</param>
        /// <returns>文件列表响应</returns>
        public async Task<FileListResponse> ListFilesAsync(FileListRequest request)
        {
            try
            {
                var files = new List<FileInfo>();

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

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="request">文件上传请求</param>
        /// <returns>文件上传响应</returns>
        public async Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request)
        {
            try
            {
                if (request.FileData == null || request.FileData.Length == 0)
                {
                    return new FileUploadResponse
                    {
                        Success = false,
                        Message = "文件数据为空"
                    };
                }

                // 生成文件ID
                var fileId = GenerateFileId(request.FileName);
                var filePath = GetFilePath(fileId, request.Category);

                // 保存文件到磁盘
                await File.WriteAllBytesAsync(filePath, request.FileData);

                // 创建文件信息
                var fileInfo = new FileInfo
                {
                    FileId = fileId,
                    OriginalName = request.FileName,
                    FileSize = request.FileData.Length,
                    Category = request.Category,
                    UploadedBy = request.UploadedBy,
                    UploadedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    FilePath = filePath,
                    MimeType = GetMimeType(request.FileName)
                };

                // 保存文件信息到缓存
                await SaveFileInfoToCacheAsync(fileInfo);

                _logger.LogInformation($"文件上传成功: {request.FileName} -> {fileId}");

                return new FileUploadResponse
                {
                    Success = true,
                    FileId = fileId,
                    Message = "文件上传成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"文件上传失败: {request.FileName}");
                return new FileUploadResponse
                {
                    Success = false,
                    Message = $"文件上传失败: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>文件下载响应</returns>
        public async Task<FileDownloadResponse> DownloadFileAsync(string fileId)
        {
            try
            {
                var fileInfo = await GetFileInfoFromCacheAsync(fileId);
                if (fileInfo == null)
                {
                    return new FileDownloadResponse
                    {
                        Success = false,
                        Message = "文件不存在"
                    };
                }

                if (!File.Exists(fileInfo.FilePath))
                {
                    return new FileDownloadResponse
                    {
                        Success = false,
                        Message = "文件不存在"
                    };
                }

                var fileData = await File.ReadAllBytesAsync(fileInfo.FilePath);

                _logger.LogInformation($"文件下载成功: {fileId}");

                return new FileDownloadResponse
                {
                    Success = true,
                    FileData = fileData,
                    FileName = fileInfo.OriginalName,
                    MimeType = fileInfo.MimeType,
                    Message = "文件下载成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"文件下载失败: {fileId}");
                return new FileDownloadResponse
                {
                    Success = false,
                    Message = $"文件下载失败: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>删除结果</returns>
        public async Task<FileDeleteResponse> DeleteFileAsync(string fileId)
        {
            try
            {
                var fileInfo = await GetFileInfoFromCacheAsync(fileId);
                if (fileInfo == null)
                {
                    return new FileDeleteResponse
                    {
                        Success = false,
                        Message = "文件不存在"
                    };
                }

                // 删除物理文件
                if (File.Exists(fileInfo.FilePath))
                {
                    File.Delete(fileInfo.FilePath);
                }

                // 从缓存中删除文件信息
                await RemoveFileInfoFromCacheAsync(fileId, fileInfo.Category);

                _logger.LogInformation($"文件删除成功: {fileId}");

                return new FileDeleteResponse
                {
                    Success = true,
                    Message = "文件删除成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"文件删除失败: {fileId}");
                return new FileDeleteResponse
                {
                    Success = false,
                    Message = $"文件删除失败: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>文件信息</returns>
        public async Task<FileInfoResponse> GetFileInfoAsync(string fileId)
        {
            try
            {
                var fileInfo = await GetFileInfoFromCacheAsync(fileId);
                if (fileInfo == null)
                {
                    return new FileInfoResponse
                    {
                        Success = false,
                        Message = "文件不存在"
                    };
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
                _logger.LogError(ex, $"获取文件信息失败: {fileId}");
                return new FileInfoResponse
                {
                    Success = false,
                    Message = $"获取文件信息失败: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 获取文件缩略图
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>缩略图响应</returns>
        public async Task<FileThumbnailResponse> GetFileThumbnailAsync(string fileId, int width = 100, int height = 100)
        {
            try
            {
                var fileInfo = await GetFileInfoFromCacheAsync(fileId);
                if (fileInfo == null)
                {
                    return new FileThumbnailResponse
                    {
                        Success = false,
                        Message = "文件不存在"
                    };
                }

                // 检查是否为图片文件
                if (!IsImageFile(fileInfo.MimeType))
                {
                    return new FileThumbnailResponse
                    {
                        Success = false,
                        Message = "不支持生成缩略图的文件类型"
                    };
                }

                // 生成缩略图（这里需要实现具体的缩略图生成逻辑）
                var thumbnailData = await GenerateThumbnailAsync(fileInfo.FilePath, width, height);

                return new FileThumbnailResponse
                {
                    Success = true,
                    ThumbnailData = thumbnailData,
                    Width = width,
                    Height = height,
                    Message = "获取缩略图成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"生成缩略图失败: {fileId}");
                return new FileThumbnailResponse
                {
                    Success = false,
                    Message = $"生成缩略图失败: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 搜索文件
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="category">分类</param>
        /// <returns>搜索结果</returns>
        public async Task<FileSearchResponse> SearchFilesAsync(string keyword, string category = null)
        {
            try
            {
                var results = new List<FileInfo>();
                var categories = string.IsNullOrEmpty(category) 
                    ? new[] { "Expenses", "Products", "Payments", "Manuals", "Templates" } 
                    : new[] { category };

                foreach (var cat in categories)
                {
                    var categoryKey = $"files:{cat}";
                    var fileIds = await _redisDb.SetMembersAsync(categoryKey);

                    foreach (var fileId in fileIds)
                    {
                        var fileInfo = await GetFileInfoFromCacheAsync(fileId.ToString());
                        if (fileInfo != null && 
                            fileInfo.OriginalName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        {
                            results.Add(fileInfo);
                        }
                    }
                }

                return new FileSearchResponse
                {
                    Success = true,
                    Results = results,
                    TotalCount = results.Count,
                    Message = $"找到 {results.Count} 个匹配文件"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"搜索文件失败: {keyword}");
                return new FileSearchResponse
                {
                    Success = false,
                    Message = $"搜索文件失败: {ex.Message}"
                };
            }
        }

        #region Protected Methods

        /// <summary>
        /// 获取指定分类的文件列表
        /// </summary>
        /// <param name="category">分类名称</param>
        /// <param name="files">文件列表</param>
        /// <param name="pattern">匹配模式</param>
        protected virtual async Task GetFilesByCategory(string category, List<FileInfo> files, string pattern = null)
        {
            try
            {
                var categoryPath = GetCategoryPath(category);
                if (!Directory.Exists(categoryPath))
                    return;

                var searchPattern = string.IsNullOrEmpty(pattern) ? "*.*" : $"*{pattern}*";
                var fileEntries = Directory.GetFiles(categoryPath, searchPattern, SearchOption.AllDirectories);

                foreach (var filePath in fileEntries)
                {
                    var fileInfo = new FileInfo
                    {
                        FileId = Path.GetFileNameWithoutExtension(filePath),
                        OriginalName = Path.GetFileName(filePath),
                        FileSize = new FileInfo(filePath).Length,
                        Category = category,
                        UploadedAt = File.GetCreationTime(filePath),
                        LastModified = File.GetLastWriteTime(filePath),
                        FilePath = filePath,
                        MimeType = GetMimeType(Path.GetFileName(filePath))
                    };

                    files.Add(fileInfo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"获取分类文件列表异常: {category}");
            }
        }

        /// <summary>
        /// 生成文件ID
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件ID</returns>
        protected virtual string GenerateFileId(string fileName)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var hash = ComputeFileHash(fileName + timestamp);
            return $"{timestamp}_{hash.Substring(0, 8)}";
        }

        /// <summary>
        /// 计算文件哈希值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>哈希值</returns>
        protected virtual string ComputeFileHash(string input)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="category">分类</param>
        /// <returns>文件路径</returns>
        protected virtual string GetFilePath(string fileId, string category)
        {
            var categoryPath = GetCategoryPath(category);
            return Path.Combine(categoryPath, $"{fileId}.dat");
        }

        /// <summary>
        /// 获取分类路径
        /// </summary>
        /// <param name="category">分类</param>
        /// <returns>分类路径</returns>
        protected virtual string GetCategoryPath(string category)
        {
            return category.ToLower() switch
            {
                "expenses" => Path.Combine(_storageRoot, "Images", "Expenses"),
                "products" => Path.Combine(_storageRoot, "Images", "Products"),
                "payments" => Path.Combine(_storageRoot, "Images", "Payments"),
                "manuals" => Path.Combine(_storageRoot, "Documents", "Manuals"),
                "templates" => Path.Combine(_storageRoot, "Documents", "Templates"),
                _ => Path.Combine(_storageRoot, "Temp")
            };
        }

        /// <summary>
        /// 获取MIME类型
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>MIME类型</returns>
        protected virtual string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".pdf" => "application/pdf",
                ".doc" or ".docx" => "application/msword",
                ".xls" or ".xlsx" => "application/vnd.ms-excel",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };
        }

        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="mimeType">MIME类型</param>
        /// <returns>是否为图片</returns>
        protected virtual bool IsImageFile(string mimeType)
        {
            return mimeType?.StartsWith("image/") == true;
        }

        /// <summary>
        /// 从缓存获取文件信息
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>文件信息</returns>
        protected virtual async Task<FileInfo> GetFileInfoFromCacheAsync(string fileId)
        {
            try
            {
                var cacheKey = $"file:{fileId}";
                return await _cacheManager.GetAsync<FileInfo>(cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"从缓存获取文件信息异常: {fileId}");
                return null;
            }
        }

        /// <summary>
        /// 保存文件信息到缓存
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        protected virtual async Task SaveFileInfoToCacheAsync(FileInfo fileInfo)
        {
            try
            {
                var cacheKey = $"file:{fileInfo.FileId}";
                var categoryKey = $"files:{fileInfo.Category}";

                // 保存文件信息
                await _cacheManager.PutAsync(cacheKey, fileInfo, TimeSpan.FromHours(24));

                // 添加到分类集合
                await _redisDb.SetAddAsync(categoryKey, fileInfo.FileId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"保存文件信息到缓存异常: {fileInfo.FileId}");
            }
        }

        /// <summary>
        /// 从缓存移除文件信息
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="category">分类</param>
        protected virtual async Task RemoveFileInfoFromCacheAsync(string fileId, string category)
        {
            try
            {
                var cacheKey = $"file:{fileId}";
                var categoryKey = $"files:{category}";

                // 移除文件信息
                await _cacheManager.RemoveAsync(cacheKey);

                // 从分类集合中移除
                await _redisDb.SetRemoveAsync(categoryKey, fileId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"从缓存移除文件信息异常: {fileId}");
            }
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>缩略图数据</returns>
        protected virtual async Task<byte[]> GenerateThumbnailAsync(string filePath, int width, int height)
        {
            // 这里需要实现具体的缩略图生成逻辑
            // 可以使用 System.Drawing 或 ImageSharp 等库
            await Task.CompletedTask;
            return new byte[0];
        }

        #endregion

        #region IDisposable Implementation

        private bool _disposed = false;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _cacheManager?.Dispose();
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }

    /// <summary>
    /// 文件存储配置
    /// </summary>
    public class FileStorageConfiguration
    {
        public string Root { get; set; } = @"G:\FileStorage";
        public long MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10MB
        public string[] AllowedExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt" };
        public bool EnableCompression { get; set; } = true;
        public int ThumbnailWidth { get; set; } = 100;
        public int ThumbnailHeight { get; set; } = 100;
    }
}