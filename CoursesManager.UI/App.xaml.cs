﻿using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace CoursesManager.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow mw = new();

            mw.Show();
        }
    }

}
