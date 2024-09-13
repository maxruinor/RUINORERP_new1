
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:21
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
    public partial class LogsServices : BaseServices<Logs>, ILogsServices
    {
        IMapper _mapper;
        public LogsServices(IMapper mapper, IBaseRepository<Logs> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}