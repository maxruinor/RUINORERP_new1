using RUINORERP.Business;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.CollectionExtension;
using SqlSugar;
using RUINORERP.UI.UControls;
using RUINORERP.Business.Processor;
//using System.Windows.Forms.DataVisualization.Charting;
using Krypton.Navigator;
using System.Collections;
using System.Linq.Expressions;
using AutoMapper.Internal;
using RUINORERP.Common.Extensions;


namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("库存跟踪", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.供应链管理.库存管理, BizType.库存跟踪)]
    public partial class UCInventoryTracking : BaseNavigatorGeneric<View_Inventory, View_Inventory>
    {
        public UCInventoryTracking()
        {
            InitializeComponent();

            //生成查询条件的相关实体
            ReladtedEntityType = typeof(View_Inventory);

            #region 准备枚举值在列表中显示


            System.Linq.Expressions.Expression<Func<tb_Prod, int?>> expr;
            expr = (p) => p.SourceType;
            base.MasterColNameDataDictionary.TryAdd(expr.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(GoodsSource)));

            #endregion




            txtMaxRow.Text = "100";
            //base.RelatedBillEditCol = (c => c.PurEntryNo);
            //base.ChildRelatedEntityType = typeof(tb_PurOrderDetail);
            //base.OnQueryRelatedChild += UCPurEntryQuery_OnQueryRelatedChild;
        }


        public override List<NavParts[]> AddNavParts()
        {
            List<NavParts[]> strings = new List<NavParts[]>();
            //strings.Add(new NavParts[] { NavParts.查询条件 });
            strings.Add(new NavParts[] { NavParts.查询结果, NavParts.分组显示, NavParts.结果分析1 });
            return strings;
        }


        public override void BuildQueryCondition()
        {
            //var lambdaOrder = Expressionable.Create<View_Inventory>()
            // .And(t => t.DataStatus == (int)DataStatus.确认)
            //  .And(t => t.isdeleted == false)
            // .ToExpression();//注意 这一句 不能少
            ////如果有限制则设置一下 但是注意 不应该在这设置，灵活的应该是在调用层设置
            //Filter.SetFieldLimitCondition(lambdaOrder);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.SKU);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.CNName);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.ProductNo);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Location_ID);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Type_ID);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Model);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Specifications);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.prop);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.DepartmentID);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Rack_ID);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.BOM_ID);
            base.QueryFilter.SetQueryField<View_Inventory>(c => c.Unit_ID);
        }


        private void UCInventoryTracking_Load(object sender, EventArgs e)
        {
            ReladtedEntityType = typeof(View_Inventory);

            base._UCMasterQuery.ColDisplayTypes = new List<Type>();
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_Prod));
            //是否能通过一两个主表，通过 外键去找多级关联的表？
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(View_Inventory));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(View_ProdDetail));
            base._UCMasterQuery.ColDisplayTypes.Add(typeof(tb_ProductType));
            base._UCOutlookGridGroupAnalysis.ColDisplayTypes = base._UCMasterQuery.ColDisplayTypes;

            //base._UCMasterQuery.newSumDataGridViewMaster.Use是否使用内置右键功能 = false;
            base._UCMasterQuery.newSumDataGridViewMaster.ContextMenuStrip = contextMenuStrip1;

            KryptonPage page1 = kryptonWorkspace1.AllPages().FirstOrDefault(c => c.UniqueName == NavParts.结果分析1.ToString());
            page1.Text = "纵向库存跟踪";

        }


        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.Quantity);
        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.Inv_Cost);
        }

        private void 纵向库存跟踪ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_UCMasterQuery.bindingSourceMaster.Current != null)
            {
                if (_UCMasterQuery.bindingSourceMaster.Current is View_Inventory view_Inventory)
                {
                    //带有output的存储过程 
                    var Location_ID = new SugarParameter("@Location_ID", view_Inventory.Location_ID);
                    var ProdDetailID = new SugarParameter("@ProdDetailID", view_Inventory.ProdDetailID);
                    // var ageP = new SugarParameter("@age", null, true);//设置为output
                    //var list = db.Ado.UseStoredProcedure().SqlQuery<Class1>("sp_school", nameP, ageP);//返回List
                    var list = MainForm.Instance.AppContext.Db.Ado.UseStoredProcedure().SqlQuery<Proc_InventoryTracking>("Proc_InventoryTracking", Location_ID, ProdDetailID);//返回List

                    _UCOutlookGridAnalysis1.FieldNameList = UIHelper.GetFieldNameColList(typeof(Proc_InventoryTracking));
                    List<string> SummaryCols = new List<string>();
                    SummaryCols.Add("数量");//这里要优化，按理可以是引用类型来处理
                    _UCOutlookGridAnalysis1.kryptonOutlookGrid1.SubtotalColumns = SummaryCols;
                    _UCOutlookGridAnalysis1.ColDisplayTypes = new List<Type>();
                    //这个视图是用SQL语句生成的,用生成器。
                    _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(Proc_InventoryTracking));
                    _UCOutlookGridAnalysis1.ColDisplayTypes.Add(typeof(tb_Location));


                    // _UCOutlookGridAnalysis2.GridRelated.SetRelatedInfo<View_Inventory, tb_BOM_S>(c => c.BOM_ID, r => r.BOM_ID);

                    _UCOutlookGridAnalysis1.LoadDataToGrid<Proc_InventoryTracking>(list);
                    kryptonWorkspace1.ActivePage = kryptonWorkspace1.AllPages().FirstOrDefault(c => c.UniqueName == NavParts.结果分析1.ToString());

                }
            }

        }



        private void 库存异常检测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //进出加起来不等于期末的
            int ErrorCounter = 0;
            //暂时的思路 是用 纵向库存跟踪的存储过程，查出来后再将进出明细(排除期初数据)和最后结余分别总计对比。不同的就有问题。
            foreach (DataGridViewRow dr in base._UCMasterQuery.newSumDataGridViewMaster.SelectedRows)
            {
                if (dr.DataBoundItem is View_Inventory view_Inventory)
                {
                    //带有output的存储过程 
                    var Location_ID = new SugarParameter("@Location_ID", view_Inventory.Location_ID);
                    var ProdDetailID = new SugarParameter("@ProdDetailID", view_Inventory.ProdDetailID);
                    // var ageP = new SugarParameter("@age", null, true);//设置为output
                    //var list = db.Ado.UseStoredProcedure().SqlQuery<Class1>("sp_school", nameP, ageP);//返回List
                    var list = MainForm.Instance.AppContext.Db.Ado.UseStoredProcedure().SqlQuery<Proc_InventoryTracking>("Proc_InventoryTracking", Location_ID, ProdDetailID);//返回List
                    if (list.Where(c => c.经营历程 == "进出明细" ).Sum(c => c.数量) != list.Where(c => c.经营历程 == "最后结余").Sum(c => c.数量))
                    {
                        //异常的行，背景色标识为红黄色。
                        // 设置指定行（这里假设设置第一行）的背景颜色为红黄色
                        dr.DefaultCellStyle.BackColor = Color.FromArgb(255, 64, 0);
                        dr.Cells[2].Style.ForeColor = Color.WhiteSmoke;
                        ErrorCounter++;
                    }
                }
            }
            if (ErrorCounter > 0)
            {
                MessageBox.Show("库存异常的数据行数为：" + ErrorCounter);
            }
            else
            {
                MainForm.Instance.uclog.AddLog("功能", "库存异常的数据行数为：" + ErrorCounter);
            }
        }
    }
}

