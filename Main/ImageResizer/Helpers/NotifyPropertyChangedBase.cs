//------------------------------------------------------------------------------
// <copyright file="NotifyPropertyChangedBase.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;

    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            Contract.Requires<ArgumentNullOrWhiteSpaceException>(!String.IsNullOrWhiteSpace(propertyName), "propertyName");

            if (PropertyChanged != null)
            {
                // TODO: Extract propertyName from a lambda for compile-time checking
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}