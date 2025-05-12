
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
    /// 提醒信息是通过什么规则通知了什么内容给谁在什么时间。通知记录  暂时不处理
    /// </summary>
    public partial class tb_ReminderAlertHistoryServices : BaseServices<tb_ReminderAlertHistory>, Itb_ReminderAlertHistoryServices
    {
        IMapper _mapper;
        public tb_ReminderAlertHistoryServices(IMapper mapper, IBaseRepository<tb_ReminderAlertHistory> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}