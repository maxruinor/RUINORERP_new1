using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Common.CustomAttribute
{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class NoWantIOCAttribute : Attribute
    {
        /*
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

         
            //如果控制器类上有SkipCheckLoginAttribute特性标签，则直接return
            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipCheckLoginAttribute), false))
            {

                return;
            }
            //如果控action方法上有SkipCheckLoginAttribute特性标签，则直接return
            if (filterContext.ActionDescriptor.IsDefined(typeof(SkipCheckLoginAttribute), false))
            {
                return;
            }
        */




    }
}
