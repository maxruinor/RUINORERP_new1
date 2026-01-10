
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2026 23:58:59
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
    /// 链路与规则关联表
    /// </summary>
    public partial class tb_ReminderLinkRuleRelationServices : BaseServices<tb_ReminderLinkRuleRelation>, Itb_ReminderLinkRuleRelationServices
    {
        IMapper _mapper;
        public tb_ReminderLinkRuleRelationServices(IMapper mapper, IBaseRepository<tb_ReminderLinkRuleRelation> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}