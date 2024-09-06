using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using Pipelines.Sockets.Unofficial.Arenas;
using RUINORERP.Business;
using RUINORERP.Business.Processor;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.Model;
using RUINORERP.Model;
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
        public GridViewRelated()
        {
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        }
        MenuPowerHelper menuPowerHelper;


        private bool complexType = false;

        /// <summary>
        /// 用一个属性来标识是不是复杂的类型。默认不是。
        /// 复杂是像库存跟踪一样。目标是由另一列中的具体的值来决定哪一个表
        /// </summary>
        public bool ComplexType { get => complexType; set => complexType = value; }

        private string complexTargtetField = string.Empty;

        /// <summary>
        /// 复杂类型时，目标字段
        /// </summary>
        public string ComplexTargtetField { get => complexTargtetField; set => complexTargtetField = value; }



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
        /// <param name="_ExpSourceUniqueField">要打开的窗体用的实体名</param>
        public void SetRelatedInfo<T1>(Expression<Func<T1, object>> _ExpSourceUniqueField, KeyNamePair TargetTableNameFromField)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = typeof(T1).Name;
            relatedInfo.TargetTableName = TargetTableNameFromField;

            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;

            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == TargetTableNameFromField.Key))
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
            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == TableName && c.SourceTableName == TableName))
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
        private List<RelatedInfo> RelatedInfoList { get; set; } = new List<RelatedInfo>();



        /*
               /// <summary>
        /// 关联单据的列,前面是引用单号列名，后面是 表名+原始单号列名
        /// 例如：如果在出库单中打开订单：则入订单类型，出库表中的引用订单的单号列名|订单自己的列名
        /// </summary>
        public ConcurrentDictionary<string, string> RelatedBillCols { get; set; } = new ConcurrentDictionary<string, string>();
         */

        //  RelatedBillCols.TryAdd(expRefBillNoColName.GetMemberInfo().Name, typeof(T).Name + "|" + expSourceBillNoColName.GetMemberInfo().Name);

        /*
            base.RelatedBillEditCol = (c => c.PPNo);
            base.SetRelatedBillCols<tb_SaleOrder>(c => c.SOrderNo, r => r.SaleOrderNo);
         */

        /// <summary>
        /// 用于要打开的窗体是由源来中的一个列名决定ID或编号，目标表名是由源表格中某一列的值来决定
        /// </summary>
        /// <param name="GridViewColumnFieldName"></param>
        /// <param name="CurrentRowEntity"></param>
        /// <param name="IsFromGridValue">是否从Grid中取值,只是用这个参数来区别一下没有实际作用,后面优化吧</param
        public void GuideToForm(string GridViewColumnFieldName, object CurrentRowEntity)
        {
            tb_MenuInfo RelatedMenuInfo = null;

            if (ComplexType)
            {
                #region 复杂类型

                string TargetTableKey = CurrentRowEntity.GetPropertyValue(ComplexTargtetField).ToString();
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
                            return;
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
                            return;
                        }

                        OpenTargetEntity(RelatedMenuInfo, tableName, billno);
                    }

                }
                #endregion

            }

        }



        public void GuideToForm(string GridViewColumnFieldName, DataGridViewRow CurrentRow)
        {
            tb_MenuInfo RelatedMenuInfo = null;
            RelatedInfo relatedRelationship = RelatedInfoList.FirstOrDefault(c => c.SourceUniqueField == GridViewColumnFieldName);
            if (relatedRelationship != null)
            {
                RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == relatedRelationship.TargetTableName.Key && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                if (CurrentRow.DataBoundItem is RUINORERP.Model.BaseEntity entity && relatedRelationship.TargetTableName.Key == relatedRelationship.SourceTableName)
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
                    //要查询取值
                    GuideToForm(GridViewColumnFieldName, CurrentRow.DataBoundItem);
                }
            }

        }


        public void OpenTargetEntity(tb_MenuInfo RelatedMenuInfo, string tableName, object billno)
        {

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
                    .Includes(c => c.tb_BOM_SDetails)
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

        }


    }
}
