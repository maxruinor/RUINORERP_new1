﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 19:02:38
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
    /// 入库单明细
    /// </summary>
    public partial class tb_StockInDetailServices : BaseServices<tb_StockInDetail>, Itb_StockInDetailServices
    {
        IMapper _mapper;
        public tb_StockInDetailServices(IMapper mapper, IBaseRepository<tb_StockInDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}