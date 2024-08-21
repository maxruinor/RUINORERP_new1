//-----------------------------------------------------------------------
// <copyright file="WindowsConfigurationExtensions.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Implement extension methods for .NET Core configuration</summary>
//-----------------------------------------------------------------------


using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RUINORERP.Model.Context
{
    /// <summary>
    /// Implement extension methods for base .NET configuration
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// 添加CSLA.NET服务以供应用程序使用。
        /// </summary>
        /// <param name="services">ServiceCollection object</param>
        /// <param name="options">配置CSLA的选项</param>
        public static IServiceCollection AddAppContext(this IServiceCollection services, ApplicationContext context)
        {
            // Custom configuration
            //var cslaOptions = new CslaOptions(services);
            //options?.Invoke(cslaOptions);

            // capture options object
            //services.AddScoped((p) => cslaOptions);

            // ApplicationContext defaults
            //services.AddScoped<ApplicationContext>();
           
            //注入单例，全程共享数据
            services.AddSingleton<ApplicationContext>(context);
            RegisterContextManager(services);

            // Runtime Info defaults
            //  services.TryAddScoped(typeof(IRuntimeInfo), typeof(RuntimeInfo));

            //   cslaOptions.AddRequiredDataPortalServices();

            // Default to using LocalProxy and local data portal
            // var proxyInit = services.Where(i => i.ServiceType.Equals(typeof(IDataPortalProxy))).Any();
            // if (!proxyInit)
            //{
            //   cslaOptions.DataPortal((options) => options.UseLocalProxy());
            // }

            return services;
        }

        private static void RegisterContextManager(IServiceCollection services)
        {
            services.AddScoped<ApplicationContextAccessor>();
            services.TryAddScoped(typeof(IContextManager), typeof(ApplicationContextManagerAsyncLocal));

            var contextManagerType = typeof(IContextManager);
            // default to AsyncLocal context manager
            services.AddScoped(contextManagerType, typeof(ApplicationContextManagerAsyncLocal));
            
            

            var managerInit = services.Where(i => i.ServiceType.Equals(contextManagerType)).Any();
            if (managerInit) return;

            if (LoadContextManager(services, "Csla.Blazor.WebAssembly.ApplicationContextManager, Csla.Blazor.WebAssembly")) return;
            if (LoadContextManager(services, "Csla.Xaml.ApplicationContextManager, Csla.Xaml")) return;
            if (LoadContextManager(services, "Csla.Web.Mvc.ApplicationContextManager, Csla.Web.Mvc")) return;
            if (LoadContextManager(services, "Csla.Web.ApplicationContextManager, Csla.Web")) return;
            if (LoadContextManager(services, "Csla.Windows.Forms.ApplicationContextManager, Csla.Windows.Forms")) return;

        }

        private static bool LoadContextManager(IServiceCollection services, string managerTypeName)
        {
            var managerType = Type.GetType(managerTypeName, false);
            if (managerType != null)
            {
                services.AddScoped(typeof(IContextManager), managerType);
                return true;
            }
            return false;
        }
    }
}
