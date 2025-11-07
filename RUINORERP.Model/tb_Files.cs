
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:46
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
    /// 文档表
    /// </summary>
    [Serializable()]
    [Description("文档表")]
    [SugarTable("tb_Files")]
    public partial class tb_Files: BaseEntity, ICloneable
    {
        public tb_Files()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("文档表tb_Files" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Doc_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Doc_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Doc_ID
        { 
            get{return _Doc_ID;}
            set{
            SetProperty(ref _Doc_ID, value);
                base.PrimaryKeyID = _Doc_ID;
            }
        }

        private string _Files_Path;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Files_Path",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Files_Path" ,Length=800,IsNullable = true,ColumnDescription = "" )]
        public string Files_Path
        { 
            get{return _Files_Path;}
            set{
            SetProperty(ref _Files_Path, value);
                        }
        }

        private string _FileName;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "FileName",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FileName" ,Length=100,IsNullable = true,ColumnDescription = "" )]
        public string FileName
        { 
            get{return _FileName;}
            set{
            SetProperty(ref _FileName, value);
                        }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_S.Doc_ID))]
        public virtual List<tb_BOM_S> tb_BOM_Ss { get; set; }
        //tb_BOM_S.Doc_ID)
        //Doc_ID.FK_BOMS_FILES_1)
        //tb_Files.Doc_ID)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_Files loctype = (tb_Files)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

