//------------------------------------------------------------------------------
// <copyright file="StreamReaderExtensions.cs" company="Brice Lambson">
//     Copyright (c) 2013 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Extensions
{
    using System.ServiceModel.Syndication;
    using System.Threading.Tasks;
    using System.Xml;

    internal static class Atom10FeedFormatterExtensions
    {
        public static Task ReadFromAsync(this Atom10FeedFormatter formatter, XmlReader reader)
        {
            return TaskEx.Run(() => formatter.ReadFrom(reader));
        }
    }
}
