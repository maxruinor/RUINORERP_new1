
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2024 19:35:41
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 处理销售订单的数据统计时，对存储过程提供参数的情况。手动创建的
    /// </summary>
    public class Proc_SaleOrderStatisticsByEmployeePara : BaseEntity, ICloneable
    {
        private DateTime _Start;

        /// <summary>
        /// 出库日期起
        /// </summary>
        [AdvQueryAttribute(ColName = "Start", ColDesc = "订单日期起")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Start", IsNullable = false, ColumnDescription = "订单日期起")]
        public DateTime Start
        {
            get { return _Start; }
            set
            {
                SetProperty(ref _Start, value);
            }
        }


        private DateTime _End;
        /// <summary>
        /// 出库日期止
        /// </summary>
        [AdvQueryAttribute(ColName = "End", ColDesc = "订单日期止")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "End", IsNullable = false, ColumnDescription = "订单日期止")]
        public DateTime End
        {
            get { return _End; }
            set
            {
                SetProperty(ref _End, value);
            }
        }

        private long? _Employee_ID;
        /// <summary>
        /// 业务员
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID", ColDesc = "业务员")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Employee_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "业务员")]
        [FKRelationAttribute("tb_Employee", "Employee_ID", true)]
        public long? Employee_ID
        {
            get { return _Employee_ID; }
            set
            {
                SetProperty(ref _Employee_ID, value);
            }
        }

        private long? _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID", ColDesc = "项目组")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "ProjectGroup_ID", DecimalDigits = 0, IsNullable = true, ColumnDescription = "项目组")]
        [FKRelationAttribute("tb_ProjectGroup", "ProjectGroup_ID", true)]
        public long? ProjectGroup_ID
        {
            get { return _ProjectGroup_ID; }
            set
            {
                SetProperty(ref _ProjectGroup_ID, value);
            }
        }


        //查询UI上绑定下拉关联时用到

        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }


        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }
    }

}

