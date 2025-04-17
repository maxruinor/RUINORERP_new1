using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Comm
{



    public class BlacklistEntry : INotifyPropertyChanged
    {
        private string _ip地址;
        private DateTime _解封时间;
        private string _剩余时间;
        public string IP地址
        {
            get => _ip地址;
            set
            {
                if (_ip地址 != value)
                {
                    _ip地址 = value;
                    OnPropertyChanged(nameof(IP地址));
                }
            }
        }

        public DateTime 解封时间
        {
            get => _解封时间;
            set
            {
                if (_解封时间 != value)
                {
                    _解封时间 = value;
                    OnPropertyChanged(nameof(解封时间));
                }
            }
        }


        public string 剩余时间
        {
            get
            {
                 _剩余时间 = (解封时间 - DateTime.Now).ToString(@"hh\:mm\:ss");
                return _剩余时间;
            }
            set
            {
                if (_剩余时间 != value)
                {
                    value = (解封时间 - DateTime.Now).ToString(@"hh\:mm\:ss");
                    _剩余时间 = value;
                    OnPropertyChanged(nameof(剩余时间));
                }
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



}
