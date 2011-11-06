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
    using PreEmptive.Attributes;

    public partial class App : Application
    {
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

            var bootstrapper = new Bootstrapper(e.Args);
            bootstrapper.Run();
        }

        [Teardown]
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}