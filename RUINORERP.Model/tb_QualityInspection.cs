
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:13
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
    /// 质检表
    /// </summary>
    [Serializable()]
    [Description("质检表")]
    [SugarTable("tb_QualityInspection")]
    public partial class tb_QualityInspection: BaseEntity, ICloneable
    {
        public tb_QualityInspection()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("质检表tb_QualityInspection" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _InspectionID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "InspectionID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long InspectionID
        { 
            get{return _InspectionID;}
            set{
            SetProperty(ref _InspectionID, value);
                base.PrimaryKeyID = _InspectionID;
            }
        }

        private DateTime? _InspectionDate;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "InspectionDate",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "InspectionDate" ,IsNullable = true,ColumnDescription = "" )]
        public DateTime? InspectionDate
        { 
            get{return _InspectionDate;}
            set{
            SetProperty(ref _InspectionDate, value);
                        }
        }

        private string _InspectionResult;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "InspectionResult",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "InspectionResult" ,Length=500,IsNullable = true,ColumnDescription = "" )]
        public string InspectionResult
        { 
            get{return _InspectionResult;}
            set{
            SetProperty(ref _InspectionResult, value);
                        }
        }

        private int? _ProductID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ProductID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "ProductID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        public int? ProductID
        { 
            get{return _ProductID;}
            set{
            SetProperty(ref _ProductID, value);
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
            tb_QualityInspection loctype = (tb_QualityInspection)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

