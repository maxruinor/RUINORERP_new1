
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:21
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
    /// 采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据
    /// </summary>
    public partial class tb_PurOrderServices : BaseServices<tb_PurOrder>, Itb_PurOrderServices
    {
        IMapper _mapper;
        public tb_PurOrderServices(IMapper mapper, IBaseRepository<tb_PurOrder> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}