
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:30:04
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
    /// 库存快照表
    /// </summary>
    public partial class tb_InventorySnapshotServices : BaseServices<tb_InventorySnapshot>, Itb_InventorySnapshotServices
    {
        IMapper _mapper;
        public tb_InventorySnapshotServices(IMapper mapper, IBaseRepository<tb_InventorySnapshot> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}