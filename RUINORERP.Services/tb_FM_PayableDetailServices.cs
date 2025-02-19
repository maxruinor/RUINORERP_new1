
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:57:59
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
    /// 应付明细 如果一个采购订单多次送货时，采购入库单即可对应这里的明细
    /// </summary>
    public partial class tb_FM_PayableDetailServices : BaseServices<tb_FM_PayableDetail>, Itb_FM_PayableDetailServices
    {
        IMapper _mapper;
        public tb_FM_PayableDetailServices(IMapper mapper, IBaseRepository<tb_FM_PayableDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}