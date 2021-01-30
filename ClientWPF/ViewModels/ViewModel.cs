using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientWPF.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void Notify(string changedProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(changedProperty));
        }
    }
}
