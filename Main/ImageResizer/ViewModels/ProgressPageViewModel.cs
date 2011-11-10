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
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BriceLambson.ImageResizer.Helpers;
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.Properties;
    using BriceLambson.ImageResizer.Services;
    using Microsoft.Practices.Prism.Commands;

    internal class ProgressPageViewModel : NotifyPropertyChangedBase, IDisposable
    {
        private readonly object _detailsSync = new Object();
        private readonly object _progressSync = new Object();
        private readonly IDictionary<string, Exception> _errors = new Dictionary<string, Exception>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Parameters _parameters;
        private readonly ICommand _stopCommand;

        private double _progress;
        private string _currentImage;

        public ProgressPageViewModel(Parameters parameters)
        {
            Contract.Requires(parameters != null);

            _stopCommand = new DelegateCommand(Stop);

            _parameters = parameters;
        }

        ~ProgressPageViewModel()
        {
            Dispose(false);
        }

        public event EventHandler<ProgressPageCompletedEventArgs> Completed;

        public ICommand StopCommand
        {
            get { return _stopCommand; }
        }

        public string CurrentImage
        {
            get { return _currentImage; }
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;

                    OnPropertyChanged("CurrentImage");
                }
            }
        }

        public double Progress
        {
            get { return _progress; }
            set
            {
                Contract.Requires(value >= 0 && value <= 100);

                if (_progress != value)
                {
                    _progress = value;

                    OnPropertyChanged("Progress");
                }
            }
        }

        public async void ResizeAsync()
        {
            var imageCount = _parameters.SelectedFiles.Count();
            var resizer
                = new ResizingService(
                    AdvancedSettings.Default.QualityLevel,
                    Settings.Default.ShrinkOnly,
                    Settings.Default.IgnoreRotations,
                    Settings.Default.SelectedSize,
                    new RenamingService(
                        AdvancedSettings.Default.FileNameFormat,
                        _parameters.OutputDirectory,
                        Settings.Default.ReplaceOriginals,
                        Settings.Default.SelectedSize));

            try
            {
                await TaskEx.Run(
                    () => Parallel.ForEach(
                        _parameters.SelectedFiles,
                        new ParallelOptions
                        {
                            CancellationToken = _cancellationTokenSource.Token,
                            MaxDegreeOfParallelism = Environment.ProcessorCount
                        },
                        image =>
                        {
                            lock (_detailsSync)
                            {
                                // TODO: Show more details
                                CurrentImage = Path.GetFileName(image);
                            }

                            try
                            {
                                resizer.Resize(image);
                            }
                            catch (Exception ex)
                            {
                                AddError(image, ex);
                            }

                            lock (_progressSync)
                            {
                                Progress += 100 / (double)imageCount;
                            }
                        }));
            }
            catch (OperationCanceledException)
            {
            }

            OnCompleted();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _cancellationTokenSource != null)
            {
                _cancellationTokenSource.Dispose();
            }
        }

        protected virtual void OnCompleted()
        {
            if (Completed != null)
            {
                Completed(this, new ProgressPageCompletedEventArgs(_errors));
            }
        }

        private void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private void AddError(string image, Exception ex)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(image));
            Contract.Requires(ex != null);

            lock (_errors)
            {
                _errors.Add(image, ex);
            }
        }
    }
}