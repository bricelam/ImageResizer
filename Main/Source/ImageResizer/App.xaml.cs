//------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer
{
    using System.Windows;
    using BriceLambson.ImageResizer.Properties;
    using BriceLambson.ImageResizer.ViewModels;
    using BriceLambson.ImageResizer.Views;
    using PreEmptive.Attributes;

    public partial class App : Application
    {
        public App()
        {
            if (AdvancedSettings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                AdvancedSettings.Default.Upgrade();

                AdvancedSettings.Default.UpgradeRequired = false;
                AdvancedSettings.Default.Save();
            }
        }

        private bool AllowAnalytics
        {
            get { return AdvancedSettings.Default.AllowAnalytics; }
        }

        [Setup(
            CustomEndpoint = "so-s.info/PreEmptive.Web.Services.Messaging/MessagingServiceV2.asmx",
            OptInSourceElement = SourceElements.Property,
            OptInSourceName = "AllowAnalytics")]
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var shell = new Shell
                {
                    DataContext = new ShellViewModel(e.Args)
                };
            shell.Show();
        }

        [Teardown]
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}