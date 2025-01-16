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
using Netron.GraphLib;
using RUINORERP.Business.Processor;
using SqlSugar;
using AutoMapper;
using RUINORERP.Global;
using RUINORERP.Business.AutoMapper;
using Krypton.Docking;
using Krypton.Navigator;
using RUINORERP.UI.CRM.DockUI;
using Krypton.Workspace;
using NPOI.SS.Formula.Functions;
using System.Reflection;
using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;
using System.Numerics;
using TransInstruction;

namespace RUINORERP.UI.CRM
{
    [MenuAttrAssemblyInfo("目标客户编辑", true, UIType.单表数据)]
    public partial class UCCRMCustomerEdit : BaseEditGeneric<tb_CRM_Customer>
    {
        public UCCRMCustomerEdit()
        {
            InitializeComponent();
            usedActionStatus = true;
        }

        private tb_CRM_Customer _EditEntity;
        public tb_CRM_Customer EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public async override void BindData(BaseEntity entity, ActionStatus actionStatus = ActionStatus.无操作)
        {

            tb_CRM_Customer customer = entity as tb_CRM_Customer;
            if (customer.Customer_id == 0)
            {
                //第一次建的时候 应该是业务建的。分配给本人
                customer.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                customer.CustomerStatus = (int)CustomerStatus.新增客户;
                customer.CustomerLevel = 1;
            }

            cmbCustomerStatus.Enabled = false;
            _EditEntity = customer;

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartmentID);
            //DataBindingHelper.BindData4Cmb<tb_CRM_Leads>(entity, k => k.LeadID, v => v.CustomerName, cmbLeadID, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID);
            //创建表达式
            var lambdaLeads = Expressionable.Create<tb_CRM_Leads>()
                            .And(t => t.isdeleted == false)
                            .AndIF(AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessorLeads = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_Leads).Name + "Processor");
            QueryFilter queryFilterLeads = baseProcessorLeads.GetQueryFilter();
            queryFilterLeads.FilterLimitExpressions.Add(lambdaLeads);

            DataBindingHelper.BindData4Cmb<tb_CRM_Leads>(entity, k => k.LeadID, v => v.CustomerName, cmbLeadID, queryFilterLeads.GetFilterExpression<tb_CRM_Leads>(), true);

            DataBindingHelper.BindData4Cmb<tb_CRM_Region>(entity, k => k.Region_ID, v => v.Region_Name, cmbRegion_ID);
            DataBindingHelper.BindData4Cmb<tb_Provinces>(entity, k => k.ProvinceID, v => v.ProvinceCNName, cmbProvinceID);
            DataBindingHelper.BindData4Cmb<tb_Cities>(entity, k => k.CityID, v => v.CityCNName, cmbCityID);

            DataBindingHelper.BindData4CmbByEnum<tb_CRM_Customer>(entity, k => k.CustomerStatus, typeof(CustomerStatus), cmbCustomerStatus, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Contact_Phone, txtContact_Phone, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Contact_Email, txtContact_Email, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Contact_Name, txtContact_Name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CustomerName, txtCustomerName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CustomerAddress, txtCustomerAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_CRM_Customer>(entity, t => t.RepeatCustomer, chkRepeatCustomer, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CustomerTags, txtCustomerTags, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.wwSocialTools, txtwwSocialTools, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.SocialTools, txtSocialTools, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.GetCustomerSource, txtGetCustomerSource, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.CoreProductInfo, txtCoreProdInfo, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.SalePlatform, txtSalePlatform, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Website, txtWebsite, BindDataType4TextBox.Text, false);

            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.PurchaseCount, txtPurchaseCount, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.TotalPurchaseAmount.ToString(), txtTotalPurchaseAmount, BindDataType4TextBox.Money, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.DaysSinceLastPurchase, txtDaysSinceLastPurchase, BindDataType4TextBox.Qty, false);
            DataBindingHelper.BindData4DataTime<tb_CRM_Customer>(entity, t => t.LastPurchaseDate, dtpLastPurchaseDate, false);
            DataBindingHelper.BindData4DataTime<tb_CRM_Customer>(entity, t => t.FirstPurchaseDate, dtpFirstPurchaseDate, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_Customer>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);

            txtCustomerLevel.DataBindings.Clear();
            //级别1到10
            txtCustomerLevel.Value = 0;
            var sort = new Binding("Value", entity, "CustomerLevel", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            sort.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            sort.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            txtCustomerLevel.DataBindings.Add(sort);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService <tb_CRM_CustomerValidator> (), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
            if (customer.Customer_id > 0)
            {
                btnFastFollowUp.Visible = true;
            }
            else
            {
                btnFastFollowUp.Visible = false;
            }
            entity.PropertyChanged += async (sender, s2) =>
            {
                //如果线索引入相关数据
                if ((customer.ActionStatus == ActionStatus.新增 || customer.ActionStatus == ActionStatus.修改) && customer.LeadID.HasValue && customer.LeadID.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_CRM_Customer>(c => c.LeadID))
                {
                    await ToCustomer(customer, customer.LeadID.Value);
                }
                //如果实体中的下拉可以不用选。但是UI选择了请选择会将值设置为-1，验证框架会不通过。这里强制-1就改为null
                if (customer.ActionStatus == ActionStatus.新增 || customer.ActionStatus == ActionStatus.修改)
                {
                    //下拉特征
                    if (entity.GetPropertyValue(s2.PropertyName) != null && entity.GetPropertyValue(s2.PropertyName).ToString() == "-1")
                    {
                        Type type = typeof(tb_CRM_Customer);
                        PropertyInfo fieldinfo = type.GetProperty(s2.PropertyName);
                        if (fieldinfo != null)
                        {
                            //s2.GetType() == typeof(long) && 
                            var propertyType = fieldinfo.PropertyType;
                            if (Nullable.GetUnderlyingType(propertyType) != null)
                            {
                                entity.SetPropertyValue(s2.PropertyName, null);
                            }
                        }
                    }

                }




            };

            if (CRMConfig == null)
            {
                CRMConfig = await MainForm.Instance.AppContext.Db.Queryable<tb_CRMConfig>().FirstAsync();
                if (CRMConfig != null && CRMConfig.CS_UseLeadsFunction)
                {
                    tb_CRM_LeadsController<tb_CRM_Leads> cvctr = Startup.GetFromFac<tb_CRM_LeadsController<tb_CRM_Leads>>();
                    //创建表达式
                    var lambda = Expressionable.Create<tb_CRM_Leads>()
                                    //.And(t => t.IsCustomer == true)//转过的不再转？
                                    .And(t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)
                                    .ToExpression();//注意 这一句 不能少
                    BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CRM_Leads).Name + "Processor");
                    QueryFilter queryFilterC = baseProcessor.GetQueryFilter();
                    queryFilterC.FilterLimitExpressions.Add(lambda);



                    DataBindingHelper.InitFilterForControlByExp<tb_CRM_Leads>(entity, cmbLeadID, c => c.CustomerName, queryFilterC);

                }
            }

            #region 加载跟踪情况
            bool loadTrack = false;
            UCTrackContainer uCTrackRecordses = new UCTrackContainer();
            if (customer.tb_CRM_FollowUpRecordses != null && customer.tb_CRM_FollowUpRecordses.Count > 0)
            {

                if (customer.tb_CRM_FollowUpRecordses.Count > 0)
                {
                    foreach (var item in customer.tb_CRM_FollowUpRecordses)
                    {
                        UCFollowUpRecord ucrecord = new UCFollowUpRecord();
                        ucrecord.BindData(item);
                        uCTrackRecordses.flowLayoutPanel1.Controls.Add(ucrecord);
                        loadTrack = true;
                    }
                }
            }

            UCTrackContainer uCTrackPlans = new UCTrackContainer();
            if (customer.tb_CRM_FollowUpPlanses != null && customer.tb_CRM_FollowUpPlanses.Count > 0)
            {

                if (customer.tb_CRM_FollowUpPlanses.Count > 0)
                {
                    foreach (var item in customer.tb_CRM_FollowUpPlanses)
                    {
                        UCFollowUpPlan ucplan = new UCFollowUpPlan();
                        ucplan.BindData(item);
                        uCTrackPlans.flowLayoutPanel1.Controls.Add(ucplan);
                        loadTrack = true;
                    }

                }
            }

            #region 加载联系人

            UCCRMContact uCContact = new UCCRMContact();
            if (customer.tb_CRM_Contacts != null && customer.tb_CRM_Contacts.Count > 0)
            {
                if (customer.tb_CRM_Contacts.Count > 0)
                {
                    uCContact.BindData<tb_CRM_Contact>(customer.tb_CRM_Contacts.Select(c => new
                    {
                        c.Contact_Name,
                        c.Contact_Phone,
                        c.Contact_Email
                    }).ToList());
                    loadTrack = true;
                }
            }

            #endregion

            if (loadTrack)
            {
                // Setup docking functionality
                KryptonDockingWorkspace w = kryptonDockingManager.ManageWorkspace(kryptonDockableWorkspace1);
                kryptonDockingManager.ManageControl(kryptonPanelBig, w);
                kryptonDockingManager.ManageFloating(this);

                kryptonDockingManager.ShowPageContextMenu += KryptonDockingManager1_ShowPageContextMenu;
                kryptonDockingManager.FloatingWindowAdding += KryptonDockingManager1_FloatingWindowAdding;
                kryptonDockableWorkspace1.WorkspaceCellAdding += kryptonDockableWorkspace1_WorkspaceCellAdding;

                KryptonPage kprecords = UIForKryptonHelper.NewPage("跟踪记录", uCTrackRecordses);
                kprecords.AllowDrop = false;
                kprecords.SetFlags(KryptonPageFlags.All);
                kprecords.Width = 300;
                KryptonPage kpplans = UIForKryptonHelper.NewPage("跟踪计划", uCTrackPlans);
                kpplans.AllowDrop = false;
                kpplans.SetFlags(KryptonPageFlags.All);
                kpplans.Width = 500;


                KryptonPage kpContacts = UIForKryptonHelper.NewPage("联系人", uCContact);
                kpContacts.AllowDrop = false;
                kpContacts.SetFlags(KryptonPageFlags.All);
                kpContacts.Width = 300;

                // Add docking pages
                kryptonDockingManager.AddDockspace("Control", DockingEdge.Right, new KryptonPage[] { kpplans, kpContacts });
                //kryptonDockingManager.AddDockspace("Control", DockingEdge.Right, new KryptonPage[] { kprecords, kpplans });
                kryptonDockingManager.AddToWorkspace("Workspace", new KryptonPage[] { kprecords });

                for (int i = 0; i < kryptonDockingManager.Pages.Count(); i++)
                {
                    kryptonDockingManager.Pages[i].ClearFlags(KryptonPageFlags.DockingAllowClose);//隐藏关闭的x
                }
                // kryptonDockingManager.MakeAutoHiddenRequest(kpplans.UniqueName);//默认加载时隐藏
                // kryptonDockingManager.MakeAutoHiddenRequest(kprecords.UniqueName);//默认加载时隐藏
            }


            #endregion




            base.BindData(entity);
        }

        private void kryptonDockableWorkspace1_WorkspaceCellAdding(object sender, WorkspaceCellEventArgs e)
        {
            e.Cell.Button.CloseButtonAction = CloseButtonAction.HidePage;
            e.Cell.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            KryptonWorkspaceCell cell = e.Cell;
            cell.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            cell.CloseAction += Cell_CloseAction;
            //cell.SelectedPageChanged += Cell_SelectedPageChanged;
            //cell.ShowContextMenu += Cell_ShowContextMenu;
            cell.Dock = DockStyle.Fill;
            cell.AllowDrop = true;
            cell.AllowPageDrag = true;
            //这里可以对具体的单元设置
            if (cell.Pages.FirstOrDefault(c => c.Name == "") != null)
            {

            }
        }

        private void Cell_CloseAction(object sender, CloseActionEventArgs e)
        {
            //关闭事件
            e.Action = CloseButtonAction.HidePage;
        }

        private void KryptonDockingManager1_FloatingWindowAdding(object sender, FloatingWindowEventArgs e)
        {
            e.FloatingWindow.CloseBox = false;
        }

        private void KryptonDockingManager1_ShowPageContextMenu(object sender, ContextPageEventArgs e)
        {
            //不显示右键
            e.Cancel = true;
        }


        //===

        private async Task<tb_CRM_Customer> ToCustomer(tb_CRM_Customer entity, long refId)
        {
            tb_CRM_Leads crmLeads;
            ButtonSpecAny bsa = cmbLeadID.ButtonSpecs.FirstOrDefault(c => c.UniqueName == "btnQuery");
            if (bsa == null)
            {
                return null;
            }
            //saleorder = bsa.Tag as tb_SaleOrder;

            crmLeads = await MainForm.Instance.AppContext.Db.Queryable<tb_CRM_Leads>()
            .Where(c => c.LeadID == refId)
            .SingleAsync();

            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            mapper.Map(crmLeads, entity);  // 直接将 crmLeads 的值映射到传入的 entity 对象上，保持了引用
                                           // entity = mapper.Map<tb_CRM_Customer>(crmLeads);//这个是直接重新生成了对象。
            entity.ActionStatus = ActionStatus.新增;

            List<string> tipsMsg = new List<string>();

            StringBuilder msg = new StringBuilder();
            foreach (var item in tipsMsg)
            {
                msg.Append(item).Append("\r\n");
            }
            if (tipsMsg.Count > 0)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            BusinessHelper.Instance.InitEntity(entity);
            return entity;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator(EditEntity))
            {
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        tb_CRMConfig CRMConfig = null;
        private async void UCLeadsEdit_Load(object sender, EventArgs e)
        {
            // tb_CRMConfigController<tb_CRMConfig> ctr = Startup.GetFromFac<tb_CRMConfigController<tb_CRMConfig>>();
            CRMConfig = await MainForm.Instance.AppContext.Db.Queryable<tb_CRMConfig>().FirstAsync();
            if (CRMConfig != null)
            {
                if (CRMConfig.CS_UseLeadsFunction)
                {
                    lblLeadID.Visible = true;
                    cmbLeadID.Visible = true;

                }
                else
                {
                    lblLeadID.Visible = false;
                    cmbLeadID.Visible = false;
                }
            }

            //通过动态配置得到。
            ConfigManager configManager = Startup.GetFromFac<ConfigManager>();


            //“|”号隔开
            string GetCustomerSource = configManager.GetValue("GetCustomerSource");
            //设置常用获客来源
            if (!string.IsNullOrEmpty(GetCustomerSource))
            {
                string[] GetCustomerSourceArr = GetCustomerSource.Split('|');
                AddCustomerSourceLabelsToPanel(GetCustomerSourceArr);
            }

            string[] enumStrings = Enum.GetNames(typeof(CustomerTags)).Select(x => x).ToArray();
            string combinedString = string.Join("|", enumStrings);
            //设置常用客户标签
            if (!string.IsNullOrEmpty(combinedString))
            {
                string[] CustomerTagsArr = combinedString.Split('|');
                AddCustomerTagsLabelsToPanel(CustomerTagsArr);
            }




        }



        #region 添加客户标签
        private void AddCustomerTagsLabelsToPanel(string[] CustomerTagsArr)
        {
            //在Panel容器内0为起点
            int x = 0;  // 起始 X 坐标
            int y = 0;  // 起始 Y 坐标

            foreach (var item in CustomerTagsArr)
            {
                KryptonLinkLabel klinklbl = new KryptonLinkLabel();
                klinklbl.Text = item;
                klinklbl.Name = "CustomerTags" + item;
                // 根据文本内容计算合适的大小
                using (Graphics g = kPanelCustomerTags.CreateGraphics())
                {
                    SizeF textSize = g.MeasureString(klinklbl.Text, klinklbl.Font);
                    klinklbl.Size = new Size((int)textSize.Width + 10, (int)textSize.Height + 5);
                }

                // 根据当前位置设置 Label 的位置
                klinklbl.Location = new Point(x, y);
                klinklbl.LinkClicked += KlinkTagslbl_LinkClicked;
                kPanelCustomerTags.Controls.Add(klinklbl);

                // 更新 X 坐标，以便下一个 Label 排列
                x += klinklbl.Width + 10;

                // 如果超出面板宽度，换行
                if (x + klinklbl.Width > kPanelCustomerTags.Width)
                {
                    x = 10;
                    y += klinklbl.Height + 10;
                }
            }
        }

        private void KlinkTagslbl_LinkClicked(object sender, EventArgs e)
        {
            KryptonLinkLabel klinklbl = sender as KryptonLinkLabel;
            //如果lblGetCustomerSource这个控件值中包含了标签值，则清除，如果没有包含再添加到值的集合里，用逗号隔开
            if (txtCustomerTags.Text.Contains(klinklbl.Text))
            {
                txtCustomerTags.Text = txtCustomerTags.Text.Replace(klinklbl.Text, "");

            }
            else
            {
                if (txtCustomerTags.Text.Length > 0)
                {
                    txtCustomerTags.Text = txtCustomerTags.Text + "," + klinklbl.Text;

                }
                else
                {
                    txtCustomerTags.Text = klinklbl.Text;
                }
            }
            //去掉前导后导,，全角变半角
            txtCustomerTags.Text = txtCustomerTags.Text.Replace(",,", ",");
            txtCustomerTags.Text = txtCustomerTags.Text.Replace("，,", ",");
            txtCustomerTags.Text = txtCustomerTags.Text.Replace(",，", ",");
            txtCustomerTags.Text = txtCustomerTags.Text.Replace("，，", ",");
            txtCustomerTags.Text = txtCustomerTags.Text.Trim(',');
            txtCustomerTags.Text = txtCustomerTags.Text.Trim('，');
            //操作前将数据收集  保存单据时间出错，这个方法开始是 将查询条件生效
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
        }


        #endregion

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


        #endregion

        private void btnAddContactInfo_Click(object sender, EventArgs e)
        {
            object frm = Activator.CreateInstance(typeof(UCCRMContactEdit));
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                BaseEditGeneric<tb_CRM_Contact> frmaddg = frm as BaseEditGeneric<tb_CRM_Contact>;
                frmaddg.CurMenuInfo = this.CurMenuInfo;
                frmaddg.Text = "联系人编辑";
                frmaddg.bindingSourceEdit.DataSource = new List<tb_CRM_Contact>();
                object obj = frmaddg.bindingSourceEdit.AddNew();
                tb_CRM_Contact ContactInfo = obj as tb_CRM_Contact;
                ContactInfo.Customer_id = _EditEntity.Customer_id;
                ContactInfo.Contact_Name = _EditEntity.Contact_Name;
                ContactInfo.Contact_Phone = _EditEntity.Contact_Phone;
                ContactInfo.Contact_Email = _EditEntity.Contact_Email;
                ContactInfo.tb_crm_customer = _EditEntity;
                BaseEntity bty = ContactInfo as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                BusinessHelper.Instance.EditEntity(bty);
                frmaddg.BindData(bty, ActionStatus.无操作);
                if (frmaddg.ShowDialog() == DialogResult.OK)
                {
                    //暂时没有限制不让修改。但是客户对应不能是其它客户。这里再一次指定一下。
                    ContactInfo.Customer_id = _EditEntity.Customer_id;
                    UIBizSrvice.SaveCRMContact(ContactInfo);
                }
            }
        }

        private async void btnFastFollowUp_Click(object sender, EventArgs e)
        {
            object frm = Activator.CreateInstance(typeof(UCCRMFollowUpRecordsEdit));
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                BaseEditGeneric<tb_CRM_FollowUpRecords> frmaddg = frm as BaseEditGeneric<tb_CRM_FollowUpRecords>;
                frmaddg.CurMenuInfo = this.CurMenuInfo;
                frmaddg.Text = "跟进记录编辑";
                frmaddg.bindingSourceEdit.DataSource = new List<tb_CRM_FollowUpRecords>();
                object obj = frmaddg.bindingSourceEdit.AddNew();
                tb_CRM_FollowUpRecords NewInfo = obj as tb_CRM_FollowUpRecords;
                NewInfo.Customer_id = _EditEntity.Customer_id;
                NewInfo.tb_crm_customer = _EditEntity;
                NewInfo.Employee_ID = _EditEntity.Employee_ID.Value;
                BaseEntity bty = NewInfo as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                BusinessHelper.Instance.EditEntity(bty);
                frmaddg.BindData(bty, ActionStatus.新增);
                if (frmaddg.ShowDialog() == DialogResult.OK)
                {
                    BaseController<tb_CRM_FollowUpRecords> ctr = Startup.GetFromFacByName<BaseController<tb_CRM_FollowUpRecords>>(typeof(tb_CRM_FollowUpRecords).Name + "Controller");
                    ReturnResults<tb_CRM_FollowUpRecords> result = await ctr.BaseSaveOrUpdate(NewInfo);
                    if (result.Succeeded)
                    {
                        //记录添加成功后。客户如果是新客户 则转换为 潜在客户
                        if (_EditEntity != null)
                        {
                            if (_EditEntity.CustomerStatus == (int)CustomerStatus.新增客户)
                            {
                                _EditEntity.CustomerStatus = (int)CustomerStatus.潜在客户;
                                BaseController<tb_CRM_Customer> ctrContactInfo = Startup.GetFromFacByName<BaseController<tb_CRM_Customer>>(typeof(tb_CRM_Customer).Name + "Controller");
                                ReturnResults<tb_CRM_Customer> resultCustomer = await ctrContactInfo.BaseSaveOrUpdate(_EditEntity);
                                if (resultCustomer.Succeeded)
                                {

                                }
                            }

                            //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                            //只处理需要缓存的表
                            if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(tb_CRM_FollowUpRecords).Name, out pair))
                            {
                                //如果有更新变动就上传到服务器再分发到所有客户端
                                OriginalData odforCache = ActionForClient.更新缓存<tb_CRM_FollowUpRecords>(result.ReturnObject);
                                byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                                MainForm.Instance.ecs.client.Send(buffer);
                            }
                        }
                    }
                }
            }
        }

        private void cmbLeadID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCollaborate_Click(object sender, EventArgs e)
        {
            object frm = Activator.CreateInstance(typeof(UCCRMCollaboratorEdit));
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                BaseEditGeneric<tb_CRM_Collaborator> frmaddg = frm as BaseEditGeneric<tb_CRM_Collaborator>;
                frmaddg.CurMenuInfo = this.CurMenuInfo;
                frmaddg.Text = "协作人编辑";
                frmaddg.bindingSourceEdit.DataSource = new List<tb_CRM_Collaborator>();
                object obj = frmaddg.bindingSourceEdit.AddNew();
                tb_CRM_Collaborator ContactInfo = obj as tb_CRM_Collaborator;
                ContactInfo.Customer_id = _EditEntity.Customer_id;
                ContactInfo.Created_at = System.DateTime.Now;
                ContactInfo.Created_by= MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID.Value;
                BaseEntity bty = ContactInfo as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                BusinessHelper.Instance.EditEntity(bty);
                frmaddg.BindData(bty, ActionStatus.无操作);
                if (frmaddg.ShowDialog() == DialogResult.OK)
                {
                    //暂时没有限制不让修改。但是客户对应不能是其它客户。这里再一次指定一下。
                    ContactInfo.Customer_id = _EditEntity.Customer_id;
                    UIBizSrvice.SaveCRMCollaborator(ContactInfo);
                }
            }
        }
    }
}
