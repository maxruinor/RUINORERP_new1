using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.AutoMapper;
using System;

namespace RUINORERP.Extensions
{
    /// <summary>
    /// Automapper 启动服务
    /// </summary>
    public static class AutoMapperSetup
    {

        /// <summary>
        /// 具体实现代码不应该在这里。应该用参数传进来,
        /// 因为可能项目之间多处引用 混乱
        /// </summary>
        /// <param name="services"></param>
        [Obsolete]
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //services.AddAutoMapper(typeof(AutoMapperConfig));
           services.AddAutoMapper(typeof(RUINORERP.AutoMapper.CustomProfile));
          //  services.AddAutoMapper(typeof(RUINORERP.AutoMapper.AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();
        }


        

    }
}
