
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/15/2024 19:01:21
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
    /// 销售退货翻新物料明细表
    /// </summary>
    public partial class tb_SaleOutReRefurbishedMaterialsDetailServices : BaseServices<tb_SaleOutReRefurbishedMaterialsDetail>, Itb_SaleOutReRefurbishedMaterialsDetailServices
    {
        IMapper _mapper;
        public tb_SaleOutReRefurbishedMaterialsDetailServices(IMapper mapper, IBaseRepository<tb_SaleOutReRefurbishedMaterialsDetail> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}