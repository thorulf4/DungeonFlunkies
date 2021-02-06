using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientWPF
{
    public class Player : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string name = "";
        private string sessionToken = "";


        public string Name { get { return name; } set { name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name")); } }
        public string SessionToken { get { return sessionToken; } set { sessionToken = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("sessionToken")); } }
    }
}
