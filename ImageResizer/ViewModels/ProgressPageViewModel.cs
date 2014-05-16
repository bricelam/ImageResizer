//------------------------------------------------------------------------------
// <copyright file="ProgressPageViewModel.cs" company="Brice Lambson">
//     Copyright (c) 2011-2013 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BriceLambson.ImageResizer.Helpers;
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.Properties;
    using BriceLambson.ImageResizer.Services;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    internal class ProgressPageViewModel : ViewModelBase, IDisposable
    {
        private readonly object _detailsSync = new Object();
        private readonly object _progressSync = new Object();
        private readonly ICollection<ResizeError> _errors = new Collection<ResizeError>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly string[] _args;
        private readonly ICommand _stopCommand;

        private double _progress;
        private string _currentImage;

        public ProgressPageViewModel(string[] args)
        {
            Debug.Assert(args != null);

            Messenger.Default.Register<ProgressPageLoadedMessage>(this, m => OnLoadedAsync());

            _stopCommand = new RelayCommand(Stop);
            _args = args;
        }

        public event EventHandler<ProgressPageCompletedEventArgs> Completed;

        public ICommand StopCommand
        {
            get { return _stopCommand; }
        }

        public string CurrentImage
        {
            get { return _currentImage; }
            set { Set(() => CurrentImage, ref _currentImage, value); }
        }

        public double Progress
        {
            get { return _progress; }
            set
            {
                Debug.Assert(value >= 0 && value <= 100);

                Set(() => Progress, ref _progress, value);
            }
        }

        private async void OnLoadedAsync()
        {
            var parameters = await ParametersHelper.ParseAsync(_args);
            ResizeAsync(parameters);
        }

        private async void ResizeAsync(Parameters parameters)
        {
            var imageCount = parameters.SelectedFiles.Count();
            var resizer
                = new ResizingService(
                    AdvancedSettings.Default.QualityLevel,
                    Settings.Default.ShrinkOnly,
                    Settings.Default.IgnoreRotations,
                    AdvancedSettings.Default.KeepMetadata,
                    Settings.Default.SelectedSize,        
                    new RenamingService(
                        AdvancedSettings.Default.FileNameFormat,
                        parameters.OutputDirectory,
                        Settings.Default.ReplaceOriginals,
                        Settings.Default.SelectedSize));

            try
            {
                await TaskEx.Run(
                    () =>
                        Parallel.ForEach(
                            parameters.SelectedFiles,
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

        public virtual void Dispose()
        {
            _cancellationTokenSource.Dispose();
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
            Debug.Assert(!String.IsNullOrWhiteSpace(image));
            Debug.Assert(ex != null);

            lock (_errors)
            {
                _errors.Add(new ResizeError(image, ex));
            }
        }
    }
}