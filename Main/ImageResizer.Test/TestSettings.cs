//------------------------------------------------------------------------------
// <copyright file="TestSettings.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace ImageResizerTests
{
    using BriceLambson.ImageResizer.Model;

    internal class TestSettings : ISettings
    {
        public TestSettings()
        {
            FileNameFormat = "{0} ({1})";
            QualityLevel = 75;
            SelectedSize = new ResizeSize
            {
                Name = "Small",
                Mode = Mode.Scale,
                Width = 854,
                Height = 480,
                Unit = Unit.Pixels
            };
        }

        public string FileNameFormat { get; set; }
        public int QualityLevel { get; set; }
        public bool ReplaceOriginals { get; set; }
        public ResizeSize SelectedSize { get; set; }
        public bool ShrinkOnly { get; set; }
        public bool IgnoreRotations { get; set; }
    }
}
