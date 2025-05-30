﻿
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
using RUINORERP.Model.Base;

namespace RUINORERP.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    [SugarTable("Proc_SaleOutStatisticsByEmployee")]
    public partial class Proc_SaleOutStatisticsByEmployee: BaseViewEntity
    {
        public Proc_SaleOutStatisticsByEmployee()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("Proc_SaleOutStatisticsByEmployee" + "外键ID与对应主主键名称不一致。请修改数据库");
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

        private int? _总销售出库数量;
        
        
        /// <summary>
        /// 总销售出库数量
        /// </summary>

        [AdvQueryAttribute(ColName = "总销售出库数量",ColDesc = "总销售出库数量")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "总销售出库数量" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "总销售出库数量" )]
        [Display(Name = "总销售出库数量")]
        public int? 总销售出库数量 
        { 
            get{return _总销售出库数量;}            set{                SetProperty(ref _总销售出库数量, value);                }
        }

        private decimal? _出库成交金额;
        
        
        /// <summary>
        /// 出库成交金额
        /// </summary>

        [AdvQueryAttribute(ColName = "出库成交金额",ColDesc = "出库成交金额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "出库成交金额" , DecimalDigits = 255,Length=8,IsNullable = true,ColumnDescription = "出库成交金额" )]
        [Display(Name = "出库成交金额")]
        public decimal? 出库成交金额 
        { 
            get{return _出库成交金额;}            set{                SetProperty(ref _出库成交金额, value);                }
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

        private decimal? _退货税额;
        
        
        /// <summary>
        /// 退货税额
        /// </summary>

        [AdvQueryAttribute(ColName = "退货税额",ColDesc = "退货税额")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "退货税额" , DecimalDigits = 255,Length=8,IsNullable = true,ColumnDescription = "退货税额" )]
        [Display(Name = "退货税额")]
        public decimal? 退货税额 
        { 
            get{return _退货税额;}            set{                SetProperty(ref _退货税额, value);                }
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
        
        
        /// <summary>
        /// 佣金返还
        /// </summary>

        [AdvQueryAttribute(ColName = "佣金返还",ColDesc = "佣金返还")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType ="Decimal",  ColumnName = "佣金返还" , DecimalDigits = 255,Length=8,IsNullable = true,ColumnDescription = "佣金返还" )]
        [Display(Name = "佣金返还")]
        public decimal? 佣金返还 
        { 
            get{return _佣金返还;}            set{                SetProperty(ref _佣金返还, value);                }
        }

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


        private decimal? _成本;


        /// <summary>
        /// 成本
        /// </summary>

        [AdvQueryAttribute(ColName = "成本", ColDesc = "成本")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "成本", DecimalDigits = 255, Length = 8, IsNullable = true, ColumnDescription = "成本")]
        [Display(Name = "成本")]
        public decimal? 成本
        {
            get { return _成本; }            set
            {                SetProperty(ref _成本, value);
            }
        }

        private decimal? _毛利润;


        /// <summary>
        /// 毛利润
        /// </summary>

        [AdvQueryAttribute(ColName = "毛利润", ColDesc = "毛利润")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "毛利润", DecimalDigits = 255, Length = 8, IsNullable = true, ColumnDescription = "毛利润")]
        [Display(Name = "毛利润")]
        public decimal? 毛利润
        {
            get { return _毛利润; }            set
            {                SetProperty(ref _毛利润, value);
            }
        }


        private decimal? _毛利率;


        /// <summary>
        /// 毛利率
        /// </summary>

        [AdvQueryAttribute(ColName = "毛利率", ColDesc = "毛利率")]
        [SugarColumn(ColumnDataType = "money", SqlParameterDbType = "Decimal", ColumnName = "毛利率", DecimalDigits = 6, Length = 8, IsNullable = true, ColumnDescription = "毛利率")]
        [Display(Name = "毛利率")]
        public decimal? 毛利率
        {
            get { return _毛利率; }            set
            {                SetProperty(ref _毛利率, value);
            }
        }





        //如果为false,则不可以。
        private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}


  

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

