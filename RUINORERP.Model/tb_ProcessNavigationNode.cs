
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/24/2025 17:01:21
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
    /// 
    /// </summary>
    [Serializable()]
    [Description("")]
    [SugarTable("tb_ProcessNavigationNode")]
    public partial class tb_ProcessNavigationNode: BaseEntity, ICloneable
    {
        public tb_ProcessNavigationNode()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_ProcessNavigationNode" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _NodeID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "NodeID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true, IsIdentity = true)]
        public long NodeID
        { 
            get{return _NodeID;}
            set{
            SetProperty(ref _NodeID, value);
                base.PrimaryKeyID = _NodeID;
            }
        }

        private long _ProcessNavID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ProcessNavID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProcessNavID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ProcessNavigation","ProcessNavID")]
        public long ProcessNavID
        { 
            get{return _ProcessNavID;}
            set{
            SetProperty(ref _ProcessNavID, value);
                        }
        }

        private string _NodeCode;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "NodeCode",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "NodeCode" ,Length=100,IsNullable = true,ColumnDescription = "" )]
        public string NodeCode
        { 
            get{return _NodeCode;}
            set{
            SetProperty(ref _NodeCode, value);
                        }
        }

        private string _NodeName;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "NodeName",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "NodeName" ,Length=200,IsNullable = true,ColumnDescription = "" )]
        public string NodeName
        { 
            get{return _NodeName;}
            set{
            SetProperty(ref _NodeName, value);
                        }
        }

        private string _Description;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Description" ,Length=500,IsNullable = true,ColumnDescription = "" )]
        public string Description
        { 
            get{return _Description;}
            set{
            SetProperty(ref _Description, value);
                        }
        }

        private int _BusinessType= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "BusinessType",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BusinessType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        public int BusinessType
        { 
            get{return _BusinessType;}
            set{
            SetProperty(ref _BusinessType, value);
                        }
        }

        private long? _MenuID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "MenuID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "MenuID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_MenuInfo","MenuID")]
        public long? MenuID
        { 
            get{return _MenuID;}
            set{
            SetProperty(ref _MenuID, value);
                        }
        }

        private long? _ModuleID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ModuleID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ModuleDefinition","ModuleID")]
        public long? ModuleID
        { 
            get{return _ModuleID;}
            set{
            SetProperty(ref _ModuleID, value);
                        }
        }

        private long? _ChildNavigationID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ChildNavigationID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ChildNavigationID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_ProcessNavigation","ChildNavigationID")]
        public long? ChildNavigationID
        { 
            get{return _ChildNavigationID;}
            set{
            SetProperty(ref _ChildNavigationID, value);
                        }
        }

        private string _FormName;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "FormName",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "FormName" ,Length=200,IsNullable = true,ColumnDescription = "" )]
        public string FormName
        { 
            get{return _FormName;}
            set{
            SetProperty(ref _FormName, value);
                        }
        }

        private string _ClassPath;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "ClassPath",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ClassPath" ,Length=300,IsNullable = true,ColumnDescription = "" )]
        public string ClassPath
        { 
            get{return _ClassPath;}
            set{
            SetProperty(ref _ClassPath, value);
                        }
        }

        private string _NodeColor;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "NodeColor",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "NodeColor" ,Length=50,IsNullable = true,ColumnDescription = "" )]
        public string NodeColor
        { 
            get{return _NodeColor;}
            set{
            SetProperty(ref _NodeColor, value);
                        }
        }

        private float _PositionX= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PositionX",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "float", SqlParameterDbType ="Single",  ColumnName = "PositionX" ,IsNullable = false,ColumnDescription = "" )]
        public float PositionX
        { 
            get{return _PositionX;}
            set{
            SetProperty(ref _PositionX, value);
                        }
        }

        private float _PositionY= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PositionY",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "float", SqlParameterDbType ="Single",  ColumnName = "PositionY" ,IsNullable = false,ColumnDescription = "" )]
        public float PositionY
        { 
            get{return _PositionY;}
            set{
            SetProperty(ref _PositionY, value);
                        }
        }

        private float _Width= ((140));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Width",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "float", SqlParameterDbType ="Single",  ColumnName = "Width" ,IsNullable = false,ColumnDescription = "" )]
        public float Width
        { 
            get{return _Width;}
            set{
            SetProperty(ref _Width, value);
                        }
        }

        private float _Height= ((80));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Height",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "float", SqlParameterDbType ="Single",  ColumnName = "Height" ,IsNullable = false,ColumnDescription = "" )]
        public float Height
        { 
            get{return _Height;}
            set{
            SetProperty(ref _Height, value);
                        }
        }

        private string _NodeType;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "NodeType",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "NodeType" ,Length=50,IsNullable = true,ColumnDescription = "" )]
        public string NodeType
        { 
            get{return _NodeType;}
            set{
            SetProperty(ref _NodeType, value);
                        }
        }

        private int _SortOrder= ((0));
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "SortOrder",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "SortOrder" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" )]
        public int SortOrder
        { 
            get{return _SortOrder;}
            set{
            SetProperty(ref _SortOrder, value);
                        }
        }

        private DateTime _CreateTime;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "CreateTime",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "CreateTime" ,IsNullable = false,ColumnDescription = "" )]
        public DateTime CreateTime
        { 
            get{return _CreateTime;}
            set{
            SetProperty(ref _CreateTime, value);
                        }
        }

        private DateTime _UpdateTime;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "UpdateTime",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "UpdateTime" ,IsNullable = false,ColumnDescription = "" )]
        public DateTime UpdateTime
        { 
            get{return _UpdateTime;}
            set{
            SetProperty(ref _UpdateTime, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(MenuID))]
        public virtual tb_MenuInfo tb_menuinfo { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(ModuleID))]
        public virtual tb_ModuleDefinition tb_moduledefinition { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(ProcessNavID))]
        public virtual tb_ProcessNavigation tb_processnavigation { get; set; }

        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(ChildNavigationID))]
        public virtual tb_ProcessNavigation tb_processnavigationByChildNavigation { get; set; }



        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
         if("ProcessNavID"!="ChildNavigationID")
        {
        // rs=false;
        }
return rs;
}






       
        

        public override object Clone()
        {
            tb_ProcessNavigationNode loctype = (tb_ProcessNavigationNode)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

