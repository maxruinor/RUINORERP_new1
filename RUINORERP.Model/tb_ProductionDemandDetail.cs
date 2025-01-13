
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/19/2024 12:18:09
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
    /// 生产需求分析表明细
    /// </summary>
    [Serializable()]
    [Description("生产需求分析表明细")]
    [SugarTable("tb_ProductionDemandDetail")]
    public partial class tb_ProductionDemandDetail: BaseEntity, ICloneable
    {
        public tb_ProductionDemandDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("生产需求分析表明细tb_ProductionDemandDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PDCID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PDCID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PDCID
        { 
            get{return _PDCID;}
            set{
            base.PrimaryKeyID = _PDCID;
            SetProperty(ref _PDCID, value);
            }
        }

        private long? _PDID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PDID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PDID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ProductionDemand","PDID")]
        public long? PDID
        { 
            get{return _PDID;}
            set{
            SetProperty(ref _PDID, value);
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
            }
        }

        private string _property;
        /// <summary>
        /// 属性
        /// </summary>
        [AdvQueryAttribute(ColName = "property",ColDesc = "属性")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "property" ,Length=255,IsNullable = true,ColumnDescription = "属性" )]
        public string property
        { 
            get{return _property;}
            set{
            SetProperty(ref _property, value);
            }
        }

        private long _Location_ID;
        /// <summary>
        /// 库位
        /// </summary>
        [AdvQueryAttribute(ColName = "Location_ID",ColDesc = "库位")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Location_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "库位" )]
        [FKRelationAttribute("tb_Location","Location_ID")]
        public long Location_ID
        { 
            get{return _Location_ID;}
            set{
            SetProperty(ref _Location_ID, value);
            }
        }

        private long? _BOM_ID;
        /// <summary>
        /// 标准配方
        /// </summary>
        [AdvQueryAttribute(ColName = "BOM_ID",ColDesc = "标准配方")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "标准配方" )]
        [FKRelationAttribute("tb_BOM_S","BOM_ID")]
        public long? BOM_ID
        { 
            get{return _BOM_ID;}
            set{
            SetProperty(ref _BOM_ID, value);
            }
        }

        private long? _ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public long? ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
            }
        }

        private long? _ParentId;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ParentId",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ParentId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public long? ParentId
        { 
            get{return _ParentId;}
            set{
            SetProperty(ref _ParentId, value);
            }
        }

        private int _NetRequirement= ((0));
        /// <summary>
        /// 净需求
        /// </summary>
        [AdvQueryAttribute(ColName = "NetRequirement",ColDesc = "净需求")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "NetRequirement" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "净需求" )]
        public int NetRequirement
        { 
            get{return _NetRequirement;}
            set{
            SetProperty(ref _NetRequirement, value);
            }
        }

        private int _GrossRequirement= ((0));
        /// <summary>
        /// 毛需求
        /// </summary>
        [AdvQueryAttribute(ColName = "GrossRequirement",ColDesc = "毛需求")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "GrossRequirement" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "毛需求" )]
        public int GrossRequirement
        { 
            get{return _GrossRequirement;}
            set{
            SetProperty(ref _GrossRequirement, value);
            }
        }

        private int _NeedQuantity= ((0));
        /// <summary>
        /// 实际需求
        /// </summary>
        [AdvQueryAttribute(ColName = "NeedQuantity",ColDesc = "实际需求")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "NeedQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "实际需求" )]
        public int NeedQuantity
        { 
            get{return _NeedQuantity;}
            set{
            SetProperty(ref _NeedQuantity, value);
            }
        }

        private int _MissingQuantity= ((0));
        /// <summary>
        /// 缺少数量
        /// </summary>
        [AdvQueryAttribute(ColName = "MissingQuantity",ColDesc = "缺少数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "MissingQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "缺少数量" )]
        public int MissingQuantity
        { 
            get{return _MissingQuantity;}
            set{
            SetProperty(ref _MissingQuantity, value);
            }
        }

        private DateTime _RequirementDate;
        /// <summary>
        /// 需求日期
        /// </summary>
        [AdvQueryAttribute(ColName = "RequirementDate",ColDesc = "需求日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "RequirementDate" ,IsNullable = false,ColumnDescription = "需求日期" )]
        public DateTime RequirementDate
        { 
            get{return _RequirementDate;}
            set{
            SetProperty(ref _RequirementDate, value);
            }
        }

        private int _BookInventory= ((0));
        /// <summary>
        /// 账面库存
        /// </summary>
        [AdvQueryAttribute(ColName = "BookInventory",ColDesc = "账面库存")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BookInventory" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "账面库存" )]
        public int BookInventory
        { 
            get{return _BookInventory;}
            set{
            SetProperty(ref _BookInventory, value);
            }
        }

        private int _AvailableStock= ((0));
        /// <summary>
        /// 可用库存
        /// </summary>
        [AdvQueryAttribute(ColName = "AvailableStock",ColDesc = "可用库存")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "AvailableStock" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "可用库存" )]
        public int AvailableStock
        { 
            get{return _AvailableStock;}
            set{
            SetProperty(ref _AvailableStock, value);
            }
        }

        private int _InTransitInventory= ((0));
        /// <summary>
        /// 在途库存
        /// </summary>
        [AdvQueryAttribute(ColName = "InTransitInventory",ColDesc = "在途库存")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "InTransitInventory" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "在途库存" )]
        public int InTransitInventory
        { 
            get{return _InTransitInventory;}
            set{
            SetProperty(ref _InTransitInventory, value);
            }
        }

        private int _MakeProcessInventory= ((0));
        /// <summary>
        /// 在制库存
        /// </summary>
        [AdvQueryAttribute(ColName = "MakeProcessInventory",ColDesc = "在制库存")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "MakeProcessInventory" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "在制库存" )]
        public int MakeProcessInventory
        { 
            get{return _MakeProcessInventory;}
            set{
            SetProperty(ref _MakeProcessInventory, value);
            }
        }

        private int _Sale_Qty= ((0));
        /// <summary>
        /// 拟销售量
        /// </summary>
        [AdvQueryAttribute(ColName = "Sale_Qty",ColDesc = "拟销售量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Sale_Qty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "拟销售量" )]
        public int Sale_Qty
        { 
            get{return _Sale_Qty;}
            set{
            SetProperty(ref _Sale_Qty, value);
            }
        }

        private int _NotOutQty= ((0));
        /// <summary>
        /// 未发数量
        /// </summary>
        [AdvQueryAttribute(ColName = "NotOutQty",ColDesc = "未发数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "NotOutQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "未发数量" )]
        public int NotOutQty
        { 
            get{return _NotOutQty;}
            set{
            SetProperty(ref _NotOutQty, value);
            }
        }

        private string _Summary;
        /// <summary>
        /// 摘要
        /// </summary>
        [AdvQueryAttribute(ColName = "Summary",ColDesc = "摘要")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Summary" ,Length=255,IsNullable = true,ColumnDescription = "摘要" )]
        public string Summary
        { 
            get{return _Summary;}
            set{
            SetProperty(ref _Summary, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(BOM_ID))]
        public virtual tb_BOM_S tb_bom_s { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(PDID))]
        public virtual tb_ProductionDemand tb_productiondemand { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }



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
                    Type type = typeof(tb_ProductionDemandDetail);
                    
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
            tb_ProductionDemandDetail loctype = (tb_ProductionDemandDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

