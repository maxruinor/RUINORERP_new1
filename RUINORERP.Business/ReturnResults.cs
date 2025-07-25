using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public class ReturnResults<T>
    {
        private bool succeeded = false;
        private string error;
        private T returnObject;
        public ReturnResults()
        {
           
        }
        public ReturnResults(bool _Succeeded)
        {
            Succeeded = _Succeeded;
        }
        public bool Succeeded { get => succeeded; set => succeeded = value; }

        public string ErrorMsg { get => error; set => error = value; }

        private List<string> _errorMsgs = new List<string>();

        public T ReturnObject { get => returnObject; set => returnObject = value; }


        private object returnObjectAsOtherEntity;
        /// <summary>
        /// 销售出库审核时，如果生成了应该收款单则返回时放到这里
        /// </summary>
        public object ReturnObjectAsOtherEntity  { get => returnObjectAsOtherEntity; set => returnObjectAsOtherEntity = value; }

       // public List<string> ErrorMsgs { get => _errorMsgs; set => _errorMsgs = value; }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">生成对象的表</typeparam>
    public class ReturnMainSubResults<T>
    {
        private bool succeeded = false;
        private string error;
        private T returnObject;
        public T ReturnObject { get => returnObject; set => returnObject = value; }


        //private List<object> returnSubObjlist;
        //public List<object> ReturnSubObjlist { get => returnSubObjlist; set => returnSubObjlist = value; }

        public bool Succeeded { get => succeeded; set => succeeded = value; }
        public string ErrorMsg { get => error; set => error = value; }
        
    }

}
