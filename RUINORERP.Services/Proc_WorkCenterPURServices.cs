
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2024 19:41:21
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
    public partial class Proc_WorkCenterPURServices : BaseServices<Proc_WorkCenterPUR>, IProc_WorkCenterPURServices
    {
        IMapper _mapper;
        public Proc_WorkCenterPURServices(IMapper mapper, IBaseRepository<Proc_WorkCenterPUR> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}