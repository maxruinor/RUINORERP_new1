using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.Network.Services;
using SqlSugar;
using System;
using System.Linq;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 实体导入辅助类
    /// 为不同实体类型提供导入前的特殊字段处理
    /// </summary>
    public class EntityImportHelper
    {
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
            if (!category.CategoryLevel.HasValue)
            {
                category.CategoryLevel = 1;
            }

            // 4. 设置排序号
            if (!category.Sort.HasValue)
            {
                int maxSort = db.Queryable<tb_ProdCategories>()
                    .Max(c => c.Sort) ?? 0;
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

    }
}
