
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:47
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 流程图定义
    /// </summary>
    [Serializable()]
    [Description("流程图定义")]
    [SugarTable("tb_FlowchartDefinition")]
    public partial class tb_FlowchartDefinition: BaseEntity, ICloneable
    {
        public tb_FlowchartDefinition()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("流程图定义tb_FlowchartDefinition" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ID;
        /// <summary>
        /// 流程图
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "流程图" , IsPrimaryKey = true)]
        public long ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                base.PrimaryKeyID = _ID;
            }
        }

        private long? _ModuleID;
        /// <summary>
        /// 模块
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleID",ColDesc = "模块")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ModuleID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "模块" )]
        [FKRelationAttribute("tb_ModuleDefinition","ModuleID")]
        public long? ModuleID
        { 
            get{return _ModuleID;}
            set{
            SetProperty(ref _ModuleID, value);
                        }
        }

        private string _FlowchartNo;
        /// <summary>
        /// 流程图编号
        /// </summary>
        [AdvQueryAttribute(ColName = "FlowchartNo",ColDesc = "流程图编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FlowchartNo" ,Length=50,IsNullable = false,ColumnDescription = "流程图编号" )]
        public string FlowchartNo
        { 
            get{return _FlowchartNo;}
            set{
            SetProperty(ref _FlowchartNo, value);
                        }
        }

        private string _FlowchartName;
        /// <summary>
        /// 流程图名称
        /// </summary>
        [AdvQueryAttribute(ColName = "FlowchartName",ColDesc = "流程图名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FlowchartName" ,Length=20,IsNullable = false,ColumnDescription = "流程图名称" )]
        public string FlowchartName
        { 
            get{return _FlowchartName;}
            set{
            SetProperty(ref _FlowchartName, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ModuleID))]
        public virtual tb_ModuleDefinition tb_moduledefinition { get; set; }


        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FlowchartItem.ID))]
        public virtual List<tb_FlowchartItem> tb_FlowchartItems { get; set; }
        //tb_FlowchartItem.ID)
        //ID.FK_TB_FLOWC_REF_TB_FLOWCTION)
        //tb_FlowchartDefinition.ID)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_FlowchartLine.ID))]
        public virtual List<tb_FlowchartLine> tb_FlowchartLines { get; set; }
        //tb_FlowchartLine.ID)
        //ID.FK_TB_FLOWC_REFFLOWCLINE)
        //tb_FlowchartDefinition.ID)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_FlowchartDefinition loctype = (tb_FlowchartDefinition)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

