using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{
    public class AutoMapperProfile<SourceType, DestinationType> : Profile
    {
        public AutoMapperProfile()
        {
            // 为每个需要映射的类型创建映射规则
            CreateMap<SourceType, DestinationType>()
                .ForMember(dest => dest.pro, opt => opt.IgnoreIf(src => src.Property == null || src.Property.GetType().GetCustomAttribute<SugarColumn>()?.IsIgnore == true));
        }
    }
}
