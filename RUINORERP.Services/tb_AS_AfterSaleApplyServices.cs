
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/09/2025 13:49:56
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
    /// 售后申请单 -登记，评估，清单，确认。目标是维修翻新
    /// </summary>
    public partial class tb_AS_AfterSaleApplyServices : BaseServices<tb_AS_AfterSaleApply>, Itb_AS_AfterSaleApplyServices
    {
        IMapper _mapper;
        public tb_AS_AfterSaleApplyServices(IMapper mapper, IBaseRepository<tb_AS_AfterSaleApply> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}