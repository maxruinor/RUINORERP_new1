
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2026 23:59:01
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
    /// 提醒规则
    /// </summary>
    public partial class tb_ReminderRuleServices : BaseServices<tb_ReminderRule>, Itb_ReminderRuleServices
    {
        IMapper _mapper;
        public tb_ReminderRuleServices(IMapper mapper, IBaseRepository<tb_ReminderRule> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}