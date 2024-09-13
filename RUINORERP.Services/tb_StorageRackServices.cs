
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:37
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
    /// 货架信息表
    /// </summary>
    public partial class tb_StorageRackServices : BaseServices<tb_StorageRack>, Itb_StorageRackServices
    {
        IMapper _mapper;
        public tb_StorageRackServices(IMapper mapper, IBaseRepository<tb_StorageRack> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}