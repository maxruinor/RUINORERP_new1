using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using RUINORERP.Business.CommService;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using Autofac;
using Krypton.Toolkit;
using RUINORERP.Common.Helper;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINORERP.Model;
using RUINOR.Core;
using RUINORERP.UI.Common;
using RUINORERP.UI.BI;
using RUINORERP.Common.CustomAttribute;
using System.Collections.Concurrent;
using RUINORERP.Business;
using RUINORERP.Global.CustomAttribute;
using ObjectsComparer;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Model.Base;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Business.Processor;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using TransInstruction;
using RUINORERP.UI.SuperSocketClient;
using RUINORERP.Model.Models;
using System.Diagnostics;
using RUINORERP.Global.Model;
using RUINORERP.UI.CommonUI;
using RUINORERP.UI.FormProperty;
using System.Web.UI;
using Control = System.Windows.Forms.Control;
using SqlSugar;
using SourceGrid.Cells.Models;
using SixLabors.ImageSharp.Memory;
using Netron.NetronLight;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINORERP.UI.UserCenter;
using RUINORERP.UI.UserPersonalized;
using RUINORERP.UI.UControls;
using Newtonsoft.Json;
using Fireasy.Common.Extensions;
using RUINORERP.Global;
using FastReport.Table;


namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 用于解析DataGridView显示ID列时，转换为对应名称的工具类
    /// </summary>
    public class DataGridViewDisplayTextResolver<T> where T : class
    {


        /// <summary>
        ///  
        /// </summary>
        private HashSet<ReferenceKeyMapping> ReferenceKeyMappings { get; set; } = new HashSet<ReferenceKeyMapping>();

        // 用于存储固定字典值的映射
        //private Dictionary<string, List<KeyValuePair<object, string>>> FixedDictionaryMappings { get; set; } = new Dictionary<string, List<KeyValuePair<object, string>>>();
        private HashSet<FixedDictionaryMapping> FixedDictionaryMappings { get; set; } = new HashSet<FixedDictionaryMapping>();

        // 用于存储列的显示类型
        private Dictionary<string, string> ColumnDisplayTypes { get; set; } = new Dictionary<string, string>();

        // 用于存储外键列的映射
        private Dictionary<string, string> ReferenceKeyColumnMappings { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 用于存储外键表的列表信息
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<string, string>>> ReferenceTableList { get; set; } = new ConcurrentDictionary<string, List<KeyValuePair<string, string>>>();


        public DataGridViewDisplayTextResolver()
        {

        }


        private void InitializeFixedDictionaryMappings()
        {
            // 检查类型 T 是否包含 DataStatus 列
            if (typeof(T).GetProperty(nameof(DataStatus)) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, nameof(DataStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus))));
            }

            // 检查类型 T 是否包含 ApprovalStatus 列
            if (typeof(T).GetProperty(nameof(ApprovalStatus)) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, nameof(ApprovalStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus))));
            }

            // 检查类型 T 是否包含 PayStatus 列
            if (typeof(T).GetProperty(nameof(PayStatus)) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, nameof(PayStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(PayStatus))));
            }

            // 检查类型 T 是否包含 Priority 列
            if (typeof(T).GetProperty(nameof(Priority)) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, nameof(Priority), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Priority))));
            }

            // 检查类型 T 是否包含 PurReProcessWay 列
            if (typeof(T).GetProperty(nameof(PurReProcessWay)) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, nameof(PurReProcessWay), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PurReProcessWay))));
            }

            if (typeof(T).GetProperty(nameof(PurReProcessWay)) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, nameof(PurReProcessWay), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PurReProcessWay))));
            }

            List<KeyValuePair<object, string>> Genderkvlist = new List<KeyValuePair<object, string>>();
            Genderkvlist.Add(new KeyValuePair<object, string>(true, "男"));
            Genderkvlist.Add(new KeyValuePair<object, string>(false, "女"));
            Expression<Func<tb_Employee, bool?>> expr;
            expr = (p) => p.Gender;// == name;
            var mb = expr.GetMemberInfo();
            string colName = mb.Name;
            if (typeof(tb_Employee).Name == typeof(T).Name && typeof(T).GetProperty(colName) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(tb_Employee).Name, colName, Genderkvlist));
            }


            List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
            kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
            kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
            System.Linq.Expressions.Expression<Func<tb_Employee, bool?>> expr1;
            expr1 = (p) => p.Is_available;// == name;
            System.Linq.Expressions.Expression<Func<tb_Employee, bool?>> expr2;
            expr2 = (p) => p.Is_enabled;// == name;
            string colName1 = expr1.GetMemberInfo().Name;
            string colName2 = expr2.GetMemberInfo().Name;
            if (typeof(tb_Employee).Name == typeof(T).Name && typeof(T).GetProperty(colName1) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(tb_Employee).Name, colName1, Genderkvlist));
            }
            if (typeof(tb_Employee).Name == typeof(T).Name && typeof(T).GetProperty(colName2) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(tb_Employee).Name, colName2, Genderkvlist));
            }

        }
        // 初始化方法
        public void Initialize(DataGridView dataGridView)
        {
            // 初始化固定字典映射
            InitializeFixedDictionaryMappings();
            InitializeReferenceKeyMapping();
            dataGridView.CellFormatting += DataGridView_CellFormatting;
        }


        private void InitializeReferenceKeyMapping()
        {
            string tableName = typeof(T).Name;
            if (!ReferenceTableList.ContainsKey(tableName))
            {
                List<KeyValuePair<string, string>> kvlist = new List<KeyValuePair<string, string>>();
                foreach (var field in typeof(T).GetProperties())
                {
                    //获取指定类型的自定义特性
                    object[] attrs = field.GetCustomAttributes(false);
                    foreach (var attr in attrs)
                    {
                        if (attr is FKRelationAttribute)
                        {
                            FKRelationAttribute fkrattr = attr as FKRelationAttribute;

                            //TODO:特殊处理：因 为fkrattr.FKTableName 如果是tb_ProdDetail 换为 视图，产品表没有缓存
                            //// SetDictDataSource<tb_ProdDetail>(k => k.ProdDetailID, v => v.SKU);//这个不能缓存 在girdsetvalue时
                            //SetDictDataSource<View_ProdDetail>(k => k.ProdDetailID.Value, v => v.CNName);
                            if (fkrattr.FKTableName == "tb_ProdDetail")
                            {
                                fkrattr.FKTableName = "View_ProdDetail";
                            }

                            KeyValuePair<string, string> kv = new KeyValuePair<string, string>(fkrattr.FK_IDColName, fkrattr.FKTableName);
                            kvlist.Add(kv);

                            ReferenceKeyMapping mapping = new ReferenceKeyMapping(fkrattr.FKTableName, fkrattr.FK_IDColName, tableName);
                            AddReferenceKeyMapping(fkrattr.FK_IDColName, mapping);
                        }
                    }
                }
                if (kvlist.Count > 0)
                {
                    ReferenceTableList.TryAdd(tableName, kvlist);
                }
            }
        }

        // 添加外键映射
        public void AddReferenceKeyMapping(string columnName, ReferenceKeyMapping mapping)
        {
            if (!ReferenceKeyMappings.Contains(mapping))
            {
                ReferenceKeyMappings.Add(mapping);
            }
        }

        /// <summary>
        /// 手动添加外键关联指向
        /// </summary>
        /// <typeparam name="source">单位表</typeparam>
        /// <typeparam name="target">单位换算，产品等引用单位的表，字段和主键不一样时使用</typeparam>
        /// <param name="SourceField"></param>
        /// <param name="TargetField"></param>
        public void AddReferenceKeyMapping<source, target>(Expression<Func<source, object>> SourceField, Expression<Func<target, object>> TargetField)
        {
            MemberInfo Sourceinfo = SourceField.GetMemberInfo();
            MemberInfo Targetinfo = TargetField.GetMemberInfo();

            if (ReferenceKeyMappings == null)
            {
                ReferenceKeyMappings = new HashSet<ReferenceKeyMapping>();
            }
            ReferenceKeyMapping mapping = new ReferenceKeyMapping(typeof(source).Name, Sourceinfo.Name, typeof(target).Name, Targetinfo.Name);

            //以目标为主键，原始的相同的只能为值
            ReferenceKeyMappings.Add(mapping);
        }


        // 添加固定字典映射
        public void AddFixedDictionaryMapping(string tableName, string columnName, List<KeyValuePair<object, string>> mappings)
        {
            if (typeof(T).GetProperty(tableName) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, columnName, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus))));
            }
        }

        // 添加列显示类型
        public void AddColumnDisplayType(string columnName, string displayType)
        {
            ColumnDisplayTypes[columnName] = displayType;
        }

        // 添加外键列映射
        public void AddReferenceKeyColumnMapping(string columnName, string foreignKeyColumnName)
        {
            ReferenceKeyColumnMappings[columnName] = foreignKeyColumnName;
        }

        // 单元格格式化事件处理
        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;

            // 如果列是隐藏的，直接返回
            if (!dataGridView.Columns[e.ColumnIndex].Visible)
            {
                return;
            }

            if (e.Value == null)
            {
                e.Value = "";
                return;
            }

            // 获取列名
            string columnName = dataGridView.Columns[e.ColumnIndex].Name;

            // 处理固定字典值
            var fixedMapping = FixedDictionaryMappings.FirstOrDefault(t => t.TableName == typeof(T).Name && t.KeyFieldName == columnName);
            if (fixedMapping != null)
            {
                List<KeyValuePair<object, string>> mappings = fixedMapping.KeyValuePairList;
                KeyValuePair<object, string> matchedPair = mappings.FirstOrDefault(pair => pair.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                if (matchedPair.Value != null)
                {
                    e.Value = matchedPair.Value;
                    return;
                }
            }

            // 处理特殊列类型（如图片）
            if (ColumnDisplayTypes.ContainsKey(columnName))
            {
                string displayType = ColumnDisplayTypes[columnName];
                if (displayType == "Image")
                {
                    if (e.Value is byte[])
                    {
                        using (MemoryStream ms = new MemoryStream((byte[])e.Value))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            e.Value = image;
                        }
                    }
                }
            }


            // 处理外键映射
            string displayName = GetDisplayNameByReferenceKeyMappings(typeof(T).Name, columnName, e.Value);
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Value = displayName;
                return;
            }


        }

        // 获取显示名称
        private string GetDisplayNameByReferenceKeyMappings(string tableName, string columnName, object value)
        {
            try
            {
                // 如果值为空，直接返回空字符串
                if (value == null)
                {
                    return string.Empty;
                }

                // 如果值是 decimal 类型，直接返回
                if (value.GetType().Name == "Decimal")
                {
                    return value.ToString();
                }

                // 如果值不是 long 类型，直接返回
                if (value.GetType().Name != "Int64")
                {
                    return value.ToString();
                }
                string NameValue = string.Empty;
                // 特殊字段处理（如创建人、修改人）
                #region
                //如果是修改人创建人统一处理,并且要放在前面
                //定义两个字段为了怕后面修改，不使用字符串
                //2024-2-17 完善 这里应该是有点小问题
                Expression<Func<tb_Employee, long>> Created_by;
                Expression<Func<tb_Employee, long>> Modified_by;
                Expression<Func<tb_Employee, long>> Approver_by;

                Approver_by = c => c.Employee_ID;
                Created_by = c => c.Created_by.Value;
                Modified_by = c => c.Modified_by.Value;

                if (columnName == Created_by.GetMemberInfo().Name || columnName == Modified_by.GetMemberInfo().Name || columnName == "Approver_by")
                {
                    if (value.ToString() == "0")
                    {
                        return "";
                    }
                    string SourceTableName = typeof(tb_Employee).Name;
                    object objText = BizCacheHelper.Instance.GetValue(SourceTableName, value);
                    if (objText != null && objText.ToString() != "System.Object")
                    {
                        NameValue = objText.ToString();
                        return NameValue;
                    }
                }


                //视图暂时没有实体生成时没有设置关联外键的特性，所以在具体业务实现时 手工指定成一个集合了。
                if (!tableName.Contains("View") && type != null)
                {
                    BizCacheHelper.Manager.SetFkColList(type);
                }

                #endregion

                return UIHelper.ShowGridColumnsNameValue(mapping.ReferenceTableName, mapping.ReferenceOriginalFieldName, value);

                // 自引用表处理（如类别表）
                if (mapping.IsSelfReferencing)
                {
                    string tableName = mapping.ReferenceTableName;
                    if (!BizCacheHelper.Manager.FkPairTableList.ContainsKey(tableName))
                    {
                        List<KeyValuePair<string, string>> kvSelflist = new List<KeyValuePair<string, string>>();
                        //Expression<Func<T, long?>> expKey = mapping.KeyExpression;
                        //MemberInfo minfo = expKey.GetMemberInfo();
                        string key = mapping.ReferenceOriginalFieldName;
                        KeyValuePair<string, string> kv = new KeyValuePair<string, string>(key, typeof(T).Name);
                        kvSelflist.Add(kv);
                        if (kvSelflist.Count > 0)
                        {
                            BizCacheHelper.Manager.FkPairTableList.TryAdd(tableName, kvSelflist);
                        }
                    }
                }

                // 获取外键表的名称和字段名
                string baseTableName = mapping.ReferenceTableName;
                string keyFieldName = mapping.ReferenceOriginalFieldName;

                // 获取外键表的值
                object obj = BizCacheHelper.Instance.GetValue(baseTableName, value);
                if (obj != null)
                {
                    return obj.ToString();
                }
            }
            catch (Exception ex)
            {
                // 异常处理
                Console.WriteLine($"Error getting display name: {ex.Message}");
            }

            return string.Empty;
        }
    }

    // 外键映射类 多种情况 
    //普通映射 一个表对应一个主键字段 一个显示名称字段
    //比方菜单表中，引用了模块用的ID，如果ID名和模块名一样时。是一种情况。如果不一样，则要指定别名。


    /// <summary>
    /// 以显示tb_Unit_Conversion为例子
    /// </summary>
    public class ReferenceKeyMapping
    {
        /// <summary>
        /// 引用的表名
        /// Unit_ID
        /// </summary>
        public string ReferenceTableName { get; set; } // 表名

        /// <summary>
        /// 引用的表中的键字段名
        /// 通常是关联表的主键字段名，比方菜单表中引用了模块用的ID
        /// Unit_ID
        /// </summary>
        public string ReferenceOriginalFieldName { get; set; } // ID键字段名


        /// <summary>
        /// 要显示的表的目标的字段名，通常可能会和引用的关联表主键值一样。
        /// 如果不一样时才要特别指定
        /// tb_Unit_Conversion
        /// </summary>
        public string MappedTargetTableName { get; set; } // ID键字段名

        /// <summary>
        /// 要显示的表的目标的字段名，通常可能会和引用的关联表主键值一样。
        /// 如果不一样时才要特别指定
        /// Source_unit_id
        /// </summary>
        public string MappedTargetFieldName { get; set; } // ID键字段名

        /// <summary>
        /// 一般是键值对，但是有时可能是编号，
        /// 如：仓库ID 默认是仓库名称，也可以显示为仓库编号
        /// 这里是默认的
        /// UnitName
        /// </summary>
        public string ReferenceDefaultDisplayFieldName { get; set; } // 将要显示的名称字段名

        /// <summary>
        /// 一般是键值对，但是有时可能是编号，
        /// 如：仓库ID 默认是仓库名称，也可以显示为仓库编号
        /// UnitNo 【实际单据没有这个字段】 这里代指  有名称 有编号的情况
        /// </summary>
        public string ReferenceTargetedDisplayFieldName { get; set; } // 将要显示的名称字段名


        /// <summary>
        /// 一般是键值对，但是有时可能是编号，
        /// 如：仓库ID 默认是仓库名称，也可以显示为仓库编号
        /// </summary>
        public string ReferenceDisplayFieldName { get; set; } // 将要显示的名称字段名
        public bool IsSpecialField { get; set; } // 是否是特殊字段（如创建人、修改人）
        public string SpecialFieldName { get; set; } // 特殊字段的表名
        public bool IsSelfReferencing { get; set; } // 是否是自引用表
        //public List<KeyValuePair<object, string>> KeyValuePairList { get; set; } // 键值对集合

        public ReferenceKeyMapping(string referenceTableName, string referenceOriginalFieldName, string mappedTargetTableName, string mappedTargetFieldName = null, string referenceDefaultDisplayFieldName = null, string referenceTargetedDisplayFieldName = null)
        {
            ReferenceTableName = referenceTableName;
            MappedTargetTableName = mappedTargetTableName;
            ReferenceOriginalFieldName = referenceOriginalFieldName;
            MappedTargetFieldName = mappedTargetFieldName;
            if (string.IsNullOrEmpty(mappedTargetFieldName))
            {
                MappedTargetFieldName = ReferenceOriginalFieldName;
            }

        }

        //public ReferenceKeyMapping(string tableName, string keyFieldName, string valueFieldName = null, bool isSpecialField = false, string specialFieldName = null, bool isSelfReferencing = false)
        //{
        //    ReferenceTableName = tableName;
        //    KeyFieldName = keyFieldName;
        //    DisplayFieldName = valueFieldName;
        //    IsSpecialField = isSpecialField;
        //    SpecialFieldName = specialFieldName;
        //    IsSelfReferencing = isSelfReferencing;
        //}

    }


    /// <summary>
    /// 固定字典值映射类,比方单据主表中单据状态字段，一个审核状态有 提交、审核、结案等状态
    /// </summary>
    public class FixedDictionaryMapping
    {
        public string TableName { get; set; } // 表名
        public string KeyFieldName { get; set; } // 要显示字段名
        public List<KeyValuePair<object, string>> KeyValuePairList { get; set; } // 键值对集合

        public FixedDictionaryMapping(string tableName, string keyFieldName, List<KeyValuePair<object, string>> keyValuePairList)
        {
            TableName = tableName;
            KeyFieldName = keyFieldName;
            KeyValuePairList = keyValuePairList;
        }

    }
}

