//------------------------------------------------------------------------------
// <copyright file="Shell.xaml.cs" company="Brice Lambson">
//     Copyright (c) 2011-2013 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Views
{
    using System.Diagnostics;
    using System.Windows;
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.ViewModels;
    using GalaSoft.MvvmLight.Messaging;

    public partial class Shell : Window
    {
        public Shell()
        {
            Messenger.Default.Register<CloseShellMessage>(this, m => Close());
            Messenger.Default.Register<ShowAdvancedMessage>(this, m => ShowAdvanced());
            Messenger.Default.Register<UpdateAvailableMessage>(this, m => ShowUpdateAvailable(m.Content));

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new ShellLoadedMessage());
        }

        private void ShowAdvanced()
        {
            var view = new AdvancedPageView();
            view.Show();
            //TODO: Implement this
            //var message = "Coming soon...\r\n" +
            //              " * Editable default sizes\r\n" +
            //              " * Option to keep modified date\r\n" +
            //              " * Custom filenames\r\n" +
            //              " * Select JPEG quality level\r\n" +
            //              " * Option to minimize file size";

            //MessageBox.Show(message, "Advanced Options", MessageBoxButton.OK, MessageBoxImage.Information);  
        }

        private void ShowUpdateAvailable(Update update)
        {
            Debug.Assert(update != null);

            // TODO: Show a notification once per day for a total of three times that opens
            //       this dialog when clicked
            var updateAvailable = new UpdateAvailableView
            {
                DataContext = new UpdateAvailableViewModel(update)
            };

            updateAvailable.ShowDialog();
        }
    }
}