using ClientWPF.Utils.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientWPF.ViewModels
{
    public class ChatBoxVm : ViewModel
    {
        public string ChatBox { get; set; }
        public string MessageInput { get; set; }

        public RelayCommand SendMessage { get {
                return new RelayCommand(o =>
                {

                });
            }
        }

    }
}
