//------------------------------------------------------------------------------
// <copyright file="InputPageCompletedEventArgs.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.ViewModels
{
    using System;

    internal class InputPageCompletedEventArgs : EventArgs
    {
        public InputPageCompletedEventArgs(bool cancelled)
        {
            this.Cancelled = cancelled;
        }

        public bool Cancelled { get; set; }
    }
}
