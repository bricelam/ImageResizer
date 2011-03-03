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
    using BriceLambson.ImageResizer.Model;
    using BriceLambson.ImageResizer.Properties;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Prism.Events;

    internal class InputPageViewModel
    {
        private IEventAggregator eventAggregator;
        private List<ResizeSize> sizes;

        public InputPageViewModel(Settings settings, IEventAggregator eventAggregator)
        {
            this.Settings = settings;
            this.eventAggregator = eventAggregator;

            // Aggregate the sizes
            this.sizes = new List<ResizeSize>();
            this.sizes.AddRange(settings.DefaultSizes);
            this.sizes.Add(settings.CustomSize);

            this.ShowAdvancedCommand = new DelegateCommand(this.ShowAdvanced);
            this.ResizeCommand = new DelegateCommand(this.Resize);
            this.CancelCommand = new DelegateCommand(this.Cancel);
        }

        public event EventHandler<InputPageCompletedEventArgs> Completed;

        public Settings Settings { get; private set; }

        public List<ResizeSize> Sizes
        {
            get { return this.sizes; }
        }

        public ICommand ShowAdvancedCommand { get; set; }

        public ICommand ResizeCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        private void OnCompleted(bool cancelled)
        {
            if (this.Completed != null)
            {
                var e = new InputPageCompletedEventArgs(cancelled);

                this.Completed(this, e);
            }
        }

        private void ShowAdvanced()
        {
            this.eventAggregator.GetEvent<ShowAdvancedEvent>().Publish(null);
        }

        private void Resize()
        {
            // TODO: This may conflict with the Advanced settings
            this.Settings.Save();

            this.OnCompleted(false);
        }

        private void Cancel()
        {
            this.OnCompleted(true);
        }
    }
}
