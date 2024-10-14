
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/14/2024 18:29:34
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
    /// 调拨单-两个仓库之间的库存转移
    /// </summary>
    public partial class tb_StockTransferServices : BaseServices<tb_StockTransfer>, Itb_StockTransferServices
    {
        IMapper _mapper;
        public tb_StockTransferServices(IMapper mapper, IBaseRepository<tb_StockTransfer> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}