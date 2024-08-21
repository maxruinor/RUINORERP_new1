
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/26/2024 19:53:38
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
    public partial class Proc_SaleOutStatisticsByEmployeeServices : BaseServices<Proc_SaleOutStatisticsByEmployee>, IProc_SaleOutStatisticsByEmployeeServices
    {
        IMapper _mapper;
        public Proc_SaleOutStatisticsByEmployeeServices(IMapper mapper, IBaseRepository<Proc_SaleOutStatisticsByEmployee> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}