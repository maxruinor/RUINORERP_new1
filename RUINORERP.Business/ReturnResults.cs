using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    /// <summary>
    /// 通用的布尔结果与数据列表组合返回类
    /// 适用于需要同时返回判断结果和相关数据列表的场景
    /// </summary>
    public class BooleanWithDataListResult<TData>
    {
        
        /// <summary>
        /// 关联的数据列表
        /// </summary>
        public List<TData> DataList { get; set; }
        public string ErrorMsg { get; internal set; }
        public bool Succeeded { get; internal set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BooleanWithDataListResult()
        {
            DataList = new List<TData>();
        }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hasData">是否有数据</param>
        /// <param name="dataList">数据列表</param>
        public BooleanWithDataListResult(bool hasData, List<TData> dataList)
        {
            DataList = dataList ?? new List<TData>();
        }
    }
    
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
