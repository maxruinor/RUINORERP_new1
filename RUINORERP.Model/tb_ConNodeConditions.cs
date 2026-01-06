
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:42
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
    /// 流程步骤 为转移条件集合，Field为条件左参数，Operator为操作操作符如果值类型为String则表达式只能为==或者!=，Value为表达式值
    /// </summary>
    [Serializable()]
    [Description("流程步骤")]
    [SugarTable("tb_ConNodeConditions")]
    public partial class tb_ConNodeConditions: BaseEntity, ICloneable
    {
        public tb_ConNodeConditions()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("流程步骤 为转移条件集合，Field为条件左参数，Operator为操作操作符如果值类型为String则表达式只能为==或者!=，Value为表达式值tb_ConNodeConditions" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ConNodeConditions_Id;
        /// <summary>
        /// 条件
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ConNodeConditions_Id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "条件" , IsPrimaryKey = true)]
        public long ConNodeConditions_Id
        { 
            get{return _ConNodeConditions_Id;}
            set{
            SetProperty(ref _ConNodeConditions_Id, value);
                base.PrimaryKeyID = _ConNodeConditions_Id;
            }
        }

        private string _Field;
        /// <summary>
        /// 表达式
        /// </summary>
        [AdvQueryAttribute(ColName = "Field",ColDesc = "表达式")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Field" ,Length=50,IsNullable = false,ColumnDescription = "表达式" )]
        public string Field
        { 
            get{return _Field;}
            set{
            SetProperty(ref _Field, value);
                        }
        }

        private string _Operator;
        /// <summary>
        /// 操作符
        /// </summary>
        [AdvQueryAttribute(ColName = "Operator",ColDesc = "操作符")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Operator" ,Length=50,IsNullable = true,ColumnDescription = "操作符" )]
        public string Operator
        { 
            get{return _Operator;}
            set{
            SetProperty(ref _Operator, value);
                        }
        }

        private string _Value;
        /// <summary>
        /// 表达式值
        /// </summary>
        [AdvQueryAttribute(ColName = "Value",ColDesc = "表达式值")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Value" ,Length=50,IsNullable = true,ColumnDescription = "表达式值" )]
        public string Value
        { 
            get{return _Value;}
            set{
            SetProperty(ref _Value, value);
                        }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_NextNodes.ConNodeConditions_Id))]
        public virtual List<tb_NextNodes> tb_NextNodeses { get; set; }
        //tb_NextNodes.ConNodeConditions_Id)
        //ConNodeConditions_Id.FK_TB_NEXTN_REFERENCE_TB_CONNO)
        //tb_ConNodeConditions.ConNodeConditions_Id)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_ConNodeConditions loctype = (tb_ConNodeConditions)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

