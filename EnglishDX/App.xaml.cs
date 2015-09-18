using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace EnglishDX
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e) {
            if (e.Args.Count() > 0 && e.Args[0] == "-testBase") {
                ViewModel.IsTestMode = true;
            }
        }
    }
}
