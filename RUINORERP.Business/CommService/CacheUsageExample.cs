using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Business.CommService;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 缓存使用示例
    /// </summary>
    public class CacheUsageExample
    {
        private readonly IEntityCacheManager _cacheManager;
        private readonly ILogger<CacheUsageExample> _logger;

        public CacheUsageExample(IEntityCacheManager cacheManager, ILogger<CacheUsageExample> logger)
        {
            _cacheManager = cacheManager;
            _logger = logger;
        }

        /// <summary>
        /// 初始化表结构信息示例
        /// </summary>
        public void InitializeTableSchemas()
        {
            // 初始化公司表结构信息
            _cacheManager.InitializeTableSchema<tb_Company>(
                k => k.ID, 
                v => v.CNName,
                description: "系统使用者公司");

            // 初始化部门表结构信息
            _cacheManager.InitializeTableSchema<tb_Department>(
                k => k.DepartmentID, 
                v => v.DepartmentName,
                description: "部门信息");

            // 初始化员工表结构信息
            _cacheManager.InitializeTableSchema<tb_Employee>(
                k => k.Employee_ID, 
                v => v.Employee_Name,
                description: "员工信息");

            // 初始化产品表结构信息
            _cacheManager.InitializeTableSchema<tb_Prod>(
                k => k.ProdBaseID, 
                v => v.CNName,
                description: "产品信息");

            _logger.LogInformation("表结构信息初始化完成");
        }

        /// <summary>
        /// 缓存操作示例
        /// </summary>
        public async Task CacheOperationExample()
        {
            try
            {
                // 1. 更新实体列表缓存
                var companies = new List<tb_Company>
                {
                    new tb_Company { ID = 1, CNName = "公司A", CompanyCode = "COMP001" },
                    new tb_Company { ID = 2, CNName = "公司B", CompanyCode = "COMP002" }
                };
                _cacheManager.UpdateEntityList(companies);

                // 2. 获取实体列表
                var companyList = _cacheManager.GetEntityList<tb_Company>();
                _logger.LogInformation($"获取到 {companyList.Count} 个公司");

                // 3. 根据ID获取实体
                var company = _cacheManager.GetEntity<tb_Company>(1);
                if (company != null)
                {
                    _logger.LogInformation($"获取到公司: {company.CNName}");
                }

                // 4. 更新单个实体
                if (company != null)
                {
                    company.CNName = "公司A(更新)";
                    _cacheManager.UpdateEntity(company);
                }

                // 5. 获取显示值
                var displayName = _cacheManager.GetDisplayValue("tb_Company", 1);
                _logger.LogInformation($"公司显示名称: {displayName}");

                // 6. 删除实体
                _cacheManager.DeleteEntity<tb_Company>(2);
                _logger.LogInformation("删除公司ID为2的记录");

                // 7. 验证删除结果
                var remainingCompanies = _cacheManager.GetEntityList<tb_Company>();
                _logger.LogInformation($"删除后剩余 {remainingCompanies.Count} 个公司");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "缓存操作示例执行失败");
            }
        }
    }
}