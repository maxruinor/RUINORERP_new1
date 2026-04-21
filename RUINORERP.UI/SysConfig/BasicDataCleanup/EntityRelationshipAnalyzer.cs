// **************************************
// 文件：EntityRelationshipAnalyzer.cs
// 项目：RUINORERP
// 作者：AI Assistant
// 时间：2026-04-20
// 描述：实体关系分析器，用于分析数据库表之间的外键依赖关系
// **************************************

using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 外键类型枚举
    /// </summary>
    public enum ForeignKeyType
    {
        /// <summary>
        /// 当前表引用其他表(OneToOne)
        /// </summary>
        ReferencesOther,

        /// <summary>
        /// 其他表引用当前表(OneToMany)
        /// </summary>
        ReferencedByOthers
    }

    /// <summary>
    /// 导航关系(基于SqlSugar的Navigate属性)
    /// </summary>
    public class NavigationRelation
    {
        /// <summary>
        /// 导航属性名称
        /// </summary>
        public string NavigationPropertyName { get; set; }

        /// <summary>
        /// 外键字段名
        /// </summary>
        public string ForeignKeyColumn { get; set; }

        /// <summary>
        /// 关联的实体类型
        /// </summary>
        public Type RelatedEntityType { get; set; }

        /// <summary>
        /// 关联的表名
        /// </summary>
        public string RelatedTableName { get; set; }

        /// <summary>
        /// 外键类型(引用方向)
        /// </summary>
        public ForeignKeyType Type { get; set; }

        /// <summary>
        /// 导航类型(OneToOne/OneToMany)
        /// </summary>
        public NavigateType NavigateType { get; set; }
    }

    /// <summary>
    /// 实体关系信息
    /// </summary>
    public class EntityRelationshipInfo
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 实体表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 实体描述
        /// </summary>
        public string EntityDescription { get; set; }

        /// <summary>
        /// 主键名称
        /// </summary>
        public string PrimaryKeyName { get; set; }

        /// <summary>
        /// 外键关系列表(该实体依赖的其他表) - 来自FKRelationAttribute
        /// </summary>
        public List<ForeignKeyRelation> ForeignKeyRelations { get; set; }

        /// <summary>
        /// 被引用关系列表(其他表依赖该实体的外键) - 来自FKRelationAttribute
        /// </summary>
        public List<ReferencedRelation> ReferencedRelations { get; set; }

        /// <summary>
        /// 导航关系列表 - 来自SqlSugar的Navigate属性
        /// </summary>
        public List<NavigationRelation> NavigationRelations { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EntityRelationshipInfo()
        {
            ForeignKeyRelations = new List<ForeignKeyRelation>();
            ReferencedRelations = new List<ReferencedRelation>();
            NavigationRelations = new List<NavigationRelation>();
        }
    }

    /// <summary>
    /// 外键关系
    /// </summary>
    public class ForeignKeyRelation
    {
        /// <summary>
        /// 外键字段名
        /// </summary>
        public string ForeignKeyColumn { get; set; }

        /// <summary>
        /// 引用的表名
        /// </summary>
        public string ReferencedTableName { get; set; }

        /// <summary>
        /// 引用的字段名
        /// </summary>
        public string ReferencedColumnName { get; set; }

        /// <summary>
        /// 引用的实体类型
        /// </summary>
        public Type ReferencedEntityType { get; set; }
    }

    /// <summary>
    /// 被引用关系
    /// </summary>
    public class ReferencedRelation
    {
        /// <summary>
        /// 引用该表的实体类型
        /// </summary>
        public Type ReferencingEntityType { get; set; }

        /// <summary>
        /// 引用该表的表名
        /// </summary>
        public string ReferencingTableName { get; set; }

        /// <summary>
        /// 外键字段名
        /// </summary>
        public string ForeignKeyColumn { get; set; }
    }

    /// <summary>
    /// 清理顺序节点
    /// </summary>
    public class CleanupOrderNode
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 清理顺序（数字越小越先清理）
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否已清理
        /// </summary>
        public bool IsCleaned { get; set; }
    }

    /// <summary>
    /// 实体关系分析器
    /// 用于分析数据库表之间的外键依赖关系，并生成清理顺序
    /// </summary>
    public class EntityRelationshipAnalyzer
    {
        private readonly ISqlSugarClient _db;
        private readonly Dictionary<string, EntityRelationshipInfo> _relationshipCache;
        private readonly List<Type> _entityTypes;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">数据库连接</param>
        public EntityRelationshipAnalyzer(ISqlSugarClient db)
        {
            _db = db;
            _relationshipCache = new Dictionary<string, EntityRelationshipInfo>();
            _entityTypes = new List<Type>();
            LoadEntityTypes();
        }

        /// <summary>
        /// 加载所有实体类型
        /// </summary>
        private void LoadEntityTypes()
        {
            var assembly = typeof(tb_Prod).Assembly;
            var entityTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType == typeof(BaseEntity))
                .ToList();
            _entityTypes.AddRange(entityTypes);
        }

        /// <summary>
        /// 获取实体关系信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>实体关系信息</returns>
        public EntityRelationshipInfo GetEntityRelationship(Type entityType)
        {
            string key = entityType.FullName;
            if (!_relationshipCache.ContainsKey(key))
            {
                _relationshipCache[key] = AnalyzeEntity(entityType);
            }
            return _relationshipCache[key];
        }

        /// <summary>
        /// 分析实体关系
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>实体关系信息</returns>
        private EntityRelationshipInfo AnalyzeEntity(Type entityType)
        {
            var info = new EntityRelationshipInfo
            {
                EntityType = entityType,
                TableName = GetTableName(entityType),
                EntityDescription = entityType.GetCustomAttribute<DescriptionAttribute>()?.Description ?? entityType.Name,
                PrimaryKeyName = GetPrimaryKeyName(entityType)
            };

            // 1. 分析FKRelationAttribute(已有)
            AnalyzeForeignKeys(entityType, info);

            // 2. 分析Navigate导航属性(新增)
            AnalyzeNavigationProperties(entityType, info);

            // 3. 去重合并
            MergeAndDeduplicateRelations(info);

            return info;
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>表名</returns>
        private string GetTableName(Type entityType)
        {
            var sugarTableAttr = entityType.GetCustomAttribute<SugarTable>();
            return sugarTableAttr?.TableName ?? entityType.Name;
        }

        /// <summary>
        /// 获取主键名称
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>主键名称</returns>
        private string GetPrimaryKeyName(Type entityType)
        {
            var properties = entityType.GetProperties()
                .Where(p => p.GetCustomAttribute<SugarColumn>()?.IsPrimaryKey == true)
                .ToList();

            if (properties.Any())
            {
                return properties.First().Name;
            }

            // 默认主键
            var defaultPK = entityType.GetProperty(entityType.Name + "ID");
            if (defaultPK != null)
            {
                return defaultPK.Name;
            }

            return "ID";
        }

        /// <summary>
        /// 分析外键关系
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="info">实体关系信息</param>
        private void AnalyzeForeignKeys(Type entityType, EntityRelationshipInfo info)
        {
            var properties = entityType.GetProperties()
                .Where(p => p.GetCustomAttribute<FKRelationAttribute>() != null)
                .ToList();

            foreach (var property in properties)
            {
                var fkAttr = property.GetCustomAttribute<FKRelationAttribute>();
                if (fkAttr != null)
                {
                    var fkRelation = new ForeignKeyRelation
                    {
                        ForeignKeyColumn = property.Name,
                        ReferencedTableName = fkAttr.FKTableName,
                        ReferencedColumnName = fkAttr.FK_IDColName,
                        ReferencedEntityType = FindEntityTypeByTableName(fkAttr.FKTableName)
                    };

                    info.ForeignKeyRelations.Add(fkRelation);

                    // 添加到被引用关系
                    AddReferencedRelation(entityType, info.TableName, property.Name, fkAttr.FKTableName);
                }
            }
        }

        /// <summary>
        /// 分析实体的导航属性(OneToOne/OneToMany)
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="info">实体关系信息</param>
        private void AnalyzeNavigationProperties(Type entityType, EntityRelationshipInfo info)
        {
            var navProperties = entityType.GetProperties()
                .Where(p => p.GetCustomAttribute<Navigate>() != null)
                .ToList();

            foreach (var prop in navProperties)
            {
                var navAttr = prop.GetCustomAttribute<Navigate>();
                var sugarCol = prop.GetCustomAttribute<SugarColumn>();

                // 只处理标记为IsIgnore的导航属性
                if (sugarCol?.IsIgnore != true)
                    continue;
                var NatureType = navAttr.GetNavigateType();
                try
                {
                    switch (NatureType)
                    {
                        case NavigateType.OneToOne:
                            // OneToOne: 当前表引用其他表
                            // 例如: [Navigate(NavigateType.OneToOne, nameof(BOM_ID))]
                            // ForeignKey字段是 BOM_ID
                            AddNavigationRelation(info, prop, navAttr, ForeignKeyType.ReferencesOther);
                            break;

                        case NavigateType.OneToMany:
                            // OneToMany: 其他表引用当前表
                            // 例如: [Navigate(NavigateType.OneToMany, nameof(tb_ProdSplitDetail.ProdDetailID))]
                            // 表示其他表通过ProdDetailID引用当前表
                            AddNavigationRelation(info, prop, navAttr, ForeignKeyType.ReferencedByOthers);
                            break;

                        case NavigateType.ManyToOne:
                            // ManyToOne: 类似OneToOne,当前表引用其他表
                            AddNavigationRelation(info, prop, navAttr, ForeignKeyType.ReferencesOther);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"分析导航属性 {prop.Name} 失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 添加导航关系
        /// </summary>
        /// <param name="info">实体关系信息</param>
        /// <param name="property">导航属性</param>
        /// <param name="navAttr">Navigate特性</param>
        /// <param name="fkType">外键类型</param>
        private void AddNavigationRelation(EntityRelationshipInfo info, PropertyInfo property, Navigate navAttr, ForeignKeyType fkType)
        {
            try
            {
                // 从Navigate表达式中提取外键字段名或关联实体类型
                string foreignKeyField = null;
                Type relatedEntityType = null;

                // Navigate属性的第二个参数是字符串，表示外键字段或关联字段
                // 例如: nameof(BOM_ID) 或 nameof(tb_ProdSplitDetail.ProdDetailID)
                var navigateExpression = navAttr.GetType().GetProperty("Expression")?.GetValue(navAttr)?.ToString();

                if (!string.IsNullOrEmpty(navigateExpression))
                {
                    // 解析表达式，提取字段名
                    // 格式可能是 "BOM_ID" 或 "tb_ProdSplitDetail.ProdDetailID"
                    var parts = navigateExpression.Split('.');
                    if (parts.Length == 1)
                    {
                        // 简单字段名，如 "BOM_ID"
                        foreignKeyField = parts[0];
                    }
                    else if (parts.Length >= 2)
                    {
                        // 完整路径，如 "tb_ProdSplitDetail.ProdDetailID"
                        foreignKeyField = parts[parts.Length - 1];

                        // 尝试从类型名称推断关联实体类型
                        string relatedTypeName = parts[0];
                        relatedEntityType = FindEntityTypeByTableName(relatedTypeName) ??
                                          FindEntityTypeByName(relatedTypeName);
                    }
                }

                // 如果无法从表达式获取，尝试从属性类型推断
                if (relatedEntityType == null && property.PropertyType.IsGenericType)
                {
                    // List<T> 类型
                    var genericArgs = property.PropertyType.GetGenericArguments();
                    if (genericArgs.Length > 0)
                    {
                        relatedEntityType = genericArgs[0];
                    }
                }
                else if (relatedEntityType == null)
                {
                    // 非集合类型
                    relatedEntityType = property.PropertyType;
                }
                var NatureType = navAttr.GetNavigateType();
                var navRelation = new NavigationRelation
                {
                    NavigationPropertyName = property.Name,
                    ForeignKeyColumn = foreignKeyField,
                    RelatedEntityType = relatedEntityType,
                    RelatedTableName = relatedEntityType != null ? GetTableName(relatedEntityType) : null,
                    Type = fkType,
                    NavigateType = NatureType
                };

                info.NavigationRelations.Add(navRelation);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"添加导航关系失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据实体名称查找实体类型
        /// </summary>
        /// <param name="entityName">实体名称</param>
        /// <returns>实体类型</returns>
        private Type FindEntityTypeByName(string entityName)
        {
            return _entityTypes.FirstOrDefault(t => t.Name == entityName);
        }

        /// <summary>
        /// 去重合并关系(避免FKRelation和Navigate重复记录同一外键)
        /// </summary>
        /// <param name="info">实体关系信息</param>
        private void MergeAndDeduplicateRelations(EntityRelationshipInfo info)
        {
            // 检查NavigationRelations中的ForeignKeyColumn是否已存在于ForeignKeyRelations中
            var existingFkColumns = info.ForeignKeyRelations.Select(fk => fk.ForeignKeyColumn).ToHashSet();

            // 移除重复的导航关系(如果外键字段已通过FKRelation记录)
            info.NavigationRelations.RemoveAll(nav =>
                !string.IsNullOrEmpty(nav.ForeignKeyColumn) &&
                existingFkColumns.Contains(nav.ForeignKeyColumn));
        }

        /// <summary>
        /// 添加被引用关系
        /// </summary>
        /// <param name="referencingType">引用的实体类型</param>
        /// <param name="referencingTable">引用的表名</param>
        /// <param name="fkColumn">外键列名</param>
        /// <param name="referencedTable">被引用的表名</param>
        private void AddReferencedRelation(Type referencingType, string referencingTable, string fkColumn, string referencedTable)
        {
            string key = referencedTable;
            if (!_relationshipCache.ContainsKey(key))
            {
                var referencedEntityType = FindEntityTypeByTableName(referencedTable);
                if (referencedEntityType != null)
                {
                    _relationshipCache[key] = new EntityRelationshipInfo
                    {
                        EntityType = referencedEntityType,
                        TableName = referencedTable,
                        EntityDescription = referencedEntityType.GetCustomAttribute<DescriptionAttribute>()?.Description ?? referencedTable
                    };
                }
            }

            if (_relationshipCache.ContainsKey(referencedTable))
            {
                var referencedRelation = new ReferencedRelation
                {
                    ReferencingEntityType = referencingType,
                    ReferencingTableName = referencingTable,
                    ForeignKeyColumn = fkColumn
                };

                _relationshipCache[referencedTable].ReferencedRelations.Add(referencedRelation);
            }
        }

        /// <summary>
        /// 根据表名查找实体类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>实体类型</returns>
        private Type FindEntityTypeByTableName(string tableName)
        {
            return _entityTypes.FirstOrDefault(t =>
            {
                var sugarTableAttr = t.GetCustomAttribute<SugarTable>();
                return sugarTableAttr?.TableName == tableName || t.Name == tableName;
            });
        }

        /// <summary>
        /// 生成清理顺序（拓扑排序）
        /// 先清理依赖其他表的记录，最后清理被依赖的表
        /// </summary>
        /// <param name="targetEntityType">目标实体类型</param>
        /// <returns>清理顺序列表</returns>
        public List<CleanupOrderNode> GenerateCleanupOrder(Type targetEntityType)
        {
            var cleanupOrder = new List<CleanupOrderNode>();
            var visited = new HashSet<string>();
            var visiting = new HashSet<string>();

            // 从目标表开始，找到所有依赖它的表
            var dependentTables = new List<Type>();
            FindDependentTables(targetEntityType, dependentTables, new HashSet<string>());

            // 拓扑排序
            void Visit(Type entityType)
            {
                string key = entityType.FullName;
                if (visited.Contains(key))
                {
                    return;
                }

                if (visiting.Contains(key))
                {
                    throw new InvalidOperationException($"检测到循环依赖：{entityType.Name}");
                }

                visiting.Add(key);

                // 先处理依赖的表（外键指向的表）
                var entityInfo = GetEntityRelationship(entityType);
                foreach (var fkRelation in entityInfo.ForeignKeyRelations)
                {
                    if (fkRelation.ReferencedEntityType != null && fkRelation.ReferencedEntityType != targetEntityType)
                    {
                        Visit(fkRelation.ReferencedEntityType);
                    }
                }

                visiting.Remove(key);
                visited.Add(key);

                cleanupOrder.Add(new CleanupOrderNode
                {
                    EntityType = entityType,
                    TableName = GetTableName(entityType),
                    Order = cleanupOrder.Count + 1
                });
            }

            // 处理所有依赖表
            foreach (var dependentType in dependentTables)
            {
                Visit(dependentType);
            }

            // 最后处理目标表
            Visit(targetEntityType);

            return cleanupOrder;
        }

        /// <summary>
        /// 查找依赖目标表的所有表
        /// </summary>
        /// <param name="targetEntityType">目标实体类型</param>
        /// <param name="dependentTables">依赖表列表</param>
        /// <param name="visited">已访问的表</param>
        private void FindDependentTables(Type targetEntityType, List<Type> dependentTables, HashSet<string> visited)
        {
            string targetTable = GetTableName(targetEntityType);

            foreach (var entityType in _entityTypes)
            {
                if (visited.Contains(entityType.FullName))
                {
                    continue;
                }

                var entityInfo = GetEntityRelationship(entityType);
                bool hasDependency = entityInfo.ForeignKeyRelations.Any(fk =>
                    fk.ReferencedTableName == targetTable || fk.ReferencedEntityType == targetEntityType);

                if (hasDependency)
                {
                    visited.Add(entityType.FullName);
                    dependentTables.Add(entityType);
                    // 递归查找
                    FindDependentTables(entityType, dependentTables, visited);
                }
            }
        }

        /// <summary>
        /// 获取所有依赖目标表的表名列表
        /// </summary>
        /// <param name="targetEntityType">目标实体类型</param>
        /// <returns>依赖表列表</returns>
        public List<string> GetDependentTableNames(Type targetEntityType)
        {
            var dependentTables = new List<Type>();
            FindDependentTables(targetEntityType, dependentTables, new HashSet<string>());
            return dependentTables.Select(t => GetTableName(t)).ToList();
        }

        /// <summary>
        /// 获取实体所有的外键关系
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>外键关系列表</returns>
        public List<ForeignKeyRelation> GetForeignKeys(Type entityType)
        {
            var entityInfo = GetEntityRelationship(entityType);
            return entityInfo.ForeignKeyRelations;
        }

        /// <summary>
        /// 获取所有引用该实体的关系
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>被引用关系列表</returns>
        public List<ReferencedRelation> GetReferencedRelations(Type entityType)
        {
            var entityInfo = GetEntityRelationship(entityType);
            return entityInfo.ReferencedRelations;
        }

        /// <summary>
        /// 获取外键字段名
        /// </summary>
        /// <param name="relatedEntityType">关联实体类型</param>
        /// <param name="mainEntityType">主实体类型</param>
        /// <returns>外键字段名</returns>
        public string GetForeignKeyField(Type relatedEntityType, Type mainEntityType)
        {
            var entityInfo = GetEntityRelationship(relatedEntityType);
            var mainTableName = GetTableName(mainEntityType);

            var fkRelation = entityInfo.ForeignKeyRelations
                .FirstOrDefault(fk => fk.ReferencedTableName == mainTableName || fk.ReferencedEntityType == mainEntityType);

            return fkRelation?.ForeignKeyColumn;
        }
    }
}
