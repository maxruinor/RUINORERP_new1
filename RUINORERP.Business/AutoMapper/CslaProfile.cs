using AutoMapper;
using RUINORERP.Business.UseCsla;
using RUINORERP.Model;
using RUINORERP.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
namespace RUINORERP.Business.AutoMapper
{
    public class CslaProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CslaProfile()
        {
            CreateMap<tb_LocationType, tb_LocationTypeEditInfo>();
            CreateMap<tb_LocationTypeEditInfo, tb_LocationType>();

           
        }
    }
}
