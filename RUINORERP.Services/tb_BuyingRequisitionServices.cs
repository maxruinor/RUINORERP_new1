
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:28
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
    /// 请购单，可能来自销售订单,也可以来自其他日常需求也可能来自生产需求也可以直接录数据，是一个纯业务性的数据表
   
    /// </summary>
    public partial class tb_BuyingRequisitionServices : BaseServices<tb_BuyingRequisition>, Itb_BuyingRequisitionServices
    {
        IMapper _mapper;
        public tb_BuyingRequisitionServices(IMapper mapper, IBaseRepository<tb_BuyingRequisition> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}