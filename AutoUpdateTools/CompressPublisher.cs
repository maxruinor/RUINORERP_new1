using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AutoUpdate;

namespace AutoUpdateTools
{
    /// <summary>
    /// 压缩发布管理器
    /// 负责将应用程序更新文件压缩并生成发布包
    /// </summary>
    public class CompressPublisher
    {
        /// <summary>
        /// 更新配置XML文件路径
        /// </summary>
        private string UpdateXmlPath { get; set; }

        /// <summary>
        /// 源文件目录
        /// </summary>
        private string SourceDirectory { get; set; }

        /// <summary>
        /// 发布目标目录
        /// </summary>
        private string PublishDirectory { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="updateXmlPath">更新配置XML文件路径</param>
        /// <param name="sourceDirectory">源文件目录</param>
        /// <param name="publishDirectory">发布目标目录</param>
        public CompressPublisher(string updateXmlPath, string sourceDirectory, string publishDirectory)
        {
            UpdateXmlPath = updateXmlPath;
            SourceDirectory = sourceDirectory;
            PublishDirectory = publishDirectory;
        }

        /// <summary>
        /// 压缩并发布更新包
        /// </summary>
        /// <param name="includeVersionInfo">是否在压缩包中包含版本信息</param>
        /// <param name="compressAllFiles">是否压缩所有文件（包括未变更的文件）</param>
        /// <returns>发布结果</returns>
        public PublishResult Publish(bool includeVersionInfo = true, bool compressAllFiles = false)
        {
            PublishResult result = new PublishResult();

            try
            {
                // 确保发布目录存在
                if (!Directory.Exists(PublishDirectory))
                {
                    Directory.CreateDirectory(PublishDirectory);
                }

                // 加载更新XML文件
                XDocument updateXml = XDocument.Load(UpdateXmlPath);
                
                // 获取应用ID和版本信息
                string appId = updateXml.Root?.Attribute("AppId")?.Value ?? "Unknown";
                string version = updateXml.Root?.Attribute("Version")?.Value ?? "1.0.0.0";

                result.AppId = appId;
                result.Version = version;

                // 准备要压缩的文件列表
                List<string> filesToCompress = new List<string>();
                List<FileInfo> fileInfos = new List<FileInfo>();

                // 获取需要压缩的文件
                var fileElements = updateXml.Descendants("File");
                foreach (var fileElement in fileElements)
                {
                    string fileName = fileElement.Attribute("Name")?.Value;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string fullPath = Path.Combine(SourceDirectory, fileName);
                        if (File.Exists(fullPath))
                        {
                            filesToCompress.Add(fileName);
                            fileInfos.Add(new FileInfo(fullPath));
                        }
                    }
                }

                result.FilesProcessed = filesToCompress.Count;

                // 创建压缩文件名
                string zipFileName = $"{appId}_{version}.zip";
                
                // 如果需要，在压缩包中包含版本信息文件
                if (includeVersionInfo)
                {
                    string versionInfoPath = Path.Combine(SourceDirectory, "version_info.xml");
                    CreateVersionInfoFile(versionInfoPath, appId, version, filesToCompress);
                    fileInfos.Add(new FileInfo(versionInfoPath));
                }

                // 执行压缩
                if (fileInfos.Count > 0)
                {
                    GZipResult compressResult = GZip.Compress(
                        fileInfos.ToArray(), 
                        SourceDirectory, 
                        PublishDirectory, 
                        zipFileName
                    );

                    result.Success = !compressResult.Errors;
                    result.OutputFile = compressResult.ZipFile;
                    result.CompressionPercent = compressResult.CompressionPercent;

                    // 更新发布XML，添加压缩包信息
                    UpdatePublishXml(updateXml, zipFileName);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 创建版本信息文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="appId">应用ID</param>
        /// <param name="version">版本号</param>
        /// <param name="files">文件列表</param>
        private void CreateVersionInfoFile(string filePath, string appId, string version, List<string> files)
        {
            XDocument doc = new XDocument(
                new XElement("VersionInfo",
                    new XAttribute("AppId", appId),
                    new XAttribute("Version", version),
                    new XAttribute("PublishDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    new XElement("Files",
                        files.Select(f => new XElement("File", new XAttribute("Name", f)))
                    )
                )
            );
            
            // 确保目录存在
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            doc.Save(filePath);
        }

        /// <summary>
        /// 更新发布XML文件，添加压缩包信息
        /// </summary>
        /// <param name="updateXml">更新XML文档</param>
        /// <param name="zipFileName">压缩文件名</param>
        private void UpdatePublishXml(XDocument updateXml, string zipFileName)
        {
            // 添加或更新压缩包信息节点
            var compressNode = updateXml.Descendants("CompressedPackage").FirstOrDefault();
            if (compressNode == null)
            {
                compressNode = new XElement("CompressedPackage");
                updateXml.Root.Add(compressNode);
            }
            
            compressNode.SetAttributeValue("FileName", zipFileName);
            compressNode.SetAttributeValue("PublishDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            
            // 保存更新后的XML
            updateXml.Save(Path.Combine(PublishDirectory, Path.GetFileName(UpdateXmlPath)));
        }
    }

    /// <summary>
    /// 发布结果类
    /// 存储发布过程的结果信息
    /// </summary>
    public class PublishResult
    {
        /// <summary>
        /// 是否发布成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 发布版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 处理的文件数量
        /// </summary>
        public int FilesProcessed { get; set; }

        /// <summary>
        /// 输出文件路径
        /// </summary>
        public string OutputFile { get; set; }

        /// <summary>
        /// 压缩百分比
        /// </summary>
        public double CompressionPercent { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}