using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RUINORERP.Model;
using RUINORERP.Common.Helper;

namespace RUINORERP.Business.Base
{
    /// <summary>
    /// 实体状态保护器 - 用于在事务执行过程中保护实体状态
    /// 当事务回滚时，可以将内存中的实体状态恢复到事务开始前的状态
    /// </summary>
    public class EntityStateProtector
    {
        // 存储实体快照字典：<实体引用, 克隆副本或选择性快照>
        private Dictionary<object, object> _entitySnapshots = new();
        
        // 存储选择性保护的字段信息：<实体引用, 字段名列表>
        private Dictionary<object, List<string>> _selectiveFields = new();

        /// <summary>
        /// 记录需要保护的实体（完整克隆）
        /// </summary>
        public void Protect<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity != null && !_entitySnapshots.ContainsKey(entity))
            {
                // ✅ 关键修复：使用运行时实际类型进行克隆，避免类型丢失
                var entityType = entity.GetType();
                object snapshot;
                
                try
                {
                    // 使用反射调用泛型方法，确保保持具体类型
                    var cloneMethod = typeof(CloneHelper).GetMethod("DeepCloneObject", new[] { entityType });
                    if (cloneMethod != null)
                    {
                        var genericCloneMethod = cloneMethod.MakeGenericMethod(entityType);
                        snapshot = genericCloneMethod.Invoke(null, new object[] { entity });
                    }
                    else
                    {
                        // 回退：尝试使用泛型版本
                        var genericMethod = typeof(CloneHelper).GetMethods()
                            .FirstOrDefault(m => m.Name == "DeepCloneObject" && m.IsGenericMethodDefinition);
                        
                        if (genericMethod != null)
                        {
                            var specificMethod = genericMethod.MakeGenericMethod(entityType);
                            snapshot = specificMethod.Invoke(null, new object[] { entity });
                        }
                        else
                        {
                            // 最后回退：非泛型版本
                            snapshot = CloneHelper.DeepCloneObject(entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"克隆实体失败: {ex.Message}，使用回退方案");
                    snapshot = CloneHelper.DeepCloneObject(entity);
                }
                
                _entitySnapshots[entity] = snapshot;
                _selectiveFields[entity] = null; // null表示完整克隆
            }
        }

        /// <summary>
        /// 选择性保护实体 - 只保护指定的字段
        /// 使用Lambda表达式指定字段，类型安全且IDE友好
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">要保护的实体</param>
        /// <param name="propertyExpressions">要保护的属性表达式列表，如: e => e.DataStatus, e => e.ApprovalStatus</param>
        /// <example>
        /// <code>
        /// // 只保护状态相关字段
        /// StateProtector.ProtectSelective(entity, 
        ///     e => e.DataStatus, 
        ///     e => e.ApprovalStatus,
        ///     e => e.ExecutionStatus);
        /// </code>
        /// </example>
        public void ProtectSelective<TEntity>(
            TEntity entity, 
            params Expression<Func<TEntity, object>>[] propertyExpressions) where TEntity : BaseEntity
        {
            if (entity == null || _entitySnapshots.ContainsKey(entity))
                return;

            // 提取属性名列表
            var propertyNames = propertyExpressions
                .Select(expr => GetPropertyName(expr))
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList();

            if (propertyNames.Count == 0)
            {
                throw new ArgumentException("至少需要指定一个要保护的属性", nameof(propertyExpressions));
            }

            // 创建选择性快照（只复制指定字段）
            var snapshot = CreateSelectiveSnapshot(entity, propertyNames);
            
            _entitySnapshots[entity] = snapshot;
            _selectiveFields[entity] = propertyNames;
        }

        /// <summary>
        /// 批量保护实体（完整克隆）
        /// </summary>
        public void ProtectRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            foreach (var entity in entities)
            {
                Protect(entity);
            }
        }

        /// <summary>
        /// 批量选择性保护实体
        /// </summary>
        public void ProtectRangeSelective<TEntity>(
            IEnumerable<TEntity> entities,
            params Expression<Func<TEntity, object>>[] propertyExpressions) where TEntity : BaseEntity
        {
            foreach (var entity in entities)
            {
                ProtectSelective(entity, propertyExpressions);
            }
        }

        /// <summary>
        /// 恢复所有实体到原始状态
        /// 根据保护类型自动选择完整恢复或选择性恢复
        /// </summary>
        public void RestoreAll()
        {
            foreach (var kvp in _entitySnapshots)
            {
                var entity = kvp.Key;
                var snapshot = kvp.Value;

                // 检查是否是选择性保护
                if (_selectiveFields.TryGetValue(entity, out var fieldNames) && fieldNames != null)
                {
                    // 选择性恢复：只恢复指定字段
                    RestoreSelectiveFields(entity, snapshot, fieldNames);
                }
                else
                {
                    // 完整恢复：直接遍历属性赋值，避免反射调用外部方法
                    RestoreEntityValuesInternal(entity, snapshot);
                }
            }
        }

        /// <summary>
        /// 内部方法：直接遍历属性恢复实体值
        /// 简单高效的属性拷贝，避免复杂的反射调用
        /// </summary>
        private void RestoreEntityValuesInternal(object entity, object snapshot)
        {
            if (entity == null || snapshot == null) return;
            
            var entityType = entity.GetType();
            var properties = entityType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            
            foreach (var property in properties)
            {
                // 跳过只读属性和导航属性
                if (!property.CanWrite || !property.CanRead) continue;
                
                // 跳过导航属性（标记为 IsIgnore 或有 Navigate 特性）
                var sugarColumn = property.GetCustomAttribute<SqlSugar.SugarColumn>();
                if (sugarColumn?.IsIgnore == true) continue;
                
                var navigateAttr = property.GetCustomAttribute<SqlSugar.Navigate>();
                if (navigateAttr != null) continue;
                
                try
                {
                    var value = property.GetValue(snapshot);
                    property.SetValue(entity, value);
                }
                catch (Exception ex)
                {
                    // 记录但不中断，继续恢复其他属性
                    System.Diagnostics.Debug.WriteLine($"恢复属性 {property.Name} 失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 清理快照（事务成功后调用）
        /// </summary>
        public void Clear()
        {
            _entitySnapshots.Clear();
            _selectiveFields.Clear();
        }

        /// <summary>
        /// 检查是否已经有实体被保护
        /// </summary>
        public bool HasProtectedEntities => _entitySnapshots.Count > 0;

        #region 私有辅助方法

        /// <summary>
        /// 从Lambda表达式中提取属性名
        /// </summary>
        private string GetPropertyName<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
                return null;

            var body = expression.Body;

            // 处理转换表达式（值类型转object时会产生Convert）
            if (body is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
            {
                body = unary.Operand;
            }

            // 提取成员表达式
            if (body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            return null;
        }

        /// <summary>
        /// 创建选择性快照（只复制指定字段）
        /// </summary>
        private TEntity CreateSelectiveSnapshot<TEntity>(TEntity entity, List<string> propertyNames) where TEntity : BaseEntity
        {
            var snapshot = Activator.CreateInstance<TEntity>();
            var entityType = typeof(TEntity);

            foreach (var propertyName in propertyNames)
            {
                var propertyInfo = entityType.GetProperty(propertyName);
                if (propertyInfo != null && propertyInfo.CanRead && propertyInfo.CanWrite)
                {
                    var value = propertyInfo.GetValue(entity);
                    
                    // 对于复杂类型，进行深克隆；简单类型直接赋值
                    if (value != null && IsComplexType(propertyInfo.PropertyType))
                    {
                        var clonedValue = CloneHelper.DeepCloneObject(value);
                        propertyInfo.SetValue(snapshot, clonedValue);
                    }
                    else
                    {
                        propertyInfo.SetValue(snapshot, value);
                    }
                }
            }

            return snapshot;
        }

        /// <summary>
        /// 选择性恢复指定字段
        /// </summary>
        private void RestoreSelectiveFields(object entity, object snapshot, List<string> fieldNames)
        {
            var entityType = entity.GetType();

            foreach (var fieldName in fieldNames)
            {
                var propertyInfo = entityType.GetProperty(fieldName);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    var value = propertyInfo.GetValue(snapshot);
                    propertyInfo.SetValue(entity, value);
                }
            }
        }

        /// <summary>
        /// 判断是否为复杂类型（需要深克隆）
        /// </summary>
        private bool IsComplexType(Type type)
        {
            // 基本类型、字符串、DateTime、Decimal等不需要深克隆
            if (type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || 
                type == typeof(decimal) || type == typeof(Guid))
            {
                return false;
            }

            // 可空类型检查底层类型
            if (Nullable.GetUnderlyingType(type) != null)
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return IsComplexType(underlyingType);
            }

            // 其他类型视为复杂类型
            return true;
        }

        #endregion
    }
}