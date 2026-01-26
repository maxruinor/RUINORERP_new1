using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;
using RUINORERP.Model;
using SqlSugar;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 产品分类导入器
    /// 负责处理产品分类的导入和层级关系处理
    /// </summary>
    public class CategoryImporter
    {
        private readonly ISqlSugarClient _db;
        
        /// <summary>
        /// 分类路径分隔符
        /// </summary>
        private const char PathSeparator = '>';
        
        /// <summary>
        /// 已存在的分类映射，用于快速查找
        /// </summary>
        private Dictionary<string, tb_ProdCategories> _existingCategories;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">SqlSugar数据库客户端</param>
        public CategoryImporter(ISqlSugarClient db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            LoadExistingCategories();
        }
        
        /// <summary>
        /// 加载已存在的分类
        /// </summary>
        private void LoadExistingCategories()
        {
            _existingCategories = _db.Queryable<tb_ProdCategories>()
                .ToList()
                .ToDictionary(c => c.Category_name, StringComparer.OrdinalIgnoreCase);
        }
        
        /// <summary>
        /// 导入产品分类
        /// </summary>
        /// <param name="categoryPath">分类路径，例如：电子产品>手机>智能手机</param>
        /// <returns>分类ID</returns>
        public long ImportCategory(string categoryPath)
        {
            if (string.IsNullOrWhiteSpace(categoryPath))
            {
                throw new ArgumentException("分类路径不能为空", nameof(categoryPath));
            }
            
            // 分割分类路径
            var categoryNames = categoryPath.Split(new char[] { PathSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .ToList();
            
            if (categoryNames.Count == 0)
            {
                throw new InvalidOperationException("分类路径格式不正确");
            }
            
            // 处理分类层级
            long parentId = 0;
            int level = 1;
            
            foreach (var categoryName in categoryNames)
            {
                parentId = EnsureCategoryExists(categoryName, parentId, level);
                level++;
            }
            
            return parentId;
        }
        
        /// <summary>
        /// 确保分类存在，如果不存在则创建
        /// </summary>
        /// <param name="categoryName">分类名称</param>
        /// <param name="parentId">父分类ID</param>
        /// <param name="level">分类级别</param>
        /// <returns>分类ID</returns>
        private long EnsureCategoryExists(string categoryName, long parentId, int level)
        {
            // 检查是否已存在该分类
            if (_existingCategories.TryGetValue(categoryName, out var existingCategory))
            {
                return existingCategory.Category_ID;
            }
            
            // 创建新分类
            var newCategory = new tb_ProdCategories
            {
                Category_name = categoryName,
                CategoryCode = GenerateCategoryCode(categoryName, parentId),
                Is_enabled = true,
                CategoryLevel = level,
                Sort = _existingCategories.Count + 1,
                Parent_id = parentId > 0 ? (long?)parentId : null,
                Notes = "导入的产品分类"
            };
            
            // 插入到数据库
            var categoryId = _db.Insertable(newCategory).ExecuteReturnBigIdentity();
            newCategory.Category_ID = categoryId;
            
            // 添加到已存在分类映射
            _existingCategories[categoryName] = newCategory;
            
            return categoryId;
        }
        
        /// <summary>
        /// 生成分类编码
        /// </summary>
        /// <param name="categoryName">分类名称</param>
        /// <param name="parentId">父分类ID</param>
        /// <returns>分类编码</returns>
        private string GenerateCategoryCode(string categoryName, long parentId)
        {
            // 简单的分类编码生成逻辑，可根据实际需求调整
            string parentCode = string.Empty;
            
            if (parentId > 0)
            {
                var parentCategory = _existingCategories.Values.FirstOrDefault(c => c.Category_ID == parentId);
                if (parentCategory != null)
                {
                    parentCode = parentCategory.CategoryCode;
                }
            }
            
            // 取分类名称的前4个字母作为编码后缀
            string suffix = categoryName.Length > 4 ? categoryName.Substring(0, 4) : categoryName;
            suffix = suffix.ToUpper().Replace(" ", "");
            
            if (!string.IsNullOrEmpty(parentCode))
            {
                return $"{parentCode}_{suffix}";
            }
            else
            {
                return suffix;
            }
        }
        
        /// <summary>
        /// 批量导入分类
        /// </summary>
        /// <param name="categoryPaths">分类路径列表</param>
        /// <returns>分类路径到分类ID的映射</returns>
        public Dictionary<string, long> BatchImportCategories(List<string> categoryPaths)
        {
            if (categoryPaths == null || categoryPaths.Count == 0)
            {
                return new Dictionary<string, long>();
            }
            
            var categoryMap = new Dictionary<string, long>();
            
            foreach (var categoryPath in categoryPaths.Distinct())
            {
                try
                {
                    var categoryId = ImportCategory(categoryPath);
                    categoryMap[categoryPath] = categoryId;
                }
                catch (Exception ex)
                {
                    // 记录错误，但继续处理其他分类
                    Console.WriteLine($"导入分类 {categoryPath} 失败: {ex.Message}");
                }
            }
            
            return categoryMap;
        }
        
        /// <summary>
        /// 获取分类ID
        /// </summary>
        /// <param name="categoryName">分类名称</param>
        /// <returns>分类ID，如果不存在则返回0</returns>
        public long GetCategoryId(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return 0;
            }
            
            if (_existingCategories.TryGetValue(categoryName, out var category))
            {
                return category.Category_ID;
            }
            
            return 0;
        }
    }
}