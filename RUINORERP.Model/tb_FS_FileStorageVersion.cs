
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/25/2025 15:32:20
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
    /// 文件版本表
    /// </summary>
    [Serializable()]
    [Description("文件版本表")]
    [SugarTable("tb_FS_FileStorageVersion")]
    public partial class tb_FS_FileStorageVersion: BaseEntity, ICloneable
    {
        public tb_FS_FileStorageVersion()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("文件版本表tb_FS_FileStorageVersion" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _VersionId;
        /// <summary>
        /// 版本ID
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "VersionId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "版本ID" , IsPrimaryKey = true)]
        public long VersionId
        { 
            get{return _VersionId;}
            set{
            SetProperty(ref _VersionId, value);
                base.PrimaryKeyID = _VersionId;
            }
        }

        private long? _FileId;
        /// <summary>
        /// 文件ID
        /// </summary>
        [AdvQueryAttribute(ColName = "FileId",ColDesc = "文件ID")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "FileId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "文件ID" )]
        [FKRelationAttribute("tb_FS_FileStorageInfo","FileId")]
        public long? FileId
        { 
            get{return _FileId;}
            set{
            SetProperty(ref _FileId, value);
                        }
        }

        private int _VersionNo;
        /// <summary>
        /// 版本号
        /// </summary>
        [AdvQueryAttribute(ColName = "VersionNo",ColDesc = "版本号")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "VersionNo" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "版本号" , IsIdentity = true)]
        public int VersionNo
        { 
            get{return _VersionNo;}
            set{
            SetProperty(ref _VersionNo, value);
                        }
        }

        private string _StorageFileName;
        /// <summary>
        /// 存储文件名
        /// </summary>
        [AdvQueryAttribute(ColName = "StorageFileName",ColDesc = "存储文件名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "StorageFileName" ,Length=255,IsNullable = false,ColumnDescription = "存储文件名" )]
        public string StorageFileName
        { 
            get{return _StorageFileName;}
            set{
            SetProperty(ref _StorageFileName, value);
                        }
        }

        private string _UpdateReason;
        /// <summary>
        /// 存储路径
        /// </summary>
        [AdvQueryAttribute(ColName = "UpdateReason",ColDesc = "存储路径")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "UpdateReason" ,Length=300,IsNullable = false,ColumnDescription = "存储路径" )]
        public string UpdateReason
        { 
            get{return _UpdateReason;}
            set{
            SetProperty(ref _UpdateReason, value);
                        }
        }

        private string _HashValue;
        /// <summary>
        /// 文件哈希值
        /// </summary>
        [AdvQueryAttribute(ColName = "HashValue",ColDesc = "文件哈希值")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "HashValue" ,Length=64,IsNullable = true,ColumnDescription = "文件哈希值" )]
        public string HashValue
        { 
            get{return _HashValue;}
            set{
            SetProperty(ref _HashValue, value);
                        }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at
        { 
            get{return _Modified_at;}
            set{
            SetProperty(ref _Modified_at, value);
                        }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by
        { 
            get{return _Modified_by;}
            set{
            SetProperty(ref _Modified_by, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(FileId))]
        public virtual tb_FS_FileStorageInfo tb_fs_filestorageinfo { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_FS_FileStorageVersion loctype = (tb_FS_FileStorageVersion)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

