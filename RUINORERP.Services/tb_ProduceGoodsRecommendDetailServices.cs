
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/19/2024 12:18:08
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
    /// 自制成品建议
    /// </summary>
    public partial class tb_ProduceGoodsRecommendDetailServices : BaseServices<tb_ProduceGoodsRecommendDetail>, Itb_ProduceGoodsRecommendDetailServices
    {
        IMapper _mapper;
        public tb_ProduceGoodsRecommendDetailServices(IMapper mapper, IBaseRepository<tb_ProduceGoodsRecommendDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}