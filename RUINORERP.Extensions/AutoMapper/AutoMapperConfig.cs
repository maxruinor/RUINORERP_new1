using AutoMapper;
using RUINORERP.Extensions;

namespace RUINORERP.AutoMapper
{
    /// <summary>
    /// 静态全局 AutoMapper 配置文件 暂时不使用。因为配置文件中是实体类具体业务性，超出公共     IMapper mapper = AutoMapperConfigTest.RegisterMappings().CreateMapper();
    /// </summary>
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CustomProfile());
            });
        }


        public static void ConfigureAutoMapper<T>()
        {
            var type = typeof(T);
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(type, type); // 这里使用泛型类型T作为映射的源和目标类型
            });
            var mapper = configuration.CreateMapper();
            // 使用mapper进行映射操作
        }
    }
}
