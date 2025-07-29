using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using Pipelines.Sockets.Unofficial.Arenas;
using RUINORERP.Business;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.Processor;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.Model;
using RUINORERP.Model;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.FM;
using RUINORERP.UI.PSI.INV;
using SourceGrid2.Win32;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{

    /// <summary>
    /// 表格显示关联单据等
    /// 有两种方式，一种是通过单号关联，一种是直接关联，主子表形式等，即：按列关联
    /// 另一种方式是库存跟踪时，按某列每行中的固定值关联
    /// </summary>
    public class GridViewRelated
    {

        private readonly EntityLoader _loader;
        private readonly EnhancedBizTypeMapper _mapper;
        private readonly EntityBizMappingService _mappingService;
        public GridViewRelated()
        {

            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            // 通过依赖注入获取服务实例
            _loader = Startup.GetFromFac<EntityLoader>();
            _mapper = Startup.GetFromFac<EnhancedBizTypeMapper>();
            _mappingService = Startup.GetFromFac<EntityBizMappingService>();
        }


        /// <summary>
        /// 双击时当前窗体的菜单信息
        /// 相当于是要打开目标的上级菜单信息
        /// </summary>
        public tb_MenuInfo FromMenuInfo { get; set; } = new tb_MenuInfo();

        MenuPowerHelper menuPowerHelper;


        private bool complexType = false;

        /// <summary>
        /// 用一个属性来标识是不是复杂的类型。默认不是。
        /// 复杂是像库存跟踪一样。目标是由另一列中的具体的值来决定哪一个表
        /// </summary>
        public bool ComplexType { get => complexType; set => complexType = value; }



        private List<KeyValuePair<string, string>> complexTargtetField = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// 复杂类型时，目标字段
        /// </summary>
        public List<KeyValuePair<string, string>> ComplexTargtetField { get => complexTargtetField; set => complexTargtetField = value; }


        /// <summary>
        /// 设置复杂关联字段，由类型决定编号是哪种业务类型的窗体。
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="_ExpBizType"></param>
        /// <param name="_ExpBillNo"></param>
        public void SetComplexTargetField<T1>(Expression<Func<T1, object>> _ExpBizType, Expression<Func<T1, object>> _ExpBillNo)
        {
            ComplexTargtetField.Add(new KeyValuePair<string, string>(_ExpBizType.GetMemberInfo().Name, _ExpBillNo.GetMemberInfo().Name));
        }

        //public void GuideToForm<T>(tb_MenuInfo RelatedMenuInfo, string GridViewColumnFieldName, string RelatedTargetColName, object RelatedTargetEntity)
        //{
        //    // 应该是只是双击单号才生效
        //    if (GridViewColumnFieldName == RelatedTargetColName)
        //    {
        //        //要把单据信息显示的菜单传过去
        //        if (RelatedMenuInfo != null)
        //        {


        //            menuPowerHelper.ExecuteEvents(RelatedMenuInfo, RelatedTargetEntity);
        //        }
        //        else
        //        {
        //            MessageBox.Show("请确认你有足够权限查询对应单据，或请联系管理员。");
        //        }
        //    }
        //}



        //public void GuideToForm(tb_MenuInfo RelatedMenuInfo, string GridViewColumnFieldName,  object RelatedTargetEntity)
        //{
        //    // 应该是只是双击单号才生效
        //    if (GridViewColumnFieldName == RelatedTargetColName)
        //    {
        //        //要把单据信息显示的菜单传过去
        //        if (RelatedMenuInfo != null)
        //        {
        //            menuPowerHelper.ExecuteEvents(RelatedMenuInfo, RelatedTargetEntity);
        //        }
        //        else
        //        {
        //            MessageBox.Show("请确认你有足够权限查询对应单据，或请联系管理员。");
        //        }
        //    }
        //}


        ///// <summary>
        ///// 设置关联单据的列
        ///// 关联单据的列，key:引用单号列名  value:前面是表名+|+原始单号列名
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="M"></typeparam>
        ///// <param name="table"></param>
        ///// <param name="expBillNoColName"></param>
        ///// <returns></returns>
        //public void SetRelatedBillCols<T>(Expression<Func<T, string>> expSourceBillNoColName, Expression<Func<M, string>> expRefBillNoColName)
        //{
        //    RelatedBillCols.TryAdd(expRefBillNoColName.GetMemberInfo().Name, typeof(T).Name + "|" + expSourceBillNoColName.GetMemberInfo().Name);
        //}

        /// <summary>
        ///  设置关联单据的列,T1:来源表，显示的实体，目标是指向，要打开的窗体用的实体，以及关联的列的字段名，可能是ID，也可能是单号
        /// 
        /// </summary>
        /// <typeparam name="T1">来源 表格目前显示的实体</typeparam>
        /// <param name="_ExpSourceUniqueField">要打开的窗体用的实体名</param>
        /// <param name="_ExpTargetDisplayField">以及关联的列的字段名，可能是ID，也可能是单号</param>
        public void SetRelatedInfo<T1>(Expression<Func<T1, object>> _ExpSourceUniqueField, string TargetTableName, string TargetDisplayField)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = typeof(T1).Name;
            relatedInfo.TargetTableName = new KeyNamePair(TargetTableName, string.Empty);

            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            relatedInfo.TargetDisplayField = TargetDisplayField;
            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == TargetTableName))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }


        ///// <summary>
        /////  设置关联单据的列,T1:来源表，显示的实体，目标是指向，要打开的窗体用的实体，
        /////  分析out表格中 实际只要知道目标表名即可，和 来源实体中的对应列名，因为打开 窗体时判断了。写死的,后面优化吧TODO:
        ///// 
        ///// </summary>
        ///// <typeparam name="T1">来源 表格目前显示的实体</typeparam>
        ///// <param name="_ExpSourceUniqueField">要打开的窗体用的实体名</param>
        //public void SetRelatedInfo<T1>(Expression<Func<T1, object>> _ExpSourceUniqueField, string TargetTableName)
        //{
        //    RelatedInfo relatedInfo = new RelatedInfo();
        //    relatedInfo.SourceTableName = typeof(T1).Name;
        //    relatedInfo.TargetTableName = new KeyNamePair(TargetTableName, string.Empty);

        //    relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;

        //    if (!RelatedInfoList.Any(c => c.TargetTableName.Key == TargetTableName))
        //    {
        //        RelatedInfoList.Add(relatedInfo);
        //    }
        //}

        /// <summary>
        ///  设置关联单据的列,T1:来源表，显示的实体，目标是指向，要打开的窗体用的实体，
        ///  分析out表格中 实际只要知道目标表名即可，和 来源实体中的对应列名，因为打开 窗体时判断了。写死的,后面优化吧TODO:
        /// 
        /// </summary>
        /// <typeparam name="T1">来源 表格目前显示的实体</typeparam>
        /// <param name="_ExpSourceUniqueField">双击的列名</param>
        /// <param name="TargetTableNameFromField">这个参数能指定一个单据编号 是来自不同的 实体，类似由这个表其它列来指定 如biztype</param>
        public void SetRelatedInfo<T1>(Expression<Func<T1, object>> _ExpSourceUniqueField, KeyNamePair TargetTableNameFromField)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = typeof(T1).Name;
            relatedInfo.TargetTableName = TargetTableNameFromField;
            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == TargetTableNameFromField.Key && c.SourceUniqueField == relatedInfo.SourceUniqueField))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }




        /// <summary>
        /// 打开自己本身的窗体（双击哪一列会跳到单据编辑菜单）
        /// 
        /// </summary>
        /// <typeparam name="T1">来源 表格目前显示的实体</typeparam>
        /// <typeparam name="T2">目标 要打开的窗体用的实体</typeparam>
        /// <param name="_ExpSourceUniqueField">来源的唯一字段</param>
        /// <param name="_ExpTargetDisplayField">目标的显示字段</param>
        public void SetRelatedInfo<T1>(Expression<Func<T1, object>> _ExpSourceUniqueField)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = typeof(T1).Name;
            relatedInfo.TargetTableName = new KeyNamePair(typeof(T1).Name, string.Empty);

            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            relatedInfo.TargetDisplayField = _ExpSourceUniqueField.GetMemberInfo().Name;
            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == typeof(T1).Name))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }


        public void SetRelatedInfo(string TableName, string FieldName)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = TableName;
            relatedInfo.TargetTableName = new KeyNamePair(TableName, string.Empty);
            relatedInfo.SourceUniqueField = FieldName;
            relatedInfo.TargetDisplayField = FieldName;

            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == TableName && c.SourceTableName == TableName && c.SourceUniqueField == FieldName))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }

        /// <summary>
        /// 关联单据
        /// </summary>
        /// <typeparam name="T1">来源 表格目前显示的实体</typeparam>
        /// <typeparam name="T2">目标 要打开的窗体用的实体</typeparam>
        /// <param name="_ExpSourceUniqueField">来源的唯一字段</param>
        /// <param name="_ExpTargetDisplayField">目标的显示字段</param>
        public void SetRelatedInfo<T1, T2>(Expression<Func<T1, object>> _ExpSourceUniqueField, Expression<Func<T2, object>> _ExpTargetDisplayField)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = typeof(T1).Name;
            relatedInfo.TargetTableName = new KeyNamePair(typeof(T2).Name, string.Empty);

            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            relatedInfo.TargetDisplayField = _ExpTargetDisplayField.GetMemberInfo().Name;
            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == typeof(T2).Name))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }
        /// <summary>
        /// 关联单据
        /// </summary>
        /// <typeparam name="T1">来源 表格目前显示的实体</typeparam>
        /// <typeparam name="T2">目标 要打开的窗体用的实体</typeparam>
        /// <param name="_ExpSourceUniqueField">来源的唯一字段</param>
        /// <param name="_ExpTargetDisplayField">目标的显示字段</param>
        public void SetRelatedInfo<T1>(Expression<Func<T1, object>> _ExpSourceUniqueField, Expression<Func<T1, object>> _ExpTargetDisplayField)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = typeof(T1).Name;
            relatedInfo.TargetTableName = new KeyNamePair(typeof(T1).Name, string.Empty);

            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            relatedInfo.TargetDisplayField = _ExpTargetDisplayField.GetMemberInfo().Name;
            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == typeof(T1).Name))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }

        /// <summary>
        /// 关联单据时  业务和数据表不一致时，需要指定业务表名
        /// </summary>
        public List<RelatedInfo> RelatedInfoList { get; set; } = new List<RelatedInfo>();




        /// <summary>
        /// 用于要打开的窗体是由源来中的一个列名决定ID或编号，目标表名是由源表格中某一列的值来决定
        /// </summary>
        /// <param name="GridViewColumnFieldName">SourceBillNo</param>
        /// <param name="CurrentRowEntity"></param>
        /// <param name="IsFromGridValue">是否从Grid中取值,只是用这个参数来区别一下没有实际作用,后面优化吧</param
        public object GuideToForm(string GridViewColumnFieldName, object CurrentRowEntity)
        {
            object bizKey = null;
            tb_MenuInfo RelatedMenuInfo = null;

            if (ComplexType)
            {
                string TargetTableKey = string.Empty;
                #region 复杂类型
                var result = ComplexTargtetField.FirstOrDefault(pair => pair.Value == GridViewColumnFieldName);

                if (result.Key != null)
                {
                    //通过业务类型找
                    TargetTableKey = CurrentRowEntity.GetPropertyValue(result.Key).ToString();
                    Console.WriteLine($"找到的键值对: Key = {result.Key}, Value = {result.Value}");
                }
                else
                {
                    Console.WriteLine($"未找到值为 {GridViewColumnFieldName} 的键值对。");
                }


                //通过业务类型找到目标表名对应的菜单   
                RelatedInfo relatedRelationship = RelatedInfoList.FirstOrDefault(c => c.SourceUniqueField == GridViewColumnFieldName && c.TargetTableName.Key == TargetTableKey);
                if (relatedRelationship != null)
                {
                    string tableName = relatedRelationship.TargetTableName.Name;
                    //这里是显示明细
                    //要把单据信息传过去
                    //                RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == tableName && m.ClassPath.Contains(tableName.Replace("tb_", "UC").ToString().Replace("Query", ""))).FirstOrDefault();
                    RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == relatedRelationship.TargetTableName.Name && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo == null)
                    {
                        //特殊情况：没有关联的单据 uc控件窗体名称和实体名称不一致时
                        if (tableName == typeof(tb_ProductionDemand).Name)
                        {
                            RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == tableName && m.ClassPath.Contains("UCProduceRequirement")).FirstOrDefault();
                        }
                        //特殊情况：没有关联的单据
                        if (tableName == typeof(tb_BOM_S).Name)
                        {
                            RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == tableName && m.ClassPath.Contains("UCBillOfMaterials")).FirstOrDefault();
                        }
                    }

                    if (RelatedMenuInfo != null)
                    {
                        //一般是主键和编号来关联，通过数据类型来区别

                        var billno = CurrentRowEntity.GetPropertyValue(relatedRelationship.SourceUniqueField);
                        if (billno == null)
                        {
                            return null;
                        }
                        else
                        {
                            bizKey = billno;
                        }
                        OpenTargetEntity(RelatedMenuInfo, tableName, billno);
                    }

                }
                #endregion
            }
            else
            {
                #region 普通类型
                RelatedInfo relatedRelationship = RelatedInfoList.FirstOrDefault(c => c.SourceUniqueField == GridViewColumnFieldName);
                if (relatedRelationship != null)
                {
                    string tableName = relatedRelationship.TargetTableName.Key;
                    //这里是显示明细
                    //要把单据信息传过去
                    //                RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == tableName && m.ClassPath.Contains(tableName.Replace("tb_", "UC").ToString().Replace("Query", ""))).FirstOrDefault();
                    RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == relatedRelationship.TargetTableName.Key && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                    if (RelatedMenuInfo == null)
                    {
                        //特殊情况：没有关联的单据 uc控件窗体名称和实体名称不一致时
                        if (tableName == typeof(tb_ProductionDemand).Name)
                        {
                            RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == tableName && m.ClassPath.Contains("UCProduceRequirement")).FirstOrDefault();
                        }
                        //特殊情况：没有关联的单据
                        if (tableName == typeof(tb_BOM_S).Name)
                        {
                            RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == tableName && m.ClassPath.Contains("UCBillOfMaterials")).FirstOrDefault();
                        }
                    }

                    if (RelatedMenuInfo != null)
                    {
                        //一般是主键和编号来关联，通过数据类型来区别

                        var billno = CurrentRowEntity.GetPropertyValue(relatedRelationship.SourceUniqueField);
                        if (billno == null)
                        {
                            return null;
                        }
                        else
                        {
                            bizKey = billno;
                        }
                        OpenTargetEntity(RelatedMenuInfo, tableName, billno);
                    }

                }
                #endregion

            }
            return bizKey;
        }


        /// <summary>
        /// 顺便传回业务主键 可能是ID，可能是编号
        /// </summary>
        /// <param name="GridViewColumnFieldName"></param>
        /// <param name="CurrentRow"></param>
        /// <returns></returns>
        public object GuideToForm(string GridViewColumnFieldName, DataGridViewRow CurrentRow)
        {
            object bizKey = null;
            tb_MenuInfo RelatedMenuInfo = null;
            RelatedInfo relatedRelationship = RelatedInfoList.FirstOrDefault(c => c.SourceUniqueField == GridViewColumnFieldName);
            if (relatedRelationship != null)
            {

                //先判断是否多个，合并时会有两个再来根据属性标识去找正确的
                List<tb_MenuInfo> TargetMenus = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == relatedRelationship.TargetTableName.Key
                && m.BIBaseForm == "BaseBillEditGeneric`2").ToList();
                if (TargetMenus.Count > 1)
                {
                    if (!string.IsNullOrEmpty(TargetMenus[0].BizInterface) && !string.IsNullOrEmpty(TargetMenus[1].BizInterface))
                    {
                        if (TargetMenus[0].BizInterface == TargetMenus[1].BizInterface
                            && TargetMenus[0].BizInterface == nameof(ISharedIdentification))
                        {

                            //数据库中保存的是枚举的名称 Flag1
                            //为了通用 共享一组单据的业务表。如 应收应付。  基类做好了 查询 和 单据编辑。分别对应有四个两组子类
                            //这时 标记要对应统一。如果应收单和应收查询  都是  Flag1.可以是Flag2 但是要一样
                            string Flag = string.Empty;
                            if (this.FromMenuInfo == null)
                            {
                                MessageBox.Show("请联系管理员，配置入口菜单");
                            }
                            else
                            {
                                Flag = this.FromMenuInfo.UIPropertyIdentifier;
                            }

                            var sss = CurrentRow.DataBoundItem.GetPropertyInfo(nameof(SharedFlag));
                            RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble
                             && m.EntityName == relatedRelationship.TargetTableName.Key
                             && m.BIBaseForm == "BaseBillEditGeneric`2"
                             && m.UIPropertyIdentifier == Flag
                       ).FirstOrDefault();
                        }

                    }

                }
                else
                {
                    RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == relatedRelationship.TargetTableName.Key
                     && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                }
                if (CurrentRow.DataBoundItem is RUINORERP.Model.BaseEntity entity
                    && relatedRelationship.TargetTableName.Key == relatedRelationship.SourceTableName
                    && !CurrentRow.DataBoundItem.GetType().Name.Contains("View_")//排除视图
                    )
                {
                    if (RelatedMenuInfo != null)
                    {
                        menuPowerHelper.ExecuteEvents(RelatedMenuInfo, entity);
                    }
                    else
                    {
                        MessageBox.Show("请确认你有足够权限查询对应单据，或请联系管理员。");
                    }
                }
                else
                {
                    //要查询取值,视图也适用于这里
                    bizKey = GuideToForm(GridViewColumnFieldName, CurrentRow.DataBoundItem);
                }
            }
            return bizKey;
        }

        public void OpenTargetEntity(tb_MenuInfo RelatedMenuInfo, string tableName, object billno)
        {

            // 1. 把表名变成实体类型
            var entityType = _mappingService.GetEntityTypeByTableName(tableName);

            var entity = _loader.LoadEntityInternal(entityType, billno);
            if (entity != null)
            {
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, entity);
                return;
            }
            // 加载实体数据
            //var order = _loader.LoadEntity(tableName, billno);
            //if (order != null)
            //{
            //    menuPowerHelper.ExecuteEvents(RelatedMenuInfo, order);
            //    return;
            //}


            if (tableName == typeof(tb_SaleOutRe).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOutRe>()
                    .Includes(c => c.tb_SaleOutReDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.SaleOutRe_ID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.ReturnNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_FM_ExpenseClaim).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_FM_ExpenseClaim>()
                    .Includes(c => c.tb_FM_ExpenseClaimDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.ClaimMainID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.ClaimNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }
            if (tableName == typeof(tb_FM_OtherExpense).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_FM_OtherExpense>()
                    .Includes(c => c.tb_FM_OtherExpenseDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.ExpenseMainID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.ExpenseNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_ProductionPlan).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>()
                    .Includes(c => c.tb_ProductionPlanDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.PPID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.PPNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_ProductionDemand).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ProductionDemand>()
                    .Includes(c => c.tb_ProductionDemandTargetDetails)
                    .Includes(c => c.tb_ProductionDemandDetails)
                    .Includes(c => c.tb_ProduceGoodsRecommendDetails)
                    .Includes(c => c.tb_ManufacturingOrders)
                    .Includes(c => c.tb_PurGoodsRecommendDetails)
                    .Includes(c => c.tb_productionplan)
                    .WhereIF(billno.GetType() == typeof(long), c => c.PDID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.PDNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_BOM_S).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                    .Includes(c => c.tb_BOM_SDetails, d => d.tb_BOM_SDetailSubstituteMaterials)
                    .Includes(c => c.view_ProdInfo)
                    .Includes(c => c.tb_BOM_SDetailSecondaries)
                    .WhereIF(billno.GetType() == typeof(long), c => c.BOM_ID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.BOM_No == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_MaterialRequisition).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_MaterialRequisition>()
                    .Includes(c => c.tb_MaterialRequisitionDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.MR_ID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.MaterialRequisitionNO == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }
            if (tableName == typeof(tb_MaterialReturn).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_MaterialReturn>()
                    .Includes(c => c.tb_MaterialReturnDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.MRE_ID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.BillNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_FinishedGoodsInv).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_FinishedGoodsInv>()
                    .Includes(c => c.tb_FinishedGoodsInvDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.FG_ID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.DeliveryBillNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }


            if (tableName == typeof(tb_ManufacturingOrder).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ManufacturingOrder>()
                    .Includes(c => c.tb_ManufacturingOrderDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.MOID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.MONO == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_SaleOrder).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOrder>()
                    .Includes(c => c.tb_SaleOrderDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.SOrder_ID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.SOrderNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_SaleOut).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_SaleOut>()
                    .Includes(c => c.tb_SaleOutDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.SaleOut_MainID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.SaleOutNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_PurOrder).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_PurOrder>()
                    .Includes(c => c.tb_PurOrderDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.PurOrder_ID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.PurOrderNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }
            if (tableName == typeof(tb_PurEntry).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntry>()
                    .Includes(c => c.tb_PurEntryDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.PurEntryID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.PurEntryNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }
            if (tableName == typeof(tb_Stocktake).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_Stocktake>()
                    .Includes(c => c.tb_StocktakeDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.MainID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.CheckNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_ProdBorrowing).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ProdBorrowing>()
                    .Includes(c => c.tb_ProdBorrowingDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.BorrowID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.BorrowNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }


            if (tableName == typeof(tb_ProdReturning).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ProdReturning>()
                    .Includes(c => c.tb_ProdReturningDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.ReturnID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.ReturnNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_ProdMerge).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ProdMerge>()
                    .Includes(c => c.tb_ProdMergeDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.MergeID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.MergeNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_ProdSplit).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ProdSplit>()
                    .Includes(c => c.tb_ProdSplitDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.SplitID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.SplitNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }
            if (tableName == typeof(tb_StockTransfer).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_StockTransfer>()
                    .Includes(c => c.tb_StockTransferDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.StockTransferID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.StockTransferNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_ProdConversion).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ProdConversion>()
                    .Includes(c => c.tb_ProdConversionDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.ConversionID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.ConversionNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_PurReturnEntry).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_PurReturnEntry>()
                    .Includes(c => c.tb_PurReturnEntryDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.PurReEntry_ID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.PurReEntryNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_PurEntryRe).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_PurEntryRe>()
                    .Includes(c => c.tb_PurEntryReDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.PurEntryRe_ID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.PurEntryReNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }
            if (tableName == typeof(tb_StockOut).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_StockOut>()
                    .Includes(c => c.tb_StockOutDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.MainID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.BillNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }
            if (tableName == typeof(tb_StockIn).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_StockIn>()
                    .Includes(c => c.tb_StockInDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.MainID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.BillNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_MRP_ReworkEntry).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_MRP_ReworkEntry>()
                    .Includes(c => c.tb_MRP_ReworkEntryDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.ReworkEntryID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.ReworkEntryNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }
            if (tableName == typeof(tb_FM_PreReceivedPayment).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_FM_PreReceivedPayment>()
                    .WhereIF(billno.GetType() == typeof(long), c => c.PreRPID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.PreRPNO == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_FM_ReceivablePayable).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_FM_ReceivablePayable>()
                    .Includes(c => c.tb_FM_ReceivablePayableDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.ARAPId == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.ARAPNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }
            if (tableName == typeof(tb_FM_PriceAdjustment).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_FM_PriceAdjustment>()
                    .Includes(c => c.tb_FM_PriceAdjustmentDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.AdjustId == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.AdjustNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }

            if (tableName == typeof(tb_FM_PaymentRecord).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_FM_PaymentRecord>()
                    .Includes(c => c.tb_FM_PaymentRecordDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.PaymentId == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.PaymentNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }



            if (tableName == typeof(tb_MRP_ReworkReturn).Name)
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_MRP_ReworkReturn>()
                    .Includes(c => c.tb_MRP_ReworkReturnDetails)
                    .WhereIF(billno.GetType() == typeof(long), c => c.ReworkReturnID == billno.ToLong())
                    .WhereIF(billno.GetType() == typeof(string), c => c.ReworkReturnNo == billno.ToString())
                    .Single();
                menuPowerHelper.ExecuteEvents(RelatedMenuInfo, obj);
            }
        }


    }
}
