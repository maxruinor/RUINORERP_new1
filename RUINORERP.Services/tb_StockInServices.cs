
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 19:02:37
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
    /// 入库单 非生产领料/退料
    /// </summary>
    public partial class tb_StockInServices : BaseServices<tb_StockIn>, Itb_StockInServices
    {
        IMapper _mapper;
        public tb_StockInServices(IMapper mapper, IBaseRepository<tb_StockIn> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}