using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助管理器，负责管理应用程序的帮助系统
    /// </summary>
    public static class HelpManager
    {
        private static string _helpFilePath;
        private static readonly Dictionary<string, string> _formHelpMapping = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> _formTitleMapping = new Dictionary<string, string>();
        private static HelpSystemConfig _config = HelpSystemConfig.Load();

        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        /// <param name="helpFilePath">帮助文件路径</param>
        public static void Initialize(string helpFilePath)
        {
            if (!_config.IsHelpSystemEnabled)
                return;

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
            if (!_config.IsHelpSystemEnabled || form == null)
                return;

            if (string.IsNullOrEmpty(_helpFilePath))
            {
                MessageBox.Show("帮助系统未初始化，请联系系统管理员。", "帮助系统", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string helpPage = GetHelpPageForForm(form);
                string title = GetHelpTitleForForm(form);
                
                if (!string.IsNullOrEmpty(helpPage))
                {
                    // 记录帮助查看历史
                    HelpHistoryManager.RecordView(helpPage, title);
                    
                    // 显示帮助页面
                    ShowHelpPage(helpPage);
                }
                else
                {
                    // 显示默认帮助页面
                    ShowHelpPage(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示帮助时出错: {ex.Message}", "帮助系统", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据帮助键显示帮助
        /// </summary>
        /// <param name="helpKey">帮助键</param>
        public static void ShowHelpByKey(string helpKey)
        {
            if (!_config.IsHelpSystemEnabled || string.IsNullOrEmpty(helpKey) || string.IsNullOrEmpty(_helpFilePath))
                return;

            try
            {
                // 根据帮助键显示特定帮助页面
                string helpPage = $"topics/{helpKey}.html";
                
                // 记录帮助查看历史
                HelpHistoryManager.RecordView(helpPage, helpKey);
                
                // 显示帮助页面
                ShowHelpPage(helpPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示帮助时出错: {ex.Message}", "帮助系统", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示控件的帮助（智能帮助）
        /// </summary>
        /// <param name="form">窗体实例</param>
        /// <param name="focusedControl">当前焦点控件</param>
        public static void ShowHelpForControl(Form form, Control focusedControl)
        {
            if (!_config.IsHelpSystemEnabled || form == null) return;
            
            string helpPage = null;
            string title = null;
            
            // 1. 首先检查控件是否有特定帮助键
            if (focusedControl?.Tag is string helpKey)
            {
                helpPage = $"controls/{helpKey}.html";
                title = helpKey;
            }
            // 2. 根据控件类型和名称智能匹配
            else if (focusedControl != null)
            {
                helpPage = GetSmartControlHelpPage(focusedControl);
                title = focusedControl.Name;
            }
            
            // 3. 如果没有找到控件特定帮助，回退到窗体帮助
            if (string.IsNullOrEmpty(helpPage))
            {
                ShowHelp(form);
                return;
            }
            
            try
            {
                // 记录帮助查看历史
                HelpHistoryManager.RecordView(helpPage, title);
                
                // 显示帮助页面
                ShowHelpPage(helpPage);
            }
            catch (Exception ex)
            {
                // 回退到窗体帮助
                ShowHelp(form);
            }
        }

        /// <summary>
        /// 显示帮助页面
        /// </summary>
        /// <param name="helpPage">帮助页面路径</param>
        private static void ShowHelpPage(string helpPage)
        {
            try
            {
                // 确保帮助系统窗体已经创建并显示
                ShowHelpSystemForm();
                
                // 如果指定了帮助页面，则导航到该页面
                if (!string.IsNullOrEmpty(helpPage))
                {
                    // 注意：这里需要通过某种方式将帮助页面路径传递给HelpSystemForm
                    // 可以使用事件、静态变量或其他方式实现
                    // 暂时使用静态变量的方式
                    LastRequestedHelpPage = helpPage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示帮助页面时出错: {ex.Message}", "帮助系统", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示帮助系统主窗体
        /// </summary>
        private static void ShowHelpSystemForm()
        {
            try
            {
                // 查找是否已经有一个HelpSystemForm实例在运行
                HelpSystemForm existingForm = null;
                foreach (Form form in Application.OpenForms)
                {
                    if (form is HelpSystemForm)
                    {
                        existingForm = (HelpSystemForm)form;
                        break;
                    }
                }
                
                // 如果没有找到现有的帮助系统窗体，则创建一个新的
                if (existingForm == null)
                {
                    existingForm = new HelpSystemForm();
                    existingForm.Show();
                }
                else
                {
                    // 如果已经存在，则将其激活
                    if (existingForm.WindowState == FormWindowState.Minimized)
                    {
                        existingForm.WindowState = FormWindowState.Normal;
                    }
                    existingForm.Activate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示帮助系统时出错: {ex.Message}", "帮助系统", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据控件类型和名称智能匹配帮助页面
        /// </summary>
        /// <param name="control">控件实例</param>
        /// <returns>帮助页面路径</returns>
        private static string GetSmartControlHelpPage(Control control)
        {
            // 基于控件类型和名称的智能匹配
            string controlType = control.GetType().Name.ToLower();
            string controlName = control.Name.ToLower();
            
            // 按钮控件帮助匹配
            if (controlType.Contains("button"))
            {
                if (controlName.Contains("save")) return "controls/button_save.html";
                if (controlName.Contains("delete")) return "controls/button_delete.html";
                if (controlName.Contains("add")) return "controls/button_add.html";
                if (controlName.Contains("edit")) return "controls/button_edit.html";
                if (controlName.Contains("cancel")) return "controls/button_cancel.html";
                return "controls/button_general.html";
            }
            
            // 文本框控件帮助匹配
            if (controlType.Contains("textbox"))
            {
                return "controls/textbox_general.html";
            }
            
            // 下拉框控件帮助匹配
            if (controlType.Contains("combobox"))
            {
                return "controls/combobox_general.html";
            }
            
            // 数据网格控件帮助匹配
            if (controlType.Contains("datagrid") || controlType.Contains("grid"))
            {
                return "controls/grid_general.html";
            }
            
            // 标签控件帮助匹配
            if (controlType.Contains("label"))
            {
                return "controls/label_general.html";
            }
            
            // 复选框控件帮助匹配
            if (controlType.Contains("checkbox"))
            {
                return "controls/checkbox_general.html";
            }
            
            // 单选框控件帮助匹配
            if (controlType.Contains("radiobutton"))
            {
                return "controls/radiobutton_general.html";
            }
            
            // 日期时间控件帮助匹配
            if (controlType.Contains("datetimepicker"))
            {
                return "controls/datetimepicker_general.html";
            }
            
            // 数值控件帮助匹配
            if (controlType.Contains("numericupdown"))
            {
                return "controls/numericupdown_general.html";
            }
            
            return null;
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
        /// 获取窗体对应的帮助标题
        /// </summary>
        /// <param name="form">窗体实例</param>
        /// <returns>帮助页面标题</returns>
        private static string GetHelpTitleForForm(Form form)
        {
            // 1. 首先检查是否有HelpMapping特性
            var formType = form.GetType();
            if (_formTitleMapping.ContainsKey(formType.FullName))
            {
                return _formTitleMapping[formType.FullName];
            }

            // 2. 检查是否实现了IHelpProvider接口
            if (form is IHelpProvider helpProvider)
            {
                return helpProvider.GetHelpTitle();
            }

            // 3. 默认使用窗体名称
            return form.Text ?? form.Name;
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
            if (!_config.IsHelpSystemEnabled || formType == null)
                return;

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
        
        /// <summary>
        /// 获取帮助系统配置
        /// </summary>
        public static HelpSystemConfig Config => _config;
        
        /// <summary>
        /// 重新加载配置
        /// </summary>
        public static void ReloadConfig()
        {
            _config = HelpSystemConfig.Load();
        }
        
        /// <summary>
        /// 最后请求的帮助页面路径
        /// </summary>
        public static string LastRequestedHelpPage { get; set; }
    }
}