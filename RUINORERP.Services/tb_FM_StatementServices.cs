
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:13
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
    /// 对账单
    /// </summary>
    public partial class tb_FM_StatementServices : BaseServices<tb_FM_Statement>, Itb_FM_StatementServices
    {
        IMapper _mapper;
        public tb_FM_StatementServices(IMapper mapper, IBaseRepository<tb_FM_Statement> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}