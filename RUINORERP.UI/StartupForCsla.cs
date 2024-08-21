using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RUINORERP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI
{
    class StartupForCsla
    {

        public StartupForCsla(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }
        public IContainer ApplicationContainer { get; private set; }
        public IConfigurationRoot Configuration { get; private set; }
        //在ConfigureServices中注册依赖项。
        //在下面的Configure方法之前由运行时调用。

        private void start()
        {
            var host1 = new HostBuilder()
            .ConfigureServices((hostContext, services) => services
            // register window and page types here
                .AddSingleton<MainForm>()
             .AddSingleton<Form2>()

   //.AddTransient<Pages.PersonEditPage>()
   //.AddTransient<Pages.PersonListPage>()
   //.AddTransient<tb_UnitEntity>()

   // register other services here
   //.AddTransient<Itb_LocationTypeDal, tb_LocationTypeDal>()

   // .AddCsla(options => options.AddWindowsForms())
   .AddLogging(configure => configure.AddConsole())

).Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //将服务添加到集合中。
            services.AddTransient<Form2>();
            //创建 container builder.
            var builder = new ContainerBuilder();
            //注册依赖项，填充服务
            //集合，并构建容器。
            //
            //请注意，Populate基本上是添加内容的foreach
            //进入集合中的Autofac。 如果你注册
            //在Autofac之前的东西填充然后填充的东西
            // ServiceCollection可以覆盖那些东西; 如果你注册
            // AFTER填充这些注册可以覆盖的东西
            //在ServiceCollection中。 根据需要混合搭配。
            builder.Populate(services);
            // builder.RegisterType<RUINORERP.Business.UseCsla.tb_LocationTypeDal>().As<RUINORERP.Business.UseCsla.Itb_LocationTypeDal>();
            this.ApplicationContainer = builder.Build();
            //根据容器创建IServiceProvider
            return new AutofacServiceProvider(this.ApplicationContainer);
        }
        //配置是添加中间件的位置。 这称之为
        // ConfigureServices。 您可以使用IApplicationBuilder.ApplicationServices
        //如果你需要从容器中解决问题。

    }

    //==

}


