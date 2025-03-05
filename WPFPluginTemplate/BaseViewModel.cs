using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFPluginTemplate
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
#pragma warning disable
        public event PropertyChangedEventHandler PropertyChanged; // Достаточно добавить только эту строку, так как мы используем Fody
#pragma warning restore
        protected void OnPropertyChange([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void Set<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (!field.Equals(value))
            {
                field = value;
                OnPropertyChange();
            }
        }
    }
}
