using System;
using System.Linq;
using RUINORERP.Model;
using SqlSugar;

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
        public static void PreProcessEntity(Type entityType, BaseEntity entity, ISqlSugarClient db)
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
                // case "tb_Products":
                //     ProcessProducts(entity as tb_Products, db);
                //     break;
                default:
                    // 默认处理
                    break;
            }
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
                category.CategoryCode = GenerateCategoryCode(category.Category_name, db);
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
        /// 生成分类编码
        /// </summary>
        /// <param name="categoryName">分类名称</param>
        /// <param name="db">数据库客户端</param>
        /// <returns>分类编码</returns>
        private static string GenerateCategoryCode(string categoryName, ISqlSugarClient db)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return $"CAT_{DateTime.Now:yyyyMMddHHmmss}";
            }

            // 获取已有分类的最大序号
            var maxId = db.Queryable<tb_ProdCategories>()
                .Where(c => c.Category_name.Contains(categoryName) || 
                           c.CategoryCode.StartsWith("CAT"))
                .OrderByDescending(c => c.Category_ID)
                .Select(c => c.Category_ID)
                .First();

            return $"CAT{(maxId + 1):D6}";
        }
    }
}
