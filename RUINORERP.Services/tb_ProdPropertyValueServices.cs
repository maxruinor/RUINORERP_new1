
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:48:21
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
    /// 产品属性值表
    /// </summary>
    public partial class tb_ProdPropertyValueServices : BaseServices<tb_ProdPropertyValue>, Itb_ProdPropertyValueServices
    {
        IMapper _mapper;
        public tb_ProdPropertyValueServices(IMapper mapper, IBaseRepository<tb_ProdPropertyValue> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}