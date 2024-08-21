using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RUINORERP.Model.Base
{
    /*
           /// <summary>
        /// 设备ID
        /// </summary>
        public string DevId
        {
            set
            {
                _devId = value;
                OnPropertyChanged("DevId");
            }
            get { return _devId; }
        }
     
     */
    public class BaseModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
