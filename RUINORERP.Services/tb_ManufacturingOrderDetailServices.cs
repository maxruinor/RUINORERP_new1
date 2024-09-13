
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:50
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
    /// 制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节
    /// </summary>
    public partial class tb_ManufacturingOrderDetailServices : BaseServices<tb_ManufacturingOrderDetail>, Itb_ManufacturingOrderDetailServices
    {
        IMapper _mapper;
        public tb_ManufacturingOrderDetailServices(IMapper mapper, IBaseRepository<tb_ManufacturingOrderDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}