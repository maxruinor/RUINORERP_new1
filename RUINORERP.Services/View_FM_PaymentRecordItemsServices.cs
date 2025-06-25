
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/24/2025 20:49:01
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
    /// 收付款单明细统计
    /// </summary>
    public partial class View_FM_PaymentRecordItemsServices : BaseServices<View_FM_PaymentRecordItems>, IView_FM_PaymentRecordItemsServices
    {
        IMapper _mapper;
        public View_FM_PaymentRecordItemsServices(IMapper mapper, IBaseRepository<View_FM_PaymentRecordItems> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}