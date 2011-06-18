//------------------------------------------------------------------------------
// <copyright file="ProgressPageCompletedEventArgs.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.ViewModels
{
    using System;
    using System.Collections.Generic;

    internal class ProgressPageCompletedEventArgs : EventArgs
    {
        public ProgressPageCompletedEventArgs(IDictionary<string, Exception> errors)
        {
            this.Errors = errors;
        }

        public IDictionary<string, Exception> Errors { get; set; }
    }
}
