using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.DI
{
    public static class CommonDIConfig
    {/// <summary>
     /// Configure common services that are used across all layers
     /// </summary>
        public static void ConfigureCommonServices(IServiceCollection services)
        {
            // Common services
            //services.AddSingleton<ILoggerService, Log4NetService>();
            //services.AddSingleton<IAuthorizationService, AuthorizationService>();
            //services.AddSingleton<IAuthenticationService, AuthenticationService>();



      
        }

        /// <summary>
        /// Configure common container registrations
        /// </summary>
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            // Register common interceptors
            //builder.RegisterType<BaseDataCacheAOP>();

            //// Register common filters
            //builder.RegisterType<Extensions.Filter.GlobalExceptionsFilter>();
        }
    }
}
