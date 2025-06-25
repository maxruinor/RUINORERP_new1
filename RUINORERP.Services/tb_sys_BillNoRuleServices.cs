
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/25/2025 12:21:40
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
    /// 业务编号规则
    /// </summary>
    public partial class tb_sys_BillNoRuleServices : BaseServices<tb_sys_BillNoRule>, Itb_sys_BillNoRuleServices
    {
        IMapper _mapper;
        public tb_sys_BillNoRuleServices(IMapper mapper, IBaseRepository<tb_sys_BillNoRule> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}