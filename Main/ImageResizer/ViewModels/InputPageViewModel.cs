//------------------------------------------------------------------------------
// <copyright file="InputPageViewModel.cs" company="Brice Lambson">
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
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.Properties;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Events;
    using Microsoft.Practices.ServiceLocation;

    internal class InputPageViewModel
    {
        private readonly List<ResizeSize> _sizes = new List<ResizeSize>();
        private readonly ICommand _showAdvancedCommand;
        private readonly ICommand _resizeCommand;
        private readonly ICommand _cancelCommand;

        public InputPageViewModel()
        {
            // Aggregate the sizes
            Sizes.AddRange(AdvancedSettings.Default.DefaultSizes);
            Sizes.Add(Settings.CustomSize);

            _showAdvancedCommand = new DelegateCommand(ShowAdvanced);
            _resizeCommand = new DelegateCommand(Resize);
            _cancelCommand = new DelegateCommand(Cancel);
        }

        public event EventHandler<InputPageCompletedEventArgs> Completed;

        public Settings Settings
        {
            get { return Settings.Default; }
        }

        public List<ResizeSize> Sizes
        {
            get { return _sizes; }
        }

        public ICommand ShowAdvancedCommand
        {
            get { return _showAdvancedCommand; }
        }

        public ICommand ResizeCommand
        {
            get { return _resizeCommand; }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
        }

        protected virtual void OnCompleted(bool cancelled)
        {
            if (Completed != null)
            {
                Completed(this, new InputPageCompletedEventArgs(cancelled));
            }
        }

        private void ShowAdvanced()
        {
            ServiceLocator.Current
                .GetInstance<IEventAggregator>()
                .GetEvent<ShowAdvancedEvent>()
                .Publish(null);
        }

        private void Resize()
        {
            Settings.Default.Save();

            OnCompleted(false);
        }

        private void Cancel()
        {
            OnCompleted(true);
        }
    }
}