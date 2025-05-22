
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:57
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
    /// 客户厂商认证文件表
    /// </summary>
    [Serializable()]
    [Description("客户厂商认证文件表")]
    [SugarTable("tb_CustomerVendorFiles")]
    public partial class tb_CustomerVendorFiles: BaseEntity, ICloneable
    {
        public tb_CustomerVendorFiles()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("客户厂商认证文件表tb_CustomerVendorFiles" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _File_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "File_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long File_ID
        { 
            get{return _File_ID;}
            set{
            SetProperty(ref _File_ID, value);
                base.PrimaryKeyID = _File_ID;
            }
        }

        private long? _CustomerVendor_ID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerVendor_ID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CustomerVendor_ID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_CustomerVendor","CustomerVendor_ID")]
        public long? CustomerVendor_ID
        { 
            get{return _CustomerVendor_ID;}
            set{
            SetProperty(ref _CustomerVendor_ID, value);
                        }
        }

        private string _FileName;
        /// <summary>
        /// 文件名
        /// </summary>
        [AdvQueryAttribute(ColName = "FileName",ColDesc = "文件名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FileName" ,Length=200,IsNullable = true,ColumnDescription = "文件名" )]
        public string FileName
        { 
            get{return _FileName;}
            set{
            SetProperty(ref _FileName, value);
                        }
        }

        private string _FileType;
        /// <summary>
        /// 文件类型
        /// </summary>
        [AdvQueryAttribute(ColName = "FileType",ColDesc = "文件类型")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FileType" ,Length=50,IsNullable = true,ColumnDescription = "文件类型" )]
        public string FileType
        { 
            get{return _FileType;}
            set{
            SetProperty(ref _FileType, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(CustomerVendor_ID))]
        public virtual tb_CustomerVendor tb_customervendor { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}




        public override object Clone()
        {
            tb_CustomerVendorFiles loctype = (tb_CustomerVendorFiles)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

