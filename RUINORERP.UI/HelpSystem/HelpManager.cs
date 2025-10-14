using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace RUINORERP.UI.Common.HelpSystem
{
    /// <summary>
    /// 帮助管理器，负责管理应用程序的帮助系统
    /// </summary>
    public static class HelpManager
    {
        private static string _helpFilePath;
        private static readonly Dictionary<string, string> _formHelpMapping = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> _formTitleMapping = new Dictionary<string, string>();

        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        /// <param name="helpFilePath">帮助文件路径</param>
        public static void Initialize(string helpFilePath)
        {
            if (string.IsNullOrEmpty(helpFilePath))
                throw new ArgumentException("帮助文件路径不能为空", nameof(helpFilePath));

            if (!File.Exists(helpFilePath))
                throw new FileNotFoundException($"帮助文件未找到: {helpFilePath}", helpFilePath);

            _helpFilePath = helpFilePath;

            // 预扫描程序集中的帮助映射
            ScanAssemblyForHelpMappings();
        }

        /// <summary>
        /// 扫描程序集中的帮助映射
        /// </summary>
        private static void ScanAssemblyForHelpMappings()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (typeof(Form).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var helpAttribute = type.GetCustomAttribute<HelpMappingAttribute>();
                        if (helpAttribute != null)
                        {
                            _formHelpMapping[type.FullName] = helpAttribute.HelpPage;
                            _formTitleMapping[type.FullName] = helpAttribute.Title ?? type.Name;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常，避免影响主程序
                Debug.WriteLine($"扫描帮助映射时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示指定窗体的帮助
        /// </summary>
        /// <param name="form">窗体实例</param>
        public static void ShowHelp(Form form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            if (string.IsNullOrEmpty(_helpFilePath))
            {
                MessageBox.Show("帮助系统未初始化，请联系系统管理员。", "帮助系统", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string helpPage = GetHelpPageForForm(form);
                if (!string.IsNullOrEmpty(helpPage))
                {
                    // 使用CHM帮助文件显示帮助
                    Help.ShowHelp(form, _helpFilePath, HelpNavigator.Topic, helpPage);
                }
                else
                {
                    // 显示默认帮助页面
                    Help.ShowHelp(form, _helpFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示帮助时出错: {ex.Message}", "帮助系统", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 获取窗体对应的帮助页面
        /// </summary>
        /// <param name="form">窗体实例</param>
        /// <returns>帮助页面路径</returns>
        private static string GetHelpPageForForm(Form form)
        {
            // 1. 首先检查是否有HelpMapping特性
            var formType = form.GetType();
            if (_formHelpMapping.ContainsKey(formType.FullName))
            {
                return _formHelpMapping[formType.FullName];
            }

            // 2. 检查是否实现了IHelpProvider接口
            if (form is IHelpProvider helpProvider)
            {
                return helpProvider.GetHelpPage();
            }

            // 3. 基于窗体类型名称的智能匹配
            return GetSmartHelpPage(formType);
        }

        /// <summary>
        /// 基于窗体类型名称的智能匹配
        /// </summary>
        /// <param name="formType">窗体类型</param>
        /// <returns>帮助页面路径</returns>
        private static string GetSmartHelpPage(Type formType)
        {
            string typeName = formType.Name;

            // 基础数据类型窗体匹配
            if (typeName.StartsWith("UC") && (typeName.Contains("Edit") || typeName.Contains("Manage") || typeName.Contains("Master")))
            {
                return $"basics/{typeName}.html";
            }

            // 单据类型窗体匹配
            if (typeName.StartsWith("UC") && (typeName.Contains("Order") || typeName.Contains("Entry") || typeName.Contains("Bill")))
            {
                return $"documents/{typeName}.html";
            }

            // 列表类型窗体匹配
            if (typeName.StartsWith("UC") && (typeName.Contains("Query") || typeName.Contains("List") || typeName.EndsWith("Grid")))
            {
                return $"lists/{typeName}.html";
            }

            // 默认匹配
            return $"general/{typeName}.html";
        }

        /// <summary>
        /// 注册窗体帮助映射（运行时注册）
        /// </summary>
        /// <param name="formType">窗体类型</param>
        /// <param name="helpPage">帮助页面路径</param>
        /// <param name="title">帮助页面标题</param>
        public static void RegisterHelpMapping(Type formType, string helpPage, string title = null)
        {
            if (formType == null)
                throw new ArgumentNullException(nameof(formType));

            if (string.IsNullOrEmpty(helpPage))
                throw new ArgumentException("帮助页面路径不能为空", nameof(helpPage));

            _formHelpMapping[formType.FullName] = helpPage;
            _formTitleMapping[formType.FullName] = title ?? formType.Name;
        }

        /// <summary>
        /// 获取帮助文件路径
        /// </summary>
        public static string HelpFilePath => _helpFilePath;

        /// <summary>
        /// 检查帮助系统是否已初始化
        /// </summary>
        public static bool IsInitialized => !string.IsNullOrEmpty(_helpFilePath) && File.Exists(_helpFilePath);
    }
}