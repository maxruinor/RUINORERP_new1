using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business;
using RUINORERP.UI.Common;
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.UI.SysConfig;
using System.Diagnostics;
using RUINORERP.Business.CommService;
using RUINORERP.Global;

namespace RUINORERP.UI.CRM
{


    [MenuAttrAssemblyInfo("客户线索编辑", true, UIType.单表数据)]
    public partial class UCLeadsEdit : BaseEditGeneric<tb_CRM_Leads>
    {
        public UCLeadsEdit()
        {
            InitializeComponent();
            usedActionStatus = true;
        }

        private tb_CRM_Leads _EditEntity;
        public tb_CRM_Leads EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(BaseEntity entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
          
            tb_CRM_Leads leads = entity as tb_CRM_Leads;
            cmbtxtLeadsStatus.Enabled = false;
            _EditEntity = leads;
            
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);

            if (_EditEntity.LeadID == 0)
            {
                _EditEntity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                //var obj = RUINORERP.Business.Cache.EntityCacheHelper.GetEntity(nameof(tb_Employee), _EditEntity.Employee_ID);
                //if (obj != null)
                //{
                //    var emp = obj as tb_Employee;
                //    cmbEmployee_ID.SelectedIndex = cmbEmployee_ID.FindStringExact(emp.Employee_Name);
                //}
                _EditEntity.LeadsStatus = (int)LeadsStatus.新建;
            }

            DataBindingHelper.BindData4CmbByEnum<tb_CRM_Leads>(entity, k => k.LeadsStatus, typeof(LeadsStatus), cmbtxtLeadsStatus, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.wwSocialTools, txtwwSocialTools, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.SocialTools, txtSocialTools, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.CustomerName, txtCustomerName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.GetCustomerSource, txtGetCustomerSource, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.InterestedProducts, txtInterestedProducts, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Contact_Name, txtContact_Name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Contact_Phone, txtContact_Phone, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Contact_Email, txtContact_Email, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Position, txtPosition, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.SalePlatform, txtSalePlatform, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Website, txtWebsite, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Leads>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            //如果属性变化 则状态为修改
            entity.PropertyChanged += (sender, s2) =>
            {
                if (EditEntity == null)
                {
                    return;
                }

            };
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
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void UCLeadsEdit_Load(object sender, EventArgs e)
        {
            ConfigManager configManager = Startup.GetFromFac<ConfigManager>();
            //“|”号隔开
            string GetCustomerSource = configManager.GetValue("GetCustomerSource");
            //设置常用获客来源
            if (!string.IsNullOrEmpty(GetCustomerSource))
            {
                string[] GetCustomerSourceArr = GetCustomerSource.Split('|');
                AddLabelsToPanel(GetCustomerSourceArr);
            }
        }

        private void AddLabelsToPanel(string[] GetCustomerSourceArr)
        {
            //在Panel容器内0为起点
            int x = 0;  // 起始 X 坐标
            int y = 0;  // 起始 Y 坐标

            foreach (var item in GetCustomerSourceArr)
            {
                KryptonLinkLabel klinklbl = new KryptonLinkLabel();
                klinklbl.Text = item;
                klinklbl.Name = "GetCustomerSource" + item;
                // 根据文本内容计算合适的大小
                using (Graphics g = kPanelGetCustomerSource.CreateGraphics())
                {
                    SizeF textSize = g.MeasureString(klinklbl.Text, klinklbl.Font);
                    klinklbl.Size = new Size((int)textSize.Width + 10, (int)textSize.Height + 5);
                }

                // 根据当前位置设置 Label 的位置
                klinklbl.Location = new Point(x, y);
                klinklbl.LinkClicked += Klinklbl_LinkClicked;
                kPanelGetCustomerSource.Controls.Add(klinklbl);

                // 更新 X 坐标，以便下一个 Label 排列
                x += klinklbl.Width + 10;

                // 如果超出面板宽度，换行
                if (x + klinklbl.Width > kPanelGetCustomerSource.Width)
                {
                    x = 10;
                    y += klinklbl.Height + 10;
                }
            }
        }

        private void Klinklbl_LinkClicked(object sender, EventArgs e)
        {
            KryptonLinkLabel klinklbl = sender as KryptonLinkLabel;
            //如果lblGetCustomerSource这个控件值中包含了标签值，则清除，如果没有包含再添加到值的集合里，用逗号隔开
            if (txtGetCustomerSource.Text.Contains(klinklbl.Text))
            {
                txtGetCustomerSource.Text = txtGetCustomerSource.Text.Replace(klinklbl.Text, "");

            }
            else
            {
                if (txtGetCustomerSource.Text.Length > 0)
                {
                    txtGetCustomerSource.Text = txtGetCustomerSource.Text + "," + klinklbl.Text;

                }
                else
                {
                    txtGetCustomerSource.Text = klinklbl.Text;
                }
            }
            //去掉前导后导,，全角变半角
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Replace(",,", ",");
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Replace("，,", ",");
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Replace(",，", ",");
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Replace("，，", ",");
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Trim(',');
            txtGetCustomerSource.Text = txtGetCustomerSource.Text.Trim('，');
            //操作前将数据收集  保存单据时间出错，这个方法开始是 将查询条件生效
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
        }
    }
}
