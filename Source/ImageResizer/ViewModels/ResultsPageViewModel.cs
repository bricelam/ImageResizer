//------------------------------------------------------------------------------
// <copyright file="ResultsPageViewModel.cs" company="Brice Lambson">
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
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using BriceLambson.ImageResizer.Models;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    internal class ResultsPageViewModel : ViewModelBase
    {
        private readonly ICommand _closeCommand;
        private readonly ICollection<ResizeError> _errors;

        public ResultsPageViewModel(ICollection<ResizeError> errors)
        {
            Contract.Requires(errors != null);

            _closeCommand = new RelayCommand(Close);

            _errors = errors;
        }

        public event EventHandler<EventArgs> Completed;

        public ICommand CloseCommand
        {
            get { return _closeCommand; }
        }

        public ICollection<ResizeError> Errors
        {
            get { return _errors; }
        }

        private void Close()
        {
            if (Completed != null)
            {
                Completed(this, EventArgs.Empty);
            }
        }
    }
}