
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/30/2024 00:18:28
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
    /// UI表格列设置
    /// </summary>
    public partial class tb_UIGridColsSettingServices : BaseServices<tb_UIGridColsSetting>, Itb_UIGridColsSettingServices
    {
        IMapper _mapper;
        public tb_UIGridColsSettingServices(IMapper mapper, IBaseRepository<tb_UIGridColsSetting> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}