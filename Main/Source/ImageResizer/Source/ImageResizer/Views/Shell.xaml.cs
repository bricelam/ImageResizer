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
    using System.Windows;
    using BriceLambson.ImageResizer.ViewModels;
    using Microsoft.Practices.Prism.Events;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            this.InitializeComponent();

            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<CloseShellEvent>().Subscribe(this.HandleCloseShell, ThreadOption.UIThread);
            eventAggregator.GetEvent<ShowAdvancedEvent>().Subscribe(this.HandleShowAdvanced, ThreadOption.UIThread);
            eventAggregator.GetEvent<UpdateAvailableEvent>().Subscribe(this.HandleUpdateAvailable, ThreadOption.UIThread);
        }

        private void HandleCloseShell(object o)
        {
            this.Close();
        }

        private void HandleShowAdvanced(object o)
        {
            // TODO: Implement this
            var message = "Coming soon...\r\n" +
                          " * JPEG Quality Level\r\n" +
                          " * User-defined default sizes\r\n" +
                          " * User-defined filename format\r\n" +
                          " * Optionally strip metadata";

            MessageBox.Show(message, "Advanced Options", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void HandleUpdateAvailable(UpdateAvailableViewModel viewModel)
        {
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
