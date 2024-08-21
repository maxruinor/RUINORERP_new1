
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/17/2024 19:26:52
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
    /// 库存视图
    /// </summary>
    public partial class View_InventoryServices : BaseServices<View_Inventory>, IView_InventoryServices
    {
        IMapper _mapper;
        public View_InventoryServices(IMapper mapper, IBaseRepository<View_Inventory> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}