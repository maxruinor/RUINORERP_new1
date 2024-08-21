
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/13/2023 17:34:15
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
    public partial class tb_Inv_Alert_AttributesServices : BaseServices<tb_Inv_Alert_Attributes>, Itb_Inv_Alert_AttributesServices
    {
        IMapper _mapper;
        public tb_Inv_Alert_AttributesServices(IMapper mapper, IBaseRepository<tb_Inv_Alert_Attributes> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}