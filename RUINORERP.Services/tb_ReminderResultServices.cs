
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/26/2025 12:18:31
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
    /// 用户接收提醒内容
    /// </summary>
    public partial class tb_ReminderResultServices : BaseServices<tb_ReminderResult>, Itb_ReminderResultServices
    {
        IMapper _mapper;
        public tb_ReminderResultServices(IMapper mapper, IBaseRepository<tb_ReminderResult> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}