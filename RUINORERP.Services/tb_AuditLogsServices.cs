﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/06/2025 14:52:02
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
    /// 审计日志表
    /// </summary>
    public partial class tb_AuditLogsServices : BaseServices<tb_AuditLogs>, Itb_AuditLogsServices
    {
        IMapper _mapper;
        public tb_AuditLogsServices(IMapper mapper, IBaseRepository<tb_AuditLogs> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}