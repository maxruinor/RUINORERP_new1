using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.AutoMapper;
using System;

namespace RUINORERP.Business.AutoMapper
{
    /// <summary>
    /// Automapper 启动服务
    /// </summary>
    public static class AutoMapperHelper
    {

        /// <summary>
        /// 
        ///  
        /// </summary>
        /// <param name="services"></param>

        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            _serviceProvider = services.BuildServiceProvider();
           // services.AddAutoMapper(typeof(AutoMapperConfig));
          //  services.AddAutoMapper(typeof(RUINORERP.AutoMapper.CustomProfile));
          //  services.AddAutoMapper(typeof(RUINORERP.AutoMapper.AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();
        }

        private static IServiceProvider _serviceProvider;

        //public static void UseStateAutoMapper()
        //{
        ////     _serviceProvider = applicationBuilder.ApplicationServices;
        //}

        public static TDestination Map<TDestination>(object source)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();

            return mapper.Map<TDestination>(source);
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();

            return mapper.Map<TSource, TDestination>(source);
        }


        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();

            return mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TDestination>(this object source)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();

            return mapper.Map<TDestination>(source);
        }


    }
}
