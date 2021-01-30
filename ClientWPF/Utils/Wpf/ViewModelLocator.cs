using ClientWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClientWPF.Utils.Wpf
{
    public class ViewModelLocator
    {
        private IServiceProvider provider;
        bool isDesigner = false;

        public ViewModelLocator()
        {
            try
            {
                provider = ((App)Application.Current).provider;
            }catch(InvalidCastException e)
            {
                isDesigner = true;
            }
        }

        public SceneManagerVm SceneManager { get
            {
                if (isDesigner)
                    return new SceneManagerVm();

                var manager = provider.GetRequiredService<SceneManagerVm>();
                manager.SetServiceProvider(provider);
                return manager;
            } }

        public ChatBoxVm ChatBox
        {
            get
            {
                if (isDesigner)
                    return new ChatBoxVm();

                var chatBox = provider.GetRequiredService<ChatBoxVm>();
                return chatBox;
            }
        }
    }
}
