
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 23:00:24
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
    /// 总账表来源于凭证分类汇总是财务报表的基础数据
    /// </summary>
    public partial class tb_FM_GeneralLedgerServices : BaseServices<tb_FM_GeneralLedger>, Itb_FM_GeneralLedgerServices
    {
        IMapper _mapper;
        public tb_FM_GeneralLedgerServices(IMapper mapper, IBaseRepository<tb_FM_GeneralLedger> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}