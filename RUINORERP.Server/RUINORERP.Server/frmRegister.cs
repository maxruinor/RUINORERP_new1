using FluentValidation.Results;
using HLH.Lib.Helper;
using HLH.Lib.Security;
using HLH.Lib.Security.HLH.Lib.Security;
using Mapster;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RUINORERP.Business;
using RUINORERP.Global;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RUINORERP.Server
{
    /// <summary>
    /// 根据客户的一些基本信息（关键字段） 生成一个注册码。再根据注册码和唯一标识以及注册信息加密成机器码。
    /// 验证时是将机器码解密再来对比信息是否符合
    /// </summary>
    public partial class frmRegister : frmBase
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private async void frmRegister_Load(object sender, EventArgs e)
        {
            //直接打开时
            if (EditEntity == null)
            {
                ////没有返回Null，如果结果大于1条会抛出错误
                EditEntity = await Program.AppContextData.Db.CopyNew().Queryable<tb_sys_RegistrationInfo>().SingleAsync();
            }

            //加载功能模块
            checkedListBoxMod.Items.Clear();
            //checkedListBoxMod.Items.AddRange(EnumHelper.GetEnumDescriptionList(typeof(GlobalFunctionModule)).ToArray());
            descriptionToValueMap = EnumHelper.GetEnumDescriptionToValueMap(typeof(GlobalFunctionModule));
            foreach (var description in descriptionToValueMap.Keys)
            {
                checkedListBoxMod.Items.Add(description);
            }


            //加载注册的信息
            BindData(EditEntity);
            if (EditEntity.IsRegistered)
            {
                toolStrip1.Visible = false;
                dtpRegistrationDate.Visible = true;
                lblRegistrationDate.Visible = true;
            }
            else
            {
                toolStrip1.Visible = true;
                dtpRegistrationDate.Visible = false;
                lblRegistrationDate.Visible = false;
            }

            btnRegister.Enabled = !EditEntity.IsRegistered;
        }
        private void LoadSelections(List<GlobalFunctionModule> selectedModules)
        {
            for (int i = 0; i < checkedListBoxMod.Items.Count; i++)
            {
                string description = checkedListBoxMod.Items[i].ToString();
                GlobalFunctionModule value = (GlobalFunctionModule)descriptionToValueMap[description];
                checkedListBoxMod.SetItemChecked(i, selectedModules.Contains(value));
            }
        }

        private tb_sys_RegistrationInfo _EditEntity;

        public tb_sys_RegistrationInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_sys_RegistrationInfo entity)
        {
            txtCompanyName.DataBindings.Add(new Binding("Text", entity, nameof(entity.CompanyName), true, DataSourceUpdateMode.OnValidation));
            txtContactName.DataBindings.Add(new Binding("Text", entity, nameof(entity.ContactName), true, DataSourceUpdateMode.OnValidation));
            txtPhoneNumber.DataBindings.Add(new Binding("Text", entity, nameof(entity.PhoneNumber), true, DataSourceUpdateMode.OnValidation));
            txtRegistrationCode.DataBindings.Add(new Binding("Text", entity, nameof(entity.RegistrationCode), true, DataSourceUpdateMode.OnValidation));
            txtConcurrentUsers.DataBindings.Add(new Binding("Text", entity, nameof(entity.ConcurrentUsers), true, DataSourceUpdateMode.OnValidation));
            dtpExpirationDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.ExpirationDate), true, DataSourceUpdateMode.OnValidation));
            txtProductVersion.DataBindings.Add(new Binding("Text", entity, nameof(entity.ProductVersion), true, DataSourceUpdateMode.OnValidation));
            txtRegInfo.DataBindings.Add(new Binding("Text", entity, nameof(entity.MachineCode), true, DataSourceUpdateMode.OnValidation));
            //cmbLicenseType.DataBindings.Add(new Binding("SelectedValue", entity, nameof(entity.LicenseType), true, DataSourceUpdateMode.OnValidation));
            dtpPurchaseDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.PurchaseDate), true, DataSourceUpdateMode.OnValidation));
            dtpRegistrationDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.RegistrationDate), true, DataSourceUpdateMode.OnValidation));
            chkIsRegistered.DataBindings.Add(new Binding("Checked", entity, nameof(entity.IsRegistered), true, DataSourceUpdateMode.OnValidation));
            txtRemarks.DataBindings.Add(new Binding("Text", entity, nameof(entity.Remarks), true, DataSourceUpdateMode.OnValidation));

            if (!string.IsNullOrEmpty(entity.LicenseType))
            {
                cmbLicenseType.SelectedIndex = cmbLicenseType.FindString(entity.LicenseType);
            }

            //功能模块可以显示到UI。但是保存到DB中是加密了的。取出来到时也要解密? 
            if (!string.IsNullOrEmpty(entity.FunctionModule))
            {
                //先解密
                entity.FunctionModule = EncryptionHelper.AesDecryptByHashKey(entity.FunctionModule, "FunctionModule");

                //将,号隔开的枚举名称字符串变成List<GlobalFunctionModule>
                List<GlobalFunctionModule> selectedModules = new List<GlobalFunctionModule>();
                string[] enumNameArray = entity.FunctionModule.Split(',');
                foreach (var item in enumNameArray)
                {
                    selectedModules.Add((GlobalFunctionModule)Enum.Parse(typeof(GlobalFunctionModule), item));
                }
                LoadSelections(selectedModules);
            }




            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                if (s2.PropertyName == entity.GetPropertyName<tb_sys_RegistrationInfo>(c => c.MachineCode))
                {
                    btnRegister.Enabled = true;
                }
            };

        }



        private async void btnRegister_Click(object sender, EventArgs e)
        {
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);

            BaseController<tb_sys_RegistrationInfo> ctr = Startup.GetFromFacByName<BaseController<tb_sys_RegistrationInfo>>(typeof(tb_sys_RegistrationInfo).Name + "Controller");

            if (CheckBaseInfo())
            {
                //生成机器码。除了使用了这里的注册信息。还绑定电脑的唯一的ID像CPUID等
                string fixpwd = "ruinor1234567890";
                string regcode = SecurityService.GenerateRegistrationCode(fixpwd, EditEntity.MachineCode);
                if (regcode.Equals(EditEntity.RegistrationCode))
                {
                    EditEntity.IsRegistered = true;
                    EditEntity.RegistrationDate = System.DateTime.Now;
                    MessageBox.Show("恭喜您，注册成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var entiry = await Program.AppContextData.Db.Storageable(EditEntity).DefaultAddElseUpdate().ExecuteReturnEntityAsync();
                    if (entiry.RegistrationInfoD > 0)
                    {
                        frmMain.Instance.PrintInfoLog("保存成功");
                    }
                    else
                    {
                        frmMain.Instance.PrintInfoLog("保存失败");
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("注册失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    EditEntity.IsRegistered = false;
                }

            }
        }


        private bool CheckBaseInfo()
        {
            tb_sys_RegistrationInfoValidator validator = new tb_sys_RegistrationInfoValidator();
            ValidationResult results = validator.Validate(EditEntity);
            IList<ValidationFailure> failures = results.Errors;
            //validator.ValidateAndThrow(info);
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in failures)
            {
                msg.Append(counter.ToString() + ") ");
                msg.Append(item.ErrorMessage).Append("\r\n");
                counter++;
            }

            bool rs = true;
            if (checkedListBoxMod.CheckedItems.Count == 0)
            {
                msg.Append(counter + 1.ToString() + ") ");
                msg.Append("请选择要使用的功能模块。").Append("\r\n");
                counter++;
                rs = false;
            }

            if (!results.IsValid || msg.Length > 0)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            return results.IsValid && rs;
        }

        private Dictionary<string, Enum> descriptionToValueMap;

        private async void tsbtnSaveRegInfo_Click(object sender, EventArgs e)
        {
            if (CheckBaseInfo())
            {
                List<GlobalFunctionModule> selectedModules = new List<GlobalFunctionModule>();
                for (int i = 0; i < checkedListBoxMod.Items.Count; i++)
                {
                    if (checkedListBoxMod.GetItemChecked(i))
                    {
                        string description = checkedListBoxMod.Items[i].ToString();
                        selectedModules.Add((GlobalFunctionModule)descriptionToValueMap[description]);
                    }
                }
                EditEntity.FunctionModule = string.Join(",", selectedModules);
                //加密
                EditEntity.FunctionModule = EncryptionHelper.AesEncryptByHashKey(EditEntity.FunctionModule, "FunctionModule");

                if (EditEntity.RegistrationCode == null)
                {
                    EditEntity.RegistrationCode = string.Empty;
                }
                var entiry = await Program.AppContextData.Db.Storageable(EditEntity).DefaultAddElseUpdate().ExecuteReturnEntityAsync();
                if (entiry.RegistrationInfoD > 0)
                {
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }

        }

        private void btnCreateRegInfo_Click(object sender, EventArgs e)
        {
            //将硬件信息我注册信息加密后给到提供商
            if (CheckBaseInfo())
            {
                List<GlobalFunctionModule> selectedModules = new List<GlobalFunctionModule>();
                for (int i = 0; i < checkedListBoxMod.Items.Count; i++)
                {
                    if (checkedListBoxMod.GetItemChecked(i))
                    {
                        string description = checkedListBoxMod.Items[i].ToString();
                        selectedModules.Add((GlobalFunctionModule)descriptionToValueMap[description]);
                    }
                }
                EditEntity.FunctionModule = string.Join(",", selectedModules);

                //生成机器码前 不加密。因为注册码生成时。提供商要审核时。要看到明码
                EditEntity.MachineCode = frmMain.Instance.CreateMachineCode(EditEntity);

                //加密
                EditEntity.FunctionModule = EncryptionHelper.AesEncryptByHashKey(EditEntity.FunctionModule, "FunctionModule");

                try
                {
                    //将reginfo复制到剪贴板中。并且提示”请将注册信息提供给软件服务商。
                    Clipboard.SetText(EditEntity.MachineCode);
                    MessageBox.Show("注册信息在剪贴板中，请提供给软件所有人注册。");
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void cmbLicenseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditEntity.LicenseType = cmbLicenseType.SelectedItem.ToString();
        }

        private void cmbDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
             一个月
            三个月
            六个月
            一年
            两年
            三年
            五年
            十年
             */

            switch (cmbDays.SelectedItem.ToString())
            {
                case "一个月":
                     EditEntity.ExpirationDate=System.DateTime.Now.AddMonths(1);
                    break;
                case "三个月":
                    EditEntity.ExpirationDate = System.DateTime.Now.AddMonths(3);
                    break;
                case "六个月":
                    EditEntity.ExpirationDate = System.DateTime.Now.AddMonths(6);
                    break;
                case "一年":
                    EditEntity.ExpirationDate = System.DateTime.Now.AddYears(1);
                    break;
                case "两年":
                    EditEntity.ExpirationDate = System.DateTime.Now.AddYears(2);
                    break;
                case "三年":
                    EditEntity.ExpirationDate = System.DateTime.Now.AddYears(3);
                    break;
                case "五年":
                    EditEntity.ExpirationDate = System.DateTime.Now.AddYears(5);
                    break;
                case "十年":
                    EditEntity.ExpirationDate = System.DateTime.Now.AddYears(10);
                    break;
                default:
                    break;
            }

        }
    }

    public class SelectiveContractResolver : DefaultContractResolver
    {
        private readonly List<string> _propertyNamesToSerialize;

        public SelectiveContractResolver(List<string> propertyNamesToSerialize)
        {
            _propertyNamesToSerialize = propertyNamesToSerialize;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            return properties.Where(p => _propertyNamesToSerialize.Contains(p.PropertyName)).ToList();
        }
    }


    //public enum LicenseType
    //{
    //    试用版本 = 1,
    //    正式版本 = 2
    //}

}
