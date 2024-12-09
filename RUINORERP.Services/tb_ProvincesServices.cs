
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:46
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
    /// 省份表
    /// </summary>
    public partial class tb_ProvincesServices : BaseServices<tb_Provinces>, Itb_ProvincesServices
    {
        IMapper _mapper;
        public tb_ProvincesServices(IMapper mapper, IBaseRepository<tb_Provinces> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}