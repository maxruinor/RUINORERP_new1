
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/21/2025 19:17:35
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
    /// 单位换算表
    /// </summary>
    public partial class tb_Unit_ConversionServices : BaseServices<tb_Unit_Conversion>, Itb_Unit_ConversionServices
    {
        IMapper _mapper;
        public tb_Unit_ConversionServices(IMapper mapper, IBaseRepository<tb_Unit_Conversion> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}