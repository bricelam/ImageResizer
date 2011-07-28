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
    using System.Globalization;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using BriceLambson.ImageResizer.Model;
    using Microsoft.VisualBasic.FileIO;

    internal class ResizingService
    {
        private ISettings settings;
        private Lazy<RenamingService> lazyRenamer;

        public ResizingService(ISettings settings)
        {
            // NOTE: This is lazy because it has the potential of never being used
            this.lazyRenamer = new Lazy<RenamingService>(() => new RenamingService(settings));
            this.settings = settings;
        }

        public string Resize(string sourcePath, string outputDirectory)
        {
            bool encoderDefaulted = false;
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
                var temp = encoder.CodecInfo;
            }
            catch (NotSupportedException)
            {
                // Fallback to JPEG encoder
                encoder = new JpegBitmapEncoder();
                encoderDefaulted = true;
            }

            // TODO: Copy container-level metadata if codec supports it
            this.SetEncoderSettings(encoder);

            string destinationPath = null;

            // NOTE: Only TIFF and GIF images support multiple frames
            foreach (var sourceFrame in decoder.Frames)
            {
                // Apply the transform
                var transform = this.GetTransform(sourceFrame);
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
                        sourcePath = Path.ChangeExtension(sourcePath, ".jpg");
                    }

                    if (this.settings.ReplaceOriginals)
                    {
                        destinationPath = sourcePath;
                    }
                    else
                    {
                        destinationPath = this.lazyRenamer.Value.Rename(sourcePath, outputDirectory, destinationFrame);
                    }
                }
            }

            // Move any existing file to the Recycle Bin
            if (File.Exists(destinationPath))
            {
                // TODO: Is there a better way to do this without a reference to Microsoft.VisualBasic?
                FileSystem.DeleteFile(destinationPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            }

            using (var destinationStream = File.OpenWrite(destinationPath))
            {
                // Save the final image
                encoder.Save(destinationStream);
            }

            return destinationPath;
        }

        private void SetEncoderSettings(BitmapEncoder encoder)
        {
            var jpegEncoder = encoder as JpegBitmapEncoder;

            if (jpegEncoder != null)
            {
                jpegEncoder.QualityLevel = this.settings.QualityLevel;
            }
        }

        // NOTE: This could be changed to return a TransformGroup which would allow a
        //       combination of transforms to be performed on the image
        private Transform GetTransform(BitmapSource source)
        {
            var size = this.settings.SelectedSize;
            var scaleX = UnitHelper.ConvertToScale(size.Width, size.Unit, source.PixelWidth, source.DpiX);
            var scaleY = UnitHelper.ConvertToScale(size.Height, size.Unit, source.PixelHeight, source.DpiY);

            if (size.Mode == Mode.Scale)
            {
                var minScale = Math.Min(scaleX, scaleY);

                scaleX = minScale;
                scaleY = minScale;
            }
            else if (size.Mode != Mode.Stretch)
            {
                throw new NotSupportedException(String.Format(CultureInfo.InvariantCulture, "The mode '{0}' is not yet supported.", size.Mode));
            }

            if (this.settings.ShrinkOnly)
            {
                var maxScale = Math.Max(scaleX, scaleY);

                if (maxScale > 1.0)
                {
                    scaleX = 1.0;
                    scaleY = 1.0;
                }
            }

            // TODO: Ignore image rotations
            return new ScaleTransform(scaleX, scaleY);
        }
    }
}
