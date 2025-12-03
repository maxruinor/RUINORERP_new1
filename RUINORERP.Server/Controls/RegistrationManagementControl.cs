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
            await LoadRegistrationInfo();
            InitializeFunctionModules();
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

            // 添加新的数据绑定
            txtCompanyName.DataBindings.Add(new Binding("Text", entity, nameof(entity.CompanyName), true, DataSourceUpdateMode.OnValidation));
            txtContactName.DataBindings.Add(new Binding("Text", entity, nameof(entity.ContactName), true, DataSourceUpdateMode.OnValidation));
            txtPhoneNumber.DataBindings.Add(new Binding("Text", entity, nameof(entity.PhoneNumber), true, DataSourceUpdateMode.OnValidation));
            txtRegistrationCode.DataBindings.Add(new Binding("Text", entity, nameof(entity.RegistrationCode), true, DataSourceUpdateMode.OnValidation));
            txtConcurrentUsers.DataBindings.Add(new Binding("Text", entity, nameof(entity.ConcurrentUsers), true, DataSourceUpdateMode.OnValidation));
            dtpExpirationDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.ExpirationDate), true, DataSourceUpdateMode.OnValidation));
            txtProductVersion.DataBindings.Add(new Binding("Text", entity, nameof(entity.ProductVersion), true, DataSourceUpdateMode.OnValidation));
            txtMachineCode.DataBindings.Add(new Binding("Text", entity, nameof(entity.MachineCode), true, DataSourceUpdateMode.OnValidation));
            dtpPurchaseDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.PurchaseDate), true, DataSourceUpdateMode.OnValidation));
            dtpRegistrationDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.RegistrationDate), true, DataSourceUpdateMode.OnValidation));
            chkIsRegistered.DataBindings.Add(new Binding("Checked", entity, nameof(entity.IsRegistered), true, DataSourceUpdateMode.OnValidation));
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
                // 解密功能模块信息
                string decryptedModule = EncryptionHelper.AesDecryptByHashKey(functionModule, "FunctionModule");

                // 将逗号分隔的枚举名称字符串转换为List<GlobalFunctionModule>
                List<GlobalFunctionModule> selectedModules = new List<GlobalFunctionModule>();
                string[] enumNameArray = decryptedModule.Split(',');
                foreach (var item in enumNameArray)
                {
                    if (Enum.TryParse(typeof(GlobalFunctionModule), item, out object module))
                    {
                        selectedModules.Add((GlobalFunctionModule)module);
                    }
                }

                LoadSelections(selectedModules);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "加载功能模块失败");
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
                // 如果已注册但过期，显示续期选项
                showRenewalOption = isRegistered && isExpired;
            }

            // 工具栏按钮控制
            tsbtnSaveRegInfo.Visible = !isRegistered || showRenewalOption; // 未注册或过期时可保存
            tsbtnCreateRegInfo.Visible = !isRegistered || showRenewalOption; // 未注册或过期时可生成
            tsbtnRenewRegInfo.Visible = showRenewalOption; // 只有过期时显示续期按钮
            //btnGenerateMachineCode.Text = showRenewalOption ? "续期" : "注册"; // 过期时显示"续期"
            btnGenerateMachineCode.Enabled = !isRegistered || showRenewalOption; // 未注册或过期时可注册/续期

            // 注册日期显示和编辑控制
            dtpRegistrationDate.Visible = isRegistered; // 已注册时显示（包括过期）
            dtpRegistrationDate.Enabled = !isRegistered || showRenewalOption; // 未注册或续期时可编辑
            lblRegistrationDate.Visible = isRegistered;

            // 授权期限控制：只在未注册或过期时显示（用于注册时计算到期时间）
            lblDays.Visible = !isRegistered || showRenewalOption;
            cmbDays.Visible = !isRegistered || showRenewalOption;

            // 如果已过期，添加过期提示
            if (isExpired)
            {
                lblRegistrationDate.Text = "注册日期（过期）:";
                dtpRegistrationDate.Visible = true;
                lblRegistrationDate.Visible = true;
            }
            else
            {
                lblRegistrationDate.Text = "注册日期:";
            }

            // 根据注册状态设置控件只读状态
            SetControlsReadOnly(isRegistered && !isExpired); // 有效注册时只读，过期时可编辑
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
            if (!ValidateRegistrationInfo())
            {
                return;
            }

            try
            {
                // 获取选中的功能模块
                var selectedModules = GetSelectedFunctionModules();
                _currentRegistrationInfo.FunctionModule = string.Join(",", selectedModules);

                // 加密功能模块信息
                _currentRegistrationInfo.FunctionModule = EncryptionHelper.AesEncryptByHashKey(
                    _currentRegistrationInfo.FunctionModule, "FunctionModule");

                // 保存到数据库
                var result = await _registrationService.SaveRegistrationInfoAsync(_currentRegistrationInfo);

                if (result)
                {
                    MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("保存失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _logger.LogWarning("注册信息保存失败");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存注册信息失败");
                MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 生成注册信息
        /// </summary>
        private void btnCreateRegInfo_Click(object sender, EventArgs e)
        {
            if (!ValidateRegistrationInfo())
            {
                return;
            }

            try
            {
                // 获取选中的功能模块（生成机器码前不加密）
                var selectedModules = GetSelectedFunctionModules();
                _currentRegistrationInfo.FunctionModule = string.Join(",", selectedModules);

                // 生成机器码
                _currentRegistrationInfo.MachineCode = _registrationService.CreateMachineCode(_currentRegistrationInfo);

                // 加密功能模块信息
                _currentRegistrationInfo.FunctionModule = EncryptionHelper.AesEncryptByHashKey(
                    _currentRegistrationInfo.FunctionModule, "FunctionModule");

                // 复制到剪贴板
                Clipboard.SetText(_currentRegistrationInfo.MachineCode);
                MessageBox.Show("注册信息已复制到剪贴板，请提供给软件服务商进行注册。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成注册信息失败");
                MessageBox.Show($"生成注册信息失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 注册系统/续期
        /// </summary>
        private async void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateRegistrationInfo())
            {
                return;
            }

            try
            {
                // 判断是续期还是新注册
                bool isRenewal = _currentRegistrationInfo.IsRegistered &&
                               _registrationService.IsRegistrationExpired(_currentRegistrationInfo);

                // 验证注册码
                bool isValid = await _registrationService.ValidateRegistrationAsync(_currentRegistrationInfo);

                if (isValid)
                {
                    _currentRegistrationInfo.IsRegistered = true;

                    // 如果是续期，更新注册时间为当前时间，但保留原购买时间等信息
                    if (isRenewal)
                    {
                        _currentRegistrationInfo.RegistrationDate = DateTime.Now;
                        _logger.LogInformation("系统续期成功");
                    }
                    else
                    {
                        _currentRegistrationInfo.RegistrationDate = DateTime.Now;
                        _logger.LogInformation("系统注册成功");
                    }

                    // 保存注册状态
                    var result = await _registrationService.SaveRegistrationInfoAsync(_currentRegistrationInfo);

                    if (result)
                    {
                        string successMsg = isRenewal ?
                            "恭喜您，续期成功！系统注册已延期。" :
                            "恭喜您，注册成功！";

                        MessageBox.Show(successMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 更新主窗体的注册信息
                        await UpdateMainFormRegistrationInfo();

                        UpdateUIState(true);
                    }
                    else
                    {
                        string errorMsg = isRenewal ? "续期状态保存失败" : "注册状态保存失败";
                        MessageBox.Show(errorMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    string errorTitle = isRenewal ? "续期失败" : "注册失败";
                    MessageBox.Show($"{errorTitle}！注册码无效。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _currentRegistrationInfo.IsRegistered = false;
                    _logger.LogWarning($"{errorTitle}验证失败：注册码无效");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "注册/续期失败");
                MessageBox.Show($"操作失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 验证注册信息
        /// </summary>
        /// <returns>验证是否通过</returns>
        private bool ValidateRegistrationInfo()
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

            try
            {
                var registrationInfo = frmMainNew.Instance.registrationInfo;
                if (registrationInfo != null)
                {
                    bool isValid = _registrationService.CheckRegistered(registrationInfo);
                    if (isValid)
                    {
                        MessageBox.Show("注册信息验证通过", "验证结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("注册信息验证失败", "验证结果", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"验证注册信息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// 显示过期警告信息
        /// </summary>
        private void PrintExpirationWarning()
        {
            if (_currentRegistrationInfo != null)
            {
                var daysExpired = DateTime.Now - _currentRegistrationInfo.ExpirationDate;
                string warningMsg = $"系统注册已于 {_currentRegistrationInfo.ExpirationDate:yyyy-MM-dd} 过期，已过期 {daysExpired.Days} 天。\n\n" +
                                 "请进行续期或重新注册以继续使用系统。";

                MessageBox.Show(warningMsg, "注册已过期", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 续期按钮点击事件
        /// </summary>
        private void btnRenewRegInfo_Click(object sender, EventArgs e)
        {
            if (_currentRegistrationInfo == null)
            {
                MessageBox.Show("注册信息未加载", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 检查是否真的需要续期
            if (!_currentRegistrationInfo.IsRegistered)
            {
                MessageBox.Show("系统尚未注册，请使用注册功能", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!_registrationService.IsRegistrationExpired(_currentRegistrationInfo))
            {
                MessageBox.Show("注册尚未到期，无需续期", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 显示续期对话框
            DialogResult result = MessageBox.Show(
                $"当前注册已于 {_currentRegistrationInfo.ExpirationDate:yyyy-MM-dd} 过期。\n\n" +
                "续期功能将帮助您延长系统的使用期限。\n\n" +
                "是否立即开始续期流程？",
                "系统续期",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 启用续期相关的UI
                EnableRenewalMode();
            }
        }

        /// <summary>
        /// 启用续期模式
        /// </summary>
        private void EnableRenewalMode()
        {
            // 启用工具栏的续期相关按钮
            tsbtnCreateRegInfo.Enabled = true;
            tsbtnSaveRegInfo.Enabled = true;
            btnGenerateMachineCode.Enabled = true;
            btnGenerateMachineCode.Text = "续期";

            // 启用授权期限选择，用于计算新的到期时间
            lblDays.Visible = true;
            cmbDays.Visible = true;

            // 启用相关编辑字段
            SetControlsReadOnly(false);

            // 聚焦到授权期限选择
            cmbDays.Focus();

            _logger.LogInformation("已启用续期模式");
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

            switch (cmbDays.SelectedItem.ToString())
            {
                case "一个月":
                    _currentRegistrationInfo.ExpirationDate = DateTime.Now.AddMonths(1);
                    break;
                case "三个月":
                    _currentRegistrationInfo.ExpirationDate = DateTime.Now.AddMonths(3);
                    break;
                case "六个月":
                    _currentRegistrationInfo.ExpirationDate = DateTime.Now.AddMonths(6);
                    break;
                case "一年":
                    _currentRegistrationInfo.ExpirationDate = DateTime.Now.AddYears(1);
                    break;
                case "两年":
                    _currentRegistrationInfo.ExpirationDate = DateTime.Now.AddYears(2);
                    break;
                case "三年":
                    _currentRegistrationInfo.ExpirationDate = DateTime.Now.AddYears(3);
                    break;
                case "五年":
                    _currentRegistrationInfo.ExpirationDate = DateTime.Now.AddYears(5);
                    break;
                case "十年":
                    _currentRegistrationInfo.ExpirationDate = DateTime.Now.AddYears(10);
                    break;
            }
        }

        /// <summary>
        /// 更新主窗体的注册信息
        /// </summary>
        private async Task UpdateMainFormRegistrationInfo()
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
                var registrationInfo = frmMainNew.Instance.registrationInfo;
                if (registrationInfo != null)
                {
                    string machineCode = _registrationService.CreateMachineCode(registrationInfo);
                    txtMachineCode.Text = machineCode;
                    MessageBox.Show("机器码已生成", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成机器码时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}