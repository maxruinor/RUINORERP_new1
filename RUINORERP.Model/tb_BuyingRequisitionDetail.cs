
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:28
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
    /// 请购单明细表
    /// </summary>
    [Serializable()]
    [Description("tb_BuyingRequisitionDetail")]
    [SugarTable("tb_BuyingRequisitionDetail")]
    public partial class tb_BuyingRequisitionDetail: BaseEntity, ICloneable
    {
        public tb_BuyingRequisitionDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_BuyingRequisitionDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PuRequisition_ChildID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PuRequisition_ChildID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PuRequisition_ChildID
        { 
            get{return _PuRequisition_ChildID;}
            set{
            base.PrimaryKeyID = _PuRequisition_ChildID;
            SetProperty(ref _PuRequisition_ChildID, value);
            }
        }

        private long? _PuRequisition_ID;
        /// <summary>
        /// 请购单
        /// </summary>
        [AdvQueryAttribute(ColName = "PuRequisition_ID",ColDesc = "请购单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PuRequisition_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "请购单" )]
        [FKRelationAttribute("tb_BuyingRequisition","PuRequisition_ID")]
        public long? PuRequisition_ID
        { 
            get{return _PuRequisition_ID;}
            set{
            SetProperty(ref _PuRequisition_ID, value);
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 货品详情
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "货品详情")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "货品详情" )]
        [FKRelationAttribute("tb_ProdDetail","ProdDetailID")]
        public long ProdDetailID
        { 
            get{return _ProdDetailID;}
            set{
            SetProperty(ref _ProdDetailID, value);
            }
        }

        private DateTime? _RequirementDate;
        /// <summary>
        /// 需求日期
        /// </summary>
        [AdvQueryAttribute(ColName = "RequirementDate",ColDesc = "需求日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "RequirementDate" ,IsNullable = true,ColumnDescription = "需求日期" )]
        public DateTime? RequirementDate
        { 
            get{return _RequirementDate;}
            set{
            SetProperty(ref _RequirementDate, value);
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

        private int _ActualRequiredQty;
        /// <summary>
        /// 需求数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ActualRequiredQty",ColDesc = "需求数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ActualRequiredQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "需求数量" )]
        public int ActualRequiredQty
        { 
            get{return _ActualRequiredQty;}
            set{
            SetProperty(ref _ActualRequiredQty, value);
            }
        }

        private int _Quantity;
        /// <summary>
        /// 请购数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "请购数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "请购数量" )]
        public int Quantity
        { 
            get{return _Quantity;}
            set{
            SetProperty(ref _Quantity, value);
            }
        }

        private decimal? _EstimatedPrice;
        /// <summary>
        /// 预估价格
        /// </summary>
        [AdvQueryAttribute(ColName = "EstimatedPrice",ColDesc = "预估价格")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "EstimatedPrice" , DecimalDigits = 6,IsNullable = true,ColumnDescription = "预估价格" )]
        public decimal? EstimatedPrice
        { 
            get{return _EstimatedPrice;}
            set{
            SetProperty(ref _EstimatedPrice, value);
            }
        }

        private int _DeliveredQuantity= ((0));
        /// <summary>
        /// 已交数量
        /// </summary>
        [AdvQueryAttribute(ColName = "DeliveredQuantity",ColDesc = "已交数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "DeliveredQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "已交数量" )]
        public int DeliveredQuantity
        { 
            get{return _DeliveredQuantity;}
            set{
            SetProperty(ref _DeliveredQuantity, value);
            }
        }

        private string _Purpose;
        /// <summary>
        /// 用途
        /// </summary>
        [AdvQueryAttribute(ColName = "Purpose",ColDesc = "用途")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Purpose" ,Length=500,IsNullable = false,ColumnDescription = "用途" )]
        public string Purpose
        { 
            get{return _Purpose;}
            set{
            SetProperty(ref _Purpose, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=1000,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        private bool? _Purchased= false;
        /// <summary>
        /// 已采购
        /// </summary>
        [AdvQueryAttribute(ColName = "Purchased",ColDesc = "已采购")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Purchased" ,IsNullable = true,ColumnDescription = "已采购" )]
        public bool? Purchased
        { 
            get{return _Purchased;}
            set{
            SetProperty(ref _Purchased, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(PuRequisition_ID))]
        public virtual tb_BuyingRequisition tb_buyingrequisition { get; set; }



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
                    Type type = typeof(tb_BuyingRequisitionDetail);
                    
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
            tb_BuyingRequisitionDetail loctype = (tb_BuyingRequisitionDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

