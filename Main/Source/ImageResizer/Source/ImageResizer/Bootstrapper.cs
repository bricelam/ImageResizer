//------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer
{
    using System.Windows;
    using BriceLambson.ImageResizer.Services;
    using BriceLambson.ImageResizer.ViewModels;
    using BriceLambson.ImageResizer.Views;
    using Microsoft.Practices.Prism.MefExtensions;

    internal class Bootstrapper : MefBootstrapper
    {
        private string[] args;

        public Bootstrapper(string[] args)
        {
            this.args = args;
        }

        protected override DependencyObject CreateShell()
        {
            var shell = new Shell
            {
                DataContext = new ShellViewModel(this.args)
            };

            return shell;
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }
    }
}
