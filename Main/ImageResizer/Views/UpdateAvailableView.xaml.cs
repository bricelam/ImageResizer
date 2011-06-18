//------------------------------------------------------------------------------
// <copyright file="UpdateAvailableView.xaml.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for UpdateAvailableView.xaml
    /// </summary>
    public partial class UpdateAvailableView : Window
    {
        public UpdateAvailableView()
        {
            this.InitializeComponent();

            this.CloseButton.Click += (_, e) => this.Close();
        }
    }
}
