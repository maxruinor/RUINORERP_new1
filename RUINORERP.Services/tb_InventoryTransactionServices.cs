
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:09:52
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
    /// 库存流水表
    /// </summary>
    public partial class tb_InventoryTransactionServices : BaseServices<tb_InventoryTransaction>, Itb_InventoryTransactionServices
    {
        IMapper _mapper;
        public tb_InventoryTransactionServices(IMapper mapper, IBaseRepository<tb_InventoryTransaction> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}