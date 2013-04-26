//------------------------------------------------------------------------------
// <copyright file="InputPageCompletedEventArgs.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Models
{
    using System;

    internal class InputPageCompletedEventArgs : EventArgs
    {
        private readonly bool _cancelled;

        public InputPageCompletedEventArgs(bool cancelled)
        {
            _cancelled = cancelled;
        }

        public bool Cancelled
        {
            get { return _cancelled; }
        }
    }
}