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
    using GalaSoft.MvvmLight.Messaging;

    public partial class Shell : Window, IDisposable
    {
        public Shell()
        {
            InitializeComponent();

            Messenger.Default.Register<CloseShellMessage>(this, HandleCloseShell);
            Messenger.Default.Register<ShowAdvancedMessage>(this, HandleShowAdvanced);
            Messenger.Default.Register<UpdateAvailableMessage>(this, HandleUpdateAvailable);
        }

        ~Shell()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Messenger.Default.Unregister(this);
            }
        }

        private void HandleCloseShell(CloseShellMessage m)
        {
            Contract.Requires(m != null);

            Close();
        }

        private void HandleShowAdvanced(ShowAdvancedMessage m)
        {
            Contract.Requires(m != null);

            // TODO: Implement this
            var message = "Coming soon...\r\n" +
                          " * Editable default sizes\r\n" +
                          " * Option to keep modified date\r\n" +
                          " * Custom filenames\r\n" +
                          " * Select JPEG quality level\r\n" +
                          " * Option to minimize file size";

            MessageBox.Show(message, "Advanced Options", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void HandleUpdateAvailable(UpdateAvailableMessage m)
        {
            Contract.Requires(m != null);

            // TODO: Show a notification once per day for a total of three times that opens
            //       this dialog when clicked
            var updateAvailable = new UpdateAvailableView
            {
                DataContext = new UpdateAvailableViewModel(m.Content)
            };

            updateAvailable.ShowDialog();
        }
    }
}