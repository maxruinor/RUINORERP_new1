using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Krypton.Toolkit;
using RUINORERP.Common.Helper;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINORERP.Model;
using RUINOR.Core;
using RUINORERP.UI.Common;
using RUINORERP.UI.BI;
using RUINORERP.Common.CustomAttribute;
using System.Linq.Expressions;
using System.Collections.Concurrent;
using RUINORERP.Business;
using RUINORERP.Global.CustomAttribute;
using ObjectsComparer;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Model.Base;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Business.Processor;
using System.Reflection;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.IO;

using RUINORERP.UI.SuperSocketClient;
using RUINORERP.Model.Models;
using System.Diagnostics;
using RUINORERP.Global.Model;
using RUINORERP.UI.CommonUI;
using RUINORERP.UI.FormProperty;
using System.Web.UI;
using Control = System.Windows.Forms.Control;
using SqlSugar;
using SourceGrid.Cells.Models;
using RUINORERP.Business.CommService;
using SixLabors.ImageSharp.Memory;
using Netron.NetronLight;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINORERP.UI.UserCenter;
using RUINORERP.UI.UserPersonalized;
using RUINORERP.UI.UControls;
using Newtonsoft.Json;
using Fireasy.Common.Extensions;
using FastReport.DevComponents.DotNetBar.Controls;
using RUINORERP.UI.ChartFramework.Models;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;

namespace RUINORERP.UI.BaseForm
{

    public partial class BaseChartReport : BaseUControl
    {
        public BaseChartReport()
        {
            InitializeComponent();
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                if (!this.DesignMode)
                {
                    //权限菜单
                    if (CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty())
                    {
                        CurMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.FormName == this.Name && m.ClassPath == this.ToString()).FirstOrDefault();
                        if ((CurMenuInfo == null || CurMenuInfo.ClassPath.IsNullOrEmpty()) && !MainForm.Instance.AppContext.IsSuperUser)
                        {
                            //MessageBox.Show(this.ToString() + " BaseChartReport菜单不能为空，请联系管理员。");
                            MainForm.Instance.PrintInfoLog(this.ToString() + " BaseChartReport菜单不能为空，请联系管理员。");
                            return;
                        }
                    }

                    #region 绑定菜单事件

                    foreach (var item in BaseToolStrip.Items)
                    {
                        if (item is ToolStripButton btnItem)
                        {
                            ToolStripButton subItem = item as ToolStripButton;
                            subItem.Click += Item_Click;
                            ControlButton(subItem);

                        }
                        if (item is ToolStripDropDownButton subItemDr)
                        {
                            ControlButton(subItemDr);
                            subItemDr.Click += Item_Click;
                            //下一级
                            if (subItemDr.HasDropDownItems)
                            {
                                foreach (var sub in subItemDr.DropDownItems)
                                {
                                    ToolStripMenuItem subStripMenuItem = sub as ToolStripMenuItem;
                                    ControlButton(subStripMenuItem);
                                    subStripMenuItem.Click += Item_Click;
                                }

                            }
                        }


                    }

                    #endregion
                    Builder();
                    // dataGridView1.CellFormatting -= DataGridView1_CellFormatting;
                }
            }
        }

        /// <summary>
        /// 用来保存外键表名与外键主键列名  通过这个打到对应的名称。
        /// </summary>
        public static ConcurrentDictionary<string, string> FKValueColNameTBList = new ConcurrentDictionary<string, string>();


        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }


        /// <summary>
        /// 外键关联点，意思是：单位换算表中的两个单位字段不等于原始单位表中的主键字段，这里手动关联指向一下
        /// </summary>
        public ConcurrentDictionary<string, string> ForeignkeyPoints { get; set; } = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 手动添加外键关联指向
        /// </summary>
        /// <typeparam name="T1">单位表</typeparam>
        /// <typeparam name="T2">单位换算，产品等引用单位的表，字段和主键不一样时使用</typeparam>
        /// <param name="SourceField"></param>
        /// <param name="TargetField"></param>
        public void SetForeignkeyPointsList<T1, T2>(Expression<Func<T1, object>> SourceField, Expression<Func<T2, object>> TargetField)
        {
            MemberInfo Sourceinfo = SourceField.GetMemberInfo();
            string sourceName = Sourceinfo.Name;
            MemberInfo Targetinfo = TargetField.GetMemberInfo();
            string TargetName = Targetinfo.Name;
            if (ForeignkeyPoints == null)
            {
                ForeignkeyPoints = new ConcurrentDictionary<string, string>();
            }
            //以目标为主键，原始的相同的只能为值
            ForeignkeyPoints.TryAdd(TargetName, sourceName);
        }


        public virtual void Builder()
        {
            QueryConditionBuilder();
            //默认展开查询条件
            if (QueryConditionFilter != null && QueryConditionFilter.QueryFields.Count > 0)
            {
                kryptonHeaderGroupTop.Collapsed = false;
            }
            else
            {
                kryptonHeaderGroupTop.Collapsed = true;
            }
            BuildSummaryCols();
            BuildInvisibleCols();
            BuildRelatedDisplay();
        }

        /// <summary>
        /// 构建关联显示的一些数据
        /// </summary>
        public virtual void BuildRelatedDisplay()
        {

        }

        public virtual void BuildSummaryCols()
        {

        }

        public virtual void BuildInvisibleCols()
        {
            //if (_UCBillChildQuery_Related.InvisibleCols == null)
            //{
            //    _UCBillChildQuery_Related.InvisibleCols = new List<string>();
            //}
            //_UCBillChildQuery_Related.InvisibleCols.AddRange(ExpressionHelper.ExpressionListToStringList(ChildRelatedInvisibleCols));
            //_UCBillChildQuery_Related.DefaultHideCols = new List<string>();

            //UIHelper.ControlColumnsInvisible(CurMenuInfo, _UCBillChildQuery_Related.InvisibleCols, _UCBillChildQuery_Related.DefaultHideCols);
        }


        public void ControlButton(ToolStripMenuItem btnItem)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Buttons == null)
                {
                    btnItem.Visible = false;
                }
                else
                {
                    //如果因为热键 Text改变了。到时再处理
                    tb_P4Button p4b = CurMenuInfo.tb_P4Buttons.Where(b => b.tb_buttoninfo.BtnText == btnItem.Text).FirstOrDefault();
                    if (p4b != null)
                    {
                        btnItem.Visible = p4b.IsVisble;
                        btnItem.Enabled = p4b.IsEnabled;
                    }
                    else
                    {
                        btnItem.Visible = false;
                    }
                }
            }
        }

        public void ControlButton(ToolStripDropDownButton btnItem)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Buttons == null)
                {
                    btnItem.Visible = false;
                }
                else
                {
                    //如果因为热键 Text改变了。到时再处理
                    tb_P4Button p4b = CurMenuInfo.tb_P4Buttons.Where(b => b.tb_buttoninfo.BtnText == btnItem.Text).FirstOrDefault();
                    if (p4b != null)
                    {
                        btnItem.Visible = p4b.IsVisble;
                        btnItem.Enabled = p4b.IsEnabled;
                    }
                    else
                    {
                        btnItem.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// 没有权限的菜单不显示
        /// </summary>
        /// <param name="btnItem"></param>
        public void ControlButton(ToolStripButton btnItem)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Buttons == null)
                {
                    btnItem.Visible = false;
                }
                else
                {
                    //如果因为热键 Text改变了。到时再处理
                    tb_P4Button p4b = CurMenuInfo.tb_P4Buttons.Where(b => b.tb_buttoninfo.BtnText == btnItem.Text).FirstOrDefault();
                    if (p4b != null)
                    {
                        btnItem.Visible = p4b.IsVisble;
                        btnItem.Enabled = p4b.IsEnabled;
                    }
                    else
                    {
                        btnItem.Visible = false;
                    }
                }
            }
            else
            {
                btnItem.Visible = true;
            }
        }



        protected Result ComPare<C>(C t, C s)
        {
            Result result = new Result();
            var comparer = new ObjectsComparer.Comparer<C>();
            IEnumerable<ObjectsComparer.Difference> differences;
            bool isEqual = comparer.Compare(t, s, out differences);
            result.IsEqual = isEqual;
            if (!isEqual)
            {
                string differencesMsg = string.Join(Environment.NewLine, differences);
                result.Msg = differencesMsg;
            }
            return result;
        }



        public class Result
        {
            public bool IsEqual { get; set; }
            public string Msg { get; set; }

        }


        private Type _EditForm;

        /// <summary>
        /// 这个的作用在哪里？
        /// </summary>
        public Type EditForm { get => _EditForm; set => _EditForm = value; }




        /// <summary>
        /// 不同情况，显示不同的可用情况
        /// </summary>
        internal void ToolBarEnabledControl(MenuItemEnums menu)
        {
            //ToolStripButton

            switch (menu)
            {

                case MenuItemEnums.查询:

                    break;

                //case MenuItemEnums.高级查询:
                //    toolStripButtonSave.Enabled = false;
                //    toolStripBtnAdvQuery.Visible = true;
                //    ControlButton(toolStripBtnAdvQuery);
                //    break;
                case MenuItemEnums.关闭:
                    break;
                case MenuItemEnums.刷新:
                    break;
                case MenuItemEnums.打印:
                    break;
                case MenuItemEnums.导出:
                    break;
                default:
                    break;
            }

        }











        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public virtual void LimitQueryConditionsBuilder()
        {

        }

        private void Item_Click(object sender, EventArgs e)
        {
            MainForm.Instance.AppContext.log.ActionName = sender.ToString();
            if (sender.ToString().Length > 0)
            {
                DoButtonClick(EnumHelper.GetEnumByString<MenuItemEnums>(sender.ToString()));
            }
            else
            {

            }

        }

        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected virtual void DoButtonClick(MenuItemEnums menuItem)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            switch (menuItem)
            {

                case MenuItemEnums.查询:

                    Query();


                    break;

                case MenuItemEnums.关闭:
                    Exit(this);
                    break;
                case MenuItemEnums.刷新:
                    break;
                case MenuItemEnums.导出:
                    ExportExcel();
                    break;

                case MenuItemEnums.属性:
                    Property();
                    break;
                case MenuItemEnums.帮助:
                    SysHelp(CurMenuInfo.CaptionCN);
                    break;
                default:
                    break;
            }
        }

        public frmFormProperty frm = null;
        protected virtual void Property()
        {
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //保存属性
                ToolBarEnabledControl(MenuItemEnums.属性);
                //MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("属性", EditEntity);
            }
        }

        /// <summary>
        /// 帮助
        /// </summary>
        /// <param name="PageName">欢迎.htm</param>
        /// <param name="TagID">html标签a中的id或name</param>
        public virtual void SysHelp(string PageName = null, string TagID = null)
        {
            //hh.exe参数（全）

            /*
             hh.exe	-800	将Help viewer设为800*600
            -title	将chm以窗口800*600显示
            -register	注册hh.exe，将其设为默认的chm文档的shell
            -decompile	反编译chm文件，就是将chm拆散开来，对于破坏狂和翻译人员比较有用，懒人就免了
            -mapid	如果你记住chm中htm、html的id，那么用它定位htm、html文件
            -safe	迫使hh.exe以安全模式打开chm。安全模式？就是所有的快捷键都失效
            原文链接：https://blog.csdn.net/tuwen/article/details/3166696
             */
            // 指定 CHM 文件路径和要定位的页面及段落（这里只是示例，你需要根据实际情况设置）
            string chmFilePath = System.IO.Path.Combine(Application.StartupPath, "help.chm");
            //string targetPage = "基础资料.htm";
            //string targetParagraph1 = "包装信息.htm";
            //string targetParagraph2 = "包装信息\\卡通箱";
            //string targetParagraph3 = "卡通箱.htm";

            // 使用 HH.exe 来打开 CHM 文件并指定定位
            try
            {
                if (PageName.IsNullOrEmpty())
                {
                    Process.Start("hh.exe", $"{chmFilePath}");
                }
                else
                {
                    //Process.Start("hh.exe", $"\"{chmFilePath}\"::{targetPage}#{targetParagraph1}");

                    PageName += ".htm";//必须带.htm后缀
                    if (TagID.IsNullOrEmpty())
                    {
                        Process.Start("hh.exe", $"{chmFilePath}::{PageName}");
                    }
                    else
                    {
                        Process.Start("hh.exe", $"{chmFilePath}::{PageName}#{TagID}");
                    }
                }
                //测试ok
                //Process.Start("hh.exe", "E:\\CodeRepository\\SynologyDrive\\RUINORERP\\RUINORERP.UI\\bin\\Debug\\help.chm::欢迎.htm");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开 CHM 文件出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #region 定义所有工具栏的方法
        public virtual void ExportExcel()
        {
            //var EditEntitys = ListDataSoure.DataSource as List<T>;
            //if (EditEntitys == null || EditEntitys.Count == 0)
            //{
            //    return false;
            //}
            // UIExcelHelper.ExportExcel(dataGridView1);
        }




        public virtual void QueryConditionBuilder()
        {
            //BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(T).Name + "Processor");
            //QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        /// <summary>
        /// 扩展带条件查询
        /// </summary>
        protected virtual void ExtendedQuery(bool UseAutoNavQuery = false)
        {
            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;



            //既然前台指定的查询哪些字段，到时可以配置。这里应该是 除软件删除外的。其他字段不需要

            int pageNum = 1;
            int pageSize = int.Parse(txtMaxRows.Text);

            //提取指定的列名，即条件集合
            // List<string> queryConditions = new List<string>();
            //queryConditions = new List<string>(QueryConditionFilter.QueryFields.Select(t => t.FieldName).ToList());
            //list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, queryConditions, QueryConditionFilter.GetFilterExpression<T>(), QueryDto, pageNum, pageSize) as List<T>;

            if (QueryConditionFilter.FilterLimitExpressions == null)
            {
                QueryConditionFilter.FilterLimitExpressions = new List<LambdaExpression>();
            }


            ToolBarEnabledControl(MenuItemEnums.查询);
        }



        /// <summary>
        /// 保存默认隐藏的列  
        /// HashSet比List性能更好
        /// 为了提高性能，特别是当 InvisibleCols 和 DefaultHideCols 列表较大时，可以使用 HashSet<string> 替代 List<string>。HashSet<string> 的查找性能更高（平均时间复杂度为 O(1)），而 List<string> 的查找性能为 O(n)。
        /// </summary>
        public HashSet<string> DefaultHideCols { get; set; } = new HashSet<string>();




        /// <summary>
        /// 保存不可见的列
        /// </summary>
        public HashSet<string> InvisibleCols { get; set; } = new HashSet<string>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="QueryParameters"></param>
        /// <param name="nodeParameter"></param>
        internal override void LoadQueryParametersToUI(BaseEntity QueryParameters, QueryParameter nodeParameter)
        {
            if (QueryParameters != null && nodeParameter != null)
            {
                if (nodeParameter.queryFilter != null)
                {
                    QueryConditionFilter = nodeParameter.queryFilter;
                }

                //nodeParameter参数中包含了这个实体的KEY主键是可以通过主键来查询到准确的一行数据
                // QueryConditionFilter.SetQueryField<tb_CRM_FollowUpPlans>(c => c.Customer_id, true);

                #region  因为时间不会去掉选择，这里特殊处理
                foreach (var item in nodeParameter.queryFilter.QueryFields)
                {
                    Type propertyType = null;
                    if (item.FieldPropertyInfo.PropertyType.IsGenericType && item.FieldPropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = item.FieldPropertyInfo.PropertyType.GenericTypeArguments[0];
                    }
                    else
                    {
                        propertyType = item.FieldPropertyInfo.PropertyType;
                    }

                    if (propertyType.Name == "DateTime")
                    {
                        //因为查询UI生成时。自动 转换成代理类如：tb_SaleOutProxy，并且时间是区间型式,将起为null即可
                        QueryDtoProxy.SetPropertyValue(item.FieldName + "_Start", null);

                        if (kryptonPanel条件生成容器.Controls.Find(item.FieldName, true)[0] is UCAdvDateTimerPickerGroup timerPickerGroup)
                        {
                            timerPickerGroup.dtp1.Checked = false;
                            timerPickerGroup.dtp2.Checked = false;
                        }
                        //KryptonDateTimePicker dtp = _UCBillQueryCondition.kryptonPanelQuery.Controls.Find(item.FieldName + "_Start", true) as KryptonDateTimePicker;
                        //if (dtp != null)
                        //{
                        //    dtp.check
                        //}
                    }


                }

                #endregion

                QueryDtoProxy = QueryParameters;
                ExtendedQuery(true);
            }
            else
            {
                Refreshs();
            }
        }





        /// <summary>
        /// 与高级查询执行结果公共使用，如果null时，则执行普通查询？
        /// </summary>
        /// <param name="UseNavQuery">是否使用自动导航</param>
        //[MustOverride]
        public  override void Query(bool UseAutoNavQuery = false)
        {
            if (Edited)
            {
                if (MessageBox.Show("你有数据没有保存，当前操作会丢失数据\r\n你确定不保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
            }


            if (QueryConditionFilter == null || QueryConditionFilter.QueryFields == null || QueryConditionFilter.QueryFields.Count == 0)
            {


            }
            else
            {
                ExtendedQuery(UseAutoNavQuery);
            }
            ToolBarEnabledControl(MenuItemEnums.查询);


        }

  





        #endregion


        private void CloseTheForm(object thisform)
        {
            KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }
            if ((thisform as Control).Parent is KryptonPage)
            {
                KryptonPage page = (thisform as Control).Parent as KryptonPage;
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                page.Dispose();
            }
            else
            {
                Form frm = (thisform as Control).Parent.Parent as Form;
                frm.Close();
            }

            /*
           if (page == null)
           {
               //浮动

           }
           else
           {
               //活动内
               if (cell.Pages.Contains(page))
               {
                   cell.Pages.Remove(page);
                   page.Dispose();
               }
           }
           */
        }


        protected override void Exit(object thisform)
        {
            //保存请求的配置？
            //UIBizSrvice.SaveGridSettingData(CurMenuInfo, dataGridView1, typeof(T));
            if (!Edited)
            {
                //退出
                CloseTheForm(thisform);
            }
            else
            {
                if (MessageBox.Show(this, "有数据没有保存\r\n你确定要退出吗?   这里是不是可以进一步提示 哪些内容没有保存？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //退出
                    CloseTheForm(thisform);
                }
            }


        }



        protected override void Refreshs()
        {
            LimitQueryConditionsBuilder();
            if (!Edited)
            {
                Query();
            }
            else
            {
                if (MessageBox.Show(this, "有数据没有保存\r\n你确定要重新加载吗", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Query();
                }
            }

        }




        private void BaseList_Load(object sender, EventArgs e)
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime || this.DesignMode)
            {
                return;
            }
            if (!this.DesignMode)
            {
                QueryDtoProxy = LoadQueryConditionToUI(4);
            }
        }

        /// <summary>
        /// 默认不是模糊查询
        /// </summary>
        /// <param name="useLike"></param>
        public BaseEntity LoadQueryConditionToUI(decimal QueryConditionShowColQty = 4)
        {
            //为了验证设置的属性
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            //UIQueryHelper<T> uIQueryHelper = new UIQueryHelper<T>();
            kryptonPanel条件生成容器.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(kryptonPanel条件生成容器, true, null);
            kryptonPanel条件生成容器.Visible = false;
            kryptonPanel条件生成容器.Controls.Clear();
            kryptonPanel条件生成容器.SuspendLayout();
            QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(DataRequest), true, kryptonPanel条件生成容器, QueryConditionFilter, QueryConditionShowColQty);
            //if (MainForm.Instance.AppContext.CurrentUser_Role == null && MainForm.Instance.AppContext.IsSuperUser)
            //{
            //   // QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(T), true, kryptonPanel条件生成容器, QueryConditionFilter, QueryConditionShowColQty);
            //}
            //else
            //{
            //    //暂时默认了uselike
            //    tb_UIMenuPersonalization menuSetting = MainForm.Instance.AppContext.CurrentUser_Role_Personalized.tb_UIMenuPersonalizations.FirstOrDefault(c => c.MenuID == CurMenuInfo.MenuID);
            //    if (menuSetting != null)
            //    {
            //       // QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(T), true, kryptonPanel条件生成容器, QueryConditionFilter, menuSetting);
            //    }
            //    else
            //    {
            //      //  QueryDtoProxy = UIGenerateHelper.CreateQueryUI(typeof(T), true, kryptonPanel条件生成容器, QueryConditionFilter, QueryConditionShowColQty);
            //    }
            //}

            kryptonPanel条件生成容器.ResumeLayout();
            kryptonPanel条件生成容器.Visible = true;

            return QueryDtoProxy;

        }

        private void kryptonHeaderGroupTop_CollapsedChanged(object sender, EventArgs e)
        {

        }



    }
}

