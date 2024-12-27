
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/27/2024 11:23:52
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
    /// 标准物料表BOM明细的替代材料表-使用优化级按库存量-成本-保质时间在配置来确定
    /// </summary>
    public partial class tb_BOM_SDetailSubstituteMaterialServices : BaseServices<tb_BOM_SDetailSubstituteMaterial>, Itb_BOM_SDetailSubstituteMaterialServices
    {
        IMapper _mapper;
        public tb_BOM_SDetailSubstituteMaterialServices(IMapper mapper, IBaseRepository<tb_BOM_SDetailSubstituteMaterial> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}