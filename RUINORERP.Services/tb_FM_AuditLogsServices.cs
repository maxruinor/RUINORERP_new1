
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/27/2025 18:00:24
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
    /// 财务审计日志
    /// </summary>
    public partial class tb_FM_AuditLogsServices : BaseServices<tb_FM_AuditLogs>, Itb_FM_AuditLogsServices
    {
        IMapper _mapper;
        public tb_FM_AuditLogsServices(IMapper mapper, IBaseRepository<tb_FM_AuditLogs> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}