using Krypton.Toolkit;
using Netron.GraphLib;
using RUINORERP.Business;
using RUINORERP.Business.LogicaService;
using RUINORERP.Common;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.UI.UserCenter.DataParts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winista.Text.HtmlParser.Lex;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("角色信息编辑", true, UIType.单表数据)]
    public partial class UCRoleInfoEdit : BaseEditGeneric<tb_RoleInfo>
    {
        public UCRoleInfoEdit()
        {
            InitializeComponent();
        }





        tb_RoleInfo roleinfo;
        public override void BindData(BaseEntity entity)
        {
            roleinfo = entity as tb_RoleInfo;
            DataBindingHelper.BindData4TextBox<tb_RoleInfo>(entity, t => t.RoleName, txtRoleName, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBox<tb_RoleInfo>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text, true);
            base.errorProviderForAllInput.DataSource = entity;
            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_RoleInfoValidator>(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
            //为了性能通用查询中没有添加自动导航了
            if (roleinfo.tb_rolepropertyconfig == null && roleinfo.RolePropertyID.HasValue)
            {
                var tb_rolepropertyconfig = MainForm.Instance.AppContext.Db.Queryable<tb_RolePropertyConfig>().Where(c => c.RolePropertyID == roleinfo.RolePropertyID.Value)
            .Single();
                roleinfo.tb_rolepropertyconfig = tb_rolepropertyconfig;
            }


            //处理角色属性
            if (roleinfo.tb_rolepropertyconfig != null)
            {
                chk创建角色属性.Checked = true;
                //如果添加了就不能删除
                chk创建角色属性.Enabled = false;
                //BindDataForProperty(roleinfo.tb_rolepropertyconfig);
            }
            else
            {
                chk创建角色属性.Checked = false;
            }
            base.BindData(entity);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }




        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator())
            {
                if (chk创建角色属性.Checked)
                {
                    var roleinfo = bindingSourceEdit.Current as tb_RoleInfo;
                    if (!base.Validator(roleinfo.tb_rolepropertyconfig))
                    {
                        return;
                    }

                }
                else
                {
                    roleinfo.tb_rolepropertyconfig = null;
                    roleinfo.RolePropertyID = null;
                }
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void chk创建角色属性_CheckedChanged(object sender, EventArgs e)
        {
            if (chk创建角色属性.Checked == true)
            {
                if (roleinfo.tb_rolepropertyconfig == null)
                {
                    roleinfo.tb_rolepropertyconfig = new tb_RolePropertyConfig();
                    roleinfo.tb_rolepropertyconfig.RolePropertyName = $"{roleinfo.RoleName}的角色属性配置";
                }
                BindDataForProperty(roleinfo.tb_rolepropertyconfig);
            }
            kryptonGroupBox角色属性.Visible = chk创建角色属性.Checked;
        }

        public void BindDataForProperty(tb_RolePropertyConfig propertyConfig)
        {
            DataBindingHelper.BindData4TextBox<tb_RolePropertyConfig>(propertyConfig, t => t.QtyDataPrecision, txtQtyDataPrecision, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_RolePropertyConfig>(propertyConfig, t => t.TaxRateDataPrecision, txtTaxRateDataPrecision, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_RolePropertyConfig>(propertyConfig, t => t.MoneyDataPrecision, txtMoneyDataPrecision, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(propertyConfig, t => t.CurrencyDataPrecisionAutoAddZero, chkCurrencyDataPrecisionAutoAddZero, false);
            DataBindingHelper.BindData4TextBox<tb_RolePropertyConfig>(propertyConfig, t => t.CostCalculationMethod, txtCostCalculationMethod, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(propertyConfig, t => t.ShowDebugInfo, chkShowDebugInfo, false);
            DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(propertyConfig, t => t.SaleBizLimited, chkSaleBizLimited, false);
            DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(propertyConfig, t => t.DepartBizLimited, chkDepartBizLimited, false);
            DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(propertyConfig, t => t.PurchsaeBizLimited, chkPurchsaeBizLimited, false);
            DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(propertyConfig, t => t.ExclusiveLimited, chk启用责任人独占, false);
            DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(propertyConfig, t => t.QueryPageLayoutCustomize, chkQueryPageLayoutCustomize, false);
            DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(propertyConfig, t => t.QueryGridColCustomize, chkQueryGridColCustomize, false);
            DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(propertyConfig, t => t.BillGridColCustomize, chkBillGridColCustomize, false);
            DataBindingHelper.BindData4CheckBox<tb_RolePropertyConfig>(propertyConfig, t => t.OwnershipControl, chkOwnershipControl, false);
            //DataBindingHelper.InitDataToCmbChkWithCondition<tb_RolePropertyConfig>(propertyConfig, t => t.DataBoardUnits, chkDataBoardUnits, BindDataType4TextBox.Text, false);
            #region chkDataBoardUnits 绑定
            chkDataBoardUnits.Items.Clear();
            Array enumValues = Enum.GetValues(typeof(DataBoardUnit));
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            string currentName;
            while (e.MoveNext())
            {
                currentValue = (int)e.Current;
                currentName = e.Current.ToString();
                chkDataBoardUnits.Items.Add(currentName);
            }
            var depa = new Binding("Text", propertyConfig, "DataBoardUnits", true, DataSourceUpdateMode.OnPropertyChanged);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) =>
            {
                args.Value = args.Value == null ? new List<object>() : args.Value;
            };

            depa.Parse += (s, args) =>
            {
                args.Value = args.Value == null ? new List<object>() : args.Value;
            };
            chkDataBoardUnits.DataBindings.Add(depa);
            #endregion





            //如果属性变化 则状态为修改
            propertyConfig.PropertyChanged += (sender, s2) =>
            {
                //也可以指定具体属性变化
                roleinfo.ActionStatus = ActionStatus.修改;
            };

        }

    
    }
}

