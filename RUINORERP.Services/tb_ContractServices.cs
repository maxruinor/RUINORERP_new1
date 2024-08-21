
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:46:51
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
    /// 先销售合同再订单,条款内容后面补充
    /// </summary>
    public partial class tb_ContractServices : BaseServices<tb_Contract>, Itb_ContractServices
    {
        IMapper _mapper;
        public tb_ContractServices(IMapper mapper, IBaseRepository<tb_Contract> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}