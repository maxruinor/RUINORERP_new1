
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:26
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
    /// BOM配置历史 数据保存在BOM中 只是多份一样，细微区别用版本号标识
    /// </summary>
    public partial class tb_BOMConfigHistoryServices : BaseServices<tb_BOMConfigHistory>, Itb_BOMConfigHistoryServices
    {
        IMapper _mapper;
        public tb_BOMConfigHistoryServices(IMapper mapper, IBaseRepository<tb_BOMConfigHistory> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}