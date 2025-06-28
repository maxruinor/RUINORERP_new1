using Krypton.Toolkit;
using RUINORERP.Common;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using System.ComponentModel;
using System.Text.RegularExpressions;
using RUINORERP.Model;
using System.Collections;
using System.Reflection.Emit;
//using System.Workflow.ComponentModel.Serialization;
using RUINORERP.Model.Base;

namespace RUINORERP.UI.Common
{


    public class EnumBindingHelper
    {
        /// <summary>
        /// 初始化枚举值  
        /// List<EnumEntityMember> list = new List<EnumEntityMember>();
        /// list = pat.GetListByEnum(2);
        /// EnumBindingHelper.InitDataToCmbByEnumOnWhere<tb_Prod>(list, e => e.PropertyType, cmbPropertyType);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listSource"></param>
        /// <param name="expKey"></param>
        /// <param name="cmbBox"></param>
        public void InitDataToCmbByEnumOnWhere<T>(List<EnumEntityMember> listSource, Expression<Func<T, int?>> expKey, KryptonComboBox cmbBox)
        {
            MemberInfo minfo = expKey.GetMemberInfo();
            string key = minfo.Name;
            InitDataToCmbByEnumOnWhere(listSource, key, cmbBox);
            if (listSource.Find(w => w.Selected == true) != null)
            {
                cmbBox.SelectedValue = listSource.Find(w => w.Selected == true).Value;
            }
            
        }


        /// <summary>
        /// 枚举名称要与DB表中的字段名相同
        /// </summary>
        /// <param name="listSource"></param>
        /// <paramref name="EntityKeyColName">在实体中的字段名</param>
        /// <param name="cmbBox"></param>
        public void InitDataToCmbByEnumOnWhere(List<EnumEntityMember> listSource, string EntityKeyColName, KryptonComboBox cmbBox)
        {
            //枚举值为int，动态生成一个类再绑定，
            var type = typeof(EnumEntityMember);
            // var type = enumType;
            var aName = new System.Reflection.AssemblyName(Assembly.GetExecutingAssembly().GetName().Name);

            TypeConfig typeConfig = new TypeConfig();
            typeConfig.FullName = aName.Name;

            //要创建的属性
            PropertyConfig propertyConfigKey = new PropertyConfig();
            propertyConfigKey.PropertyName = EntityKeyColName;// 在实体中的字段名,type.Name;默认枚举名改为可以指定名
            propertyConfigKey.PropertyType = typeof(int);//枚举值为int 默认

            PropertyConfig propertyConfigName = new PropertyConfig();
            propertyConfigName.PropertyName = "Description";
            propertyConfigName.PropertyType = typeof(string);

            typeConfig.Properties.Add(propertyConfigKey);
            typeConfig.Properties.Add(propertyConfigName);
            Type newType = TypeBuilderHelper.BuildType(typeConfig);

            List<object> newList = new List<object>();


            foreach (var item in listSource)
            {
                object newObj = Activator.CreateInstance(newType);
                newObj.SetPropertyValue(EntityKeyColName, item.Value.ToInt());
                newObj.SetPropertyValue("Description", item.Description);
                newList.Add(newObj);
            }
            object sobj = Activator.CreateInstance(newType);
            sobj.SetPropertyValue(EntityKeyColName, -1);
            sobj.SetPropertyValue("Description", "请选择");
            newList.Insert(0, sobj);
            BindingSource bs = new BindingSource();
            bs.DataSource = newList;
            ComboBoxHelper.InitDropList(bs, cmbBox, EntityKeyColName, "Description", ComboBoxStyle.DropDownList, false);

        }


    }
}
