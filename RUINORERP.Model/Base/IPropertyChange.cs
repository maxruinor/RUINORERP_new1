using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace RUINORERP.Model.Base
{
    public interface IPropertyChange: INotifyPropertyChanged
    {
        // public event PropertyChangedEventHandler PropertyChanged;
        bool SuppressNotifyPropertyChanged { get; set; }

        void OnPropertyChanged(string propertyName);

        void OnPropertyChanged<T>(Expression<Func<T>> expr);
        void SetProperty<T>(ref T propField, T value, Expression<Func<T>> expr);
    }
}
