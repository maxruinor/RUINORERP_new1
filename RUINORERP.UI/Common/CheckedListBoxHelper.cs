using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Common.Extensions;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Global.EnumExt;
using Netron.GraphLib;

namespace RUINORERP.UI.Common
{
   
    public static partial class CheckedListBoxHelper
    {
        #region 枚举绑定方法



        /// <summary>
        /// 为 CheckedListBox 绑定枚举多选值
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="propertyExpression">属性表达式（指向实体中的 List<int>属性）</param>
        /// <param name="checkedList">CheckedListBox 控件</param>
        /// <param name="excludeEnums">需要排除的枚举值</param>
        public static void BindData4CheckedListBox<TEntity, TEnum>(
        TEntity entity,
        Expression<Func<TEntity, List<int>>> propertyExpression,
        Krypton.Toolkit.KryptonCheckedListBox checkedList,
        params TEnum[] excludeEnums) where TEnum : Enum
        {
            // 清除现有绑定
            checkedList.DataBindings.Clear();

            // 获取属性信息
            MemberExpression memberExpr = (MemberExpression)propertyExpression.Body;
            PropertyInfo propertyInfo = (PropertyInfo)memberExpr.Member;
            string propertyName = propertyInfo.Name;

            // 1. 获取枚举数据源
            var enumItems = GetEnumItems<TEnum>(excludeEnums);
            //// 2. 使用BindingSource作为中间层（关键修正）
            //BindingSource bindingSource = new BindingSource();
            //bindingSource.DataSource = enumItems;  // 将列表交给BindingSource

            checkedList.Items.Clear();

            checkedList.Items.AddRange(enumItems.Select(x => x.Description).ToArray());

            // 设置初始选中状态
            SetInitialCheckedStates<TEntity, TEnum>(entity, propertyInfo, checkedList);

            // 绑定值变更事件（同步到实体）
            checkedList.ItemCheck += (sender, e) =>
            UpdateEntityFromCheckList<TEntity, TEnum>(entity, propertyInfo, checkedList, e);
        }

        /// <summary>
        /// 获取枚举项（包含描述和值）
        /// </summary>
        private static List<EnumItem> GetEnumItems<TEnum>(params TEnum[] excludeEnums) where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            return Enum.GetValues(enumType)
            .Cast<TEnum>()
            .Where(e => !excludeEnums.Contains(e))
            .Select(e => new EnumItem
            {
                Value = Convert.ToInt32(e),
                Description = GetEnumDescription(enumType, e.ToString())
            })
            .ToList();
        }

        /// <summary>
        /// 获取枚举的 Description 属性,如果没有描述属性则返回枚举名称
        /// </summary>
        private static string GetEnumDescription(Type enumType, string enumName)
        {
            var fieldInfo = enumType.GetField(enumName);
            var descriptionAttr = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttr?.Description ?? enumName;
        }

        /// <summary>
        /// 设置初始选中状态
        /// </summary>
        private static void SetInitialCheckedStates<TEntity, TEnum>(
        TEntity entity,
        PropertyInfo propertyInfo,
        Krypton.Toolkit.KryptonCheckedListBox checkedList) where TEnum : Enum
        {
            var selectedValues = (List<int>)propertyInfo.GetValue(entity);
            if (selectedValues == null) return;

            for (int i = 0; i < checkedList.Items.Count; i++)
            {

                // 获取项的文本描述
                string itemDescription = checkedList.Items[i]?.ToString();
                if (string.IsNullOrEmpty(itemDescription))
                    continue;

                // 通过描述获取对应的EnumItem
                var enumItem = GetEnumItemByDescription<TEnum>(itemDescription);
                if (enumItem != null && selectedValues.Contains(enumItem.Value))
                {
                    checkedList.SetItemChecked(i, true);
                }
            }
        }

        /// <summary>
        /// 根据选中状态更新实体
        /// </summary>
        /// <summary>
        /// 根据选中状态更新实体（支持TEnum类型）
        /// </summary>
        private static void UpdateEntityFromCheckList<TEntity, TEnum>(
            TEntity entity,
            PropertyInfo propertyInfo,
            Krypton.Toolkit.KryptonCheckedListBox checkedList,
            ItemCheckEventArgs e) where TEnum : Enum
        {
            // 存储最终选中的值集合
            List<int> selectedValues = new List<int>();

            // 处理所有已选中项
            foreach (var item in checkedList.CheckedItems)
            {
                string itemDesc = item?.ToString();
                if (!string.IsNullOrEmpty(itemDesc))
                {
                    var enumItem = GetEnumItemByDescription<TEnum>(itemDesc);
                    if (enumItem != null)
                    {
                        selectedValues.Add(enumItem.Value);
                    }
                }
            }

            // 处理当前正在变更的项
            string currentItemDesc = checkedList.Items[e.Index]?.ToString();
            if (!string.IsNullOrEmpty(currentItemDesc))
            {
                var currentEnumItem = GetEnumItemByDescription<TEnum>(currentItemDesc);
                if (currentEnumItem != null)
                {
                    // 根据新状态添加或移除值
                    if (e.NewValue == CheckState.Checked)
                    {
                        if (!selectedValues.Contains(currentEnumItem.Value))
                        {
                            selectedValues.Add(currentEnumItem.Value);
                        }
                    }
                    else
                    {
                        selectedValues.Remove(currentEnumItem.Value);
                    }
                }
            }

            // 去重并更新实体属性
            propertyInfo.SetValue(entity, selectedValues.Distinct().ToList());
        }


        /// <summary>
        /// 通过描述文本获取对应的EnumItem
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="description">枚举的描述文本</param>
        /// <returns>对应的EnumItem，找不到则返回null</returns>
        public static EnumItem GetEnumItemByDescription<TEnum>(string description) where TEnum : Enum
        {
            Type enumType = typeof(TEnum);

            // 查找具有指定描述的枚举值
            foreach (var enumValue in Enum.GetValues(enumType))
            {
                string enumDescription = GetEnumDescription(enumType, enumValue.ToString());
                if (enumDescription == description)
                {
                    return new EnumItem
                    {
                        Value = Convert.ToInt32(enumValue),
                        Description = enumDescription
                    };
                }
            }

            // 如果找不到匹配的描述，尝试匹配枚举名称本身
            foreach (var enumValue in Enum.GetValues(enumType))
            {
                if (enumValue.ToString() == description)
                {
                    return new EnumItem
                    {
                        Value = Convert.ToInt32(enumValue),
                        Description = GetEnumDescription(enumType, enumValue.ToString())
                    };
                }
            }

            return null; // 找不到匹配项
        }


        /// <summary>
        /// 将枚举值转换为EnumItem
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <returns>对应的EnumItem</returns>
        public static EnumItem ConvertToEnumItem<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            return new EnumItem
            {
                Value = Convert.ToInt32(enumValue),
                Description = GetEnumDescription(enumType, enumValue.ToString())
            };
        }

        /// <summary>
        /// 将EnumItem转换为对应的枚举值
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="item">EnumItem实例</param>
        /// <returns>对应的枚举值</returns>
        public static TEnum ConvertToEnum<TEnum>(EnumItem item) where TEnum : Enum
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), item.Value);
        }
        /// <summary>
        /// 枚举项数据结构
        /// </summary>
        public class EnumItem
        {
            public int Value { get; set; }
            public string Description { get; set; }
        }
        #endregion

        #region 对象列表绑定方法

        /// <summary>
        /// 为 CheckedListBox 绑定对象列表（支持多选）
        /// </summary>
        public static void BindData4CheckedListBox<TEntity, TItem>(
            TEntity entity,
            Expression<Func<TEntity, List<long>>> propertyExpression,
            Krypton.Toolkit.KryptonCheckedListBox checkedList,
            IEnumerable<TItem> dataSource,
            Expression<Func<TItem, long>> idExpression,
            Expression<Func<TItem, string>> displayExpression,
            params long[] excludeIds)
        {
            // 清除现有绑定
            checkedList.DataBindings.Clear();
            checkedList.Items.Clear();

            // 获取属性信息
            var propertyInfo = GetPropertyInfo(propertyExpression);

            // 编译表达式获取值
            var idFunc = idExpression.Compile();
            var displayFunc = displayExpression.Compile();

            // 处理数据源（排除指定ID）
            var itemList = dataSource
                .Where(item => !excludeIds.Contains(idFunc(item)))
                .ToList();

            // 创建键值对列表
            var keyValueItems = itemList
                .Select(item => new KeyValueItem<long>
                {
                    Value = idFunc(item),
                    Description = displayFunc(item)
                })
                .ToList();

            // 绑定到CheckedListBox
            foreach (var item in keyValueItems)
            {
                checkedList.Items.Add(item.Description);
            }

            // 设置初始选中状态
            SetInitialCheckedStates(entity, propertyInfo, checkedList, keyValueItems);

            // 绑定值变更事件
            checkedList.ItemCheck += (sender, e) =>
                UpdateEntityFromCheckList(entity, propertyInfo, checkedList, keyValueItems, e);
        }

        /// <summary>
        /// 通用方法：为 CheckedListBox 绑定对象列表（支持任意值类型）
        /// </summary>
        public static void BindData4CheckedListBox<TEntity, TValue, TItem>(
            TEntity entity,
            Expression<Func<TEntity, List<TValue>>> propertyExpression,
            Krypton.Toolkit.KryptonCheckedListBox checkedList,
            IEnumerable<TItem> dataSource,
            Expression<Func<TItem, TValue>> idExpression,
            Expression<Func<TItem, string>> displayExpression,
            params TValue[] excludeValues)
        {
            // 清除现有绑定
            checkedList.DataBindings.Clear();
            checkedList.Items.Clear();

            // 获取属性信息
            var propertyInfo = GetPropertyInfo(propertyExpression);

            // 编译表达式获取值
            var idFunc = idExpression.Compile();
            var displayFunc = displayExpression.Compile();

            // 处理数据源（排除指定值）
            var excludeSet = new HashSet<TValue>(excludeValues);
            var itemList = dataSource
                .Where(item => !excludeSet.Contains(idFunc(item)))
                .ToList();

            // 创建键值对列表
            var keyValueItems = itemList
                .Select(item => new KeyValueItem<TValue>
                {
                    Value = idFunc(item),
                    Description = displayFunc(item)
                })
                .ToList();

            // 绑定到CheckedListBox
            foreach (var item in keyValueItems)
            {
                checkedList.Items.Add(item.Description);
            }

            // 设置初始选中状态
            SetInitialCheckedStates(entity, propertyInfo, checkedList, keyValueItems);

            // 绑定值变更事件
            checkedList.ItemCheck += (sender, e) =>
                UpdateEntityFromCheckList(entity, propertyInfo, checkedList, keyValueItems, e);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取属性信息
        /// </summary>
        private static PropertyInfo GetPropertyInfo<TEntity, TValue>(
            Expression<Func<TEntity, TValue>> propertyExpression)
        {
            if (propertyExpression.Body is MemberExpression memberExpr)
                return (PropertyInfo)memberExpr.Member;

            throw new ArgumentException("无效的属性表达式");
        }

        /// <summary>
        /// 设置初始选中状态
        /// </summary>
        private static void SetInitialCheckedStates<TEntity, TValue>(
            TEntity entity,
            PropertyInfo propertyInfo,
            Krypton.Toolkit.KryptonCheckedListBox checkedList,
            List<KeyValueItem<TValue>> keyValueItems)
        {
            var selectedValues = (List<TValue>)propertyInfo.GetValue(entity);
            if (selectedValues == null) return;

            for (int i = 0; i < checkedList.Items.Count; i++)
            {
                var itemValue = keyValueItems[i].Value;
                checkedList.SetItemChecked(i, selectedValues.Contains(itemValue));
            }
        }

        /// <summary>
        /// 更新实体值
        /// </summary>
        private static void UpdateEntityFromCheckList<TEntity, TValue>(
            TEntity entity,
            PropertyInfo propertyInfo,
            Krypton.Toolkit.KryptonCheckedListBox checkedList,
            List<KeyValueItem<TValue>> keyValueItems,
            ItemCheckEventArgs e)
        {
            // 获取当前所有选中项的值
            var selectedValues = new List<TValue>();

            // 处理已选中项
            for (int i = 0; i < checkedList.Items.Count; i++)
            {
                if (checkedList.GetItemChecked(i) && i != e.Index)
                {
                    selectedValues.Add(keyValueItems[i].Value);
                }
            }

            // 处理当前变更项
            var currentItemValue = keyValueItems[e.Index].Value;
            if (e.NewValue == CheckState.Checked)
            {
                selectedValues.Add(currentItemValue);
            }
            else
            {
                selectedValues.Remove(currentItemValue);
            }

            // 更新实体属性
            propertyInfo.SetValue(entity, selectedValues.Distinct().ToList());
        }

        #endregion

        #region 数据结构

        /// <summary>
        /// 键值对数据项
        /// </summary>
        public class KeyValueItem<TValue>
        {
            public TValue Value { get; set; }
            public string Description { get; set; }
        }

        #endregion

    }

}
