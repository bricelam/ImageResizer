//------------------------------------------------------------------------------
// <copyright file="ProgressPageViewModel.cs" company="Brice Lambson">
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
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Input;
    using BriceLambson.ImageResizer.Properties;
    using BriceLambson.ImageResizer.Services;
    using Microsoft.Practices.Prism.Commands;

    internal class ProgressPageViewModel : INotifyPropertyChanged, IDisposable
    {
        private Settings settings;
        private ParameterService parameterService;
        private BackgroundWorker backgroundWorker;
        private double progress;
        private IDictionary<string, Exception> errors;

        public ProgressPageViewModel(Settings settings, ParameterService parameterService)
        {
            this.settings = settings;
            this.parameterService = parameterService;

            // TODO: This will need to be thread-safe
            this.errors = new Dictionary<string, Exception>();

            this.backgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };
            this.backgroundWorker.DoWork += (_, e) => e.Cancel = this.Resize();
            this.backgroundWorker.RunWorkerCompleted += (_, e) => this.OnCompleted(this.errors);

            this.StopCommand = new DelegateCommand(this.Stop);
        }

        ~ProgressPageViewModel()
        {
            this.Dispose(false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ProgressPageCompletedEventArgs> Completed;

        public ICommand StopCommand { get; set; }

        public string CurrentImage { get; set; }

        public double Progress
        {
            get
            {
                return this.progress;
            }

            set
            {
                if (this.progress != value)
                {
                    this.progress = value;

                    // TODO: Extract this from a lambda for compile-time checking
                    this.OnPropertyChanged("Progress");
                }
            }
        }

        public void ResizeAsync()
        {
            if (!this.backgroundWorker.IsBusy)
            {
                this.backgroundWorker.RunWorkerAsync();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.backgroundWorker != null)
            {
                this.backgroundWorker.Dispose();
            }
        }

        private void OnCompleted(IDictionary<string, Exception> errors)
        {
            if (this.Completed != null)
            {
                this.Completed(this, new ProgressPageCompletedEventArgs(errors));
            }
        }

        private bool Resize()
        {
            var parameters = this.parameterService.Parameters;
            var cancelled = false;
            var resizer = new ResizingService(this.settings);

            var imageCount = parameters.SelectedFiles.Count;

            // TODO: Multi-thread this
            for (var i = 0; i < imageCount && !cancelled; i++)
            {
                var image = parameters.SelectedFiles[i];

                this.CurrentImage = Path.GetFileName(image);

                try
                {
                    resizer.Resize(image, parameters.OutputDirectory);
                }
                catch (Exception ex)
                {
                    this.errors[image] = ex;
                }

                // TODO: Estimate time remaining
                this.Progress = (i + 1) / (double)imageCount * 100;

                cancelled = this.backgroundWorker.CancellationPending;
            }

            return cancelled;
        }

        private void Stop()
        {
            this.backgroundWorker.CancelAsync();
        }
    }
}
