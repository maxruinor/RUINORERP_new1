using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransInstruction.DataPortal
{
    /// <summary>
    /// 延迟退出
    /// </summary>
    public class Defer
    {
        ArrayList m_list = new ArrayList();
        public Defer()
        {

        }
        public Defer(System.Action action)
        {
            Add(action);
        }
        public void Add(System.Action action)
        {            
            m_list.Add(action);
        }
        /// <summary>
        /// 执行回调函数 
        /// </summary>
        public void Run()
        {
            foreach (var action in m_list)
            {
                (action as System.Action).Invoke();
            }
        }
    }
}
