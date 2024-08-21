
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:39
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 库位表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Location")]
    public partial class tb_Location: BaseEntity, ICloneable
    {
        public tb_Location()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Location" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Location_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Location_ID
        { 
            get{return _Location_ID;}
            set{
            base.PrimaryKeyID = _Location_ID;
            SetProperty(ref _Location_ID, value);
            }
        }

        private long? _LocationType_ID;
        /// <summary>
        /// 库位类型
        /// </summary>
        [AdvQueryAttribute(ColName = "LocationType_ID",ColDesc = "库位类型")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "LocationType_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "库位类型" )]
        [FKRelationAttribute("tb_LocationType","LocationType_ID")]
        public long? LocationType_ID
        { 
            get{return _LocationType_ID;}
            set{
            SetProperty(ref _LocationType_ID, value);
            }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 联系人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "联系人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "联系人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long? Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
            }
        }

        private string _LocationCode;
        /// <summary>
        /// 仓库代码
        /// </summary>
        [AdvQueryAttribute(ColName = "LocationCode",ColDesc = "仓库代码")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "LocationCode" ,Length=50,IsNullable = false,ColumnDescription = "仓库代码" )]
        public string LocationCode
        { 
            get{return _LocationCode;}
            set{
            SetProperty(ref _LocationCode, value);
            }
        }
        private bool _Is_enabled = true;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "Is_enabled", ColDesc = "是否启用")]
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType = "Boolean", ColumnName = "Is_enabled", IsNullable = false, ColumnDescription = "是否启用")]
        public bool Is_enabled
        {
            get { return _Is_enabled; }
            set
            {
                SetProperty(ref _Is_enabled, value);
            }
        }
        private string _Tel;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Tel",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Tel" ,Length=20,IsNullable = true,ColumnDescription = "电话" )]
        public string Tel
        { 
            get{return _Tel;}
            set{
            SetProperty(ref _Tel, value);
            }
        }

        private string _Name;
        /// <summary>
        /// 仓库名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Name",ColDesc = "仓库名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Name" ,Length=50,IsNullable = false,ColumnDescription = "仓库名称" )]
        public string Name
        { 
            get{return _Name;}
            set{
            SetProperty(ref _Name, value);
            }
        }

        private string _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Desc",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Desc" ,Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string Desc
        { 
            get{return _Desc;}
            set{
            SetProperty(ref _Desc, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }
        //public virtual tb_Employee tb_Employee_ID { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(LocationType_ID))]
        public virtual tb_LocationType tb_locationtype { get; set; }
        //public virtual tb_LocationType tb_LocationType_ID { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FinishedGoodsInvDetail.Location_ID))]
        public virtual List<tb_FinishedGoodsInvDetail> tb_FinishedGoodsInvDetails { get; set; }
        //tb_FinishedGoodsInvDetail.Location_ID)
        //Location_ID.FK_FINISHEDDETAIL_REF_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrderDetail.Location_ID))]
        public virtual List<tb_PurOrderDetail> tb_PurOrderDetails { get; set; }
        //tb_PurOrderDetail.Location_ID)
        //Location_ID.FK_PURORDERDETAIL_REF_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockInDetail.Location_ID))]
        public virtual List<tb_StockInDetail> tb_StockInDetails { get; set; }
        //tb_StockInDetail.Location_ID)
        //Location_ID.FK_TB_STOCKINDETSIL_RE_LOCAT)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Prod.Location_ID))]
        public virtual List<tb_Prod> tb_Prods { get; set; }
        //tb_Prod.Location_ID)
        //Location_ID.FK_TB_PROD_REFERENCE_TB_LOCAT)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Inventory.Location_ID))]
        public virtual List<tb_Inventory> tb_Inventories { get; set; }
        //tb_Inventory.Location_ID)
        //Location_ID.FK_TB_INVENTORY_REF_LOCAT)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurOrderReDetail.Location_ID))]
        public virtual List<tb_PurOrderReDetail> tb_PurOrderReDetails { get; set; }
        //tb_PurOrderReDetail.Location_ID)
        //Location_ID.FK_PURORDERREDETAIL_REF_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StockOutDetail.Location_ID))]
        public virtual List<tb_StockOutDetail> tb_StockOutDetails { get; set; }
        //tb_StockOutDetail.Location_ID)
        //Location_ID.FK_TB_STOCKOUT_REF_TB_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Stocktake.Location_ID))]
        public virtual List<tb_Stocktake> tb_Stocktakes { get; set; }
        //tb_Stocktake.Location_ID)
        //Location_ID.FK_STOCKTAKE_REF_LOCATCATION)
        //tb_Location.Location_ID)

 

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_SDetailSecondary.Location_ID))]
        public virtual List<tb_BOM_SDetailSecondary> tb_BOM_SDetailSecondaries { get; set; }
        //tb_BOM_SDetailSecondary.Location_ID)
        //Location_ID.FK_TB_BO_SECREF_TB_LOCAT)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_StorageRack.Location_ID))]
        public virtual List<tb_StorageRack> tb_StorageRacks { get; set; }
        //tb_StorageRack.Location_ID)
        //Location_ID.FK_TB_STORA_REFERENCE_TB_LOCAT)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOrderDetail.Location_ID))]
        public virtual List<tb_SaleOrderDetail> tb_SaleOrderDetails { get; set; }
        //tb_SaleOrderDetail.Location_ID)
        //Location_ID.FK_SALEORDERDETAIL_REF_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ManufacturingOrderDetail.Location_ID))]
        public virtual List<tb_ManufacturingOrderDetail> tb_ManufacturingOrderDetails { get; set; }
        //tb_ManufacturingOrderDetail.Location_ID)
        //Location_ID.FK_MANUFACTURINGORDERDETAIL_REF_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProduceGoodsRecommendDetail.Location_ID))]
        public virtual List<tb_ProduceGoodsRecommendDetail> tb_ProduceGoodsRecommendDetails { get; set; }
        //tb_ProduceGoodsRecommendDetail.Location_ID)
        //Location_ID.FK_ProduceGoodsRecommendDetail_REF_LOCATion)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialRequisition.Location_ID))]
        public virtual List<tb_MaterialRequisition> tb_MaterialRequisitionses { get; set; }
        //tb_MaterialRequisitions.Location_ID)
        //Location_ID.FK_MATERIALREQUISITIONS_REF_LOCATION)
        //tb_Location.Location_ID)

  
        //tb_ProductionDemand.Location_ID)
        //Location_ID.FK_PRODDEMAND_REF_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurGoodsRecommendDetail.Location_ID))]
        public virtual List<tb_PurGoodsRecommendDetail> tb_PurGoodsRecommendDetails { get; set; }
        //tb_PurGoodsRecommendDetail.Location_ID)
        //Location_ID.FK_PURGOODSRECOMMENDDETAIL_RE_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProductionDemandDetail.Location_ID))]
        public virtual List<tb_ProductionDemandDetail> tb_ProductionDemandDetails { get; set; }
        //tb_ProductionDemandDetail.Location_ID)
        //Location_ID.FK_PRODUDEMANDETAIL_REF_LOCATION1)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialRequisitionDetail.Location_ID))]
        public virtual List<tb_MaterialRequisitionDetail> tb_MaterialRequisitionsDetails { get; set; }
        //tb_MaterialRequisitionsDetail.Location_ID)
        //Location_ID.FK_MATERIALREQUISIONDETAIL_REF_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialReturn.Location_ID))]
        public virtual List<tb_MaterialReturn> tb_MaterialReturns { get; set; }
        //tb_MaterialReturn.Location_ID)
        //Location_ID.FK_TB_MATERE_REF_TB_LOCAT)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_ProductionPlanDetail.Location_ID))]
        public virtual List<tb_ProductionPlanDetail> tb_ProductionPlanDetails { get; set; }
        //tb_ProductionPlanDetail.Location_ID)
        //Location_ID.FK_PRODUPLANDETAIL_REF_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_MaterialReturnDetail.Location_ID))]
        public virtual List<tb_MaterialReturnDetail> tb_MaterialReturnDetails { get; set; }
        //tb_MaterialReturnDetail.Location_ID)
        //Location_ID.FK_MATERIALREDETAIL_REF_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutDetail.Location_ID))]
        public virtual List<tb_SaleOutDetail> tb_SaleOutDetails { get; set; }
        //tb_SaleOutDetail.Location_ID)
        //Location_ID.FK_TB_SODETAIL_RE_TB_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntryDetail.Location_ID))]
        public virtual List<tb_PurEntryDetail> tb_PurEntryDetails { get; set; }
        //tb_PurEntryDetail.Location_ID)
        //Location_ID.FK_TB_PUREN_REF_TB_LOCATION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_SaleOutReDetail.Location_ID))]
        public virtual List<tb_SaleOutReDetail> tb_SaleOutReDetails { get; set; }
        //tb_SaleOutReDetail.Location_ID)
        //Location_ID.FK_SOREDETAIL_RE_TB_LOCATAION)
        //tb_Location.Location_ID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PurEntryReDetail.Location_ID))]
        public virtual List<tb_PurEntryReDetail> tb_PurEntryReDetails { get; set; }
        //tb_PurEntryReDetail.Location_ID)
        //Location_ID.FK_PURENREDETAIL_LOCATION)
        //tb_Location.Location_ID)

       

        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        #region 字段描述对应列表
        private ConcurrentDictionary<string, string> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_Location);
                    
                       foreach (PropertyInfo field in type.GetProperties())
                            {
                                foreach (Attribute attr in field.GetCustomAttributes(true))
                                {
                                    entityAttr = attr as SugarColumn;
                                    if (null != entityAttr)
                                    {
                                        if (entityAttr.ColumnDescription == null)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsIdentity)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsPrimaryKey)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.ColumnDescription.Trim().Length > 0)
                                        {
                                            fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                                        }
                                    }
                                }
                            }
                }
                
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }
        #endregion
        

        public override object Clone()
        {
            tb_Location loctype = (tb_Location)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

