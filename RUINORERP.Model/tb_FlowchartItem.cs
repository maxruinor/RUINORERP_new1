﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:59
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
    /// 流程图子项
    /// </summary>
    [Serializable()]
    [Description("流程图子项")]
    [SugarTable("tb_FlowchartItem")]
    public partial class tb_FlowchartItem: BaseEntity, ICloneable
    {
        public tb_FlowchartItem()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("流程图子项tb_FlowchartItem" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        [FKRelationAttribute("tb_FlowchartDefinition","ID")]
        public long ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                base.PrimaryKeyID = _ID;
            }
        }

        private string _IconFile_Path;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "IconFile_Path",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "IconFile_Path" ,Length=500,IsNullable = true,ColumnDescription = "" )]
        public string IconFile_Path
        { 
            get{return _IconFile_Path;}
            set{
            SetProperty(ref _IconFile_Path, value);
                        }
        }

        private string _Title;
        /// <summary>
        /// 标题
        /// </summary>
        [AdvQueryAttribute(ColName = "Title",ColDesc = "标题")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Title" ,Length=100,IsNullable = true,ColumnDescription = "标题" )]
        public string Title
        { 
            get{return _Title;}
            set{
            SetProperty(ref _Title, value);
                        }
        }

        private string _SizeString;
        /// <summary>
        /// 大小
        /// </summary>
        [AdvQueryAttribute(ColName = "SizeString",ColDesc = "大小")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SizeString" ,Length=100,IsNullable = true,ColumnDescription = "大小" )]
        public string SizeString
        { 
            get{return _SizeString;}
            set{
            SetProperty(ref _SizeString, value);
                        }
        }

        private string _PointToString;
        /// <summary>
        /// 位置
        /// </summary>
        [AdvQueryAttribute(ColName = "PointToString",ColDesc = "位置")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PointToString" ,Length=100,IsNullable = true,ColumnDescription = "位置" )]
        public string PointToString
        { 
            get{return _PointToString;}
            set{
            SetProperty(ref _PointToString, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ID))]
        public virtual tb_FlowchartDefinition tb_flowchartdefinition { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}




        public override object Clone()
        {
            tb_FlowchartItem loctype = (tb_FlowchartItem)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

