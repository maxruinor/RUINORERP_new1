using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.ImportEngine.Enums;
using RUINORERP.UI.Network.Services;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 实体导入辅助类
    /// 为不同实体类型提供导入前的特殊字段处理
    /// </summary>
    public class EntityImportHelper
    {
        /// <summary>
        /// 存储每个实体类型的预设字段列表
        /// 这些字段在导入时会自动填充默认值，无需用户在映射配置中设置
        /// Key: 实体类型名称, Value: 预设字段名集合
        /// </summary>
        private static readonly Dictionary<string, HashSet<string>> _predefinedFields = new Dictionary<string, HashSet<string>>();

        /// <summary>
        /// 静态构造函数，初始化预设字段列表
        /// 使用表达式树获取字段名，避免硬编码字符串
        /// </summary>
        static EntityImportHelper()
        {
            // tb_ProdCategories的预设字段
            // CategoryCode: 不作为预设字段，用户可配置，未配置时自动生成
            _predefinedFields["tb_ProdCategories"] = new HashSet<string>
            {
                GetFieldName<tb_ProdCategories>(x => x.Is_enabled),        // 设置默认启用状态
                GetFieldName<tb_ProdCategories>(x => x.CategoryLevel),    // 设置默认分类级别
                GetFieldName<tb_ProdCategories>(x => x.Sort),              // 设置默认排序号
                GetFieldName<tb_ProdCategories>(x => x.Notes)               // 设置默认备注
                
            };

            // tb_CustomerVendor的预设字段
            // CVCode: 不作为预设字段，用户可配置，未配置时自动生成
            _predefinedFields["tb_CustomerVendor"] = new HashSet<string>
            {
                GetFieldName<tb_CustomerVendor>(x => x.IsVendor),         // 设置是否为供应商
                GetFieldName<tb_CustomerVendor>(x => x.IsCustomer),       // 设置是否为客户
                GetFieldName<tb_CustomerVendor>(x => x.Is_enabled),       // 设置默认启用状态
                GetFieldName<tb_CustomerVendor>(x => x.Notes)             // 设置默认备注
            };
            _predefinedFields["tb_Prod"] = new HashSet<string>
            {
                GetFieldName<tb_Prod>(x => x.DataStatus),
                GetFieldName<tb_Prod>(x => x.Modified_at),
                GetFieldName<tb_Prod>(x => x.Modified_by),
                GetFieldName<tb_Prod>(x => x.Created_at),
                GetFieldName<tb_Prod>(x => x.Created_by)
            };

            _predefinedFields["tb_ProdDetail"] = new HashSet<string>
            {
                GetFieldName<tb_ProdDetail>(x => x.DataStatus),
                GetFieldName<tb_ProdDetail>(x => x.Is_enabled),
                GetFieldName<tb_ProdDetail>(x => x.Modified_at),
                GetFieldName<tb_ProdDetail>(x => x.Modified_by),
                GetFieldName<tb_ProdDetail>(x => x.Created_at),
                GetFieldName<tb_ProdDetail>(x => x.Created_by)
            };
        }

        /// <summary>
        /// 获取指定实体类型的预设字段列表
        /// 优先从 ImportConfiguration 的 SystemGeneratedConfig 中读取
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="config">导入配置（可选）</param>
        /// <returns>预设字段名集合，如果没有预设字段返回空集合</returns>
        public static HashSet<string> GetPredefinedFields(Type entityType, ImportConfiguration config = null)
        {
            if (entityType == null)
            {
                return new HashSet<string>();
            }

            var predefinedFields = new HashSet<string>();

            // 如果提供了配置，从配置的 SystemGenerated 映射中读取
            if (config?.ColumnMappings != null)
            {
                var systemGeneratedFields = config.ColumnMappings
                    .Where(m => m.ColumnDataSourceType == DataSourceType.SystemGenerated)
                    .Select(m => m.SystemField?.Key)
                    .Where(k => !string.IsNullOrEmpty(k));
                
                predefinedFields.UnionWith(systemGeneratedFields);
            }

            // 合并硬编码的预设字段（向后兼容）
            string typeName = entityType.Name;
            if (_predefinedFields.TryGetValue(typeName, out var hardcodedFields))
            {
                predefinedFields.UnionWith(hardcodedFields);
            }

            return predefinedFields;
        }

        /// <summary>
        /// 使用表达式树获取字段名
        /// 避免硬编码字符串，提高类型安全性
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="columnExpression">字段表达式</param>
        /// <returns>字段名</returns>
        private static string GetFieldName<TEntity>(Expression<Func<TEntity, object>> columnExpression)
        {
            if (columnExpression == null)
            {
                throw new ArgumentNullException(nameof(columnExpression));
            }
            return columnExpression.GetMemberInfo().Name;
        }
        /// <summary>
        /// 在导入前处理实体的特殊字段
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entity">实体对象</param>
        /// <param name="db">数据库客户端</param>
        /// <param name="importType">导入类型标识（用于区分客户和供应商等使用相同表的情况）</param>
        public static async System.Threading.Tasks.Task PreProcessEntityAsync(Type entityType, BaseEntity entity, ISqlSugarClient db, string importType = null)
        {
            if (entityType == null || entity == null)
            {
                return;
            }

            // 根据实体类型调用不同的处理方法
            string typeName = entityType.Name;

            switch (typeName)
            {
                case "tb_ProdCategories":
                    await ProcessProdCategoriesAsync(entity as tb_ProdCategories, db);
                    break;
                // TODO: 添加其他实体类型的处理
                case "tb_CustomerVendor":
                    // 根据importType区分是客户还是供应商
                    bool isCustomer = importType == "客户表";
                    bool isVendor = importType == "供应商表";
                    await CustomerVendorAsync(entity as tb_CustomerVendor, db, isCustomer, isVendor);
                    break;

                case "tb_Prod":
                    await ProdBaseAsync(entity as tb_Prod, db);
                    break;
                case "tb_ProdDetail":
                    await ProdDetailAsync(entity as tb_ProdDetail, db);
                    break;
                default:
                    // 默认处理
                    break;
            }

            Business.BusinessHelper.Instance.InitEntity(entity);
        }

        /// <summary>
        /// 批量预处理实体列表（优化版本：批量查询排序值，避免N次数据库查询）1
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体列表</param>
        /// <param name="db">数据库客户端</param>
        /// <param name="importType">导入类型标识</param>
        public static async System.Threading.Tasks.Task BatchPreProcessEntitiesAsync<T>(List<T> entities, ISqlSugarClient db, string importType = null) where T : BaseEntity
        {
            if (entities == null || entities.Count == 0)
            {
                return;
            }

            var entityType = typeof(T);
            string typeName = entityType.Name;

            // 根据实体类型进行批量处理
            switch (typeName)
            {
                case "tb_ProdCategories":
                    await BatchProcessProdCategoriesAsync(entities.Cast<tb_ProdCategories>().ToList(), db);
                    break;
                case "tb_CustomerVendor":
                    bool isCustomer = importType == "客户表";
                    bool isVendor = importType == "供应商表";
                    foreach (var entity in entities.Cast<tb_CustomerVendor>())
                    {
                        await CustomerVendorAsync(entity, db, isCustomer, isVendor);
                    }
                    break;
                case "tb_Prod":
                    foreach (var entity in entities.Cast<tb_Prod>())
                    {
                        await ProdBaseAsync(entity, db);
                    }
                    break;
                case "tb_ProdDetail":
                    foreach (var entity in entities.Cast<tb_ProdDetail>())
                    {
                        await ProdDetailAsync(entity, db);
                    }
                    break;
                default:
                    // 默认逐个处理
                    foreach (var entity in entities)
                    {
                        await PreProcessEntityAsync(entityType, entity, db, importType);
                    }
                    break;
            }

            // 统一初始化实体基础字段
            foreach (var entity in entities)
            {
                Business.BusinessHelper.Instance.InitEntity(entity);
            }
        }

        /// <summary>
        /// 通用业务编号生成辅助方法
        /// </summary>
        private static async System.Threading.Tasks.Task<string> GenerateBizCodeAsync(BaseInfoType bizType)
        {
            var bizCodeService = Startup.GetFromFac<ClientBizCodeService>();
            if (bizCodeService == null)
            {
                throw new InvalidOperationException("无法获取ClientBizCodeService实例");
            }
            return await bizCodeService.GenerateBaseInfoNoAsync(bizType);
        }

        /// <summary>
        /// 批量处理产品分类的特殊字段（优化版：一次性查询最大Sort值）
        /// </summary>
        /// <param name="categories">产品分类实体列表</param>
        /// <param name="db">数据库客户端</param>
        private static async System.Threading.Tasks.Task BatchProcessProdCategoriesAsync(List<tb_ProdCategories> categories, ISqlSugarClient db)
        {
            if (categories == null || categories.Count == 0)
            {
                return;
            }

            // 按 Parent_id 分组，每组独立计算 Sort
            var groupedByParent = categories.GroupBy(c => c.Parent_id);

            foreach (var group in groupedByParent)
            {
                var parentId = group.Key;
                var categoryList = group.ToList();

                // ✅ 批量查询：一次性获取该父级分类下的最大 Sort 值
                int maxSort = await db.Queryable<tb_ProdCategories>()
                    .WhereIF(parentId.HasValue, c => c.Parent_id == parentId)
                    .MaxAsync(c => (int?)c.Sort) ?? 0;

                // ✅ 内存累加：为每个分类分配递增的 Sort 值
                for (int i = 0; i < categoryList.Count; i++)
                {
                    var category = categoryList[i];
                    await FillProdCategoryDefaults(category, db, maxSort + i + 1);
                }
            }
        }

        /// <summary>
        /// 填充产品分类默认值
        /// </summary>
        private static async System.Threading.Tasks.Task FillProdCategoryDefaults(tb_ProdCategories category, ISqlSugarClient db, int? sortValue = null)
        {
            // 1. 自动生成分类编码（如果用户未指定）
            if (string.IsNullOrWhiteSpace(category.CategoryCode))
            {
                category.CategoryCode = await GenerateBizCodeAsync(BaseInfoType.ProCategories);
            }

            // 2. 设置默认启用状态
            category.Is_enabled ??= true;

            // 3. 设置默认分类级别
            if (!category.CategoryLevel.HasValue && category.Parent_id.HasValue)
            {
                category.CategoryLevel = 1;
            }

            // 4. 设置排序号
            if (!category.Sort.HasValue && sortValue.HasValue)
            {
                category.Sort = sortValue.Value;
            }

            // 5. 设置默认备注
            if (string.IsNullOrWhiteSpace(category.Notes))
            {
                category.Notes = "通过Excel导入";
            }
        }

        /// <summary>
        /// 处理产品分类的特殊字段（旧版：逐行查询，保留用于兼容）
        /// </summary>
        /// <param name="category">产品分类实体</param>
        /// <param name="db">数据库客户端</param>
        private static async System.Threading.Tasks.Task ProcessProdCategoriesAsync(tb_ProdCategories category, ISqlSugarClient db)
        {
            if (category == null) return;

            // 1. 自动生成分类编码（如果用户未指定）
            if (string.IsNullOrWhiteSpace(category.CategoryCode))
            {
                category.CategoryCode = await GenerateBizCodeAsync(BaseInfoType.ProCategories);
            }

            // 2. 设置默认启用状态
            category.Is_enabled ??= true;

            // 3. 设置默认分类级别
            if (!category.CategoryLevel.HasValue && category.Parent_id.HasValue)
            {
                category.CategoryLevel = 1;
            }

            // 4. 设置排序号（按父级分类分别计算）- ❌ 性能问题：每次查询数据库
            if (!category.Sort.HasValue)
            {
                int maxSort = await db.Queryable<tb_ProdCategories>()
                    .WhereIF(category.Parent_id.HasValue, c => c.Parent_id == category.Parent_id)
                    .MaxAsync(c => (int?)c.Sort) ?? 0;
                category.Sort = maxSort + 1;
            }

            // 5. 设置默认备注
            if (string.IsNullOrWhiteSpace(category.Notes))
            {
                category.Notes = "通过Excel导入";
            }
        }


        /// <summary>
        /// 处理客户/供应商的特殊字段
        /// </summary>
        private static async System.Threading.Tasks.Task CustomerVendorAsync(tb_CustomerVendor customerVendor, ISqlSugarClient db, bool IsCustomer = false, bool IsVendor = false)
        {
            if (customerVendor == null) return;

            // 1. 自动生成编码（如果用户未指定）
            if (string.IsNullOrWhiteSpace(customerVendor.CVCode))
            {
                var bizType = IsCustomer ? BaseInfoType.Customer : BaseInfoType.Supplier;
                customerVendor.CVCode = await GenerateBizCodeAsync(bizType);
            }

            customerVendor.IsVendor = IsVendor;
            customerVendor.IsCustomer = IsCustomer;
            
            // 2. 设置默认启用状态
            customerVendor.Is_enabled ??= true;

            // 3. 设置默认备注
            if (string.IsNullOrWhiteSpace(customerVendor.Notes))
            {
                customerVendor.Notes = "通过Excel导入";
            }
        }


        /// <summary>
        /// 处理产品基础信息的特殊字段
        /// </summary>
        private static async System.Threading.Tasks.Task ProdBaseAsync(tb_Prod prod, ISqlSugarClient db)
        {
            if (prod == null) return;

            // 1. 自动生成产品编号（如果用户未指定）
            if (string.IsNullOrWhiteSpace(prod.ProductNo))
            {
                prod.ProductNo = await GenerateBizCodeAsync(BaseInfoType.ProductNo);
            }
            
            prod.Is_enabled = true;
            prod.DataStatus = (int)DataStatus.新建;
        }

        /// <summary>
        /// 处理产品明细的特殊字段
        /// </summary>
        private static async System.Threading.Tasks.Task ProdDetailAsync(tb_ProdDetail proddetail, ISqlSugarClient db)
        {
            if (proddetail == null) return;

            // 1. 自动生成SKU编码（如果用户未指定）
            if (string.IsNullOrWhiteSpace(proddetail.SKU))
            {
                proddetail.SKU = await GenerateBizCodeAsync(BaseInfoType.SKU_No);
            }
            
            proddetail.Is_enabled = true;
            proddetail.isdeleted = false;
            proddetail.DataStatus = (int)DataStatus.新建;
        }

        /// <summary>
        /// 获取预定义的枚举类型（针对特殊字段）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>枚举类型，无预定义返回null</returns>
        public static Type GetPredefinedEnumType(string tableName, string fieldName)
        {
            // 使用表达式树定义字段到枚举类型的映射
            var predefinedEnums = new List<(Type EntityType, string FieldName, Type EnumType)>
            {
                // 示例格式：表名.字段名 -> 枚举类型
                (typeof(tb_Prod), nameof(tb_Prod.PropertyType), typeof(ProductAttributeType)),
                // (typeof(YourEntity), nameof(YourEntity.YourField), typeof(YourEnum)),
            };

            foreach (var mapping in predefinedEnums)
            {
                if (mapping.EntityType.Name == tableName && mapping.FieldName == fieldName)
                {
                    return mapping.EnumType;
                }
            }

            return null;
        }
    }
}
