
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/14/2024 15:01:02
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
    /// 基本单位
    /// </summary>
    public partial class tb_UnitServices : BaseServices<tb_Unit>, Itb_UnitServices
    {
        IMapper _mapper;
        public tb_UnitServices(IMapper mapper, IBaseRepository<tb_Unit> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}