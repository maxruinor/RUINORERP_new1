
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:03
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
    /// 打印模板
    /// </summary>
    [Serializable()]
    [SugarTable("tb_PrintTemplate")]
    public partial class tb_PrintTemplateQueryDto:BaseEntityDto
    {
        public tb_PrintTemplateQueryDto()
        {

        }

    
     

        private long? _PrintConfigID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintConfigID",ColDesc = "")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "PrintConfigID",IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_PrintConfig","PrintConfigID")]
        public long? PrintConfigID 
        { 
            get{return _PrintConfigID;}
            set{SetProperty(ref _PrintConfigID, value);}
        }
     

        private string _Template_NO;
        /// <summary>
        /// 模板编号
        /// </summary>
        [AdvQueryAttribute(ColName = "Template_NO",ColDesc = "模板编号")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Template_NO",Length=20,IsNullable = true,ColumnDescription = "模板编号" )]
        public string Template_NO 
        { 
            get{return _Template_NO;}
            set{SetProperty(ref _Template_NO, value);}
        }
     

        private string _Template_Name;
        /// <summary>
        /// 模板名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Template_Name",ColDesc = "模板名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Template_Name",Length=100,IsNullable = true,ColumnDescription = "模板名称" )]
        public string Template_Name 
        { 
            get{return _Template_Name;}
            set{SetProperty(ref _Template_Name, value);}
        }
     

        private int? _BizType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "业务类型")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "BizType",IsNullable = true,ColumnDescription = "业务类型" )]
        public int? BizType 
        { 
            get{return _BizType;}
            set{SetProperty(ref _BizType, value);}
        }
     

        private string _BizName;
        /// <summary>
        /// 业务名称
        /// </summary>
        [AdvQueryAttribute(ColName = "BizName",ColDesc = "业务名称")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "BizName",Length=30,IsNullable = true,ColumnDescription = "业务名称" )]
        public string BizName 
        { 
            get{return _BizName;}
            set{SetProperty(ref _BizName, value);}
        }
     

        private string _Templatet_Path;
        /// <summary>
        /// 模板路径
        /// </summary>
        [AdvQueryAttribute(ColName = "Templatet_Path",ColDesc = "模板路径")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Templatet_Path",Length=200,IsNullable = true,ColumnDescription = "模板路径" )]
        public string Templatet_Path 
        { 
            get{return _Templatet_Path;}
            set{SetProperty(ref _Templatet_Path, value);}
        }
     

        private string _Template_DataSource;
        /// <summary>
        /// 模板数据源
        /// </summary>
        [AdvQueryAttribute(ColName = "Template_DataSource",ColDesc = "模板数据源")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Template_DataSource",Length=200,IsNullable = true,ColumnDescription = "模板数据源" )]
        public string Template_DataSource 
        { 
            get{return _Template_DataSource;}
            set{SetProperty(ref _Template_DataSource, value);}
        }
     

        private string _TemplateFileData;
        /// <summary>
        /// 模板文件数据
        /// </summary>
        [AdvQueryAttribute(ColName = "TemplateFileData",ColDesc = "模板文件数据")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "TemplateFileData",Length=2147483647,IsNullable = false,ColumnDescription = "模板文件数据" )]
        public string TemplateFileData 
        { 
            get{return _TemplateFileData;}
            set{SetProperty(ref _TemplateFileData, value);}
        }
     

        private byte[] _TemplateFileStream;
        /// <summary>
        /// 模板流数据
        /// </summary>
        [AdvQueryAttribute(ColName = "TemplateFileStream",ColDesc = "模板流数据")]
        [SugarColumn(ColumnDataType = "varbinary",SqlParameterDbType ="Binary",ColumnName = "TemplateFileStream",Length=-1,IsNullable = true,ColumnDescription = "模板流数据" )]
        public byte[] TemplateFileStream 
        { 
            get{return _TemplateFileStream;}
            set{SetProperty(ref _TemplateFileStream, value);}
        }
     

        private bool? _IsDefaultTemplate= false;
        /// <summary>
        /// 默认模板
        /// </summary>
        [AdvQueryAttribute(ColName = "IsDefaultTemplate",ColDesc = "默认模板")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsDefaultTemplate",IsNullable = true,ColumnDescription = "默认模板" )]
        public bool? IsDefaultTemplate 
        { 
            get{return _IsDefaultTemplate;}
            set{SetProperty(ref _IsDefaultTemplate, value);}
        }


       
    }
}



