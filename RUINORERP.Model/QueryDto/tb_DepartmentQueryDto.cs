
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/16/2025 11:47:56
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

namespace RUINORERP.Model.QueryDto
{
    /// <summary>
    /// 部门表是否分层
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Department")]
    public partial class tb_DepartmentQueryDto:BaseEntityDto
    {
        public tb_DepartmentQueryDto()
        {

        }

    
     

        private long _ID;
        /// <summary>
        /// 公司
        /// </summary>
        [AdvQueryAttribute(ColName = "ID",ColDesc = "公司")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "ID",IsNullable = false,ColumnDescription = "公司" )]
        [FKRelationAttribute("tb_Company","ID")]
        public long ID 
        { 
            get{return _ID;}
            set{SetProperty(ref _ID, value);}
        }
     

        private string _DepartmentCode;
        /// <summary>
        /// 部门代号
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentCode",ColDesc = "部门代号")]
        [SugarColumn(ColumnDataType = "char",SqlParameterDbType ="String",ColumnName = "DepartmentCode",Length=20,IsNullable = false,ColumnDescription = "部门代号" )]
        public string DepartmentCode 
        { 
            get{return _DepartmentCode;}
            set{SetProperty(ref _DepartmentCode, value);}
        }
     

        private string _DepartmentName;
        /// <summary>
        /// 部门名称
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentName",ColDesc = "部门名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "DepartmentName",Length=255,IsNullable = false,ColumnDescription = "部门名称" )]
        public string DepartmentName 
        { 
            get{return _DepartmentName;}
            set{SetProperty(ref _DepartmentName, value);}
        }
     

        private string _TEL;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "TEL",ColDesc = "电话")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "TEL",Length=20,IsNullable = true,ColumnDescription = "电话" )]
        public string TEL 
        { 
            get{return _TEL;}
            set{SetProperty(ref _TEL, value);}
        }
     

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Notes",Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes 
        { 
            get{return _Notes;}
            set{SetProperty(ref _Notes, value);}
        }
     

        private string _Director;
        /// <summary>
        /// 责任人
        /// </summary>
        [AdvQueryAttribute(ColName = "Director",ColDesc = "责任人")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Director",Length=20,IsNullable = true,ColumnDescription = "责任人" )]
        public string Director 
        { 
            get{return _Director;}
            set{SetProperty(ref _Director, value);}
        }


       
    }
}



