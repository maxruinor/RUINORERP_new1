
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
    /// 文件信息元数据表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_FS_FileStorageInfo")]
    public partial class tb_FS_FileStorageInfoQueryDto:BaseEntityDto
    {
        public tb_FS_FileStorageInfoQueryDto()
        {

        }

    
     

        private string _OriginalFileName;
        /// <summary>
        /// 原始文件名
        /// </summary>
        [AdvQueryAttribute(ColName = "OriginalFileName",ColDesc = "原始文件名")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "OriginalFileName",Length=255,IsNullable = false,ColumnDescription = "原始文件名" )]
        public string OriginalFileName 
        { 
            get{return _OriginalFileName;}
            set{SetProperty(ref _OriginalFileName, value);}
        }
     

        private string _StorageFileName;
        /// <summary>
        /// 存储文件名
        /// </summary>
        [AdvQueryAttribute(ColName = "StorageFileName",ColDesc = "存储文件名")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "StorageFileName",Length=255,IsNullable = false,ColumnDescription = "存储文件名" )]
        public string StorageFileName 
        { 
            get{return _StorageFileName;}
            set{SetProperty(ref _StorageFileName, value);}
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
     

        private string _FileType;
        /// <summary>
        /// 文件类型
        /// </summary>
        [AdvQueryAttribute(ColName = "FileType",ColDesc = "文件类型")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "FileType",Length=200,IsNullable = false,ColumnDescription = "文件类型" )]
        public string FileType 
        { 
            get{return _FileType;}
            set{SetProperty(ref _FileType, value);}
        }
     

        private long _FileSize;
        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        [AdvQueryAttribute(ColName = "FileSize",ColDesc = "文件大小（字节）")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "FileSize",IsNullable = false,ColumnDescription = "文件大小（字节）" )]
        public long FileSize 
        { 
            get{return _FileSize;}
            set{SetProperty(ref _FileSize, value);}
        }
     

        private string _HashValue;
        /// <summary>
        /// 文件哈希值
        /// </summary>
        [AdvQueryAttribute(ColName = "HashValue",ColDesc = "文件哈希值")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "HashValue",Length=64,IsNullable = true,ColumnDescription = "文件哈希值" )]
        public string HashValue 
        { 
            get{return _HashValue;}
            set{SetProperty(ref _HashValue, value);}
        }
     

        private string _StorageProvider;
        /// <summary>
        /// 存储引擎
        /// </summary>
        [AdvQueryAttribute(ColName = "StorageProvider",ColDesc = "存储引擎")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "StorageProvider",Length=50,IsNullable = false,ColumnDescription = "存储引擎" )]
        public string StorageProvider 
        { 
            get{return _StorageProvider;}
            set{SetProperty(ref _StorageProvider, value);}
        }
     

        private string _StoragePath;
        /// <summary>
        /// 存储路径
        /// </summary>
        [AdvQueryAttribute(ColName = "StoragePath",ColDesc = "存储路径")]
        [SugarColumn(ColumnDataType = "nvarchar",SqlParameterDbType ="String",ColumnName = "StoragePath",Length=300,IsNullable = false,ColumnDescription = "存储路径" )]
        public string StoragePath 
        { 
            get{return _StoragePath;}
            set{SetProperty(ref _StoragePath, value);}
        }
     

        private int _CurrentVersion;
        /// <summary>
        /// 版本号
        /// </summary>
        [AdvQueryAttribute(ColName = "CurrentVersion",ColDesc = "版本号")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "CurrentVersion",IsNullable = false,ColumnDescription = "版本号" )]
        public int CurrentVersion 
        { 
            get{return _CurrentVersion;}
            set{SetProperty(ref _CurrentVersion, value);}
        }
     

        private int _Status= ((0));
        /// <summary>
        /// 文件状态
        /// </summary>
        [AdvQueryAttribute(ColName = "Status",ColDesc = "文件状态")]
        [SugarColumn(ColumnDataType = "int",SqlParameterDbType ="Int32",ColumnName = "Status",IsNullable = false,ColumnDescription = "文件状态" )]
        public int Status 
        { 
            get{return _Status;}
            set{SetProperty(ref _Status, value);}
        }
     

        private DateTime _ExpireTime;
        /// <summary>
        /// 过期时间
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpireTime",ColDesc = "过期时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "ExpireTime",IsNullable = false,ColumnDescription = "过期时间" )]
        public DateTime ExpireTime 
        { 
            get{return _ExpireTime;}
            set{SetProperty(ref _ExpireTime, value);}
        }
     

        private bool _IsRegistered;
        /// <summary>
        /// 已注册
        /// </summary>
        [AdvQueryAttribute(ColName = "IsRegistered",ColDesc = "已注册")]
        [SugarColumn(ColumnDataType = "bit",SqlParameterDbType ="Boolean",ColumnName = "IsRegistered",IsNullable = false,ColumnDescription = "已注册" )]
        public bool IsRegistered 
        { 
            get{return _IsRegistered;}
            set{SetProperty(ref _IsRegistered, value);}
        }
     

        private string _Description;
        /// <summary>
        /// 文件描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "文件描述")]
        [SugarColumn(ColumnDataType = "varchar",SqlParameterDbType ="String",ColumnName = "Description",Length=200,IsNullable = true,ColumnDescription = "文件描述" )]
        public string Description 
        { 
            get{return _Description;}
            set{SetProperty(ref _Description, value);}
        }
     

        private DateTime? _Created_at= '(getdate())';
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Created_at",IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at 
        { 
            get{return _Created_at;}
            set{SetProperty(ref _Created_at, value);}
        }
     

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Created_by",IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by 
        { 
            get{return _Created_by;}
            set{SetProperty(ref _Created_by, value);}
        }
     

        private DateTime? _Modified_at= '(getdate())';
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime",SqlParameterDbType ="DateTime",ColumnName = "Modified_at",IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at 
        { 
            get{return _Modified_at;}
            set{SetProperty(ref _Modified_at, value);}
        }
     

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint",SqlParameterDbType ="Int64",ColumnName = "Modified_by",IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by 
        { 
            get{return _Modified_by;}
            set{SetProperty(ref _Modified_by, value);}
        }
     

        private string _Metadata;
        /// <summary>
        /// 扩展元数据
        /// </summary>
        [AdvQueryAttribute(ColName = "Metadata",ColDesc = "扩展元数据")]
        [SugarColumn(ColumnDataType = "text",SqlParameterDbType ="String",ColumnName = "Metadata",Length=2147483647,IsNullable = true,ColumnDescription = "扩展元数据" )]
        public string Metadata 
        { 
            get{return _Metadata;}
            set{SetProperty(ref _Metadata, value);}
        }


       
    }
}



