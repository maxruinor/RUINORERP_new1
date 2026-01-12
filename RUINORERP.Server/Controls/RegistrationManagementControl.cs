using FluentValidation.Results;
using HLH.Lib.Helper;
using HLH.Lib.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RUINORERP.Business;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Server.Services;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RUINORERP.Server.Controls
{
    /// <summary>
    /// 注册管理控件
    /// 负责系统注册信息的显示、编辑和验证功能
    /// </summary>
    public partial class RegistrationManagementControl : UserControl
    {
        private readonly ILogger<RegistrationManagementControl> _logger;
        private readonly IRegistrationService _registrationService;
        private readonly IHardwareInfoService _hardwareInfoService;

        public RegistrationManagementControl()
        {
            InitializeComponent();

            // 从依赖注入容器获取服务
            _logger = Startup.GetFromFac<ILogger<RegistrationManagementControl>>();
            _registrationService = Startup.GetFromFac<IRegistrationService>();
            _hardwareInfoService = Startup.GetFromFac<IHardwareInfoService>();
        }

        private async void RegistrationManagementControl_Load(object sender, EventArgs e)
        {
            InitializeFunctionModules();
            await LoadRegistrationInfo();
        }

        /// <summary>
        /// 加载注册信息
        /// </summary>
        private async Task LoadRegistrationInfo()
        {
            try
            {
                var registrationInfo = await _registrationService.GetRegistrationInfoAsync();
                if (registrationInfo != null)
                {
                    _currentRegistrationInfo = registrationInfo;
                    BindData(registrationInfo);

                    // 检查注册状态
                    bool isRegistered = registrationInfo.IsRegistered;
                    bool isExpired = _registrationService.IsRegistrationExpired(registrationInfo);

                    UpdateUIState(isRegistered);

                    // 如果已过期，显示过期提示
                    if (isRegistered && isExpired)
                    {
                        var daysExpired = DateTime.Now - registrationInfo.ExpirationDate;
                        _logger.LogWarning($"系统注册已过期 {daysExpired.Days} 天，到期时间: {registrationInfo.ExpirationDate:yyyy-MM-dd}");

                        // 延迟显示过期提示，让界面先加载完成
                        this.BeginInvoke(new Action(() =>
                        {
                            MessageBox.Show(
                                $"系统注册已于 {registrationInfo.ExpirationDate:yyyy-MM-dd} 过期，已过期 {daysExpired.Days} 天。\n\n" +
                                "请使用续期功能或联系软件提供商重新注册。",
                                "注册已过期",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }));
                    }
                    else if (isRegistered)
                    {
                        // 检查是否即将过期（7天内）
                        var daysUntilExpiration = registrationInfo.ExpirationDate - DateTime.Now;
                        if (daysUntilExpiration.TotalDays <= 7 && daysUntilExpiration.TotalDays > 0)
                        {
                            _logger.LogWarning($"系统注册将在 {daysUntilExpiration.TotalDays:F0} 天后到期");

                            this.BeginInvoke(new Action(() =>
                            {
                                MessageBox.Show(
                                    $"系统注册将在 {daysUntilExpiration.TotalDays:F0} 天后到期。\n" +
                                    $"到期时间: {registrationInfo.ExpirationDate:yyyy-MM-dd}\n\n" +
                                    "请及时续期以避免影响正常使用。",
                                    "注册即将到期",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载注册信息失败");
                MessageBox.Show($"加载注册信息失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化功能模块列表
        /// </summary>
        private void InitializeFunctionModules()
        {
            checkedListBoxMod.Items.Clear();
            descriptionToValueMap = EnumHelper.GetEnumDescriptionToValueMap(typeof(GlobalFunctionModule));
            foreach (var description in descriptionToValueMap.Keys)
            {
                checkedListBoxMod.Items.Add(description);
            }
        }

        /// <summary>
        /// 绑定数据到UI控件
        /// </summary>
        /// <param name="entity">注册信息实体</param>
        private void BindData(tb_sys_RegistrationInfo entity)
        {
            // 清除现有绑定
            ClearDataBindings();

            txtCompanyName.DataBindings.Clear();
            // 添加新的数据绑定
            txtCompanyName.DataBindings.Add(new Binding("Text", entity, nameof(entity.CompanyName), true, DataSourceUpdateMode.OnValidation));

            txtContactName.DataBindings.Clear();
            txtContactName.DataBindings.Add(new Binding("Text", entity, nameof(entity.ContactName), true, DataSourceUpdateMode.OnValidation));

            txtPhoneNumber.DataBindings.Clear();
            txtPhoneNumber.DataBindings.Add(new Binding("Text", entity, nameof(entity.PhoneNumber), true, DataSourceUpdateMode.OnValidation));

            txtRegistrationCode.DataBindings.Clear();
            txtRegistrationCode.DataBindings.Add(new Binding("Text", entity, nameof(entity.RegistrationCode), true, DataSourceUpdateMode.OnValidation));

            txtConcurrentUsers.DataBindings.Clear();
            txtConcurrentUsers.DataBindings.Add(new Binding("Text", entity, nameof(entity.ConcurrentUsers), true, DataSourceUpdateMode.OnValidation));

            dtpExpirationDate.DataBindings.Clear();
            dtpExpirationDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.ExpirationDate), true, DataSourceUpdateMode.OnValidation));

            txtProductVersion.DataBindings.Clear();
            txtProductVersion.DataBindings.Add(new Binding("Text", entity, nameof(entity.ProductVersion), true, DataSourceUpdateMode.OnValidation));

            txtMachineCode.DataBindings.Clear();
            txtMachineCode.DataBindings.Add(new Binding("Text", entity, nameof(entity.MachineCode), true, DataSourceUpdateMode.OnValidation));

            dtpPurchaseDate.DataBindings.Clear();
            dtpPurchaseDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.PurchaseDate), true, DataSourceUpdateMode.OnValidation));

            dtpRegistrationDate.DataBindings.Clear();
            dtpRegistrationDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.RegistrationDate), true, DataSourceUpdateMode.OnValidation));

            chkIsRegistered.DataBindings.Clear();
            chkIsRegistered.DataBindings.Add(new Binding("Checked", entity, nameof(entity.IsRegistered), true, DataSourceUpdateMode.OnValidation));

            txtRemarks.DataBindings.Clear();
            txtRemarks.DataBindings.Add(new Binding("Text", entity, nameof(entity.Remarks), true, DataSourceUpdateMode.OnValidation));

            // 设置授权类型
            if (!string.IsNullOrEmpty(entity.LicenseType))
            {
                cmbLicenseType.SelectedIndex = cmbLicenseType.FindString(entity.LicenseType);
            }

            // 加载功能模块
            LoadFunctionModules(entity.FunctionModule);

            // 保存实体引用
            _currentRegistrationInfo = entity;

            // 监听属性变化
            entity.PropertyChanged += RegistrationInfo_PropertyChanged;
        }

        /// <summary>
        /// 清除数据绑定
        /// </summary>
        private void ClearDataBindings()
        {
            foreach (Control control in this.Controls)
            {
                control.DataBindings.Clear();
            }
        }

        /// <summary>
        /// 加载功能模块
        /// </summary>
        /// <param name="functionModule">功能模块字符串</param>
        private void LoadFunctionModules(string functionModule)
        {
            if (string.IsNullOrEmpty(functionModule))
            {
                return;
            }

            try
            {
                List<GlobalFunctionModule> selectedModules = new List<GlobalFunctionModule>();

                // 将逗号分隔的枚举名称字符串转换为List<GlobalFunctionModule>
                string[] enumNameArray = functionModule.Split(',');
                foreach (var item in enumNameArray)
                {
                    var trimmedItem = item.Trim();
                    if (!string.IsNullOrEmpty(trimmedItem) && Enum.TryParse(typeof(GlobalFunctionModule), trimmedItem, out object module))
                    {
                        selectedModules.Add((GlobalFunctionModule)module);
                    }
                }

                LoadSelections(selectedModules);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "加载功能模块失败");
                MessageBox.Show("加载功能模块失败，请检查注册信息是否正确。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 加载选择的功能模块
        /// </summary>
        /// <param name="selectedModules">选中的模块列表</param>
        private void LoadSelections(List<GlobalFunctionModule> selectedModules)
        {
            for (int i = 0; i < checkedListBoxMod.Items.Count; i++)
            {
                string description = checkedListBoxMod.Items[i].ToString();
                if (descriptionToValueMap.ContainsKey(description))
                {
                    GlobalFunctionModule value = (GlobalFunctionModule)descriptionToValueMap[description];
                    checkedListBoxMod.SetItemChecked(i, selectedModules.Contains(value));
                }
            }
        }

        /// <summary>
        /// 更新UI状态
        /// </summary>
        /// <param name="isRegistered">是否已注册</param>
        private void UpdateUIState(bool isRegistered)
        {
            // 获取注册服务检查是否过期
            var registrationService = Startup.GetFromFac<IRegistrationService>();
            bool isExpired = false;
            bool showRenewalOption = false;

            if (registrationService != null && _currentRegistrationInfo != null)
            {
                isExpired = registrationService.IsRegistrationExpired(_currentRegistrationInfo);
                // 如果已注册，显示续期选项（无论是否过期）
                showRenewalOption = isRegistered;
            }

            // 工具栏按钮控制
            tsbtnSaveRegInfo.Visible = !isRegistered; // 只在注册前可用
            tsbtnCreateRegInfo.Visible = !isRegistered; // 只在未注册时可用
            tsbtnRenewRegInfo.Visible = false; // 默认不显示续期菜单，只在进入续期模式时显示
            btnRenewRegInfo.Visible = isRegistered; // 已注册时（无论是否过期）都可续期
            btnGenerateMachineCode.Enabled = !isRegistered || isExpired; // 未注册或过期时可生成机器码

            // 注册日期显示和编辑控制
            dtpRegistrationDate.Visible = isRegistered; // 已注册时显示
            dtpRegistrationDate.Enabled = !isRegistered; // 已注册后不可修改
            lblRegistrationDate.Visible = isRegistered;

            // 购买日期显示和编辑控制
            dtpPurchaseDate.Enabled = !isRegistered; // 已注册后不可修改

            // 授权到期日期控制
            dtpExpirationDate.Visible = isRegistered;
            lblExpirationDate.Visible = isRegistered;

            // 授权期限控制：只在未注册或续期时显示
            lblDays.Visible = !isRegistered || (isRegistered && isExpired);
            cmbDays.Visible = !isRegistered || (isRegistered && isExpired);

            // 如果已过期，添加过期提示
            if (isExpired)
            {
                // 更新注册日期标签
                lblRegistrationDate.Text = "注册日期（已过期）:";
                lblRegistrationDate.ForeColor = Color.Red;
                dtpRegistrationDate.Visible = true;
                lblRegistrationDate.Visible = true;

                // 更新授权到期日期标签
                lblExpirationDate.Text = "授权到期（已过期）:";
                lblExpirationDate.ForeColor = Color.Red;
                dtpExpirationDate.Visible = true;
                lblExpirationDate.Visible = true;

                // 在界面顶部显示过期提示
                PrintExpirationWarning(true);
            }
            else if (isRegistered)
            {
                // 恢复正常标签
                lblRegistrationDate.Text = "注册日期:";
                lblRegistrationDate.ForeColor = SystemColors.ControlText;
                lblExpirationDate.Text = "授权到期:";
                lblExpirationDate.ForeColor = SystemColors.ControlText;

                // 隐藏过期提示
                PrintExpirationWarning(false);
            }

            // 根据注册状态设置控件只读状态
            SetControlsReadOnly(isRegistered && !isExpired); // 有效注册时只读，过期时可编辑

            // 无论是否过期，只要注册过，注册日期和购买日期就一直禁用
            if (isRegistered)
            {
                dtpRegistrationDate.Enabled = false;
                dtpPurchaseDate.Enabled = false;
            }
        }

        /// <summary>
        /// 设置控件只读状态
        /// </summary>
        /// <param name="readOnly">是否只读</param>
        private void SetControlsReadOnly(bool readOnly)
        {
            txtCompanyName.ReadOnly = readOnly;
            txtContactName.ReadOnly = readOnly;
            txtPhoneNumber.ReadOnly = readOnly;
            txtConcurrentUsers.ReadOnly = readOnly;
            dtpExpirationDate.Enabled = !readOnly;
            txtProductVersion.ReadOnly = readOnly;
            cmbLicenseType.Enabled = !readOnly;
            dtpPurchaseDate.Enabled = !readOnly;
            dtpRegistrationDate.Enabled = !readOnly;
            checkedListBoxMod.Enabled = !readOnly;
            txtRemarks.ReadOnly = readOnly;
        }

        /// <summary>
        /// 注册信息属性变化事件处理
        /// </summary>
        private void RegistrationInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(tb_sys_RegistrationInfo.MachineCode))
            {
                btnGenerateMachineCode.Enabled = true;
            }
        }

        /// <summary>
        /// 保存注册信息
        /// </summary>
        private async void tsbtnSaveRegInfo_Click(object sender, EventArgs e)
        {
            await SaveRegistrationInfoAsync("保存");
        }

        /// <summary>
        /// 公共注册信息保存方法，处理注册、续期和保存操作
        /// </summary>
        /// <param name="operationType">操作类型：注册、续期或保存</param>
        private async Task SaveRegistrationInfoAsync(string operationType)
        {
            if (!ValidateRegistrationInfo(_currentRegistrationInfo))
            {
                return;
            }

            try
            {
                // 获取选中的功能模块
                var selectedModules = GetSelectedFunctionModules();
                string moduleString = string.Join(",", selectedModules);

                // 无论是否处于调试模式，功能模块都使用明码保存
                _currentRegistrationInfo.FunctionModule = moduleString;

                _currentRegistrationInfo.IsRegistered = true;

                // 保存到数据库
                var result = await _registrationService.SaveRegistrationInfoAsync(_currentRegistrationInfo);

                if (result)
                {
                    string successMsg = operationType switch
                    {
                        "注册" => "注册成功",
                        "续期" => "续期成功",
                        "保存" => "保存成功",
                        _ => "操作成功"
                    };
                    MessageBox.Show(successMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 记录到日志
                    frmMainNew.Instance.SafeLogOperation($"系统{operationType}成功，已保存最新的注册信息", Color.Green);

                    // 更新主窗体的注册信息
                    UpdateMainFormRegistrationInfo();

                    // 更新UI状态
                    await LoadRegistrationInfo();
                    UpdateUIState(true);
                }
                else
                {
                    string failMsg = operationType switch
                    {
                        "注册" => "注册失败",
                        "续期" => "续期失败",
                        "保存" => "保存失败",
                        _ => "操作失败"
                    };
                    MessageBox.Show(failMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _logger.LogWarning($"注册信息{operationType}失败");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{operationType}注册信息失败");
                MessageBox.Show($"{operationType}失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 注册系统
        /// </summary>
        private async void btnCreateRegInfo_Click(object sender, EventArgs e)
        {
            await SaveRegistrationInfoAsync("注册");
        }

        /// <summary>
        /// 注册系统/续期
        /// </summary>
        private async void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateRegistrationInfo(_currentRegistrationInfo))
            {
                return;
            }

            try
            {
                // 检查是否已经注册且未过期，如果是，则不允许重复注册
                if (_currentRegistrationInfo.IsRegistered && !_registrationService.IsRegistrationExpired(_currentRegistrationInfo))
                {
                    MessageBox.Show("当前系统已注册且未过期，无需重复注册", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _logger.LogWarning("用户尝试重复注册有效授权");
                    return;
                }

                // 获取注册码
                string registrationCode = _currentRegistrationInfo.RegistrationCode;
                if (string.IsNullOrEmpty(registrationCode))
                {
                    MessageBox.Show("请输入注册码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 注册码格式预验证
                if (!IsValidRegistrationCodeFormat(registrationCode))
                {
                    MessageBox.Show("注册码格式无效，请检查后重新输入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 显示验证中的提示
                var progressForm = new Form
                {
                    Text = "正在验证",
                    Size = new Size(300, 100),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterScreen,
                    ControlBox = false
                };
                var label = new Label
                {
                    Text = "正在验证注册码，请稍候...",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                progressForm.Controls.Add(label);

                // 异步显示验证对话框
                progressForm.Show();

                try
                {
                    // 验证注册码
                    bool isValid = await _registrationService.ValidateRegistrationAsync(registrationCode);

                    if (isValid)
                    {
                        // 判断是续期还是新注册
                        bool isRenewal = _currentRegistrationInfo.IsRegistered &&
                                       _registrationService.IsRegistrationExpired(_currentRegistrationInfo);

                        string successMsg = isRenewal ?
                            "恭喜您，续期成功！系统注册已延期。" :
                            "恭喜您，注册成功！";

                        MessageBox.Show(successMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 更新主窗体的注册信息
                        UpdateMainFormRegistrationInfo();

                        UpdateUIState(true);
                    }
                    else
                    {
                        string errorTitle = _currentRegistrationInfo.IsRegistered ? "续期失败" : "注册失败";
                        MessageBox.Show($"{errorTitle}！注册码无效。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _currentRegistrationInfo.IsRegistered = false;
                        _logger.LogWarning($"{errorTitle}验证失败：注册码无效");
                    }
                }
                finally
                {
                    // 关闭验证对话框
                    progressForm.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "注册/续期失败");
                MessageBox.Show($"操作失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 注册码格式预验证
        /// </summary>
        /// <param name="registrationCode">注册码</param>
        /// <returns>格式是否有效</returns>
        private bool IsValidRegistrationCodeFormat(string registrationCode)
        {
            // 简单的注册码格式验证，可根据实际需求调整
            if (string.IsNullOrWhiteSpace(registrationCode))
                return false;

            // 例如：注册码长度必须在10-50个字符之间
            if (registrationCode.Length < 10 || registrationCode.Length > 50)
                return false;

            // 例如：注册码只能包含字母、数字和连字符
            return System.Text.RegularExpressions.Regex.IsMatch(registrationCode, @"^[A-Z0-9\-]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证注册信息
        /// </summary>
        /// <returns>验证是否通过</returns>
        private bool ValidateRegistrationInfo(tb_sys_RegistrationInfo registrationInfo)
        {
            if (_currentRegistrationInfo == null)
            {
                MessageBox.Show("注册信息未加载", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 使用验证器验证
            var validator = new tb_sys_RegistrationInfoValidator();
            var results = validator.Validate(_currentRegistrationInfo);

            if (!results.IsValid)
            {
                StringBuilder msg = new StringBuilder();
                int counter = 1;
                foreach (var error in results.Errors)
                {
                    msg.AppendLine($"{counter}. {error.ErrorMessage}");
                    counter++;
                }

                MessageBox.Show(msg.ToString(), "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 检查功能模块选择
            if (checkedListBoxMod.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择要使用的功能模块。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }




        /// <summary>
        /// 获取选中的功能模块
        /// </summary>
        /// <returns>选中的模块列表</returns>
        private List<GlobalFunctionModule> GetSelectedFunctionModules()
        {
            List<GlobalFunctionModule> selectedModules = new List<GlobalFunctionModule>();

            for (int i = 0; i < checkedListBoxMod.Items.Count; i++)
            {
                if (checkedListBoxMod.GetItemChecked(i))
                {
                    string description = checkedListBoxMod.Items[i].ToString();
                    if (descriptionToValueMap.ContainsKey(description))
                    {
                        selectedModules.Add((GlobalFunctionModule)descriptionToValueMap[description]);
                    }
                }
            }

            return selectedModules;
        }

        /// <summary>
        /// 授权类型变更
        /// </summary>
        private void cmbLicenseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_currentRegistrationInfo != null && cmbLicenseType.SelectedItem != null)
            {
                _currentRegistrationInfo.LicenseType = cmbLicenseType.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// 更新过期警告UI状态
        /// </summary>
        /// <param name="showWarning">是否显示警告</param>
        private void PrintExpirationWarning(bool showWarning)
        {
            // 移除弹窗显示，避免与LoadRegistrationInfo中的弹窗重复
            // 仅保留UI状态更新逻辑
            if (showWarning && _currentRegistrationInfo != null)
            {
                _logger.LogInformation("已更新UI以显示注册过期状态");
            }
        }

        /// <summary>
        /// 续期按钮点击事件
        /// </summary>
        private async void btnRenewRegInfo_Click(object sender, EventArgs e)
        {
            await SaveRegistrationInfoAsync("续期");
        }

        /// <summary>
        /// 启用续期模式
        /// </summary>
        private void EnableRenewalMode()
        {
            // 启用工具栏的续期相关按钮
            tsbtnCreateRegInfo.Enabled = true;
            tsbtnSaveRegInfo.Enabled = true;
            tsbtnRenewRegInfo.Visible = true; // 进入续期模式时显示续期菜单
            btnGenerateMachineCode.Enabled = true;

            // 启用授权期限选择，用于计算新的到期时间
            lblDays.Visible = true;
            cmbDays.Visible = true;

            // 启用相关编辑字段
            SetControlsReadOnly(false);

            // 聚焦到授权期限选择
            cmbDays.Focus();
        }

        /// <summary>
        /// 授权期限变更
        /// </summary>
        private void cmbDays_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (_currentRegistrationInfo == null || cmbDays.SelectedItem == null)
            {
                return;
            }

            // 根据注册状态选择基准日期
            DateTime baseDate;
            if (!_currentRegistrationInfo.IsRegistered)
            {
                // 未注册过：注册日期为当天，以注册日期为基准
                _currentRegistrationInfo.RegistrationDate = DateTime.Now;
                baseDate = _currentRegistrationInfo.RegistrationDate;
            }
            else
            {
                // 已注册过：以当前授权到期日期为基准
                baseDate = _currentRegistrationInfo.ExpirationDate;
            }

            // 根据选择的期限计算新的到期日期
            switch (cmbDays.SelectedItem.ToString())
            {
                case "一个月":
                    _currentRegistrationInfo.ExpirationDate = baseDate.AddMonths(1);
                    break;
                case "三个月":
                    _currentRegistrationInfo.ExpirationDate = baseDate.AddMonths(3);
                    break;
                case "六个月":
                    _currentRegistrationInfo.ExpirationDate = baseDate.AddMonths(6);
                    break;
                case "一年":
                    _currentRegistrationInfo.ExpirationDate = baseDate.AddYears(1);
                    break;
                case "两年":
                    _currentRegistrationInfo.ExpirationDate = baseDate.AddYears(2);
                    break;
                case "三年":
                    _currentRegistrationInfo.ExpirationDate = baseDate.AddYears(3);
                    break;
                case "五年":
                    _currentRegistrationInfo.ExpirationDate = baseDate.AddYears(5);
                    break;
                case "十年":
                    _currentRegistrationInfo.ExpirationDate = baseDate.AddYears(10);
                    break;
            }
        }

        /// <summary>
        /// 更新主窗体的注册信息
        /// </summary>
        private void UpdateMainFormRegistrationInfo()
        {
            try
            {
                // 更新主窗体的注册信息缓存
                if (frmMainNew.Instance != null)
                {
                    frmMainNew.Instance.registrationInfo = _currentRegistrationInfo;

                    // 重新检查系统注册状态
                    var mainFormType = typeof(frmMainNew);
                    var checkRegistrationMethod = mainFormType.GetMethod("CheckSystemRegistration",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    if (checkRegistrationMethod != null)
                    {
                        checkRegistrationMethod.Invoke(frmMainNew.Instance, null);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "更新主窗体注册信息失败");
            }
        }

        // 私有字段
        private tb_sys_RegistrationInfo _currentRegistrationInfo;
        private Dictionary<string, Enum> descriptionToValueMap;

        private void btnGenerateMachineCode_Click(object sender, EventArgs e)
        {
            try
            {
                // 生成机器码
                if (_currentRegistrationInfo == null)
                {
                    MessageBox.Show("注册信息未加载，无法生成机器码。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 校验授权到期时间至少比当前时间大一个月
                if (_currentRegistrationInfo.ExpirationDate <= DateTime.Now.AddMonths(1))
                {
                    MessageBox.Show("授权到期时间不足一个月，请先完成授权续期后再生成机器码。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查是否选择了功能模块
                var selectedModules = GetSelectedFunctionModules();
                if (selectedModules.Count == 0)
                {
                    MessageBox.Show("请至少选择一个功能模块，否则无法生成机器码。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 保存当前选中的功能模块到注册信息中
                _currentRegistrationInfo.FunctionModule = string.Join(",", selectedModules);

                // 生成机器码
                string machineCode = _registrationService.CreateMachineCode(_currentRegistrationInfo);
                txtMachineCode.Text = machineCode;

                // 将机器码复制到剪贴板，使用Invoke确保在UI线程上执行
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        Clipboard.SetText(machineCode);
                        // 显示成功消息
                        MessageBox.Show("机器码已生成并复制到剪贴板，方便发送给软件服务商进行注册。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "复制机器码到剪贴板失败");
                        // 不影响主要功能，只记录日志
                    }
                }));

                // 记录到日志
                frmMainNew.Instance.SafeLogOperation("请将机器码发送给软件服务商获取注册码", Color.Blue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成机器码时出错");
                MessageBox.Show($"生成机器码时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 我要续期按钮点击事件
        /// </summary>
        private void btnStartRenewal_Click(object sender, EventArgs e)
        {

            if (_currentRegistrationInfo == null)
            {
                MessageBox.Show("注册信息未加载", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 检查是否已注册
            if (!_currentRegistrationInfo.IsRegistered)
            {
                MessageBox.Show("系统尚未注册，请使用注册功能", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 检查是否过期
            bool isExpired = _registrationService.IsRegistrationExpired(_currentRegistrationInfo);
            string message = isExpired ?
                $"当前注册已于 {_currentRegistrationInfo.ExpirationDate:yyyy-MM-dd} 过期。" :
                $"当前注册将于 {_currentRegistrationInfo.ExpirationDate:yyyy-MM-dd} 到期。";

            // 显示续期对话框
            DialogResult result = MessageBox.Show(
                $"{message}\n\n" +
                "续期功能将帮助您延长系统的使用期限。\n\n" +
                "是否立即开始续期流程？",
                "系统续期",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 显示续期操作流程提示
                ShowRenewalProcessTips();

                // 启用续期相关的UI
                EnableRenewalMode();
            }
        }

        /// <summary>
        /// 显示续期操作流程提示
        /// </summary>
        private void ShowRenewalProcessTips()
        {
            // 使用重要的颜色（如红色）显示操作流程
            Color importantColor = Color.Red;

            // 显示操作流程
            frmMainNew.Instance.SafeLogOperation("=== 系统续期操作流程 ===", importantColor);
            frmMainNew.Instance.SafeLogOperation("1. 选择授权到期时间", importantColor);
            frmMainNew.Instance.SafeLogOperation("2. 点击'生成机器码'按钮生成机器码", importantColor);
            frmMainNew.Instance.SafeLogOperation("3. 将机器码发送到软件服务商", importantColor);
            frmMainNew.Instance.SafeLogOperation("4. 获取软件服务商提供的注册码", importantColor);
            frmMainNew.Instance.SafeLogOperation("5. 在注册码输入框中填写正确的注册码", importantColor);
            frmMainNew.Instance.SafeLogOperation("6. 点击'续期'菜单保存续期信息", importantColor);
            frmMainNew.Instance.SafeLogOperation("7. 系统将自动更新并加载最新的注册信息", importantColor);
            frmMainNew.Instance.SafeLogOperation("========================", importantColor);
        }
    }
}