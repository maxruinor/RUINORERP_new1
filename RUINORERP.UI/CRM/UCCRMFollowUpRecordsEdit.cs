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
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using SqlSugar;
using TransInstruction;

namespace RUINORERP.UI.CRM
{


    [MenuAttrAssemblyInfo("跟进记录编辑", true, UIType.单表数据)]
    public partial class UCCRMFollowUpRecordsEdit : BaseEditGeneric<tb_CRM_FollowUpRecords>
    {
        public UCCRMFollowUpRecordsEdit()
        {
            InitializeComponent();
            usedActionStatus = true;
        }

        private tb_CRM_FollowUpRecords _EditEntity;
        public tb_CRM_FollowUpRecords EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public override void BindData(BaseEntity entity, ActionStatus actionStatus = ActionStatus.无操作)
        {

            tb_CRM_FollowUpRecords record = entity as tb_CRM_FollowUpRecords;
            if (record.RecordID == 0)
            {
                //第一次建的时候 应该是业务建的。分配给本人
                record.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                record.FollowUpDate = DateTime.Now;
            }

            _EditEntity = record;

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);


            DataBindingHelper.BindData4CheckBox<tb_CRM_FollowUpRecords>(entity, t => t.HasResponse, chkHasResponse, false);

            DataBindingHelper.BindData4CmbByEnum<tb_CRM_FollowUpRecords>(entity, k => k.FollowUpMethod, typeof(FollowUpMethod), cmbFollowUpMethod, false);



            DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.FollowUpSubject, txtFollowUpSubject, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.FollowUpContent, txtFollowUpContent, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpRecords>(entity, t => t.FollowUpDate, dtpFollowUpDate, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.FollowUpResult, txtFollowUpResult, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

            //创建表达式
            var lambdaCRMCustomer = Expressionable.Create<tb_CRM_Customer>()
                            .And(t => t.Employee_ID != null)
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_Customer).Name + "Processor");
            QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
            queryFilterC.FilterLimitExpressions.Add(lambdaCRMCustomer);

            //带过滤的下拉绑定要这样
            DataBindingHelper.BindData4Cmb<tb_CRM_Customer>(entity, k => k.Customer_id, v => v.CustomerName, cmbCustomer_id, queryFilterC.GetFilterExpression<tb_CRM_Customer>(), true);
            DataBindingHelper.InitFilterForControlByExp<tb_CRM_Customer>(entity, cmbCustomer_id, c => c.CustomerName, queryFilterC);



            //创建表达式
            var lambdaLeads = Expressionable.Create<tb_CRM_Leads>()
                            .And(t => t.isdeleted == false)
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessorLeads = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_Leads).Name + "Processor");
            QueryFilter queryFilterLeads = baseProcessorLeads.GetQueryFilter();
            queryFilterLeads.FilterLimitExpressions.Add(lambdaLeads);

            DataBindingHelper.BindData4Cmb<tb_CRM_Leads>(entity, k => k.LeadID, v => v.CustomerName, cmbLeads, queryFilterLeads.GetFilterExpression<tb_CRM_Leads>(), true);

 


            //创建表达式
            var lambdaPlan = Expressionable.Create<tb_CRM_FollowUpPlans>()
                            .And(t => t.isdeleted == false)
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessorPlan = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_FollowUpPlans).Name + "Processor");
            QueryFilter queryFilterPlan = baseProcessorPlan.GetQueryFilter();
            queryFilterPlan.FilterLimitExpressions.Add(lambdaPlan);
            //来源计划
            DataBindingHelper.BindData4Cmb<tb_CRM_FollowUpPlans>(entity, t => t.PlanID, v => v.PlanContent, cmbPlanID, queryFilterPlan.GetFilterExpression<tb_CRM_FollowUpPlans>(), true);


            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_CRM_FollowUpRecordsValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }

            entity.PropertyChanged += (sender, s2) =>
            {
                //引入相关数据
                if ((record.ActionStatus == ActionStatus.新增 || record.ActionStatus == ActionStatus.修改) && record.PlanID.HasValue && record.PlanID.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_CRM_FollowUpRecords>(c => c.PlanID))
                {
                    var obj = BizCacheHelper.Instance.GetEntity<tb_CRM_FollowUpPlans>(record.PlanID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CRM_FollowUpPlans cv)
                        {
                            EditEntity.Customer_id = cv.Customer_id;
                        }
                    }
                }
            };

            if (EditEntity.Customer_id.HasValue)
            {
                btnNextFollowUpPlan.Visible = true;
            }

            //公海过来的。选择不到客户。因为这时客户还是不是这个人的。所有暂时不绑定显示出来。
            if (_EditEntity.tb_crm_customer != null)
            {
                if (_EditEntity.tb_crm_customer.Employee_ID == null || _EditEntity.tb_crm_customer.Employee_ID == 0)
                {
                    cmbCustomer_id.Visible = false;
                }
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

        private async void UCLeadsEdit_Load(object sender, EventArgs e)
        {
            tb_CRMConfig CRMConfig = await MainForm.Instance.AppContext.Db.Queryable<tb_CRMConfig>().FirstAsync();
            if (CRMConfig != null)
            {
                if (CRMConfig.CS_UseLeadsFunction)
                {
                    lblLeads.Visible = true;
                    cmbLeads.Visible = true;
                }
                else
                {
                    lblLeads.Visible = false;
                    cmbLeads.Visible = false;
                }
            }

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


        #region 添加跟进主题
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
            //一次跟进主题不能超过三个
            if (txtFollowUpSubject.Text.Split(',').Length > 3)
            {
                MessageBox.Show("一次跟进过程，主题不能超过三个", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            //如果lblGetCustomerSource这个控件值中包含了标签值，则清除，如果没有包含再添加到值的集合里，用逗号隔开
            if (txtFollowUpSubject.Text.Contains(klinklbl.Text))
            {
                txtFollowUpSubject.Text = txtFollowUpSubject.Text.Replace(klinklbl.Text, "");

            }
            else
            {
                if (txtFollowUpSubject.Text.Length > 0)
                {
                    txtFollowUpSubject.Text = txtFollowUpSubject.Text + "," + klinklbl.Text;

                }
                else
                {
                    txtFollowUpSubject.Text = klinklbl.Text;
                }
            }
            //去掉前导后导,，全角变半角
            txtFollowUpSubject.Text = txtFollowUpSubject.Text.Replace(",,", ",");
            txtFollowUpSubject.Text = txtFollowUpSubject.Text.Replace("，,", ",");
            txtFollowUpSubject.Text = txtFollowUpSubject.Text.Replace(",，", ",");
            txtFollowUpSubject.Text = txtFollowUpSubject.Text.Replace("，，", ",");
            txtFollowUpSubject.Text = txtFollowUpSubject.Text.Trim(',');
            txtFollowUpSubject.Text = txtFollowUpSubject.Text.Trim('，');
            //操作前将数据收集  保存单据时间出错，这个方法开始是 将查询条件生效
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
        }




        #endregion

        private async void btnNextFollowUpPlan_Click(object sender, EventArgs e)
        {

            object frm = Activator.CreateInstance(typeof(UCCRMFollowUpPlansEdit));
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                BaseEditGeneric<tb_CRM_FollowUpPlans> frmaddg = frm as BaseEditGeneric<tb_CRM_FollowUpPlans>;
                frmaddg.Text = "跟进计划编辑";
                frmaddg.bindingSourceEdit.DataSource = new List<tb_CRM_FollowUpPlans>();
                object obj = frmaddg.bindingSourceEdit.AddNew();
                tb_CRM_FollowUpPlans EntityInfo = obj as tb_CRM_FollowUpPlans;
                BusinessHelper.Instance.InitEntity(EntityInfo);
                if (EditEntity.Customer_id.HasValue)
                {
                    EntityInfo.Customer_id = EditEntity.Customer_id.Value;
                }

                BaseEntity bty = EntityInfo as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                frmaddg.BindData(bty, ActionStatus.新增);
                if (frmaddg.ShowDialog() == DialogResult.OK)
                {
                    BaseController<tb_CRM_FollowUpPlans> ctrContactInfo = Startup.GetFromFacByName<BaseController<tb_CRM_FollowUpPlans>>(typeof(tb_CRM_FollowUpPlans).Name + "Controller");
                    ReturnResults<tb_CRM_FollowUpPlans> result = await ctrContactInfo.BaseSaveOrUpdate(EntityInfo);
                    if (result.Succeeded)
                    {
                        if (result.Succeeded)
                        {
                            //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                            //只处理需要缓存的表
                            if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(tb_CRM_FollowUpPlans).Name, out pair))
                            {
                                //如果有更新变动就上传到服务器再分发到所有客户端
                                OriginalData odforCache = ActionForClient.更新缓存<tb_CRM_FollowUpPlans>(result.ReturnObject);
                                byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                                MainForm.Instance.ecs.client.Send(buffer);
                            }
                        }
                        MainForm.Instance.ShowStatusText("添加成功!");
                    }
                    else
                    {
                        MainForm.Instance.ShowStatusText("添加失败!");
                    }
                }
            }

        }
    }
}
