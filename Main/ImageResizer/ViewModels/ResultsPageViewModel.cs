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
    using System.Windows.Input;
    using Microsoft.Practices.Prism.Commands;

    // TODO: Finish implementing this
    internal class ResultsPageViewModel
    {
        public ResultsPageViewModel()
        {
            this.CloseCommand = new DelegateCommand(this.Close);
        }

        public event EventHandler<EventArgs> Completed;

        public ICommand CloseCommand { get; set; }

        public IDictionary<string, Exception> Errors { get; set; }

        private void OnCompleted()
        {
            if (this.Completed != null)
            {
                this.Completed(this, EventArgs.Empty);
            }
        }

        private void Close()
        {
            this.OnCompleted();
        }
    }
}
