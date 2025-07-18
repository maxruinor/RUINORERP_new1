using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RUINORERP.Model;

namespace RUINORERP.Business.AutoMapper
{
    public class ConventionBasedProfile : Profile
    {
        public ConventionBasedProfile()
        {
            // 1. 应用智能约定
            //ApplySmartConventions();

            //// 2. 忽略 BaseEntity 属性
            //IgnoreBaseEntityProperties();

            // 3. 其他约定配置...
        }
    }

     
}
