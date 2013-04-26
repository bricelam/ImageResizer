//------------------------------------------------------------------------------
// <copyright file="ProgressPageCompletedEventArgs.cs" company="Brice Lambson">
//     Copyright (c) 2011-2013 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    internal class ProgressPageCompletedEventArgs : EventArgs
    {
        private readonly ICollection<ResizeError> _errors;

        public ProgressPageCompletedEventArgs(ICollection<ResizeError> errors)
        {
            Debug.Assert(errors != null);

            _errors = errors;
        }

        public ICollection<ResizeError> Errors
        {
            get { return _errors; }
        }
    }
}