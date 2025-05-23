﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/13/2025 22:52:41
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
    /// 往来单位类型,如级别，电商，大客户，亚马逊等
    /// </summary>
    public partial class tb_CustomerVendorTypeServices : BaseServices<tb_CustomerVendorType>, Itb_CustomerVendorTypeServices
    {
        IMapper _mapper;
        public tb_CustomerVendorTypeServices(IMapper mapper, IBaseRepository<tb_CustomerVendorType> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}