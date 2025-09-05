
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/04/2025 18:02:23
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
    /// 对账单明细
    /// </summary>
    public partial class tb_FM_StatementDetailServices : BaseServices<tb_FM_StatementDetail>, Itb_FM_StatementDetailServices
    {
        IMapper _mapper;
        public tb_FM_StatementDetailServices(IMapper mapper, IBaseRepository<tb_FM_StatementDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}