
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/05/2024 17:00:21
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
    /// 生产需求分析表 是一个中间表，由计划生产单或销售订单带入数据来分析，产生采购订单再产生制令单，分析时有三步，库存不足项（包括有成品材料所有项），采购产品建议，自制品成品建议,中间表保存记录而已，操作UI上会有生成采购订单，或生产单等操作
    /// </summary>
    public partial class tb_ProductionDemandServices : BaseServices<tb_ProductionDemand>, Itb_ProductionDemandServices
    {
        IMapper _mapper;
        public tb_ProductionDemandServices(IMapper mapper, IBaseRepository<tb_ProductionDemand> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}