using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RUINORERP.Model.Context;
using SimpleHttp;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using RUINORERP.IRepository.Base;
using RUINORERP.Repository.Base;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Log4Net;
using RUINORERP.Model;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using RUINORERP.Common.Helper;
using RUINORERP.Extensions;
using RUINORERP.Business.AutoMapper;

namespace RUINORERP.WebServerConsole
{
    class Program
    {
        /// <summary>
        /// 服务管理者
        /// </summary>
        public static IServiceProvider MyServiceProvider { get; set; }

        public static void Main(string[] args)
        {
            var host = Startup.CreateHostBuilder(args).Build();
            host.Run();
            MyServiceProvider = host.Services;
            Startup.AppContextData.SetServiceProvider(MyServiceProvider);
            Startup.AppContextData.SetAutofacContainerScope(MyServiceProvider.GetAutofacRoot());
        }


    }
}

