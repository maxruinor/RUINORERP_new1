
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:24
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
    /// 提醒内容
    /// </summary>
    public partial class tb_ReminderAlertServices : BaseServices<tb_ReminderAlert>, Itb_ReminderAlertServices
    {
        IMapper _mapper;
        public tb_ReminderAlertServices(IMapper mapper, IBaseRepository<tb_ReminderAlert> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}