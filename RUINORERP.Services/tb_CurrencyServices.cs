﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:31
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
    /// 币别资料表
    /// </summary>
    public partial class tb_CurrencyServices : BaseServices<tb_Currency>, Itb_CurrencyServices
    {
        IMapper _mapper;
        public tb_CurrencyServices(IMapper mapper, IBaseRepository<tb_Currency> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}