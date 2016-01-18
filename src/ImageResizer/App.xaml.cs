using System;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using ImageResizer.Models;
using ImageResizer.Properties;
using ImageResizer.ViewModels;
using ImageResizer.Views;

namespace ImageResizer
{
    public partial class App : Application
    {
        static App()
        {
            Console.InputEncoding = Encoding.Unicode;
            DispatcherHelper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var batch = ResizeBatch.FromCommandLine(Console.In, e.Args);

            new MainWindow(new MainViewModel(batch, Settings.Default)).Show();
        }
    }
}
