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
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using BriceLambson.ImageResizer.Models;

    internal class RenamingService
    {
        private const string UniqueFileNameFormat = "{0} ({1})";

        private readonly string _fileNameFormat;
        private readonly string _outputDirectory;
        private readonly bool _replaceOriginals;
        private readonly ResizeSize _size;

        public RenamingService(string fileNameFormat, string outputDirectory, bool replaceOriginals, ResizeSize size)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(fileNameFormat));
            Contract.Requires(fileNameFormat.Contains("{0}"));
            Contract.Requires(size != null);

            _fileNameFormat = fileNameFormat;
            _outputDirectory = outputDirectory;
            _replaceOriginals = replaceOriginals;
            _size = size;
        }

        public string Rename(string sourcePath)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(sourcePath));
            Contract.Ensures(!String.IsNullOrWhiteSpace(Contract.Result<string>()));

            if (_outputDirectory == null && _replaceOriginals)
            {
                return sourcePath;
            }

            var directoryName
                = _outputDirectory
                    ?? Path.GetDirectoryName(sourcePath);
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
                _size.Name
            };

            var destinationFileName = String.Format(CultureInfo.CurrentCulture, _fileNameFormat, replacementItems);
            var destinationPath = Path.Combine(directoryName, destinationFileName + extension);
            var i = 1;

            // Ensure the file name is unique
            while (File.Exists(destinationPath))
            {
                var uniqueFileName = String.Format(CultureInfo.CurrentCulture, UniqueFileNameFormat, destinationFileName, ++i);
                destinationPath = Path.Combine(directoryName, uniqueFileName + extension);
            }

            return destinationPath;
        }
    }
}