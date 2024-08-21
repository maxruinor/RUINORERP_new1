using AutoMapper;

namespace RUINORERP.AutoMapper
{
    /// <summary>
    /// 静态全局 AutoMapper 配置文件 暂时不使用。因为配置文件中是实体类具体业务性，超出公共     IMapper mapper = AutoMapperConfigTest.RegisterMappings().CreateMapper();
    /// </summary>
    public class AutoMapperConfigTest
    {
        public static MapperConfiguration RegisterMappingsTest()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CustomProfile());
            });
        }
    }
}
