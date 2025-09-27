using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// ERP实体信息类，包含实体的元数据信息
    /// </summary>
    [Serializable()]
    public class ERPEntityInfo : ICloneable
    {
        /// <summary>
        /// 付款类型的值对应业务类型收/付款
        /// </summary>
        public IDictionary<int, BizType> EnumMaper { get; set; }
        public BizType BizType { get; internal set; } = BizType.无对应数据;
        public Type EntityType { get; internal set; }
        public string EntityName => EntityType?.Name;
        public string FullTypeName => EntityType?.AssemblyQualifiedName;
        public string TableName { get; internal set; }
        public string TableDescription { get; internal set; }
        public string IdField { get; internal set; }
        public string NoField { get; internal set; }
        public string DescriptionField { get; internal set; }
        public string DetailProperty { get; internal set; }
        public string DiscriminatorField { get; internal set; }
        public Func<object, BizType> TypeResolver { get; internal set; }
        public Dictionary<string, EntityFieldInfo> Fields { get; internal set; } = new Dictionary<string, EntityFieldInfo>();

        public EntityFieldInfo GetField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return null;

            Fields.TryGetValue(fieldName, out var fieldInfo);
            return fieldInfo;
        }

        /// <summary>
        /// 创建当前对象的浅表副本
        /// </summary>
        /// <returns>当前对象的浅表副本</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// 创建当前对象的深表副本
        /// </summary>
        /// <returns>当前对象的深表副本</returns>
        public ERPEntityInfo DeepClone()
        {
            var clone = (ERPEntityInfo)this.MemberwiseClone();
            
            // 深拷贝Fields字典
            if (this.Fields != null)
            {
                clone.Fields = new Dictionary<string, EntityFieldInfo>();
                foreach (var kvp in this.Fields)
                {
                    // EntityFieldInfo是值类型或者不可变类型，可以直接复制
                    clone.Fields[kvp.Key] = kvp.Value != null ? (EntityFieldInfo)kvp.Value.Clone() : null;
                }
            }
            
            // 深拷贝EnumMaper字典
            if (this.EnumMaper != null)
            {
                clone.EnumMaper = new Dictionary<int, BizType>();
                foreach (var kvp in this.EnumMaper)
                {
                    clone.EnumMaper[kvp.Key] = kvp.Value;
                }
            }
            
            return clone;
        }
    }

    /// <summary>
    /// 实体字段信息类，包含字段的元数据信息
    /// </summary>
    [Serializable()]
    public class EntityFieldInfo : ICloneable
    {
        public string FieldName { get; internal set; }
        public Type FieldType { get; internal set; }
        public string Description { get; internal set; }
        public bool IsPrimaryKey { get; internal set; }
        public bool IsNullable { get; internal set; }

        /// <summary>
        /// 创建当前对象的副本
        /// </summary>
        /// <returns>当前对象的副本</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class EntityInfoBuilder<TEntity> where TEntity : class
    {
        private ERPEntityInfo _entityInfo;

        internal EntityInfoBuilder()
        {
            _entityInfo = new ERPEntityInfo
            {
                EntityType = typeof(TEntity),
                TableName = typeof(TEntity).Name
            };
        }


        public ERPEntityInfo ERPEntityInfo { get { return _entityInfo; } set { _entityInfo = value; } }


        public EntityInfoBuilder<TEntity> WithBizType(BizType bizType)
        {
            _entityInfo.BizType = bizType;
            return this;
        }

        public EntityInfoBuilder<TEntity> WithTableName(string tableName)
        {
            _entityInfo.TableName = tableName;
            return this;
        }

        public EntityInfoBuilder<TEntity> WithDescription(string description)
        {
            _entityInfo.TableDescription = description;
            return this;
        }

        public EntityInfoBuilder<TEntity> WithIdField<TKey>(Expression<Func<TEntity, TKey>> idSelector)
        {
            var fieldName = GetMemberName(idSelector);
            _entityInfo.IdField = fieldName;
            AddFieldInfo(fieldName, typeof(TKey), true);
            return this;
        }

        public EntityInfoBuilder<TEntity> WithNoField(Expression<Func<TEntity, string>> noSelector)
        {
            var fieldName = GetMemberName(noSelector);
            _entityInfo.NoField = fieldName;
            AddFieldInfo(fieldName, typeof(string));
            return this;
        }

        public EntityInfoBuilder<TEntity> WithDescriptionField(Expression<Func<TEntity, string>> descriptionSelector)
        {
            var fieldName = GetMemberName(descriptionSelector);
            _entityInfo.DescriptionField = fieldName;
            AddFieldInfo(fieldName, typeof(string));
            return this;
        }

        /// <summary>
        /// 付款类型的枚举值。决定是收款单，还是付款单的类似 情况
        /// </summary>
        /// <typeparam name="TDiscriminator">判别器类型</typeparam>
        /// <param name="enumMaper">枚举映射字典</param>
        /// <returns>实体信息构建器</returns>
        public EntityInfoBuilder<TEntity> WithAddMaper<TDiscriminator>(IDictionary<TDiscriminator, BizType> enumMaper)
        {
            Dictionary<int, BizType> EnumMaper = new();
            foreach (var item in enumMaper)
            {
                int key = item.Key.ObjToInt();
                // 检查键是否已存在，避免重复添加
                if (!EnumMaper.ContainsKey(key))
                {
                    EnumMaper.Add(key, item.Value);
                }
            }
            _entityInfo.EnumMaper = EnumMaper;
            return this;
        }

        public EntityInfoBuilder<TEntity> WithDetailProperty<TDetail>(Expression<Func<TEntity, TDetail>> detailSelector)
        {
            _entityInfo.DetailProperty = GetMemberName(detailSelector);
            return this;
        }

        public EntityInfoBuilder<TEntity> WithDiscriminator<TDiscriminator>(
            Expression<Func<TEntity, TDiscriminator>> discriminatorSelector,
            Func<TDiscriminator, BizType> typeResolver)
        {
            var fieldName = GetMemberName(discriminatorSelector);
            _entityInfo.DiscriminatorField = fieldName;
            _entityInfo.TypeResolver = value => typeResolver((TDiscriminator)value);
            AddFieldInfo(fieldName, typeof(TDiscriminator));
            return this;
        }

        public EntityInfoBuilder<TEntity> WithField<TField>(Expression<Func<TEntity, TField>> fieldSelector,
            string description = null, bool isNullable = true)
        {
            var fieldName = GetMemberName(fieldSelector);
            AddFieldInfo(fieldName, typeof(TField), false, description, isNullable);
            return this;
        }

        private void AddFieldInfo(string fieldName, Type fieldType, bool isPrimaryKey = false,
            string description = null, bool isNullable = true)
        {
            if (string.IsNullOrEmpty(fieldName) || fieldType == null)
                return;

            var fieldInfo = new EntityFieldInfo
            {
                FieldName = fieldName,
                FieldType = fieldType,
                Description = description,
                IsPrimaryKey = isPrimaryKey,
                IsNullable = isNullable
            };

            _entityInfo.Fields[fieldName] = fieldInfo;
        }

        internal ERPEntityInfo Build()
        {
            return _entityInfo;
        }

        private string GetMemberName<T>(Expression<Func<TEntity, T>> expression)
        {
            if (expression.Body is MemberExpression memberExpr)
            {
                return memberExpr.Member.Name;
            }

            if (expression.Body is UnaryExpression unaryExpr && unaryExpr.Operand is MemberExpression memberExpr2)
            {
                return memberExpr2.Member.Name;
            }

            throw new ArgumentException("Expression must be a member access expression.", nameof(expression));
        }
    }
}