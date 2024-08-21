using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI
{
    /*
    public class AutoFacBootStrapper
    {
        public static void CoreAutoFacInit()
        {
            var builder = new ContainerBuilder();
            HttpConfiguration config = GlobalConfiguration.Configuration;
            SetupResolveRules(builder);
            //注册所有的Controllers,// 通过PropertiesAutowired制定类型A在获取时会自动注入A的属性//InstancePerLifetimeScope 保证对象生命周期基于请求//InstancePerDependency 对每一个依赖或每一次调用创建一个新的唯一的实例，这也是默认的创建实例的方式。
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired().InstancePerDependency();
            //注册所有的ApiControllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired().InstancePerDependency();
            builder.RegisterType<MemberQueryFilterAttribute>().PropertiesAutowired();//注意，这里要把我们的全局Filter注册到Autofac中
            builder.RegisterType<ParamsCheckFilterAttribute>().PropertiesAutowired();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            var container = builder.Build();
            //注册api容器需要使用HttpConfiguration对象
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            //注册MVC容器
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void SetupResolveRules(ContainerBuilder builder)
        {
            //WebAPI只用引用BLL和DAL的接口，不用引用实现的dll。
            //如需加载实现的程序集，将dll拷贝到bin目录下即可，不用引用dll
            var iBLL = Assembly.Load("cpm.IBLL");
            var BLL = Assembly.Load("cpm.BLL");
            var iDAL = Assembly.Load("cpm.IDAL");
            var DAL = Assembly.Load("cpm.DAL");

            //根据名称约定（服务层的接口和实现均以BLL结尾），实现服务接口和服务实现的依赖
            builder.RegisterAssemblyTypes(iBLL, BLL)
              .Where(t => t.Name.EndsWith("BLL"))
              .AsImplementedInterfaces().PropertiesAutowired().InstancePerDependency();

            //根据名称约定（数据访问层的接口和实现均以DAL结尾），实现数据访问接口和数据访问实现的依赖
            builder.RegisterAssemblyTypes(iDAL, DAL)
              .Where(t => t.Name.EndsWith("DAL"))
              .AsImplementedInterfaces().PropertiesAutowired().InstancePerDependency();
            //注册其他模块
            builder.RegisterModule<DbModule>();
            builder.RegisterModule<WebModule>();
        }
    }
    */
}
