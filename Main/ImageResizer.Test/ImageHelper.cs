//------------------------------------------------------------------------------
// <copyright file="ImageHelper.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace ImageResizerTests
{
    using System.IO;
    using System.Linq;
    using System.Windows.Media.Imaging;

    internal static class ImageHelper
    {
        public static BitmapSource OpenFrame(string file)
        {
            BitmapDecoder decoder;

            using (var stream = File.OpenRead(file))
            {
                decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }

            return decoder.Frames.First();
        }
    }
}
