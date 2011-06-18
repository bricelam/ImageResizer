//------------------------------------------------------------------------------
// <copyright file="ProgressPageView.xaml.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Views
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ProgressPageView.xaml
    /// </summary>
    public partial class ProgressPageView : UserControl
    {
        public ProgressPageView()
        {
            // TODO: Use Windows 7 taskbar progress reporting
            this.InitializeComponent();
        }
    }
}
