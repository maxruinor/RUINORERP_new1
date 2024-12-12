
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/12/2024 10:37:31
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
    /// 跟进记录表
    /// </summary>
    public partial class tb_CRM_FollowUpRecordsServices : BaseServices<tb_CRM_FollowUpRecords>, Itb_CRM_FollowUpRecordsServices
    {
        IMapper _mapper;
        public tb_CRM_FollowUpRecordsServices(IMapper mapper, IBaseRepository<tb_CRM_FollowUpRecords> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}