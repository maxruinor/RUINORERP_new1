using System;
using System.IO;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.FileManagement;

namespace RUINORERP.Server.Helpers
{
    /// <summary>
    /// 文件操作异常处理辅助类
    /// 提供统一的文件操作异常处理机制
    /// </summary>
    public static class FileOperationExceptionHelper
    {
        /// <summary>
        /// 处理文件操作异常,返回适当的错误响应
        /// </summary>
        /// <typeparam name="T">响应类型</typeparam>
        /// <param name="logger">日志记录器</param>
        /// <param name="ex">异常对象</param>
        /// <param name="operation">操作类型描述</param>
        /// <param name="createFailureFunc">创建失败响应的函数</param>
        /// <returns>错误响应</returns>
        public static T HandleFileOperationException<T>(
            ILogger logger,
            Exception ex,
            string operation,
            Func<string, T> createFailureFunc)
        {
            string errorMessage;
            int errorCode = 500;

            // 根据异常类型生成适当的错误消息
            switch (ex)
            {
                case FileNotFoundException:
                    logger?.LogError(ex, "文件操作失败 - 文件未找到: {Operation}", operation);
                    errorMessage = $"文件未找到: {ex.Message}";
                    errorCode = 404;
                    break;

                case UnauthorizedAccessException authEx:
                    logger?.LogError(ex, "文件操作失败 - 访问权限不足: {Operation}", operation);
                    errorMessage = $"文件访问权限不足: {authEx.Message}";
                    errorCode = 403;
                    break;

                case DirectoryNotFoundException dirEx:
                    logger?.LogError(ex, "文件操作失败 - 目录不存在: {Operation}", operation);
                    errorMessage = $"目录不存在: {dirEx.Message}";
                    errorCode = 404;
                    break;

                case IOException ioEx:
                    logger?.LogError(ex, "文件操作失败 - IO异常: {Operation}", operation);
                    errorMessage = $"文件读写错误: {ioEx.Message}";
                    errorCode = 500;
                    break;

                case ArgumentException argEx:
                    logger?.LogError(ex, "文件操作失败 - 参数错误: {Operation}", operation);
                    errorMessage = $"参数错误: {argEx.Message}";
                    errorCode = 400;
                    break;

                case InvalidOperationException invalidOpEx:
                    logger?.LogError(ex, "文件操作失败 - 操作无效: {Operation}", operation);
                    errorMessage = $"操作无效: {invalidOpEx.Message}";
                    errorCode = 400;
                    break;

                case TimeoutException timeoutEx:
                    logger?.LogError(ex, "文件操作失败 - 操作超时: {Operation}", operation);
                    errorMessage = $"文件操作超时: {timeoutEx.Message}";
                    errorCode = 504;
                    break;

                default:
                    logger?.LogError(ex, "文件操作失败 - 未知异常: {Operation}", operation);
                    errorMessage = $"{operation}失败: {ex.Message}";
                    errorCode = 500;
                    break;
            }

            return createFailureFunc(errorMessage);
        }

        /// <summary>
        /// 处理文件操作异常并记录详细信息
        /// </summary>
        /// <typeparam name="T">响应类型</typeparam>
        /// <param name="logger">日志记录器</param>
        /// <param name="ex">异常对象</param>
        /// <param name="operation">操作类型描述</param>
        /// <param name="fileId">文件ID(可选)</param>
        /// <param name="fileName">文件名(可选)</param>
        /// <param name="createFailureFunc">创建失败响应的函数</param>
        /// <returns>错误响应</returns>
        public static T HandleFileOperationException<T>(
            ILogger logger,
            Exception ex,
            string operation,
            long? fileId = null,
            string fileName = null,
            Func<string, int, T> createFailureFunc = null)
        {
            string errorMessage;
            int errorCode = 500;

            // 构建详细日志上下文
            var logContext = operation;
            if (fileId.HasValue)
            {
                logContext += $", FileId={fileId.Value}";
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                logContext += $", FileName={fileName}";
            }

            // 根据异常类型生成适当的错误消息和错误码
            switch (ex)
            {
                case FileNotFoundException:
                    logger?.LogError(ex, "文件操作失败 - 文件未找到: {Context}", logContext);
                    errorMessage = "文件未找到";
                    errorCode = 404;
                    break;

                case UnauthorizedAccessException authEx:
                    logger?.LogError(ex, "文件操作失败 - 访问权限不足: {Context}", logContext);
                    errorMessage = "文件访问权限不足";
                    errorCode = 403;
                    break;

                case DirectoryNotFoundException dirEx:
                    logger?.LogError(ex, "文件操作失败 - 目录不存在: {Context}", logContext);
                    errorMessage = "目录不存在";
                    errorCode = 404;
                    break;

                case IOException ioEx:
                    logger?.LogError(ex, "文件操作失败 - IO异常: {Context}", logContext);
                    errorMessage = "文件读写错误";
                    errorCode = 500;
                    break;

                case ArgumentException argEx:
                    logger?.LogError(ex, "文件操作失败 - 参数错误: {Context}", logContext);
                    errorMessage = "参数错误";
                    errorCode = 400;
                    break;

                case InvalidOperationException invalidOpEx:
                    logger?.LogError(ex, "文件操作失败 - 操作无效: {Context}", logContext);
                    errorMessage = "操作无效";
                    errorCode = 400;
                    break;

                case TimeoutException timeoutEx:
                    logger?.LogError(ex, "文件操作失败 - 操作超时: {Context}", logContext);
                    errorMessage = "文件操作超时";
                    errorCode = 504;
                    break;

                default:
                    logger?.LogError(ex, "文件操作失败 - 未知异常: {Context}", logContext);
                    errorMessage = $"{operation}失败";
                    errorCode = 500;
                    break;
            }

            // 如果提供了带错误码的创建函数,使用它
            if (createFailureFunc != null)
            {
                return createFailureFunc(errorMessage, errorCode);
            }

            // 否则返回默认失败响应
            return default(T);
        }

        /// <summary>
        /// 验证文件操作前置条件
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="operation">操作类型</param>
        /// <param name="checkExists">是否检查文件存在</param>
        /// <returns>验证通过返回null,否则返回错误消息</returns>
        public static string ValidateFileOperation(
            ILogger logger,
            string filePath,
            string operation,
            bool checkExists = true)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                logger?.LogWarning("文件操作验证失败 - 文件路径为空: {Operation}", operation);
                return "文件路径不能为空";
            }

            if (checkExists && !File.Exists(filePath))
            {
                logger?.LogWarning("文件操作验证失败 - 文件不存在: {FilePath}, 操作: {Operation}", filePath, operation);
                return "文件不存在";
            }

            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    logger?.LogWarning("文件操作验证失败 - 目录不存在: {Directory}, 操作: {Operation}", directory, operation);
                    return "文件目录不存在";
                }
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "文件操作验证失败 - 路径解析异常: {FilePath}, 操作: {Operation}", filePath, operation);
                return "文件路径无效";
            }

            return null;
        }
    }
}
