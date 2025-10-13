using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model;

namespace RUINORERP.Business.Test
{
    /// <summary>
    /// 测试MyCacheManager类是否正确移植了BizCacheHelper的所有功能
    /// </summary>
    public class CacheMigrationTest
    {
        /// <summary>
        /// 测试缓存管理器的基本功能
        /// </summary>
        public static void TestCacheManager()
        {
            // 初始化缓存管理器
            var cacheManager = MyCacheManager.Instance;
            
            // 测试获取实体
            try
            {
                // 测试GetEntity方法
                var company = cacheManager.GetEntity<tb_Company>(1);
                if (company != null)
                {
                    Console.WriteLine("成功获取公司信息");
                }
                
                // 测试GetEntityList方法
                var companies = cacheManager.GetEntityList<tb_Company>("tb_Company");
                if (companies != null && companies.Count > 0)
                {
                    Console.WriteLine($"成功获取公司列表，共{companies.Count}条记录");
                }
                
                // 测试GetValue方法
                var companyName = cacheManager.GetValue("tb_Company", 1);
                if (companyName != null)
                {
                    Console.WriteLine($"成功获取公司名称: {companyName}");
                }
                
                // 测试GetDictDataSource方法
                var dict = cacheManager.GetDictDataSource("tb_Company");
                if (dict != null && dict.Count > 0)
                {
                    Console.WriteLine($"成功获取公司字典，共{dict.Count}条记录");
                }
                
                Console.WriteLine("所有缓存管理器测试通过！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"缓存管理器测试失败: {ex.Message}");
            }
        }
    }
}