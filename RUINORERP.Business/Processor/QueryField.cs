using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace RUINORERP.Business.Processor
{
    /// <summary>
    /// 查询字段
    /// 系统对一个表或视图查询时，需要查询的字段,以及查询条件,以及查询的目标
    /// 2024-7-27进一步完善
    /// 一个字段是否可以通过另一个实体来查询，是的话要指定相同的查询KEY
    /// </summary>
    public class QueryField
    {
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayIndex { get; set; }

        Type colDataType;
        /// <summary>
        /// 列的数据类型,比方  order_id 是 long 
        /// </summary>
        public Type ColDataType { get => colDataType; set => colDataType = value; }
        //为了统一掉BaseDtoField  这部分暂时有选择性移到这里
        /// <summary>
        /// 提取的列信息时保存起来，防后面要用到
        /// </summary>
        public SugarColumn SugarCol { get; set; } = new SugarColumn();

        #region  有子条件 外键情况时才有值的字段

        /// <summary>
        ///  外键关联属性,（生成的，依赖于DB的关联） 直接保存特性更简单？
        /// </summary>
        public FKRelationAttribute fKRelationAttribute { get; set; }

        bool isRelated;
        /// <summary>
        /// 是外键关联的查询条件,一般是关联表的主键，这时也会有子过滤条件
        /// </summary>
        public bool IsRelated { get => isRelated; set => isRelated = value; }


        //在处理关联查询时，要在上层知道下层的查询实体的名称，即：外键表名

        /// <summary>
        /// 外键表名
        /// </summary>
        public string FKTableName { get; set; }


        /// <summary>
        /// 处理过程中要提前知道子过滤条件的对象的类型
        /// 相关外键表或实体查询实体。如果是视图传入T时起作用
        /// 如：传入查询条件时。如果是相关的实体字段，则要有标记为外键特性。表才有。视图手动添加？如果在条件中加，不好。另一个功能使用也要加,
        /// 视图手动加，生成会覆盖
        /// </summary>
        public Type SubQueryTargetType { get; set; } = null;


        #endregion

        List<AdvExtQueryAttribute> extendedAttribute = new List<AdvExtQueryAttribute>();

        /// <summary>
        /// 对应列的扩展特性 用于模糊搜索时动态添加的数据
        /// 意思是：比方时间 ，会变成一个区别。一个起。一个止。这时就要保存起来这个特性
        /// </summary>
        public List<AdvExtQueryAttribute> ExtendedAttribute { get => extendedAttribute; set => extendedAttribute = value; }



        //=======
        /// <summary>
        /// 当前查询目标实体的类型,通常第一个层级是T，就是要查询的实体本身。
        /// 是子条件的查询字段的话，首先来自FKRelationAttribute的表名。或可以手动指定，特别是视图时可以指定到具体的表
        /// 或没有FKRelationAttribute生成时。手动指定
        /// </summary>
        public Type QueryTargetType { get; set; } = null;

        /// <summary>
        /// 如果在Process代码中手动指定了类型。优先级最高
        /// 则要按这个约定的类型处理，在UI生成。查询条件处理等都要一致
        /// 通过这个每增加一个特殊的类型。则可以UI做一个UI控件来对应特殊类型的处理
        /// </summary>
        public AdvQueryProcessType AdvQueryFieldType { get; set; }


        public QueryField() { }

        /// <summary>
        /// 属性信息
        /// </summary>
        public PropertyInfo FieldPropertyInfo { get; set; }

        public QueryField(string fieldName)
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// 原始表编号的字段，从关联表中获取指定字段的值
        /// 一般是指名称 编号
        /// </summary>
        public string FriendlyFieldNameFromSource { get; set; }

        /// <summary>
        /// 原始表编号的字段，从关联表中获取指定字段的值
        /// 一般是批号ID，如被引用的仓库表中的ID
        /// </summary>
        public string FriendlyFieldValueFromSource { get; set; }

        /// <summary>
        /// 为了显示ID对应的其他列，要同时存在于引用表中
        /// 替换的字段，有时查一个单据时，是按编号来查的，比方在销售出库单中。查询条件中包含销售订单数据，保存了订单ID和编号，
        /// 但是编号并不是和原始订单表中的单号相同，还要指向一次到最终的表中的单号。要么这里处理。要么所有引用相关表
        /// 除了ID还要编号姓名等，还要两个表中字段相同，不然在databindhelper引用式生成取值时会出错
        /// 这个字段还是要存在于引用表中。即订单表中。订单引用其他的键值表。
        /// 如：出库单中保存了订单的ID和编号。最简单是直接显示订单ID，实际想显示订单的编号。
        /// 来源于业务表
        /// </summary>
        public string FriendlyFieldNameFormBiz { get; set; }

        /// <summary>
        /// 查询的字段名,下拉时则直接就是ID
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 默认值?
        /// </summary>
        public string Value { get; set; }


        public QueryFilter _SubFilter = new QueryFilter();

        /// <summary>
        /// 次级查询条件，这里架构不完善，暂时只方便用条件名称来搜索，不处理下拉等特殊情况
        /// </summary>
        public QueryFilter SubFilter { get => _SubFilter; set => _SubFilter = value; }

        public bool HasSubFilter { get; set; } = false;


        public QueryFilter _ParentFilter = new QueryFilter();

        /// <summary>
        ///  父级条件，暂时作用不大。调试时有用，观察
        /// </summary>
        public QueryFilter ParentFilter { get => _ParentFilter; set => _ParentFilter = value; }

        #region 如果关联的是视图中的字段情况时

        public bool IsView { get; set; } = false;




        public bool IsHidden { get; set; }
        public bool IsVisible { get; set; } = false;


        public string Caption { get; set; } = string.Empty;

        #endregion

        #region 如果是枚举特殊类型
        /// <summary>
        /// 根据不同的字段类型。给出对应的数据信息
        /// 以这个类型为标准判断应该处理的方式
        /// 这块应该可以重构，比方 用接口定义
        /// </summary>
        public QueryFieldType FieldType { get; set; }

        /// <summary>
        /// 指定了查询字段的数据类型后，就要指定具体数据
        /// </summary>
        public IQueryFieldData QueryFieldDataPara { get; set; }
        public bool? Focused { get; set; }


        //默认值1，2主要目前是针对时间区间
        public string Default1 { get; set; }
        public string Default2 { get; set; }
        public bool? EnableDefault1 { get; set; }
        public bool? EnableDefault2 { get; set; }
        public int? DiffDays1 { get; set; }

        //按天数算出来
        public int? DiffDays2 { get; set; }

        #endregion
    }






}
