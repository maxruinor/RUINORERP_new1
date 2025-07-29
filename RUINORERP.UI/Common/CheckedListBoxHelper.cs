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

namespace RUINORERP.UI.Common
{


    public static class CheckedListBoxHelper_old
    {





        /// <summary>
        /// 绑定枚举到CheckedListBox（通过表达式指定属性）
        /// </summary>
        public static void BindData4CheckedListByEnum<TEntity, TEnum>(object entity,
            Expression<Func<TEntity, List<int>?>> exp, CheckedListBox checkedList,
            params TEnum[] excludeEnums) where TEnum : Enum
        {
            checkedList.DataBindings.Clear();
            MemberInfo minfo = exp.GetMemberInfo();
            string key = minfo.Name;
            BindData4CheckedListByEnum<TEnum>(entity, key, checkedList, excludeEnums);
        }

        /// <summary>
        /// 绑定枚举到CheckedListBox（通过属性名指定属性）
        /// </summary>
        public static void BindData4CheckedListByEnum<TEnum>(object entity, string propertyName,
            CheckedListBox checkedList, params TEnum[] excludeEnums) where TEnum : Enum
        {
            // 存储属性名用于数据绑定
            checkedList.Tag = propertyName;

            // 初始化列表数据
            var dataSource = InitDataToCheckedListByEnum<TEnum>(excludeEnums);
            checkedList.DataSource = dataSource;
            checkedList.DisplayMember = "Name";
            checkedList.ValueMember = "Value";

            // 设置初始选中状态
            SetInitialCheckStates(entity, propertyName, checkedList);

            // 绑定选中状态变更事件
            checkedList.ItemCheck += (sender, e) =>
                UpdateEntityFromCheckList(entity, propertyName, checkedList);
        }

        /// <summary>
        /// 初始化CheckedListBox的枚举数据源
        /// </summary>
        private static List<EnumItem> InitDataToCheckedListByEnum<TEnum>(params TEnum[] excludeEnums)
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            var enumValues = Enum.GetValues(enumType).Cast<TEnum>()
                .Where(e => !excludeEnums.Contains(e))
                .ToList();

            var items = new List<EnumItem>();

            foreach (var enumValue in enumValues)
            {
                string name = enumValue.ToString();
                var fieldInfo = enumType.GetField(name);

                // 获取Description属性
                var descriptionAttr = fieldInfo?
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute;

                items.Add(new EnumItem
                {
                    Value = Convert.ToInt32(enumValue),
                    Name = descriptionAttr?.Description ?? name,
                    EnumValue = enumValue
                });
            }

            return items;
        }

        /// <summary>
        /// 设置初始选中状态
        /// </summary>
        private static void SetInitialCheckStates(object entity, string propertyName, CheckedListBox checkedList)
        {
            if (entity == null) return;

            // 获取实体属性值（应该是一个整数列表）
            var propertyValue = entity.GetType().GetProperty(propertyName)?
                .GetValue(entity) as List<int>;

            if (propertyValue == null || !propertyValue.Any()) return;

            // 设置选中状态
            for (int i = 0; i < checkedList.Items.Count; i++)
            {
                var item = checkedList.Items[i] as EnumItem;
                if (item != null && propertyValue.Contains(item.Value))
                {
                    checkedList.SetItemChecked(i, true);
                }
            }
        }

        /// <summary>
        /// 根据列表选中状态更新实体
        /// </summary>
        private static void UpdateEntityFromCheckList(object entity, string propertyName, CheckedListBox checkedList)
        {
            if (entity == null) return;

            // 获取所有选中项的值
            var checkedValues = checkedList.CheckedItems.Cast<EnumItem>()
                .Select(item => item.Value)
                .ToList();

            // 更新实体属性
            var property = entity.GetType().GetProperty(propertyName);
            if (property != null)
            {
                property.SetValue(entity, checkedValues);
            }
        }

        /// <summary>
        /// 枚举项数据结构
        /// </summary>
        public class EnumItem
        {
            public int Value { get; set; }
            public string Name { get; set; }
            public object EnumValue { get; set; }
        }
    }

    public static partial class CheckedListBoxHelper
    {
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
        CheckedListBox checkedList,
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
            var dt = enumItems.ToDataTable<EnumItem>();
            // 2. 使用BindingSource作为中间层（关键修正）
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = enumItems;  // 将列表交给BindingSource

            // 假设窗体上有 checkedListBox1
            BindingList<Person> people = new BindingList<Person>
            {
                new Person { Name = "张三", IsSelected = true },
                new Person { Name = "李四", IsSelected = false },
                new Person { Name = "王五", IsSelected = true }
            };

            // 3. 绑定到CheckedListBox
            // checkedList.DataSource = bindingSource;  // 绑定BindingSource而非直接绑定列表
            checkedList.DisplayMember = "Name";  // 对应EnumItem的Description属性
            checkedList.ValueMember = "Name";        // 对应EnumItem的Value属性
            
            checkedList.DataSource = people;


            // 设置初始选中状态
            SetInitialCheckedStates(entity, propertyInfo, checkedList);

            // 绑定值变更事件（同步到实体）
            checkedList.ItemCheck += (sender, e) =>
            UpdateEntityFromCheckList(entity, propertyInfo, checkedList, e);
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
        /// 获取枚举的 Description 属性
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
        private static void SetInitialCheckedStates<TEntity>(
        TEntity entity,
        PropertyInfo propertyInfo,
        CheckedListBox checkedList)
        {
            var selectedValues = (List<int>)propertyInfo.GetValue(entity);
            if (selectedValues == null) return;

            for (int i = 0; i < checkedList.Items.Count; i++)
            {
                var item = (EnumItem)checkedList.Items[i];
                if (selectedValues.Contains(item.Value))
                {
                    checkedList.SetItemChecked(i, true);
                }
            }
        }

        /// <summary>
        /// 根据选中状态更新实体
        /// </summary>
        private static void UpdateEntityFromCheckList<TEntity>(
        TEntity entity,
        PropertyInfo propertyInfo,
        CheckedListBox checkedList,
        ItemCheckEventArgs e)
        {
            // 获取当前所有选中项的值
            var selectedValues = checkedList.CheckedItems
            .Cast<EnumItem>()
            .Select(item => item.Value)
            .ToList();

            // 处理正在变更的项
            var currentItem = (EnumItem)checkedList.Items[e.Index];
            if (e.NewValue == CheckState.Checked)
            {
                selectedValues.Add(currentItem.Value);
            }
            else
            {
                selectedValues.Remove(currentItem.Value);
            }

            // 更新实体属性
            propertyInfo.SetValue(entity, selectedValues.Distinct().ToList());
        }

        /// <summary>
        /// 枚举项数据结构
        /// </summary>
        public class EnumItem
        {
            public int Value { get; set; }
            public string Description { get; set; }
        }
    }
    public class Person
    {
        public string Name { get; set; }

        // 这个属性决定 CheckState
        public bool IsSelected { get; set; }
    }
}
