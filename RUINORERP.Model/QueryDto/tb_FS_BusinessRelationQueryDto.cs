
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/25/2025 15:32:19
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
    /// 文件业务关联表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FS_BusinessRelation")]
    public partial class tb_FS_BusinessRelationQueryDto:BaseEntityDto
    {
        public tb_FS_BusinessRelationQueryDto()
        {

        }

    
     

        private long? _FileId;
        /// <summary>
        /// 文件ID
        /// </summary>
        [AdvQueryAttribute(ColName = "FileId",ColDesc = "文件ID")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "FileId",IsNullable = true,ColumnDescription = "文件ID" )]
        [FKRelationAttribute("tb_FS_FileStorageInfo","FileId")]
        public long? FileId 
        { 
            get{return _FileId;}
            set{SetProperty(ref _FileId, value);}
        }
     

        private int? _BusinessType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BusinessType",ColDesc = "业务类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "BusinessType",IsNullable = true,ColumnDescription = "业务类型" )]
        public int? BusinessType 
        { 
            get{return _BusinessType;}
            set{SetProperty(ref _BusinessType, value);}
        }
     

        private string _BusinessNo;
        /// <summary>
        /// 业务编号
        /// </summary>
        [AdvQueryAttribute(ColName = "BusinessNo",ColDesc = "业务编号")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "BusinessNo",Length=50,IsNullable = true,ColumnDescription = "业务编号" )]
        public string BusinessNo 
        { 
            get{return _BusinessNo;}
            set{SetProperty(ref _BusinessNo, value);}
        }
     

        private bool _IsMainFile;
        /// <summary>
        /// 已注册
        /// </summary>
        [AdvQueryAttribute(ColName = "IsMainFile",ColDesc = "已注册")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsMainFile",IsNullable = false,ColumnDescription = "已注册" )]
        public bool IsMainFile 
        { 
            get{return _IsMainFile;}
            set{SetProperty(ref _IsMainFile, value);}
        }


       
    }
}



