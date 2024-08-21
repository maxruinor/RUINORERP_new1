
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:39
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
    /// 审核流程明细表
    /// </summary>
    public partial class tb_ApprovalProcessDetailServices : BaseServices<tb_ApprovalProcessDetail>, Itb_ApprovalProcessDetailServices
    {
        IMapper _mapper;
        public tb_ApprovalProcessDetailServices(IMapper mapper, IBaseRepository<tb_ApprovalProcessDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}