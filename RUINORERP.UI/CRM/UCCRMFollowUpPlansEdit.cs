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
using RUINORERP.Common.Extensions;
using RUINORERP.UI.BI;
using Castle.Core.Resource;

namespace RUINORERP.UI.CRM
{


    [MenuAttrAssemblyInfo("跟进计划编辑", true, UIType.单表数据)]
    public partial class UCCRMFollowUpPlansEdit : BaseEditGeneric<tb_CRM_FollowUpPlans>
    {
        public UCCRMFollowUpPlansEdit()
        {
            InitializeComponent();
            usedActionStatus = true;
        }

        private tb_CRM_FollowUpPlans _EditEntity;
        public tb_CRM_FollowUpPlans EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(BaseEntity entity, ActionStatus actionStatus = ActionStatus.无操作)
        {

            tb_CRM_FollowUpPlans customer = entity as tb_CRM_FollowUpPlans;
            if (customer.PlanID == 0)
            {
                //第一次建的时候 应该是业务建的。分配给本人
                customer.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                customer.PlanStatus = (int)FollowUpPlanStatus.未开始;
                customer.PlanStartDate = DateTime.Now.AddDays(1);
                customer.PlanEndDate= DateTime.Now.AddDays(2);
            }

            cmbPlanStatus.Enabled = false;
            _EditEntity = customer;

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
              DataBindingHelper.BindData4Cmb<tb_CRM_Customer>(entity, k => k.Customer_id, v=>v.CustomerName, cmbCustomer_id);
         
            DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpPlans>(entity, t => t.PlanStartDate, dtpPlanStartDate, false);
            DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpPlans>(entity, t => t.PlanEndDate, dtpPlanEndDate, false);

            DataBindingHelper.BindData4CmbByEnum<tb_CRM_FollowUpPlans>(entity, k => k.PlanStatus, typeof(FollowUpPlanStatus), cmbPlanStatus, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.PlanSubject, txtPlanSubject, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.PlanContent, txtPlanContent, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

           

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_CRM_FollowUpPlansValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

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
           // ConfigManager configManager = Startup.GetFromFac<ConfigManager>();
            //“|”号隔开
            //string GetCustomerSource = configManager.GetValue("GetCustomerSource");
      
            string[] enumStrings = Enum.GetNames(typeof(FollowUpSubject)).Select(x => x).ToArray();
            string combinedString = string.Join("|", enumStrings);
            //设置主题
            if (!string.IsNullOrEmpty(combinedString))
            {
                string[] CustomerTagsArr = combinedString.Split('|');
                AddCustomerSourceLabelsToPanel(CustomerTagsArr);
            }
        }


        #region 添加客户来源
        private void AddCustomerSourceLabelsToPanel(string[] GetCustomerSourceArr)
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
                using (Graphics g = kPanelPlanSubject.CreateGraphics())
                {
                    SizeF textSize = g.MeasureString(klinklbl.Text, klinklbl.Font);
                    klinklbl.Size = new Size((int)textSize.Width + 10, (int)textSize.Height + 5);
                }

                // 根据当前位置设置 Label 的位置
                klinklbl.Location = new Point(x, y);
                klinklbl.LinkClicked += Klinklbl_LinkClicked;
                kPanelPlanSubject.Controls.Add(klinklbl);

                // 更新 X 坐标，以便下一个 Label 排列
                x += klinklbl.Width + 10;

                // 如果超出面板宽度，换行
                if (x + klinklbl.Width > kPanelPlanSubject.Width)
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
            if (txtPlanSubject.Text.Contains(klinklbl.Text))
            {
                txtPlanSubject.Text = txtPlanSubject.Text.Replace(klinklbl.Text, "");

            }
            else
            {
                if (txtPlanSubject.Text.Length > 0)
                {
                    txtPlanSubject.Text = txtPlanSubject.Text + "," + klinklbl.Text;

                }
                else
                {
                    txtPlanSubject.Text = klinklbl.Text;
                }
            }
            //去掉前导后导,，全角变半角
            txtPlanSubject.Text = txtPlanSubject.Text.Replace(",,", ",");
            txtPlanSubject.Text = txtPlanSubject.Text.Replace("，,", ",");
            txtPlanSubject.Text = txtPlanSubject.Text.Replace(",，", ",");
            txtPlanSubject.Text = txtPlanSubject.Text.Replace("，，", ",");
            txtPlanSubject.Text = txtPlanSubject.Text.Trim(',');
            txtPlanSubject.Text = txtPlanSubject.Text.Trim('，');
            //操作前将数据收集  保存单据时间出错，这个方法开始是 将查询条件生效
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
        }

        #endregion
 
    }
}
