using RUINORERP.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 忽略属性配置类
    /// 复制性新增时，增强的忽略属性配置类，支持表达式和字符串混合使用
    /// </summary>
    // 增强的忽略属性配置类，支持表达式和字符串混合使用
    public class IgnorePropertyConfiguration
    {
        private readonly Dictionary<Type, HashSet<string>> _ignoredProperties =
            new Dictionary<Type, HashSet<string>>();

        // 使用表达式树添加要忽略的属性（类型安全）
        public IgnorePropertyConfiguration Ignore<T>(Expression<Func<T, object>> expression)
        {
            try
            {
                var propertyName = expression.GetMemberInfo().Name;
                var type = typeof(T);

                if (!_ignoredProperties.ContainsKey(type))
                {
                    _ignoredProperties[type] = new HashSet<string>();
                }

                // 检查属性是否存在
                if (type.GetProperty(propertyName) != null)
                {
                    _ignoredProperties[type].Add(propertyName);
                }
                else
                {
                    // 记录日志或执行其他处理
                    Debug.WriteLine($"警告: 类型 {type.Name} 没有属性 {propertyName}");
                }

                return this;
            }
            catch (Exception ex)
            {
                // 处理表达式解析错误
                Debug.WriteLine($"表达式解析错误: {ex.Message}");
                return this;
            }
        }

        // 添加多个要忽略的属性（类型安全）
        public IgnorePropertyConfiguration Ignore<T>(params Expression<Func<T, object>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                Ignore(expression);
            }
            return this;
        }

        // 使用字符串添加要忽略的属性（灵活但需要小心使用）
        public IgnorePropertyConfiguration Ignore<T>(string propertyName)
        {
            var type = typeof(T);

            if (!_ignoredProperties.ContainsKey(type))
            {
                _ignoredProperties[type] = new HashSet<string>();
            }

            // 检查属性是否存在
            if (type.GetProperty(propertyName) != null)
            {
                _ignoredProperties[type].Add(propertyName);
            }
            else
            {
                // 记录日志或执行其他处理
                Debug.WriteLine($"警告: 类型 {type.Name} 没有属性 {propertyName}");
            }

            return this;
        }

        // 安全地添加要忽略的属性（如果属性存在）
        public IgnorePropertyConfiguration IgnoreIfExists<T>(string propertyName)
        {
            var type = typeof(T);
            var property = type.GetProperty(propertyName);

            if (property != null)
            {
                if (!_ignoredProperties.ContainsKey(type))
                {
                    _ignoredProperties[type] = new HashSet<string>();
                }

                _ignoredProperties[type].Add(propertyName);
            }

            return this;
        }

        // 添加多个要忽略的属性（字符串方式）
        public IgnorePropertyConfiguration Ignore<T>(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                Ignore<T>(propertyName);
            }
            return this;
        }

        // 合并两个配置
        public IgnorePropertyConfiguration Merge(IgnorePropertyConfiguration other)
        {
            if (other == null) return this;

            foreach (var kvp in other._ignoredProperties)
            {
                if (!_ignoredProperties.ContainsKey(kvp.Key))
                {
                    _ignoredProperties[kvp.Key] = new HashSet<string>();
                }

                foreach (var propertyName in kvp.Value)
                {
                    _ignoredProperties[kvp.Key].Add(propertyName);
                }
            }

            return this;
        }

        // 获取指定类型的忽略属性列表
        public IReadOnlyCollection<string> GetIgnoredProperties(Type type)
        {
            return _ignoredProperties.ContainsKey(type) ?
                _ignoredProperties[type].ToList().AsReadOnly() :
                new List<string>().AsReadOnly();
        }

        // 检查属性是否应该被忽略
        public bool ShouldIgnore(Type type, string propertyName)
        {
            return _ignoredProperties.ContainsKey(type) &&
                   _ignoredProperties[type].Contains(propertyName);
        }
    }
}
