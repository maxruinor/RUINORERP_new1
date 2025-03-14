
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:40:51
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
    /// 
    /// </summary>
    public partial class LogDetailsServices : BaseServices<LogDetails>, ILogDetailsServices
    {
        IMapper _mapper;
        public LogDetailsServices(IMapper mapper, IBaseRepository<LogDetails> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}