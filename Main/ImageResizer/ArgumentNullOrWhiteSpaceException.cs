//------------------------------------------------------------------------------
// <copyright file="ArgumentNullOrWhiteSpaceException.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer
{
    using System;
    using System.Runtime.Serialization;
    using BriceLambson.ImageResizer.Properties;

    [Serializable]
    public class ArgumentNullOrWhiteSpaceException : ArgumentException
    {
        public ArgumentNullOrWhiteSpaceException()
            : base(Resources.ArgumentNullOrWhiteSpaceException)
        {
        }

        public ArgumentNullOrWhiteSpaceException(string paramName)
            : base(Resources.ArgumentNullOrWhiteSpaceException, paramName)
        {
        }

        public ArgumentNullOrWhiteSpaceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ArgumentNullOrWhiteSpaceException(string message, string paramName)
            : base(message, paramName)
        {
        }

        protected ArgumentNullOrWhiteSpaceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
