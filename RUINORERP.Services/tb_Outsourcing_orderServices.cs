
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:58
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
    /// 外发加工订单表
    /// </summary>
    public partial class tb_Outsourcing_orderServices : BaseServices<tb_Outsourcing_order>, Itb_Outsourcing_orderServices
    {
        IMapper _mapper;
        public tb_Outsourcing_orderServices(IMapper mapper, IBaseRepository<tb_Outsourcing_order> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}