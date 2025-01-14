﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:10
// **************************************
using AutoMapper;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Services.BASE;
using System.Threading.Tasks;
using System;
﻿using SqlSugar;
using System.Collections.Generic;


namespace RUINORERP.Services
{
    /// <summary>
    /// 产品属性类型EVA
    /// </summary>
    public partial class tb_ProdPropertyTypeServices : BaseServices<tb_ProdPropertyType>, Itb_ProdPropertyTypeServices
    {
        IMapper _mapper;
        public tb_ProdPropertyTypeServices(IMapper mapper, IBaseRepository<tb_ProdPropertyType> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}