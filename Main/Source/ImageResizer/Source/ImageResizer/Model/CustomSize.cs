//------------------------------------------------------------------------------
// <copyright file="CustomSize.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Model
{
    using System.Xml.Serialization;
    using BriceLambson.ImageResizer.Properties;

    public class CustomSize : ResizeSize
    {
        public CustomSize()
        {
            this.Width = 1280;
            this.Height = 720;
        }

        [XmlIgnore]
        public override string Name
        {
            get
            {
                return Resources.Custom;
            }

            set
            {
                // Ignore
            }
        }
    }
}
