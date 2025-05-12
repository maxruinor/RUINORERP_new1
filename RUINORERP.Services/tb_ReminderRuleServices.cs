
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:25
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
    /// 库存策略通过这里设置的条件查询出一个库存集合提醒给用户
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