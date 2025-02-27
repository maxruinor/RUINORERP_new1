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
using Newtonsoft.Json.Linq;
using System.Collections;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 用于解析DataGridView显示ID列时，转换为对应名称的工具类
    /// 用于视图分析
    /// </summary>
    public class GridViewDisplayTextResolver
    {
        private Type _type;
        private HashSet<ReferenceKeyMapping> ReferenceKeyMappings { get; set; } = new HashSet<ReferenceKeyMapping>();
        private static IList _relatedTableTypesCache;
        public GridViewDisplayTextResolver(Type type)
        {
            _type = type;
            // 在类初始化时缓存 RelatedTableTypes
            // BaseViewEntity baseView = (BaseViewEntity)Activator.CreateInstance(_type);
            //  _relatedTableTypesCache = baseView.InstanceRelatedTableTypes;

            InitializeFixedDictionaryMappings();
            InitializeReferenceKeyMapping();
        }

        #region 缓存相关显示外键的类型
        private static readonly ConcurrentDictionary<Type, List<Type>> relatedTableCache = new ConcurrentDictionary<Type, List<Type>>();

        public static List<Type> GetRelatedTableTypes(Type type)
        {
            return relatedTableCache.GetOrAdd(type, t =>
            {
                BaseViewEntity instance = (BaseViewEntity)Activator.CreateInstance(t);
                instance.InitRelatedTableTypes();
                List<Type> RelatedTableTypes= instance.InstanceRelatedTableTypes;
                foreach (var item in RelatedTableTypes)
                {
                    BizCacheHelper.Manager.SetFkColList(item);
                }
                return RelatedTableTypes;
            });
        }
        #endregion

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




        private void InitializeFixedDictionaryMappings()
        {
            // 动态检查类型是否包含指定的属性
            foreach (var prop in _type.GetProperties())
            {
                // 检查类型 是否包含 DataStatus 列
                if (prop.Name == nameof(DataStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, prop.Name, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus))));
                }
                else if (prop.Name == nameof(ApprovalStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, prop.Name, CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus))));
                }

                // 检查类型 T 是否包含 PayStatus 列
                else if (prop.Name == nameof(PayStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(PayStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(PayStatus))));
                }
                // 检查类型 T 是否包含 Priority 列
                else if (prop.Name == nameof(Priority))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(Priority), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Priority))));
                }

                // 检查类型 T 是否包含 PurReProcessWay 列
                else if (prop.Name == nameof(PurReProcessWay))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(PurReProcessWay), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PurReProcessWay))));
                }
                 
                else if (prop.Name == nameof(PurReProcessWay))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(PurReProcessWay), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PurReProcessWay))));
                }
                else if (prop.Name == "ApprovalResults")
                {
                    List<KeyValuePair<object, string>> ApprovalResultskvlist = new List<KeyValuePair<object, string>>();
                    ApprovalResultskvlist.Add(new KeyValuePair<object, string>(true, "同意"));
                    ApprovalResultskvlist.Add(new KeyValuePair<object, string>(false, "否决"));
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, "ApprovalResults", ApprovalResultskvlist));
                }


                List<KeyValuePair<object, string>> Genderkvlist = new List<KeyValuePair<object, string>>();
                Genderkvlist.Add(new KeyValuePair<object, string>(true, "男"));
                Genderkvlist.Add(new KeyValuePair<object, string>(false, "女"));
                Expression<Func<tb_Employee, bool?>> expr;
                expr = (p) => p.Gender;// == name;
                var mb = expr.GetMemberInfo();
                string colName = mb.Name;
                if (typeof(tb_Employee).Name == _type.Name && prop.Name == colName)
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
                if (typeof(tb_Employee).Name == _type.Name && prop.Name == colName1)
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(tb_Employee).Name, colName1, kvlist1));
                }
                if (typeof(tb_Employee).Name == _type.Name && prop.Name == colName2)
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(tb_Employee).Name, colName2, kvlist1));
                }
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
            string tableName = _type.Name;
            if (!ReferenceTableList.ContainsKey(tableName))
            {
                List<KeyValuePair<string, string>> kvlist = new List<KeyValuePair<string, string>>();
                foreach (var field in _type.GetProperties())
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

                            //只处理需要缓存的表
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                            if (BizCacheHelper.Manager.NewTableList.TryGetValue(mapping.ReferenceTableName, out pair))
                            {
                                //要显示的默认值是从缓存表中获取的字段名，默认是主键ID字段对应的名称
                                mapping.ReferenceDefaultDisplayFieldName = pair.Value;
                            }
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
        /// <typeparam name="source">from单位表</typeparam>
        /// <typeparam name="target">to单位换算，产品等引用单位的表，字段和主键不一样时使用</typeparam>
        /// <param name="SourceField"></param>
        /// <param name="TargetField"></param>
        /// <param name="CustomDisplaySourceField">如果有特殊指定显示为其它列时</param>
        public void AddReferenceKeyMapping<source, target>(Expression<Func<source, object>> SourceField, Expression<Func<target, object>> TargetField, Expression<Func<source, object>> CustomDisplaySourceField = null)
        {
            MemberInfo Sourceinfo = SourceField.GetMemberInfo();
            MemberInfo Targetinfo = TargetField.GetMemberInfo();

            if (ReferenceKeyMappings == null)
            {
                ReferenceKeyMappings = new HashSet<ReferenceKeyMapping>();
            }
            ReferenceKeyMapping mapping = new ReferenceKeyMapping(typeof(source).Name, Sourceinfo.Name, typeof(target).Name, Targetinfo.Name);
            if (typeof(source).Name == typeof(target).Name)
            {
                mapping.IsSelfReferencing = true;
            }
            //只处理需要缓存的表
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
            if (BizCacheHelper.Manager.NewTableList.TryGetValue(mapping.ReferenceTableName, out pair))
            {
                //要显示的默认值是从缓存表中获取的字段名，默认是主键ID字段对应的名称
                mapping.ReferenceDefaultDisplayFieldName = pair.Value;
            }
            if (CustomDisplaySourceField != null)
            {
                MemberInfo CustomDisplayColInfo = CustomDisplaySourceField.GetMemberInfo();
                mapping.CustomDisplayColumnName = CustomDisplayColInfo.Name;
            }
            //以目标为主键，原始的相同的只能为值
            ReferenceKeyMappings.Add(mapping);
        }


        // 添加固定字典映射
        public void AddFixedDictionaryMapping(string tableName, string columnName, List<KeyValuePair<object, string>> mappings)
        {
            if (_type.GetProperty(tableName) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, columnName, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus))));
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(ApprovalStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus))));
            }
        }

        /// <summary>
        /// 添加枚举类型的固定字典映射，枚举类型做为显示值，枚举的名称做为键值
        /// </summary>
        /// <typeparam name="target">指定了目标表</typeparam>
        /// <param name="TargetField"></param>
        /// <param name="enumType"></param>
        public void AddFixedDictionaryMappingByEnum<target>(Expression<Func<target, object>> TargetField, Type enumType)
        {
            MemberInfo Targetinfo = TargetField.GetMemberInfo();
            FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(target).Name, Targetinfo.Name, CommonHelper.Instance.GetKeyValuePairs(enumType)));
        }



        //    public void AddFixedDictionaryMapping<T>(string columnName, List<KeyValuePair<object, string>> mappings)
        //{
        //    if (typeof(T).GetProperty(tableName) != null)
        //    {
        //        FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, columnName, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus))));
        //        FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, nameof(ApprovalStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus))));
        //    }
        //}

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
            var fixedMapping = FixedDictionaryMappings.FirstOrDefault(t => t.TableName == _type.Name && t.KeyFieldName == columnName);
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

            if (_type.Name.Contains("View_"))
            {
                //BaseViewEntity baseView = (BaseViewEntity)Activator.CreateInstance(_type);
                var relatedTableTypes = GetRelatedTableTypes(_type);
                foreach (var item in relatedTableTypes)
                {
                    string displayName = GetDisplayNameByReferenceKeyMappings(item.Name, columnName, e.Value);
                    if (!string.IsNullOrEmpty(displayName))
                    {
                        e.Value = displayName;
                        return;
                    }
                }
            }
            else
            {
                // 处理外键映射
                string displayName = GetDisplayNameByReferenceKeyMappings(_type.Name, columnName, e.Value);
                if (!string.IsNullOrEmpty(displayName))
                {
                    e.Value = displayName;
                    return;
                }
            }

        }

        // 获取显示名称
        private string GetDisplayNameByReferenceKeyMappings(string TargetTableName, string idColName, object IdValue)
        {
            try
            {
                // 如果值为空，直接返回空字符串
                if (IdValue == null)
                {
                    return string.Empty;
                }

                // 如果值是 decimal 类型，直接返回
                if (IdValue.GetType().Name == "Decimal")
                {
                    return IdValue.ToString();
                }

                // 如果值不是 long 类型，直接返回
                if (IdValue.GetType().Name != "Int64")
                {
                    return IdValue.ToString();
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

                if (idColName == Created_by.GetMemberInfo().Name || idColName == Modified_by.GetMemberInfo().Name || idColName == "Approver_by")
                {
                    if (IdValue.ToString() == "0")
                    {
                        return "";
                    }
                    string SourceTableName = typeof(tb_Employee).Name;
                    object objText = BizCacheHelper.Instance.GetValue(SourceTableName, IdValue);
                    if (objText != null && objText.ToString() != "System.Object")
                    {
                        NameValue = objText.ToString();
                        return NameValue;
                    }
                }

                #endregion

                ReferenceKeyMapping mapping = ReferenceKeyMappings.Where(c => c.MappedTargetTableName == TargetTableName && c.MappedTargetFieldName == idColName).FirstOrDefault();
                if (mapping != null)
                {
                    if (mapping.MappedTargetFieldName == idColName)
                    {

                        #region 从缓存中取值
                        object entity = new object();
                        //只处理需要缓存的表
                        KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                        if (BizCacheHelper.Manager.NewTableList.TryGetValue(mapping.ReferenceTableName, out pair))
                        {
                            string key = pair.Key;
                            string KeyValue = IdValue.ToString();
                            //设置属性的值
                            if (BizCacheHelper.Manager.CacheEntityList.Exists(mapping.ReferenceTableName))
                            {
                                var rslist = BizCacheHelper.Manager.CacheEntityList.Get(mapping.ReferenceTableName);
                                if (TypeHelper.IsGenericList(rslist.GetType()))
                                {
                                    var lastlist = ((IEnumerable<dynamic>)rslist).ToList();
                                    if (lastlist != null)
                                    {
                                        foreach (var item in lastlist)
                                        {
                                            var id = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(item, key);
                                            if (id != null)
                                            {
                                                if (id.ToString() == IdValue.ToString())
                                                {
                                                    entity = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(item, pair.Value);
                                                    break;
                                                }
                                            }

                                        }
                                    }
                                }
                                else if (rslist != null && TypeHelper.IsJArrayList(rslist.GetType()))
                                {

                                    var lastlist = ((IEnumerable<dynamic>)rslist).ToList();
                                    if (lastlist != null)
                                    {
                                        foreach (var item in lastlist)
                                        {
                                            // 将item转换为JObject
                                            var jobj = JObject.Parse(item.ToString());

                                            // 获取DepartmentID属性的值
                                            var id = jobj[key]?.ToString();

                                            if (id != null && id == IdValue.ToString())
                                            {
                                                //如果找到了匹配的id，获取对应的属性值，如果有特殊指定则取特殊值
                                                if (string.IsNullOrEmpty(mapping.CustomDisplayColumnName))
                                                {
                                                    // 假设你想要获取的属性是DepartmentName
                                                    var departmentName = jobj[pair.Value]?.ToString();
                                                    if (departmentName != null)
                                                    {
                                                        entity = departmentName;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    var departmentName = jobj[mapping.CustomDisplayColumnName]?.ToString();
                                                    if (departmentName != null)
                                                    {
                                                        entity = departmentName;
                                                        break;
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        if (entity != null && entity.GetType().Name != "Object")
                        {
                            NameValue = entity.ToString();
                            return NameValue;
                        }
                    }
                }
                else
                {
                    #region 没有映射的情况

                    List<KeyValuePair<string, string>> kvlist = new List<KeyValuePair<string, string>>();
                    if (BizCacheHelper.Manager.FkPairTableList.TryGetValue(TargetTableName, out kvlist))
                    {
                        var kv = kvlist.Find(k => k.Key == idColName);
                        //如果找不到呢？
                        if (kv.Key == null)
                        {
                            return string.Empty;
                        }
                        string baseTableName = kv.Value;
                        object obj = BizCacheHelper.Instance.GetValue(baseTableName, IdValue);
                        if (obj != null)
                        {
                            NameValue = obj.ToString();
                        }
                    }
                    if (string.IsNullOrEmpty(NameValue) && TargetTableName.Contains("View"))
                    {
                        object obj = BizCacheHelper.Instance.GetValue(TargetTableName, IdValue);
                        if (obj != null && obj.GetType().Name != "Object")
                        {
                            NameValue = obj.ToString();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(NameValue))
                        {
                            object obj = BizCacheHelper.Instance.GetValue(TargetTableName, IdValue);
                            if (obj != null && obj.GetType().Name != "Object")
                            {
                                NameValue = obj.ToString();
                            }
                        }
                    }
                    return NameValue;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            return IdValue.ToString();
        }

    }

    // 外键映射类 多种情况 
    //普通映射 一个表对应一个主键字段 一个显示名称字段
    //比方菜单表中，引用了模块用的ID，如果ID名和模块名一样时。是一种情况。如果不一样，则要指定别名。


}

