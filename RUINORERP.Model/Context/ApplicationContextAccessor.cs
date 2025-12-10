//-----------------------------------------------------------------------
// <copyright file="ApplicationContextAccessor.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Provides access to the correct current application</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace RUINORERP.Model.Context
{
    /// <summary>
    /// Provides access to the correct current application
    /// context manager instance depending on runtime environment.
    /// 提供对正确的当前应用程序的访问
    /// 上下文管理器实例，具体取决于运行时环境。
    /// </summary>
    public class ApplicationContextAccessor
    {
        /// <summary>
        /// 服务提供程序
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Creates a new instance of the type.
        /// </summary>
        /// <param name="contextManagerList"></param>
        /// <param name="localContextManager"></param>
        /// <param name="serviceProvider"></param>
        public ApplicationContextAccessor(IContextManager localContextManager)
        {
            ContextManager = localContextManager;
            if (ContextManager is null)
            {
                ContextManager = new ApplicationContextManagerAsyncLocal();
            }
        }

        /// <summary>
        /// Creates a new instance of the type with service provider.
        /// </summary>
        /// <param name="localContextManager"></param>
        /// <param name="serviceProvider"></param>
        public ApplicationContextAccessor(IContextManager localContextManager, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ContextManager = localContextManager;
            if (ContextManager is null)
            {
                ContextManager = new ApplicationContextManagerAsyncLocal();
            }
        }


      
        private IContextManager ContextManager { get; set; }



        /// <summary>
        ///获取对正确的当前应用程序的引用
        ///上下文管理器实例，具体取决于运行时环境。
        /// </summary>
        public IContextManager GetContextManager()
        {
            //var runtimeInfo = ServiceProvider.GetRequiredService<IRuntimeInfo>();
            //if (!runtimeInfo.LocalProxyNewScopeExists)
            return ContextManager;
            //else
            //    return LocalContextManager;
            //return LocalContextManager;
        }
    }
}
