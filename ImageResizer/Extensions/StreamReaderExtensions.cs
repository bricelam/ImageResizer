//------------------------------------------------------------------------------
// <copyright file="StreamReaderExtensions.cs" company="Brice Lambson">
//     Copyright (c) 2012 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Extensions
{
    using System.IO;
    using System.Threading.Tasks;

    internal static class StreamReaderExtensions
    {
        public static Task<string> ReadLineAsync(this StreamReader reader)
        {
            return Task.Factory.StartNew(() => reader.ReadLine());
        }
    }
}
