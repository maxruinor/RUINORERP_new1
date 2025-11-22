using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace AutoUpdate
{
    /// <summary>
    /// 压缩更新测试器
    /// 用于测试和验证压缩更新功能的使用方法
    /// </summary>
    public class CompressedUpdateTester
    {
        private AppUpdater appUpdater;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="updaterUrl">更新服务器URL</param>
        /// <param name="appId">应用程序ID</param>
        public CompressedUpdateTester(string updaterUrl, string appId)
        {
            appUpdater = new AppUpdater
            {
                UpdaterUrl = updaterUrl,
                AppId = appId
            };
        }

        /// <summary>
        /// 测试压缩更新功能
        /// </summary>
        /// <param name="packagePath">压缩包路径</param>
        /// <returns>测试结果</returns>
        public bool TestCompressedUpdate(string packagePath)
        {
            try
            {
                if (!File.Exists(packagePath))
                {
                    Debug.WriteLine($"错误：压缩包不存在 - {packagePath}");
                    return false;
                }

                // 检查是否可以处理此压缩包
                Debug.WriteLine($"开始测试压缩更新包：{packagePath}");

                // 调用处理压缩更新包的方法
                bool result = appUpdater.ProcessCompressedUpdate(packagePath);

                if (result)
                {
                    Debug.WriteLine("压缩更新包处理成功！");
                    return true;
                }
                else
                {
                    Debug.WriteLine("压缩更新包处理失败！");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"测试压缩更新时发生错误：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 测试检查压缩更新
        /// </summary>
        /// <returns>是否有可用的压缩更新</returns>
        public bool TestCheckForCompressedUpdate()
        {
            try
            {
                Debug.WriteLine("开始检查服务器是否有压缩更新包...");
                bool hasUpdate = appUpdater.CheckForCompressedUpdate();

                if (hasUpdate)
                {
                    Debug.WriteLine("检测到可用的压缩更新包！");
                }
                else
                {
                    Debug.WriteLine("当前已是最新版本，没有可用的压缩更新包。");
                }

                return hasUpdate;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"检查压缩更新时发生错误：{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 生成测试用的版本信息XML文件
        /// </summary>
        /// <param name="outputPath">输出路径</param>
        /// <param name="version">版本号</param>
        /// <param name="appId">应用程序ID</param>
        public void GenerateVersionInfoXml(string outputPath, string version, string appId)
        {
            try
            {
                // 创建VersionInfo对象
                VersionInfo versionInfo = new VersionInfo
                {
                    AppId = appId,
                    Version = version,
                    PublishDate = DateTime.Now
                };

                // 添加一些示例文件信息
                versionInfo.Files.Add("App.exe");
                versionInfo.Files.Add("App.dll");
                versionInfo.Files.Add("Resources.dat");

                // 序列化到XML文件
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(VersionInfo));
                using (StreamWriter writer = new StreamWriter(outputPath))
                {
                    serializer.Serialize(writer, versionInfo);
                }

                Debug.WriteLine($"已生成版本信息XML文件：{outputPath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"生成版本信息XML文件时发生错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 显示压缩更新使用指南
        /// </summary>
        public void ShowUsageGuide()
        {
            string guide = @"压缩更新功能使用指南：

1. 发布更新流程：
   - 使用AutoUpdateTools项目中的CompressPublisher类创建压缩更新包
   - 压缩包将包含版本信息文件version_info.xml
   - 上传压缩包到更新服务器

2. 客户端更新流程：
   - 客户端调用CheckForCompressedUpdate()检查是否有新版本
   - 如果有更新，下载压缩包
   - 调用ProcessCompressedUpdate()处理压缩包
   - 系统自动解压并应用更新，同时记录版本历史

3. 版本回滚：
   - 可以使用现有的回滚功能回滚到之前的版本
   - 版本信息已与版本历史管理器集成

4. 注意事项：
   - 压缩包文件名建议包含版本信息
   - 确保更新服务器URL和应用ID正确配置
   - 对于大型更新，压缩包方式比单文件更新更高效
";

            Debug.WriteLine(guide);
            MessageBox.Show(guide, "压缩更新功能使用指南", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}