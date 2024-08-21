using FastReport.DevComponents.DotNetBar.Controls;
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
            relatedInfo.TargetTableName = typeof(T1).Name;

            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            relatedInfo.TargetDisplayField = _ExpSourceUniqueField.GetMemberInfo().Name;
            if (!RelatedInfoList.Any(c => c.TargetTableName == typeof(T1).Name))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }


        public void SetRelatedInfo(string TableName, string FieldName)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = TableName;
            relatedInfo.TargetTableName = TableName;

            relatedInfo.SourceUniqueField = FieldName;
            relatedInfo.TargetDisplayField = FieldName;
            if (!RelatedInfoList.Any(c => c.TargetTableName == TableName && c.SourceTableName == TableName))
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
            relatedInfo.TargetTableName = typeof(T2).Name;

            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            relatedInfo.TargetDisplayField = _ExpTargetDisplayField.GetMemberInfo().Name;
            if (!RelatedInfoList.Any(c => c.TargetTableName == typeof(T2).Name))
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
        /// 某列中的每行数据内容类型来判断如何跳转
        /// </summary>
        /// <param name="RelatedMenuInfo"></param>
        /// <param name="GridViewColumnFieldName"></param>
        /// <param name="RelatedBillCols"></param>
        /// <param name="CurrentRowEntity"></param>
        public void GuideToForm(tb_MenuInfo RelatedMenuInfo, string GridViewColumnFieldName, ConcurrentDictionary<string, string> RelatedBillCols, object CurrentRowEntity)
        {

        }


        public void GuideToForm(string GridViewColumnFieldName, object CurrentRowEntity)
        {
            tb_MenuInfo RelatedMenuInfo = null;
            RelatedInfo relatedRelationship = RelatedInfoList.FirstOrDefault(c => c.SourceUniqueField == GridViewColumnFieldName);
            if (relatedRelationship != null)
            {
                string tableName = relatedRelationship.TargetTableName;
                //这里是显示明细
                //要把单据信息传过去
                //                RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == tableName && m.ClassPath.Contains(tableName.Replace("tb_", "UC").ToString().Replace("Query", ""))).FirstOrDefault();
                RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == relatedRelationship.TargetTableName && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
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

                    if (tableName == typeof(tb_ProductionPlan).Name)
                    {
                        var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>()
                            .Includes(c => c.tb_ProductionPlanDetails)
                            .Where(c => c.PPNo == billno.ToString())
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


                    //}
                }

            }
        }

        public void GuideToForm(string bizTypeName, string GridViewColumnFieldName, object CurrentRowEntity)
        {
            tb_MenuInfo RelatedMenuInfo = null;
            RelatedInfo relatedRelationship = RelatedInfoList.FirstOrDefault(c => c.SourceUniqueField == GridViewColumnFieldName);
            if (relatedRelationship != null)
            {
                foreach (var pair in RelatedInfoList)
                {
                    string tableName = pair.TargetTableName;
                    //这里是显示明细
                    //要把单据信息传过去
                    RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == tableName && m.ClassPath.Contains(tableName.Replace("tb_", "UC").ToString().Replace("Query", ""))).FirstOrDefault();
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

                        var billno = CurrentRowEntity.GetPropertyValue(pair.SourceUniqueField);

                        if (tableName == typeof(tb_ProductionPlan).Name)
                        {
                            var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ProductionPlan>()
                                .Includes(c => c.tb_ProductionPlanDetails)
                                .Where(c => c.PPNo == billno.ToString())
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


                        //}
                    }

                }

            }
        }

        public void GuideToForm(string GridViewColumnFieldName, DataGridViewRow CurrentRow)
        {
            tb_MenuInfo RelatedMenuInfo = null;
            RelatedInfo relatedRelationship = RelatedInfoList.FirstOrDefault(c => c.SourceUniqueField == GridViewColumnFieldName);
            if (relatedRelationship != null)
            {
                RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == relatedRelationship.TargetTableName && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
                if (CurrentRow.DataBoundItem is RUINORERP.Model.BaseEntity entity && relatedRelationship.TargetTableName == relatedRelationship.SourceTableName)
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



    }
}
