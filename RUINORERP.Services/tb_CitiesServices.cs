
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:41
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
    /// 城市表
    /// </summary>
    public partial class tb_CitiesServices : BaseServices<tb_Cities>, Itb_CitiesServices
    {
        IMapper _mapper;
        public tb_CitiesServices(IMapper mapper, IBaseRepository<tb_Cities> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}