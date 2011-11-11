//------------------------------------------------------------------------------
// <copyright file="Shell.xaml.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Views
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.ViewModels;
    using Microsoft.Practices.Prism.Events;
    using Microsoft.Practices.ServiceLocation;

    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();

            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<CloseShellEvent>().Subscribe(HandleCloseShell, ThreadOption.UIThread);
            eventAggregator.GetEvent<ShowAdvancedEvent>().Subscribe(HandleShowAdvanced, ThreadOption.UIThread);
            eventAggregator.GetEvent<UpdateAvailableEvent>().Subscribe(HandleUpdateAvailable, ThreadOption.UIThread);
        }

        private void HandleCloseShell(object o)
        {
            Close();
        }

        private void HandleShowAdvanced(object o)
        {
            // TODO: Implement this
            var message = "Coming soon...\r\n" +
                          " * JPEG Quality Level\r\n" +
                          " * User-defined default sizes\r\n" +
                          " * User-defined filename format\r\n" +
                          " * Optionally strip metadata\r\n" +
                          " * Optionally keep modified date";

            MessageBox.Show(message, "Advanced Options", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void HandleUpdateAvailable(UpdateAvailableViewModel viewModel)
        {
            Contract.Requires(viewModel != null);

            // TODO: Show a notification once per day for a total of three times that opens
            //       this dialog when clicked
            var updateAvailable = new UpdateAvailableView
            {
                DataContext = viewModel
            };

            updateAvailable.ShowDialog();
        }
    }
}