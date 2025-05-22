
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:52
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
    /// 单据标识 保存在主单中一个字段，作用于各种单明细的搜索过滤 有必要吗？
    /// </summary>
    [Serializable()]
    [Description("单据标识 保存在主单中一个字段，作用于各种单明细的搜索过滤 有必要吗？")]
    [SugarTable("tb_BillMarking")]
    public partial class tb_BillMarking: BaseEntity, ICloneable
    {
        public tb_BillMarking()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("单据标识 保存在主单中一个字段，作用于各种单明细的搜索过滤 有必要吗？tb_BillMarking" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _BMType_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BMType_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long BMType_ID
        { 
            get{return _BMType_ID;}
            set{
            SetProperty(ref _BMType_ID, value);
                base.PrimaryKeyID = _BMType_ID;
            }
        }

        private string _TypeName;
        /// <summary>
        /// 类型名称
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeName",ColDesc = "类型名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TypeName" ,Length=50,IsNullable = false,ColumnDescription = "类型名称" )]
        public string TypeName
        { 
            get{return _TypeName;}
            set{
            SetProperty(ref _TypeName, value);
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


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}




 

        public override object Clone()
        {
            tb_BillMarking loctype = (tb_BillMarking)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

