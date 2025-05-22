
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
    /// 流程图线
    /// </summary>
    [Serializable()]
    [Description("流程图线")]
    [SugarTable("tb_FlowchartLine")]
    public partial class tb_FlowchartLine: BaseEntity, ICloneable
    {
        public tb_FlowchartLine()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("流程图线tb_FlowchartLine" + "外键ID与对应主主键名称不一致。请修改数据库");
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

        private string _PointToString1;
        /// <summary>
        /// 大小
        /// </summary>
        [AdvQueryAttribute(ColName = "PointToString1",ColDesc = "大小")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PointToString1" ,Length=100,IsNullable = true,ColumnDescription = "大小" )]
        public string PointToString1
        { 
            get{return _PointToString1;}
            set{
            SetProperty(ref _PointToString1, value);
                        }
        }

        private string _PointToString2;
        /// <summary>
        /// 位置
        /// </summary>
        [AdvQueryAttribute(ColName = "PointToString2",ColDesc = "位置")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PointToString2" ,Length=100,IsNullable = true,ColumnDescription = "位置" )]
        public string PointToString2
        { 
            get{return _PointToString2;}
            set{
            SetProperty(ref _PointToString2, value);
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
            tb_FlowchartLine loctype = (tb_FlowchartLine)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

