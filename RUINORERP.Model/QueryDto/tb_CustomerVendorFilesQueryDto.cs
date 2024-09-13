
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:34
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
    /// 客户厂商认证文件表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_CustomerVendorFiles")]
    public partial class tb_CustomerVendorFilesQueryDto:BaseEntityDto
    {
        public tb_CustomerVendorFilesQueryDto()
        {

        }

    
     

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "CustomerVendor_ID",IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID 
        { 
            get{return _CustomerVendor_ID;}
            set{SetProperty(ref _CustomerVendor_ID, value);}
        }
     

        private string _FileName;
        /// <summary>
        /// 文件名
        /// </summary>
        [AdvQueryAttribute(ColName = "FileName",ColDesc = "文件名")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "FileName",Length=200,IsNullable = true,ColumnDescription = "文件名" )]
        public string FileName 
        { 
            get{return _FileName;}
            set{SetProperty(ref _FileName, value);}
        }
     

        private string _FileType;
        /// <summary>
        /// 文件类型
        /// </summary>
        [AdvQueryAttribute(ColName = "FileType",ColDesc = "文件类型")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "FileType",Length=50,IsNullable = true,ColumnDescription = "文件类型" )]
        public string FileType 
        { 
            get{return _FileType;}
            set{SetProperty(ref _FileType, value);}
        }


       
    }
}



