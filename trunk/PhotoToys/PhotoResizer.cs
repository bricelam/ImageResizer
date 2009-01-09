//------------------------------------------------------------------------------
// <copyright file="PhotoResizer.cs" company="Brice Lambson">
//     PhotoToys Clone
//
//     Copyright © Brice Lambson. All rights reserved.
//
//     The use and distribution terms for this software are covered by the
//     Common Public License 1.0 (http://opensource.org/licenses/cpl1.0.php)
//     which can be found in the file CPL.txt at the root of this distribution.
//     By using this software in any fashion, you are agreeing to be bound by
//     the terms of this license.
//
//     You must not remove this notice, or any other, from this software.
// </copyright>
//------------------------------------------------------------------------------

namespace PhotoToys
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    /// <summary>
    /// Utility class used to resize images.
    /// </summary>
    public static class PhotoResizer
    {
        /// <summary>
        /// Resizes an image.
        /// </summary>
        /// <param name="sourceFile">The file name of the image to resize</param>
        /// <param name="destinationDirectoryName">The output directory.</param>
        /// <param name="fileNameAppendage">A value to append to the output file name.</param>
        /// <param name="width">The new image's target width.</param>
        /// <param name="height">The new image's target height.</param>
        /// <param name="smallerOnly">A value indicating weather the image should only be made smaller.</param>
        public static void ResizePhoto(string sourceFile, string destinationDirectoryName, string fileNameAppendage, int width, int height, bool smallerOnly)
        {
            string destinationFile = GetNewFileName(sourceFile, destinationDirectoryName, fileNameAppendage);
            Image image = Image.FromFile(sourceFile);

            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            double widthRatio = width / (double)sourceWidth;
            double heightRatio = height / (double)sourceHeight;

            if (widthRatio > heightRatio)
            {
                width = (int)(heightRatio * sourceWidth);
            }
            else
            {
                height = (int)(widthRatio * sourceHeight);
            }

            if (width == sourceWidth || height == sourceHeight || (smallerOnly && (width > sourceWidth || height > sourceHeight)))
            {
                image.Dispose();
                image = null;

                if (sourceFile != destinationFile)
                {
                    File.Copy(sourceFile, destinationFile);
                }
            }
            else
            {
                Bitmap resizedImage = new Bitmap(image, width, height);

                foreach (PropertyItem propertyItem in image.PropertyItems)
                {
                    resizedImage.SetPropertyItem(propertyItem);
                }

                ImageFormat format = image.RawFormat;

                image.Dispose();
                image = null;

                // TODO: The extention and type may get out of sync here.
                resizedImage.Save(destinationFile, format);

                resizedImage.Dispose();
                resizedImage = null;
            }
        }

        /// <summary>
        /// Compiles a new file name.
        /// </summary>
        /// <param name="filePath">The original file path.</param>
        /// <param name="directoryName">The new directory name.</param>
        /// <param name="fileNameAppendage">A value to append to the new file name.</param>
        /// <returns>The compiled file name.</returns>
        private static string GetNewFileName(string filePath, string directoryName, string fileNameAppendage)
        {
            if (String.IsNullOrEmpty(fileNameAppendage))
            {
                return filePath;
            }

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            string path;
            int count = 1;

            do
            {
                path = Path.ChangeExtension(Path.Combine(directoryName, fileName + fileNameAppendage + ((count == 1) ? String.Empty : (" (" + count + ")"))), extension);
                ++count;
            }
            while (File.Exists(path));

            return path;
        }
    }
}
