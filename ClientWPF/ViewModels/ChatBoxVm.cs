using ClientWPF.Utils.Wpf;
using Shared.Alerts;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    public class ChatBoxVm : ViewModel
    {
        private readonly RequestClient client;
        private readonly Player player;

        public ChatBoxVm(RequestClient client, Player player)
        {
            this.client = client;
            this.player = player;

            ChatBox = "";
            MessageInput = "";

            client.SubscribeTo<MessageAlert>(this, ReceivedMessage);
        }

        public ChatBoxVm()
        {

        }

        private void ReceivedMessage(MessageAlert alert)
        {
            ChatBox = $"{ChatBox}\n{alert.Sender}: {alert.Message}";
            Notify("ChatBox");
        }

        public string ChatBox { get; set; }
        public string MessageInput { get; set; }

        public RelayCommand KeyDown
        {
            get
            {
                return new RelayCommand(o =>
                {
                    var e = (KeyEventArgs)o;

                    if (e.Key == Key.Enter)
                    {
                        Send();
                    }
                });
            }
        }

        public RelayCommand SendMessage { get {
                return new RelayCommand(o =>
                {
                    Send();
                });
            }
        }

        private void Send()
        {
            client.SendAction(new SayRequest { Message = MessageInput }, player);
            MessageInput = "";
            Notify("MessageInput");
        }
    }
}
