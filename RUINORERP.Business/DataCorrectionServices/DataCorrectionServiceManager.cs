using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using RUINORERP.Model.Context;

namespace RUINORERP.Business.DataCorrectionServices
{
    /// <summary>
    /// 数据修复服务管理器
    /// 注：服务通过DI容器自动注册，此类仅用于查询已注册的服务
    /// </summary>
    public class DataCorrectionServiceManager
    {
        private static ApplicationContext _appContext;
        
        /// <summary>
        /// 设置应用上下文（在应用启动时调用）
        /// </summary>
        /// <param name="appContext">应用上下文</param>
        public static void SetApplicationContext(ApplicationContext appContext)
        {
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        }
        
        /// <summary>
        /// 获取所有注册的修复服务（从DI容器）
        /// </summary>
        public static List<IDataCorrectionService> GetAllServices()
        {
            if (_appContext == null)
            {
                throw new InvalidOperationException("应用上下文未设置，请先调用SetApplicationContext方法");
            }
            
            // 使用Autofac容器获取所有IDataCorrectionService实例
            if (ApplicationContext.AutofacContainerScope != null)
            {
                return ApplicationContext.AutofacContainerScope.Resolve<IEnumerable<IDataCorrectionService>>()?.ToList() ?? new List<IDataCorrectionService>();
            }
            
            return new List<IDataCorrectionService>();
        }
        
        /// <summary>
        /// 根据名称获取服务（从DI容器）
        /// </summary>
        /// <param name="correctionName">服务名称（支持CorrrectionName或FunctionName匹配）</param>
        public static IDataCorrectionService GetService(string correctionName)
        {
            if (_appContext == null)
            {
                throw new InvalidOperationException("应用上下文未设置，请先调用SetApplicationContext方法");
            }
            
            var services = GetAllServices();
            
            // 优先使用CorrectionName匹配
            var service = services.FirstOrDefault(s => s.CorrectionName == correctionName);
            
            // 如果没有匹配到，尝试用FunctionName匹配
            if (service == null)
            {
                service = services.FirstOrDefault(s => s.FunctionName == correctionName);
            }
            
            return service;
        }
    }
}
