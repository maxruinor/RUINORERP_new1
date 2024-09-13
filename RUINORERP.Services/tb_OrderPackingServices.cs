
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:57
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
    /// 订单装箱单，针对特别订单详细处理 也用主子表来做。暂时不搞
    /// </summary>
    public partial class tb_OrderPackingServices : BaseServices<tb_OrderPacking>, Itb_OrderPackingServices
    {
        IMapper _mapper;
        public tb_OrderPackingServices(IMapper mapper, IBaseRepository<tb_OrderPacking> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}