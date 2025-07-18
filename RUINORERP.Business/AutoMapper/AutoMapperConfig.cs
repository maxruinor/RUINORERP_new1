using AutoMapper;

namespace RUINORERP.Business.AutoMapper
{
    /// <summary>
    /// 静态全局 AutoMapper 配置文件
    /// </summary>
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {

                // 全局忽略 BaseEntity 属性
                cfg.IgnoreBaseEntityProperties();
                cfg.ApplySmartConventions();


                // 也可以在指定的程序集中扫描从 Profile 继承的类，并将其添加到配置中
                //cfg.AddMaps(System.AppDomain.CurrentDomain.GetAssemblies());
                // 也可以传程序集名称（dll 名称）
                //cfg.AddMaps("LibCoreTest");

                //cfg.AddProfile(new Business.AutoMapper.CslaProfile());
                cfg.AddProfile(new CustomProfile());
               // cfg.AddProfile(new CustomProfileAll());
            });
        }

        /*


    /// <summary>
    /// Automapper 启动服务
    ///  静态全局 AutoMapper 配置文件
    /// </summary>
    public static class AutoMapperInstall
    {
        public static void AddAutoMapperInstall(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
           services.AddAutoMapper(typeof(AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();
        }


        /// <summary>
        /// 只能用就于两边属性一样的
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination MapTo<TDestination, TSource>(this TSource source)
         where TDestination : class
         where TSource : class
        {
            if (source == null) return default(TDestination);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }


    }
    */
    }
}
