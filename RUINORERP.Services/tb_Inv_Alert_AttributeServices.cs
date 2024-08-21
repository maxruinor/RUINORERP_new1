
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:47:35
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
    /// 存货预警特性表
    /// </summary>
    public partial class tb_Inv_Alert_AttributeServices : BaseServices<tb_Inv_Alert_Attribute>, Itb_Inv_Alert_AttributeServices
    {
        IMapper _mapper;
        public tb_Inv_Alert_AttributeServices(IMapper mapper, IBaseRepository<tb_Inv_Alert_Attribute> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}