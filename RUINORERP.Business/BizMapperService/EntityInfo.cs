using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 实体信息类 - 包含实体的完整元数据
    /// </summary>
    public class EntityInfo
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public BizType BizType { get; internal set; } = BizType.无对应数据;
        
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; internal set; }
        
        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName => EntityType?.Name;
        
        /// <summary>
        /// 实体完整类型名称（包含命名空间）
        /// </summary>
        public string FullTypeName => EntityType?.AssemblyQualifiedName;
        
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; internal set; }
        
        /// <summary>
        /// 表描述
        /// </summary>
        public string TableDescription { get; internal set; }
        
        /// <summary>
        /// 主键字段名
        /// </summary>
        public string IdField { get; internal set; }
        
        /// <summary>
        /// 编号字段名
        /// </summary>
        public string NoField { get; internal set; }
        
        /// <summary>
        /// 描述字段名
        /// </summary>
        public string DescriptionField { get; internal set; }
        
        /// <summary>
        /// 明细属性名
        /// </summary>
        public string DetailProperty { get; internal set; }
        
        /// <summary>
        /// 共用表区分器字段
        /// </summary>
        public string DiscriminatorField { get; internal set; }
        
        /// <summary>
        /// 共用表类型解析器
        /// </summary>
        public Func<object, BizType> TypeResolver { get; internal set; }
        
        /// <summary>
        /// 实体字段信息字典
        /// </summary>
        public Dictionary<string, EntityFieldInfo> Fields { get; internal set; }
            = new Dictionary<string, EntityFieldInfo>();
        
        /// <summary>
        /// 获取字段信息
        /// </summary>
        public EntityFieldInfo GetField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return null;
            
            Fields.TryGetValue(fieldName, out var fieldInfo);
            return fieldInfo;
        }
    }
    
    /// <summary>
    /// 实体字段信息类
    /// </summary>
    public class EntityFieldInfo
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; internal set; }
        
        /// <summary>
        /// 字段类型
        /// </summary>
        public Type FieldType { get; internal set; }
        
        /// <summary>
        /// 字段描述
        /// </summary>
        public string Description { get; internal set; }
        
        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsPrimaryKey { get; internal set; }
        
        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsNullable { get; internal set; }
    }
    
    /// <summary>
    /// 实体信息构建器 - 用于流式配置实体信息
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityInfoBuilder<TEntity> where TEntity : class
    {
        private readonly EntityInfo _entityInfo;
        
        internal EntityInfoBuilder()
        {
            _entityInfo = new EntityInfo
            {
                EntityType = typeof(TEntity),
                TableName = typeof(TEntity).Name
            };
        }
        
        /// <summary>
        /// 设置业务类型
        /// </summary>
        public EntityInfoBuilder<TEntity> WithBizType(BizType bizType)
        {
            _entityInfo.BizType = bizType;
            return this;
        }
        
        /// <summary>
        /// 设置表名
        /// </summary>
        public EntityInfoBuilder<TEntity> WithTableName(string tableName)
        {
            _entityInfo.TableName = tableName;
            return this;
        }
        
        /// <summary>
        /// 设置表描述
        /// </summary>
        public EntityInfoBuilder<TEntity> WithDescription(string description)
        {
            _entityInfo.TableDescription = description;
            return this;
        }
        
        /// <summary>
        /// 设置主键字段
        /// </summary>
        public EntityInfoBuilder<TEntity> WithIdField<TKey>(Expression<Func<TEntity, TKey>> idSelector)
        {
            var fieldName = GetMemberName(idSelector);
            _entityInfo.IdField = fieldName;
            
            // 添加字段信息
            AddFieldInfo(fieldName, typeof(TKey), true);
            
            return this;
        }
        
        /// <summary>
        /// 设置编号字段
        /// </summary>
        public EntityInfoBuilder<TEntity> WithNoField(Expression<Func<TEntity, string>> noSelector)
        {
            var fieldName = GetMemberName(noSelector);
            _entityInfo.NoField = fieldName;
            
            // 添加字段信息
            AddFieldInfo(fieldName, typeof(string));
            
            return this;
        }
        
        /// <summary>
        /// 设置描述字段
        /// </summary>
        public EntityInfoBuilder<TEntity> WithDescriptionField(Expression<Func<TEntity, string>> descriptionSelector)
        {
            var fieldName = GetMemberName(descriptionSelector);
            _entityInfo.DescriptionField = fieldName;
            
            // 添加字段信息
            AddFieldInfo(fieldName, typeof(string));
            
            return this;
        }
        
        /// <summary>
        /// 设置明细属性
        /// </summary>
        public EntityInfoBuilder<TEntity> WithDetailProperty<TDetail>(Expression<Func<TEntity, TDetail>> detailSelector)
        {
            _entityInfo.DetailProperty = GetMemberName(detailSelector);
            return this;
        }
        
        /// <summary>
        /// 设置共用表区分器
        /// </summary>
        public EntityInfoBuilder<TEntity> WithDiscriminator<TDiscriminator>(
            Expression<Func<TEntity, TDiscriminator>> discriminatorSelector,
            Func<TDiscriminator, BizType> typeResolver)
        {
            var fieldName = GetMemberName(discriminatorSelector);
            _entityInfo.DiscriminatorField = fieldName;
            _entityInfo.TypeResolver = value => typeResolver((TDiscriminator)value);
            
            // 添加字段信息
            AddFieldInfo(fieldName, typeof(TDiscriminator));
            
            return this;
        }
        
        /// <summary>
        /// 注册字段信息
        /// </summary>
        public EntityInfoBuilder<TEntity> WithField<TField>(Expression<Func<TEntity, TField>> fieldSelector, 
            string description = null, bool isNullable = true)
        {
            var fieldName = GetMemberName(fieldSelector);
            AddFieldInfo(fieldName, typeof(TField), false, description, isNullable);
            return this;
        }
        
        /// <summary>
        /// 添加字段信息
        /// </summary>
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
        
        internal EntityInfo Build()
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