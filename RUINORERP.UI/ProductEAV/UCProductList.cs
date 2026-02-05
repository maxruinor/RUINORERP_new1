using AutoMapper;
using RUINOR.Core;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.CommService;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business.Processor;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace RUINORERP.UI.ProductEAV
{

    [MenuAttrAssemblyInfo("产品管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCProductList : BaseForm.BaseListGeneric<tb_Prod>
    {

        private List<tb_ProdCategories> _categorylist = new List<tb_ProdCategories>();

        public UCProductList()
        {
            InitializeComponent();
            bool isDesignMode = this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime;

            if (!isDesignMode)
            {
                base.EditForm = typeof(frmProductEdit);
                //显示时目前只缓存了基础数据。单据也可以考虑id显示编号。后面来实现。如果缓存优化好了
                DisplayTextResolver.Initialize(dataGridView1);
                #region 准备枚举值在列表中显示
                System.Linq.Expressions.Expression<Func<tb_Prod, int?>> expr;
                expr = (p) => p.SourceType;
                base.ColNameDataDictionary.TryAdd(expr.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(GoodsSource)));
                DisplayTextResolver.AddFixedDictionaryMappingByEnum<tb_Prod>(t => t.SourceType, typeof(GoodsSource));
                #endregion

                #region 准备枚举值在列表中显示
                System.Linq.Expressions.Expression<Func<tb_Prod, int?>> exprP;
                exprP = (p) => p.PropertyType;
                base.ColNameDataDictionary.TryAdd(exprP.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(ProductAttributeType)));

                DisplayTextResolver.AddFixedDictionaryMappingByEnum<tb_Prod>(t => t.PropertyType, typeof(ProductAttributeType));
                #endregion

                // 配置图片列显示（显示缩略图，双击可查看原图）
                DisplayTextResolver.AddColumnDisplayType<tb_Prod>(p => p.Images, ColumnDisplayTypeEnum.Image);

                dataGridView1.CustomRowNo = true;
                dataGridView1.CellPainting += dataGridView1_CellPainting;

            }



        }


        // 忽略属性配置
        // 重写忽略属性配置
        protected override IgnorePropertyConfiguration ConfigureIgnoreProperties()
        {
            return base.ConfigureIgnoreProperties()
                // 主表忽略的属性
                .Ignore<tb_Prod>(
                    e => e.ProdBaseID,
                    e => e.ProductNo,
                    e => e.ShortCode,
                    e => e.DataStatus,
                    e => e.PrimaryKeyID,
                    e => e.tb_ProdDetails,
                    e => e.StateManager)
                // 明细表忽略的属性
                .Ignore<tb_ProdDetail>(
                    e => e.ProdDetailID,
                    e => e.ProdBaseID,
                    e => e.PrimaryKeyID,
                    e => e.SKU,
                    e => e.BarCode
                    );
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // 检查是否是行头
            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {

                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                // 检查是否需要标记的行
                DataGridViewRow dr = dataGridView1.Rows[e.RowIndex];
                tb_Packing tb_Packing = null;
                BoxRuleBasis basis = GDIHelper.Instance.CheckForBoxSpecBasis(dr, out tb_Packing);
                // 绘制图案
                switch (basis)
                {
                    case BoxRuleBasis.Product:
                        GDIHelper.Instance.DrawPattern(e, Color.DarkGreen);
                        break;
                    case BoxRuleBasis.Attributes:
                        //DrawPattern(e, Color.DarkMagenta);
                        GDIHelper.Instance.DrawPattern(e);
                        break;
                    case BoxRuleBasis.Product | BoxRuleBasis.Attributes:
                        GDIHelper.Instance.DrawPattern(e, Color.OrangeRed);
                        break;
                    default:
                        break;
                }
                e.Handled = true;
            }
        }


        /// <summary>
        /// 扩展带条件查询
        /// 因为产品相关性多，重写这个方法用高级导航查询
        /// </summary>     
        protected async override void ExtendedQuery(bool UseAutoNavQuery = false)
        {
            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;
            if (QueryDtoProxy == null)
            {
                return;
            }
            dataGridView1.ReadOnly = true;

            //既然前台指定的查询哪些字段，到时可以配置。这里应该是 除软件删除外的。其他字段不需要

            int pageNum = 1;
            int pageSize = int.Parse(txtMaxRows.Text);

            List<tb_Prod> list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDtoProxy, pageNum, pageSize) as List<tb_Prod>;

            List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(SummaryCols);
            if (masterlist.Count > 0)
            {
                dataGridView1.IsShowSumRow = true;
                dataGridView1.SumColumns = masterlist.ToArray();
            }

            list.ForEach(
                c =>
                {
                    c.tb_ProdDetails.ForEach(d => d.BeginOperation());
                    c.BeginOperation();
                }
            );

            ListDataSoure.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = ListDataSoure;

            ToolBarEnabledControl(MenuItemEnums.查询);
        }


        /// <summary>
        /// 产品编辑特别，修改要保存后进行。不可以新建后就修改
        /// </summary>
        protected override async Task<object> Add()
        {
            base.toolStripButtonModify.Enabled = false;
            object obj = await base.Add();
            return obj;
        }


        IList<tb_Prod> oldList;
        tb_ProdController<tb_Prod> pctr = Startup.GetFromFac<tb_ProdController<tb_Prod>>();
        tb_ProdCategoriesController<tb_ProdCategories> mca = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();


        protected async override Task<bool> Delete()
        {
            bool rs = false;
            if (MessageBox.Show("系统不建议删除基本资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                tb_Prod loc = (tb_Prod)this.bindingSourceList.Current;
                if (loc.DataStatus == (int)Global.DataStatus.确认)
                {
                    MessageBox.Show("确认后的数据不能删除。");
                }
                else
                {
                    this.bindingSourceList.Remove(loc);
                    rs = await pctr.DeleteByNavAsync(loc);
                    if (rs)
                    {
                        //缓存只是显示用，所以删除后，并不影响。等待服务器的更新机制更新即可。
                    }
                }

            }
            return rs;
        }

        /// <summary>
        /// 产品比较特殊 
        /// </summary>
        public async override Task<List<tb_Prod>> Save()
        {
            List<tb_Prod> list = new List<tb_Prod>();
            tb_ProdController<tb_Prod> pctr = Startup.GetFromFac<tb_ProdController<tb_Prod>>();
            //这里是否要用保存列表来处理
            foreach (var item in bindingSourceList.List)
            {
                var entity = item as tb_Prod;

                switch (entity.ActionStatus)
                {
                    case ActionStatus.无操作:
                        break;
                    case ActionStatus.新增:
                    case ActionStatus.修改:
                        base.toolStripButtonSave.Enabled = false;
                        ReturnResults<tb_Prod> rr = new ReturnResults<tb_Prod>();
                        rr = await pctr.SaveOrUpdateAsync(entity);
                        //rr = await base.ctr.BaseSaveOrUpdateWithChildtb_Prod(entity as tb_Prod);
                        // await ctr.SaveOrUpdate(entity as tb_Unit);
                        if (rr.Succeeded)
                        {
                            base.toolStripButtonSave.Enabled = true;
                            ToolBarEnabledControl(MenuItemEnums.保存);
                            list.Add(rr.ReturnObject);
                            MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_Prod>("产品保存", rr.ReturnObject);
                            //保存箱规
                            //if (entity.tb_BoxRuleses.Count > 0 && entity.tb_BoxRuleses[0] != null && entity.tb_BoxRuleses[0].HasChanged)
                            //{
                            //    //直接保存到DB
                            //    BaseController<tb_BoxRules> ctr = Startup.GetFromFacByName<BaseController<tb_BoxRules>>(typeof(tb_BoxRules).Name + "Controller");
                            //    ReturnResults<tb_BoxRules> rrboxrule = new ReturnResults<tb_BoxRules>();
                            //    rrboxrule = await ctr.BaseSaveOrUpdate(entity.tb_BoxRuleses[0]);
                            //    if (rr.Succeeded)
                            //    {
                            //        entity.tb_BoxRuleses[0].HasChanged = false;
                            //    }
                            //}
                            //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();

                            base._eventDrivenCacheManager.UpdateEntity<tb_Prod>(rr.ReturnObject);

                        }
                        else
                        {
                            base.toolStripButtonSave.Enabled = false;
                            MainForm.Instance.uclog.AddLog(rr.ErrorMsg, Global.UILogType.错误);
                        }
                        //tb_Unit Entity = await ctr.AddReEntityAsync(entity);
                        //如果新增 保存后。还是新增加状态，因为增加另一条。所以保存不为灰色。所以会重复增加
                        break;
                    case ActionStatus.删除:
                        break;
                    default:
                        break;
                }
                entity.HasChanged = false;
            }

            base.toolStripButtonModify.Enabled = true;
            return list;
        }

        private void UCProductList_Load(object sender, EventArgs e)
        {

        }
    }
}
