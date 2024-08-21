
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/26/2024 20:42:09
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
    /// 
    /// </summary>
    [Serializable()]
    [SugarTable("Proc_SaleOrderStatisticsByEmployee")]
    public class Proc_SaleOrderStatisticsByEmployee : BaseEntity, ICloneable
    {
        public Proc_SaleOrderStatisticsByEmployee()
        {
            FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("Proc_SaleOrderStatisticsByEmployee" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private long? _Employee_ID;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "业务员")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 255,Length=8,IsNullable = true,ColumnDescription = "业务员")]
        [Display(Name = "")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
        }

        private long? _ProjectGroup_ID;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" , DecimalDigits = 255,Length=8,IsNullable = true,ColumnDescription = "项目组")]
        [Display(Name = "")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}            set{                SetProperty(ref _ProjectGroup_ID, value);                }
        }

        private int? _总销售订单数量;


        /// <summary>
        /// 总销售订单数量
        /// </summary>

        [AdvQueryAttribute(ColName = "总销售订单数量", ColDesc = "总销售订单数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "总销售出库数量", DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "总销售订单数量")]
        [Display(Name = "总销售订单数量")]
        public int? 总销售订单数量
        { 
            get{return _总销售订单数量; }            set{                SetProperty(ref _总销售订单数量, value);                }
        }

        private decimal? _订单成交金额;
        
        
        /// <summary>
        /// 订单成交金额
        /// </summary>

        [AdvQueryAttribute(ColName = "订单成交金额",ColDesc = "订单成交金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "订单成交金额" , DecimalDigits = 255,Length=8,IsNullable = true,ColumnDescription = "订单成交金额" )]
        [Display(Name = "订单成交金额")]
        public decimal? 订单成交金额 
        { 
            get{return _订单成交金额;}            set{                SetProperty(ref _订单成交金额, value);                }
        }

        private int? _退货数量;
        
        
        /// <summary>
        /// 退货数量
        /// </summary>

        [AdvQueryAttribute(ColName = "退货数量",ColDesc = "退货数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "退货数量" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "退货数量" )]
        [Display(Name = "退货数量")]
        public int? 退货数量 
        { 
            get{return _退货数量;}            set{                SetProperty(ref _退货数量, value);                }
        }

        private decimal? _退货金额;
        
        
        /// <summary>
        /// 退货金额
        /// </summary>

        [AdvQueryAttribute(ColName = "退货金额",ColDesc = "退货金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "退货金额" , DecimalDigits = 255,Length=8,IsNullable = true,ColumnDescription = "退货金额" )]
        [Display(Name = "退货金额")]
        public decimal? 退货金额 
        { 
            get{return _退货金额;}            set{                SetProperty(ref _退货金额, value);                }
        }

        private decimal? _销售税额;
        
        
        /// <summary>
        /// 销售税额
        /// </summary>

        [AdvQueryAttribute(ColName = "销售税额",ColDesc = "销售税额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "销售税额" , DecimalDigits = 255,Length=8,IsNullable = true,ColumnDescription = "销售税额" )]
        [Display(Name = "销售税额")]
        public decimal? 销售税额 
        { 
            get{return _销售税额;}            set{                SetProperty(ref _销售税额, value);                }
        }

       

        private decimal? _佣金返点;
        
        
        /// <summary>
        /// 佣金返点
        /// </summary>

        [AdvQueryAttribute(ColName = "佣金返点",ColDesc = "佣金返点")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "佣金返点" , DecimalDigits = 255,Length=8,IsNullable = true,ColumnDescription = "佣金返点" )]
        [Display(Name = "佣金返点")]
        public decimal? 佣金返点 
        { 
            get{return _佣金返点;}            set{                SetProperty(ref _佣金返点, value);                }
        }

        private decimal? _佣金返还;
        
     

        private int? _实际成交数量;
        
        
        /// <summary>
        /// 实际成交数量
        /// </summary>

        [AdvQueryAttribute(ColName = "实际成交数量",ColDesc = "实际成交数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "实际成交数量" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "实际成交数量" )]
        [Display(Name = "实际成交数量")]
        public int? 实际成交数量 
        { 
            get{return _实际成交数量;}            set{                SetProperty(ref _实际成交数量, value);                }
        }

        private decimal? _实际成交金额;
        
        
        /// <summary>
        /// 实际成交金额
        /// </summary>

        [AdvQueryAttribute(ColName = "实际成交金额",ColDesc = "实际成交金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "实际成交金额" , DecimalDigits = 255,Length=8,IsNullable = true,ColumnDescription = "实际成交金额" )]
        [Display(Name = "实际成交金额")]
        public decimal? 实际成交金额 
        { 
            get{return _实际成交金额;}            set{                SetProperty(ref _实际成交金额, value);                }
        }







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
        [Description("列名中文描述"), Category("自定属性"), Browsable(true)]
        [SugarColumn(IsIgnore = true)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(Proc_SaleOutStatisticsByEmployee);
                    
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
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

