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
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using BriceLambson.ImageResizer.ViewModels;
    using BriceLambson.ImageResizer.Views;
    using Microsoft.Practices.Prism.MefExtensions;

    internal class Bootstrapper : MefBootstrapper
    {
        private readonly string[] _args;

        public Bootstrapper(string[] args)
        {
            Contract.Requires(args != null);

            _args = args;
        }

        protected override DependencyObject CreateShell()
        {
            return new Shell
                {
                    DataContext = new ShellViewModel(_args)
                };
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }
    }
}