//------------------------------------------------------------------------------
// <copyright file="ResizingService.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Services
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using BriceLambson.ImageResizer.Helpers;
    using BriceLambson.ImageResizer.Models;
    using Microsoft.VisualBasic.FileIO;

    internal class ResizingService
    {
        private const string DefaultEncoderExtension = ".jpg";
        private static readonly Type DefaultEncoderType = typeof(JpegBitmapEncoder);

        private readonly int _qualityLevel;
        private readonly bool _shrinkOnly;
        private readonly bool _ignoreRotations;
        private readonly ResizeSize _size;
        private readonly RenamingService _renamer;

        public ResizingService(int qualityLevel, bool shrinkOnly, bool ignoreRotations, ResizeSize size, RenamingService renamer)
        {
            Contract.Requires(qualityLevel >= 1 && qualityLevel <= 100);
            Contract.Requires(size != null);
            Contract.Requires(renamer != null);

            _qualityLevel = qualityLevel;
            _shrinkOnly = shrinkOnly;
            _ignoreRotations = ignoreRotations;
            _size = size;
            _renamer = renamer;
        }

        public string Resize(string sourcePath)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(sourcePath));
            Contract.Ensures(!String.IsNullOrWhiteSpace(Contract.Result<string>()));

            var encoderDefaulted = false;
            BitmapDecoder decoder;

            using (var sourceStream = File.OpenRead(sourcePath))
            {
                // NOTE: Using BitmapCacheOption.OnLoad here will read the entire file into
                //       memory which allows us to dispose of the file stream immediately
                decoder = BitmapDecoder.Create(sourceStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }

            var encoder = BitmapEncoder.Create(decoder.CodecInfo.ContainerFormat);

            try
            {
                // NOTE: This will throw if the codec dose not support encoding
                var _ = encoder.CodecInfo;
            }
            catch (NotSupportedException)
            {
                // Fallback to JPEG encoder
                encoder = (BitmapEncoder)Activator.CreateInstance(DefaultEncoderType);
                encoderDefaulted = true;
            }

            // TODO: Copy container-level metadata if codec supports it
            SetEncoderSettings(encoder);

            string destinationPath = null;

            // NOTE: Only TIFF and GIF images support multiple frames
            foreach (var sourceFrame in decoder.Frames)
            {
                // Apply the transform
                var transform = GetTransform(sourceFrame);
                var transformedBitmap = new TransformedBitmap(sourceFrame, transform);

                // TODO: Optionally copy metadata
                // Create the destination frame
                var thumbnail = sourceFrame.Thumbnail;
                var metadata = sourceFrame.Metadata as BitmapMetadata;
                var colorContexts = sourceFrame.ColorContexts;
                var destinationFrame = BitmapFrame.Create(transformedBitmap, thumbnail, metadata, colorContexts);

                encoder.Frames.Add(destinationFrame);

                // Set the destination path using the first frame
                if (destinationPath == null)
                {
                    if (encoderDefaulted)
                    {
                        sourcePath = Path.ChangeExtension(sourcePath, DefaultEncoderExtension);
                    }

                    destinationPath = _renamer.Rename(sourcePath);
                }
            }

            var fileExists = File.Exists(destinationPath);
            var finalPath = destinationPath;

            if (fileExists)
            {
                destinationPath = Path.GetTempFileName();
            }

            using (var destinationStream = File.OpenWrite(destinationPath))
            {
                // Save the final image
                encoder.Save(destinationStream);
            }

            // Move any existing file to the Recycle Bin
            if (fileExists)
            {
                // TODO: Is there a better way to do this without a reference to Microsoft.VisualBasic?
                FileSystem.DeleteFile(finalPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                File.Move(destinationPath, finalPath);
            }

            return finalPath;
        }

        private void SetEncoderSettings(BitmapEncoder encoder)
        {
            Contract.Requires(encoder != null);

            var jpegEncoder = encoder as JpegBitmapEncoder;

            if (jpegEncoder != null)
            {
                jpegEncoder.QualityLevel = _qualityLevel;
            }
        }

        // NOTE: This could be changed to return a TransformGroup which would allow a
        //       combination of transforms to be performed on the image
        private Transform GetTransform(BitmapSource source)
        {
            Contract.Requires(source != null);

            var width = _size.Width;
            var height = _size.Height;

            if (_ignoreRotations && _size.Mode == Mode.Scale)
            {
                if ((width > height) != (source.PixelWidth > source.PixelHeight))
                {
                    var temp = width;
                    width = height;
                    height = temp;
                }
            }

            var scaleX = UnitHelper.ConvertToScale(width, _size.Unit, source.PixelWidth, source.DpiX);
            var scaleY = UnitHelper.ConvertToScale(height, _size.Unit, source.PixelHeight, source.DpiY);

            if (_size.Mode == Mode.Scale)
            {
                var minScale = Math.Min(scaleX, scaleY);

                scaleX = minScale;
                scaleY = minScale;
            }
            else if (_size.Mode != Mode.Stretch)
            {
                throw new NotSupportedException(String.Format(CultureInfo.InvariantCulture, "The mode '{0}' is not yet supported.", _size.Mode));
            }

            if (_shrinkOnly)
            {
                var maxScale = Math.Max(scaleX, scaleY);

                if (maxScale > 1.0)
                {
                    scaleX = 1.0;
                    scaleY = 1.0;
                }
            }

            return new ScaleTransform(scaleX, scaleY);
        }
    }
}