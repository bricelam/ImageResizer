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

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var shell = new Shell
                {
                    DataContext = new ShellViewModel(e.Args)
                };
            shell.Show();
        }
    }
}