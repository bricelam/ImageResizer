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
    using Microsoft.Practices.Prism.Commands;

    internal class ResultsPageViewModel
    {
        private readonly ICommand _closeCommand;
        private readonly IDictionary<string, Exception> _errors;

        public ResultsPageViewModel(IDictionary<string, Exception> errors)
        {
            Contract.Requires(errors != null);

            _closeCommand = new DelegateCommand(Close);

            _errors = errors;
        }

        public event EventHandler<EventArgs> Completed;

        public ICommand CloseCommand
        {
            get { return _closeCommand; }
        }

        public IDictionary<string, Exception> Errors
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