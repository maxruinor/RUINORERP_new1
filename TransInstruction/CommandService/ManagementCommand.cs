using System;
using System.Collections.Generic;
using System.Text;
using TransInstruction.Enums;

namespace TransInstruction.CommandService
{
    public class ManagementCommand
    {
        public FileOperationCommand Operation { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        public T GetParameter<T>(string key)
        {
            if (Parameters.ContainsKey(key) && Parameters[key] is T value)
            {
                return value;
            }
            return default(T);
        }

        public ManagementCommandType CommandType { get; set; }
 

    }


  


}
