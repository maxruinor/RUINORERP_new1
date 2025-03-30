
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/30/2025 15:54:05
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
    /// UI表单输入数据字段设置
    /// </summary>
    public partial class tb_UIDataFieldSettingServices : BaseServices<tb_UIDataFieldSetting>, Itb_UIDataFieldSettingServices
    {
        IMapper _mapper;
        public tb_UIDataFieldSettingServices(IMapper mapper, IBaseRepository<tb_UIDataFieldSetting> dal)
        {
            this._mapper = mapper;
            base.BaseDal = dal;
        }
    }
}