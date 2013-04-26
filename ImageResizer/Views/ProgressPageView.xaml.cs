//------------------------------------------------------------------------------
// <copyright file="ProgressPageView.xaml.cs" company="Brice Lambson">
//     Copyright (c) 2011-2013 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using BriceLambson.ImageResizer.Models;
    using GalaSoft.MvvmLight.Messaging;

    public partial class ProgressPageView : UserControl
    {
        public ProgressPageView()
        {
            // TODO: Use Windows 7 taskbar progress reporting
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new ProgressPageLoadedMessage());
        }
    }
}