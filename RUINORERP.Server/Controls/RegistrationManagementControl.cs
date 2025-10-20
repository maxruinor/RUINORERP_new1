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
                    BindData(registrationInfo);
                    UpdateUIState(registrationInfo.IsRegistered);
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
            toolStrip1.Visible = !isRegistered;
            dtpRegistrationDate.Visible = isRegistered;
            lblRegistrationDate.Visible = isRegistered;
            btnRegister.Enabled = !isRegistered;
            
            // 根据注册状态设置控件只读状态
            SetControlsReadOnly(isRegistered);
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
                btnRegister.Enabled = true;
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
        /// 注册系统
        /// </summary>
        private async void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateRegistrationInfo())
            {
                return;
            }

            try
            {
                // 验证注册码
                bool isValid = await _registrationService.ValidateRegistrationAsync(_currentRegistrationInfo);
                
                if (isValid)
                {
                    _currentRegistrationInfo.IsRegistered = true;
                    _currentRegistrationInfo.RegistrationDate = DateTime.Now;
                    
                    // 保存注册状态
                    var result = await _registrationService.SaveRegistrationInfoAsync(_currentRegistrationInfo);
                    
                    if (result)
                    {
                        MessageBox.Show("恭喜您，注册成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UpdateUIState(true);
                    }
                    else
                    {
                        MessageBox.Show("注册状态保存失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("注册失败！注册码无效。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _currentRegistrationInfo.IsRegistered = false;
                    _logger.LogWarning("注册验证失败：注册码无效");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "注册失败");
                MessageBox.Show($"注册失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // 私有字段
        private tb_sys_RegistrationInfo _currentRegistrationInfo;
        private Dictionary<string, Enum> descriptionToValueMap;
    }
}