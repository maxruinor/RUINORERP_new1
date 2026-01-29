using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.Network.Services;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>预设字段名集合，如果没有预设字段返回空集合</returns>
        public static HashSet<string> GetPredefinedFields(Type entityType)
        {
            if (entityType == null)
            {
                return new HashSet<string>();
            }

            string typeName = entityType.Name;
            return _predefinedFields.TryGetValue(typeName, out var fields)
                ? new HashSet<string>(fields)
                : new HashSet<string>();
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
        public static void PreProcessEntity(Type entityType, BaseEntity entity, ISqlSugarClient db, string importType = null)
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
                    ProcessProdCategories(entity as tb_ProdCategories, db);
                    break;
                // TODO: 添加其他实体类型的处理
                case "tb_CustomerVendor":
                    // 根据importType区分是客户还是供应商
                    bool isCustomer = importType == "客户表";
                    bool isVendor = importType == "供应商表";
                    CustomerVendor(entity as tb_CustomerVendor, db, isCustomer, isVendor);
                    break;

                case "tb_Prod":
                    ProdBase(entity as tb_Prod, db);
                    break;
                case "tb_ProdDetail":
                    ProdDetail(entity as tb_ProdDetail, db);
                    break;
                default:
                    // 默认处理
                    break;
            }

            Business.BusinessHelper.Instance.InitEntity(entity);
        }

        /// <summary>
        /// 处理产品分类的特殊字段
        /// </summary>
        /// <param name="category">产品分类实体</param>
        /// <param name="db">数据库客户端</param>
        private static void ProcessProdCategories(tb_ProdCategories category, ISqlSugarClient db)
        {
            if (category == null)
            {
                return;
            }

            // 1. 自动生成分类编码（如果用户未指定）
            if (string.IsNullOrWhiteSpace(category.CategoryCode))
            {
                category.CategoryCode = ClientBizCodeService.GetBaseInfoNo(BaseInfoType.ProCategories);
            }

            // 2. 设置默认启用状态
            if (!category.Is_enabled.HasValue)
            {
                category.Is_enabled = true;
            }

            // 3. 设置默认分类级别
            if (!category.CategoryLevel.HasValue && category.Parent_id.HasValue)
            {
                category.CategoryLevel = 1;
            }

            // 4. 设置排序号
            if (!category.Sort.HasValue)
            {
                int maxSort = 0;// db.Queryable<tb_ProdCategories>().Max(c => c.Sort) ?? 0;
                category.Sort = maxSort + 1;
            }

            // 5. 设置默认备注
            if (string.IsNullOrWhiteSpace(category.Notes))
            {
                category.Notes = "通过Excel导入";
            }

        }


        /// <summary>
        /// 处理产品分类的特殊字段
        /// </summary>
        /// <param name="category">产品分类实体</param>
        /// <param name="db">数据库客户端</param>
        private static void CustomerVendor(tb_CustomerVendor customerVendor, ISqlSugarClient db, bool IsCustomer = false, bool IsVendor = false)
        {
            if (customerVendor == null)
            {
                return;
            }

            // 1. 自动生成分类编码（如果用户未指定）
            if (string.IsNullOrWhiteSpace(customerVendor.CVCode))
            {
                if (IsCustomer)
                {
                    customerVendor.CVCode = ClientBizCodeService.GetBaseInfoNo(BaseInfoType.Customer);
                }
                else
                {
                    customerVendor.CVCode = ClientBizCodeService.GetBaseInfoNo(BaseInfoType.Supplier);
                }

            }

            customerVendor.IsVendor = IsVendor;
            customerVendor.IsCustomer = IsCustomer;
            // 2. 设置默认启用状态
            if (!customerVendor.Is_enabled.HasValue)
            {
                customerVendor.Is_enabled = true;
            }

            // 5. 设置默认备注
            if (string.IsNullOrWhiteSpace(customerVendor.Notes))
            {
                customerVendor.Notes = "通过Excel导入";
            }
        }


        /// <summary>
        /// 处理产品分类的特殊字段
        /// </summary>
        /// <param name="category">产品分类实体</param>
        /// <param name="db">数据库客户端</param>
        private static void ProdBase(tb_Prod prod, ISqlSugarClient db)
        {
            if (prod == null)
            {
                return;
            }

            // 1. 自动生成分类编码（如果用户未指定）
            if (string.IsNullOrWhiteSpace(prod.ProductNo))
            {
                prod.ProductNo = ClientBizCodeService.GetBaseInfoNo(BaseInfoType.ProductNo);
            }
            prod.Is_enabled = true;
            prod.DataStatus = (int)DataStatus.新建;
            Business.BusinessHelper.Instance.InitEntity(prod);
        }

        private static void ProdDetail(tb_ProdDetail proddetail, ISqlSugarClient db)
        {
            if (proddetail == null)
            {
                return;
            }

            // 1. 自动生成分类编码（如果用户未指定）
            if (string.IsNullOrWhiteSpace(proddetail.SKU))
            {
                proddetail.SKU = ClientBizCodeService.GetBaseInfoNo(BaseInfoType.SKU_No);
            }
            proddetail.Is_enabled = true;
            proddetail.isdeleted=false;
            proddetail.DataStatus = (int)DataStatus.新建;
            Business.BusinessHelper.Instance.InitEntity(proddetail);
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
