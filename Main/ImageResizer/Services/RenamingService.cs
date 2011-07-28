//------------------------------------------------------------------------------
// <copyright file="RenamingService.cs" company="Brice Lambson">
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
    using System.Windows.Media.Imaging;
    using BriceLambson.ImageResizer.Model;

    internal class RenamingService
    {
        private ISettings settings;

        public RenamingService(ISettings settings)
        {
            // TODO: This makes reuse difficult
            this.settings = settings;
        }

        public string Rename(string sourcePath, string outputDirectory, BitmapSource destination)
        {
            var directoryName = outputDirectory ?? Path.GetDirectoryName(sourcePath);
            var fileName = Path.GetFileNameWithoutExtension(sourcePath);
            var extension = Path.GetExtension(sourcePath);

            // TODO: Define more replacement variables
            //        * Selected size width
            //        * Selected size height
            //        * Selected size units
            //        * Selected size pixel width
            //        * Selected size pixel height
            //        * Actual width
            //        * Actual height
            //        * Actual pixel width
            //        * Actual pixel height
            var replacementItems = new object[]
            {
                // {0} = Original file name
                fileName,

                // {1} = Selected size name
                this.settings.SelectedSize.Name
            };

            var destinationFileName = String.Format(CultureInfo.CurrentCulture, this.settings.FileNameFormat, replacementItems);
            var destinationPath = Path.Combine(directoryName, destinationFileName + extension);
            var i = 1;

            // Ensure the file name is unique
            while (File.Exists(destinationPath))
            {
                var uniqueFileName = String.Format(CultureInfo.CurrentCulture, "{0} ({1})", destinationFileName, ++i);
                destinationPath = Path.Combine(directoryName, uniqueFileName + extension);
            }

            return destinationPath;
        }
    }
}
