﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2024 18:45:37
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
    /// 调拨单明细
    /// </summary>
    public partial class tb_StockTransferDetailServices : BaseServices<tb_StockTransferDetail>, Itb_StockTransferDetailServices
    {
        IMapper _mapper;
        public tb_StockTransferDetailServices(IMapper mapper, IBaseRepository<tb_StockTransferDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}