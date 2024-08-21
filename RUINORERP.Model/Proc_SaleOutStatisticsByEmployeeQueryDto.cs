
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/26/2024 19:54:05
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
    [SugarTable("Proc_SaleOutStatisticsByEmployee")]
    public class Proc_SaleOutStatisticsByEmployeeQueryDto:BaseEntity, ICloneable
    {
        public Proc_SaleOutStatisticsByEmployeeQueryDto()
        {

        }

    
        private long? _Employee_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Employee_ID",Length=8,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? Employee_ID 
        { 
            get{return _Employee_ID;}            set{                SetProperty(ref _Employee_ID, value);                }
        }

        private long? _ProjectGroup_ID;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ProjectGroup_ID",Length=8,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public long? ProjectGroup_ID 
        { 
            get{return _ProjectGroup_ID;}            set{                SetProperty(ref _ProjectGroup_ID, value);                }
        }

        private int? _总销售出库数量;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "总销售出库数量",Length=4,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? 总销售出库数量 
        { 
            get{return _总销售出库数量;}            set{                SetProperty(ref _总销售出库数量, value);                }
        }

        private decimal? _出库成交金额;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "出库成交金额",Length=8,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? 出库成交金额 
        { 
            get{return _出库成交金额;}            set{                SetProperty(ref _出库成交金额, value);                }
        }

        private int? _退货数量;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "退货数量",Length=4,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? 退货数量 
        { 
            get{return _退货数量;}            set{                SetProperty(ref _退货数量, value);                }
        }

        private decimal? _退货金额;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "退货金额",Length=8,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? 退货金额 
        { 
            get{return _退货金额;}            set{                SetProperty(ref _退货金额, value);                }
        }

        private decimal? _销售税额;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "销售税额",Length=8,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? 销售税额 
        { 
            get{return _销售税额;}            set{                SetProperty(ref _销售税额, value);                }
        }

        private decimal? _退货税额;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "退货税额",Length=8,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? 退货税额 
        { 
            get{return _退货税额;}            set{                SetProperty(ref _退货税额, value);                }
        }

        private decimal? _佣金返点;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "佣金返点",Length=8,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? 佣金返点 
        { 
            get{return _佣金返点;}            set{                SetProperty(ref _佣金返点, value);                }
        }

        private decimal? _佣金返还;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "佣金返还",Length=8,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? 佣金返还 
        { 
            get{return _佣金返还;}            set{                SetProperty(ref _佣金返还, value);                }
        }

        private int? _实际成交数量;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "实际成交数量",Length=4,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public int? 实际成交数量 
        { 
            get{return _实际成交数量;}            set{                SetProperty(ref _实际成交数量, value);                }
        }

        private decimal? _实际成交金额;
        
        
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "实际成交金额",Length=8,IsNullable = true,ColumnDescription = "")]
        [Display(Name = "")]
        public decimal? 实际成交金额 
        { 
            get{return _实际成交金额;}            set{                SetProperty(ref _实际成交金额, value);                }
        }





  
        

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

