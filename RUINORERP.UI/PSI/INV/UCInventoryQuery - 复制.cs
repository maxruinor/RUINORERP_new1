using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Global;
using RUINORERP.Business;
using RUINORERP.AutoMapper;
using AutoMapper;
using RUINORERP.Common.CollectionExtension;
using SqlSugar;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;

namespace RUINORERP.UI.PSI.INV
{
    [MenuAttrAssemblyInfo("库存查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.库存管理, BizType.库存查询)]
    public partial class UCInventoryQuery : BaseMasterQueryWithCondition<View_Inventory>
    {
        public UCInventoryQuery()
        {
            InitializeComponent();
            //生成查询条件的相关实体
            ReladtedEntityType = typeof(tb_Prod);
            this.Load += UCInventoryQuery_Load;
        }

        private void UCInventoryQuery_Load(object sender, EventArgs e)
        {
            //表格显示时DataGridView1_CellFormatting 取外键类型
            base._UCBillMasterQuery.ColDisplayTypes.Add(typeof(tb_Prod));
        }


        /*
        protected async override void Query()
        {
            tb_StocktakeController<View_ProdDetail> ctr = Startup.GetFromFac<tb_StocktakeController<View_ProdDetail>>();
            //var list = await ctr.QueryAsync();
            //bindingSourceList.DataSource = list;
            //base.newSumDataGridView1.DataSource = bindingSourceList.DataSource;
            newDto = new tb_Inventory();
            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();

            //既然前台指定的查询哪些字段，到时可以配置。这里应该是 除软件删除外的。其他字段不需要
            List<string> listquery = RUINORERP.Common.Helper.ExpressionHelper.ExpressionListToStringList(QueryConditions);
            List<View_ProdDetail> list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, listquery,null, newDto) as List<View_ProdDetail>;
            bindingSourceListMain.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            newSumDataGridView1.DataSource = bindingSourceListMain;

        }*/


        public override void BuildColNameDataDictionary()
        {
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            //System.Linq.Expressions.Expression<Func<, int?>> exprApprovalStatus;
            //exprApprovalStatus = (p) => p.ApprovalStatus;
            //base.MasterColNameDataDictionary.TryAdd(exprApprovalStatus.GetMemberInfo().Name, GetKeyValuePairs(typeof(ApprovalStatus)));


            //System.Linq.Expressions.Expression<Func<tb_SaleOrder, int?>> exprPayStatus;
            //exprPayStatus = (p) => p.PayStatus;
            //base.MasterColNameDataDictionary.TryAdd(exprPayStatus.GetMemberInfo().Name, GetKeyValuePairs(typeof(PayStatus)));

            //List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            //kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            //kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            //System.Linq.Expressions.Expression<Func<tb_SaleOrderDetail, bool?>> expr2;
            //expr2 = (p) => p.Gift;// == name;
            //base.ChildColNameDataDictionary.TryAdd(expr2.GetMemberInfo().Name, kvlist1);

            /*
            //View_ProdDetailController<View_ProdDetail> dc = Startup.GetFromFac<View_ProdDetailController<View_ProdDetail>>();
            List<View_ProdDetail> list = new List<View_ProdDetail>();
            list = MainForm.Instance.AppContext.Db.Queryable<View_ProdDetail>().ToList();
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            foreach (var item in list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            System.Linq.Expressions.Expression<Func<tb_SaleOrderDetail, long>> expProdDetailID;
            expProdDetailID = (p) => p.ProdDetailID;// == name;
            base.ChildColNameDataDictionary.TryAdd(expProdDetailID.GetMemberInfo().Name, proDetailList);

            */
        }


        public override void BuildLimitQueryConditions()
        {
            //创建表达式
            var lambda = Expressionable.Create<View_Inventory>()
                            //.AndIF(CurMenuInfo.CaptionCN.Contains("客户"), t => t.IsCustomer == true)
                            // .AndIF(CurMenuInfo.CaptionCN.Contains("供应商"), t => t.IsVendor == true)
                            //.And(t => t.isdeleted == false)

                            //.And(t => t.Is_enabled == true)
                            
                            .ToExpression();//注意 这一句 不能少
            base.LimitQueryConditions = lambda;
        }

        /// <summary>
        /// 如果需要查询条件查询，就要在子类中重写这个方法
        /// </summary>
        public override void BuildQueryCondition()
        {
            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(View_Inventory).Name + "Processor");
            QueryConditionFilter = baseProcessor.GetQueryFilter();
        }

        public override void BuildSummaryCols()
        {
            base.MasterSummaryCols.Add(c => c.Quantity);

        }

        public override void BuildInvisibleCols()
        {
            base.MasterInvisibleCols.Add(c => c.Inv_Cost);
        }

        /// <summary>
        /// 初始化列表数据
        /// </summary>
        //internal void InitListData()
        //{
        //    base.newSumDataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //    base.newSumDataGridView1.XmlFileName = this.Name + nameof(tb_Inventory) + "View_ProdDetail";
        //    base.newSumDataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(View_ProdDetail));
        //    bindingSourceListMain.DataSource = new List<View_ProdDetail>();
        //    base.newSumDataGridView1.DataSource = null;
        //    绑定导航
        //    base.newSumDataGridView1.DataSource = bindingSourceListMain.DataSource;
        //}


        /*
        /// <summary>
        /// 查的是视图。特殊处理了一下。传入对应的表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //如果列是隐藏的是不是可以不需要控制显示了呢? 后面看是否是导出这块需要不需要 不然可以隐藏的直接跳过
            if (!newSumDataGridView1.Columns[e.ColumnIndex].Visible)
            {
                return;
            }
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = newSumDataGridView1.Columns[e.ColumnIndex].Name;
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                        return;
                    }

                }
            }



            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<tb_Prod>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

            //图片特殊处理
            if (newSumDataGridView1.Columns[e.ColumnIndex].Name == "Image")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                }
            }

            //处理创建人 修改人，因为这两个字段没有做外键。固定的所以可以统一处理



        }

        */

    }
}
