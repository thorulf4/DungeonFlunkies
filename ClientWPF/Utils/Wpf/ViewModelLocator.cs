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

        public ViewModelLocator()
        {
            provider = ((App)Application.Current).provider;
        }

        public SceneManagerVm SceneManager { get
            {
                provider.GetService<RequestClient>();
                return new SceneManagerVm();
            } }
    }
}
