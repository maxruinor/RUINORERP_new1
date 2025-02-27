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
using FastReport.DevComponents.DotNetBar;
using System.Runtime.InteropServices;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 所有表格显示为名称的代码都放到这里，还有一些没有重构完
    /// </summary>
    public class GridViewDisplayHelper
    {
        // 用于存储固定字典值的映射
        //private Dictionary<string, List<KeyValuePair<object, string>>> FixedDictionaryMappings { get; set; } = new Dictionary<string, List<KeyValuePair<object, string>>>();
        public HashSet<FixedDictionaryMapping> FixedDictionaryMappings { get; set; } = new HashSet<FixedDictionaryMapping>();

        /// <summary>
        ///  
        /// </summary>
        public HashSet<ReferenceKeyMapping> ReferenceKeyMappings { get; set; } = new HashSet<ReferenceKeyMapping>();


        // 用于存储列的显示类型
        public Dictionary<string, string> ColumnDisplayTypes { get; set; } = new Dictionary<string, string>();

        // 用于存储外键列的映射
        public Dictionary<string, string> ReferenceKeyColumnMappings { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 用于存储外键表的列表信息
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<string, string>>> ReferenceTableList { get; set; } = new ConcurrentDictionary<string, List<KeyValuePair<string, string>>>();

        public void InitializeFixedDictionaryMappings(Type _type)
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

        public void InitializeFixedDictionaryMappings<T>()
        {
            // 动态检查类型是否包含指定的属性
            var type = typeof(T);
            InitializeFixedDictionaryMappings(type);
        }
  

        public void InitializeReferenceKeyMapping(Type _type)
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

        public void InitializeReferenceKeyMapping<T>()
        {
            var type = typeof(T);
            InitializeReferenceKeyMapping(type);
        }
        // 添加外键映射
        public void AddReferenceKeyMapping(string columnName, ReferenceKeyMapping mapping)
        {
            if (!ReferenceKeyMappings.Contains(mapping))
            {
                ReferenceKeyMappings.Add(mapping);
            }
        }

        public void AddFixedDictionaryMapping<T>(string columnName, List<KeyValuePair<object, string>> mappings)
        {

        }


        public string GetGridViewDisplayText(string tableName, string columnName, object value)
        {
            string DisplayText = string.Empty;
            #region 
            // 处理固定字典值
            var fixedMapping = FixedDictionaryMappings.FirstOrDefault(t => t.TableName == tableName && t.KeyFieldName == columnName);
            if (fixedMapping != null)
            {
                List<KeyValuePair<object, string>> mappings = fixedMapping.KeyValuePairList;
                KeyValuePair<object, string> matchedPair = mappings.FirstOrDefault(pair => pair.Key.ToString().ToLower() == value.ToString().ToLower());
                if (matchedPair.Value != null)
                {
                    DisplayText = matchedPair.Value;
                    return DisplayText;
                }
            }

            // 处理外键映射
            string displayName = GetDisplayNameByReferenceKeyMappings(tableName, columnName, value);
            if (!string.IsNullOrEmpty(displayName))
            {
                DisplayText = displayName;
                return DisplayText;
            }

            #endregion
            return DisplayText;
        }


        // 获取显示名称
        public string GetDisplayNameByReferenceKeyMappings(string TargetTableName, string idColName, object IdValue)
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

                    //视图暂时没有实体生成时没有设置关联外键的特性，所以在具体业务实现时 手工指定成一个集合了。


                    //先处理类型本身
                    //只处理需要缓存的表
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                    if (BizCacheHelper.Manager.NewTableList.TryGetValue(TargetTableName, out pair))
                    {
                        //在基本表中但是缓存中没有则请求
                        if (!BizCacheHelper.Manager.CacheEntityList.Exists(TargetTableName))
                        {
                            UIBizSrvice.RequestCache(TargetTableName);
                        }
                        var selfTypeDisplayText = BizCacheHelper.Instance.GetValue(TargetTableName, IdValue);
                        if (selfTypeDisplayText != null && !string.IsNullOrWhiteSpace(selfTypeDisplayText.ToString())
                             && selfTypeDisplayText.GetType().Name != "Object"
                            )
                        {
                            return selfTypeDisplayText.ToString();
                        }
                    }
                    //再处理外键关联
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
}
