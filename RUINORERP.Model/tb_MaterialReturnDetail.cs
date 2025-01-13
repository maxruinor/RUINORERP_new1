
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:53
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
    /// 退料单明细
    /// </summary>
    [Serializable()]
    [Description("tb_MaterialReturnDetail")]
    [SugarTable("tb_MaterialReturnDetail")]
    public partial class tb_MaterialReturnDetail: BaseEntity, ICloneable
    {
        public tb_MaterialReturnDetail()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_MaterialReturnDetail" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Sub_ID;
        /// <summary>
        /// 明细
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Sub_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "明细" , IsPrimaryKey = true)]
        public long Sub_ID
        { 
            get{return _Sub_ID;}
            set{
            base.PrimaryKeyID = _Sub_ID;
            SetProperty(ref _Sub_ID, value);
            }
        }

        private long _MRE_ID;
        /// <summary>
        /// 主单
        /// </summary>
        [AdvQueryAttribute(ColName = "MRE_ID",ColDesc = "主单")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MRE_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "主单" )]
        [FKRelationAttribute("tb_MaterialReturn","MRE_ID")]
        public long MRE_ID
        { 
            get{return _MRE_ID;}
            set{
            SetProperty(ref _MRE_ID, value);
            }
        }

        private long _ProdDetailID;
        /// <summary>
        /// 货品
        /// </summary>
        [AdvQueryAttribute(ColName = "ProdDetailID",ColDesc = "产品")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProdDetailID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "产品" )]
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

        private int _Quantity= ((0));
        /// <summary>
        /// 数量
        /// </summary>
        [AdvQueryAttribute(ColName = "Quantity",ColDesc = "数量")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "Quantity" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "数量" )]
        public int Quantity
        { 
            get{return _Quantity;}
            set{
            SetProperty(ref _Quantity, value);
            }
        }

        private decimal _Cost= ((0));
        /// <summary>
        /// 成本
        /// </summary>
        [AdvQueryAttribute(ColName = "Cost",ColDesc = "成本")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Cost" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "成本" )]
        public decimal Cost
        { 
            get{return _Cost;}
            set{
            SetProperty(ref _Cost, value);
            }
        }

        private decimal _Price= ((0));
        /// <summary>
        /// 单价
        /// </summary>
        [AdvQueryAttribute(ColName = "Price",ColDesc = "单价")] 
        [SugarColumn(ColumnDataType = "decimal", SqlParameterDbType ="Decimal",  ColumnName = "Price" , DecimalDigits = 6,IsNullable = false,ColumnDescription = "单价" )]
        public decimal Price
        { 
            get{return _Price;}
            set{
            SetProperty(ref _Price, value);
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
        [Navigate(NavigateType.OneToOne, nameof(Location_ID))]
        public virtual tb_Location tb_location { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProdDetailID))]
        public virtual tb_ProdDetail tb_proddetail { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(MRE_ID))]
        public virtual tb_MaterialReturn tb_materialreturn { get; set; }



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
                    Type type = typeof(tb_MaterialReturnDetail);
                    
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
            tb_MaterialReturnDetail loctype = (tb_MaterialReturnDetail)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

