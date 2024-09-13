
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:52
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
    /// 领料单明细
    /// </summary>
    [Serializable()]
    [Description("tb_MaterialRequisitionDetail")]
    [SugarTable("tb_MaterialRequisitionDetail")]
    public partial class tb_MaterialRequisitionDetail: BaseEntity, ICloneable
    {
        public tb_MaterialRequisitionDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_MaterialRequisitionDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Detail_ID;
        /// <summary>
        /// 明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Detail_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "明细" , IsPrimaryKey = true)]
        public long Detail_ID
        { 
            get{return _Detail_ID;}
            set{
            base.PrimaryKeyID = _Detail_ID;
            SetProperty(ref _Detail_ID, value);
            }
        }

        private long _MR_ID;
        /// <summary>
        /// 领料单
        /// </summary>
        [AdvQueryAttribute(ColName = "MR_ID",ColDesc = "领料单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MR_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "领料单" )]
        [FKRelationAttribute("tb_MaterialRequisition","MR_ID")]
        public long MR_ID
        { 
            get{return _MR_ID;}
            set{
            SetProperty(ref _MR_ID, value);
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

        private int _ShouldSendQty= ((0));
        /// <summary>
        /// 应发数
        /// </summary>
        [AdvQueryAttribute(ColName = "ShouldSendQty",ColDesc = "应发数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ShouldSendQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "应发数" )]
        public int ShouldSendQty
        { 
            get{return _ShouldSendQty;}
            set{
            SetProperty(ref _ShouldSendQty, value);
            }
        }

        private int _ActualSentQty= ((0));
        /// <summary>
        /// 实发数
        /// </summary>
        [AdvQueryAttribute(ColName = "ActualSentQty",ColDesc = "实发数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ActualSentQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "实发数" )]
        public int ActualSentQty
        { 
            get{return _ActualSentQty;}
            set{
            SetProperty(ref _ActualSentQty, value);
            }
        }

        private int _CanQuantity= ((0));
        /// <summary>
        /// 可发数
        /// </summary>
        [AdvQueryAttribute(ColName = "CanQuantity",ColDesc = "可发数")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "CanQuantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "可发数" )]
        public int CanQuantity
        { 
            get{return _CanQuantity;}
            set{
            SetProperty(ref _CanQuantity, value);
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

        private string _CustomerPartNo;
        /// <summary>
        /// 客户型号
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerPartNo",ColDesc = "客户型号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerPartNo" ,Length=50,IsNullable = true,ColumnDescription = "客户型号" )]
        public string CustomerPartNo
        { 
            get{return _CustomerPartNo;}
            set{
            SetProperty(ref _CustomerPartNo, value);
            }
        }

        private decimal _Cost= ((0));
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Cost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本" )]
        public decimal Cost
        { 
            get{return _Cost;}
            set{
            SetProperty(ref _Cost, value);
            }
        }

        private decimal _Price= ((0));
        /// <summary>
        /// 价格
        /// </summary>
        [AdvQueryAttribute(ColName = "Price",ColDesc = "价格")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "Price" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "价格" )]
        public decimal Price
        { 
            get{return _Price;}
            set{
            SetProperty(ref _Price, value);
            }
        }

        private decimal _SubtotalPrice= ((0));
        /// <summary>
        /// 金额小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalPrice",ColDesc = "金额小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalPrice" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "金额小计" )]
        public decimal SubtotalPrice
        { 
            get{return _SubtotalPrice;}
            set{
            SetProperty(ref _SubtotalPrice, value);
            }
        }

        private decimal _SubtotalCost= ((0));
        /// <summary>
        /// 成本小计
        /// </summary>
        [AdvQueryAttribute(ColName = "SubtotalCost",ColDesc = "成本小计")] 
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "SubtotalCost" , DecimalDigits = 4,IsNullable = false,ColumnDescription = "成本小计" )]
        public decimal SubtotalCost
        { 
            get{return _SubtotalCost;}
            set{
            SetProperty(ref _SubtotalCost, value);
            }
        }

        private int _ReturnQty= ((0));
        /// <summary>
        /// 退回数量
        /// </summary>
        [AdvQueryAttribute(ColName = "ReturnQty",ColDesc = "退回数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ReturnQty" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "退回数量" )]
        public int ReturnQty
        { 
            get{return _ReturnQty;}
            set{
            SetProperty(ref _ReturnQty, value);
            }
        }

        private long _ManufacturingOrderDetailRowID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ManufacturingOrderDetailRowID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ManufacturingOrderDetailRowID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        public long ManufacturingOrderDetailRowID
        { 
            get{return _ManufacturingOrderDetailRowID;}
            set{
            SetProperty(ref _ManufacturingOrderDetailRowID, value);
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
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(MR_ID))]
        public virtual tb_MaterialRequisition tb_materialrequisition { get; set; }



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
                    Type type = typeof(tb_MaterialRequisitionDetail);
                    
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
            tb_MaterialRequisitionDetail loctype = (tb_MaterialRequisitionDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

