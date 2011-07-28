//------------------------------------------------------------------------------
// <copyright file="ResizingServiceTests.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace ImageResizerTests
{
    using BriceLambson.ImageResizer.Model;
    using BriceLambson.ImageResizer.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("Content")]
    public class ResizingServiceTests
    {
        [TestMethod]
        public void BasicResizeTest()
        {
            // Arrange
            var sourceFile = "WP_000000.jpg";
            string outputDirectory = null;

            var settings = new TestSettings
            {
                SelectedSize = new CustomSize
                {
                    Width = 96,
                    Height = 96
                }
            };

            var resizer = new ResizingService(settings);

            // Act
            var destinationFile = resizer.Resize(sourceFile, outputDirectory);

            // Assert
            var destination = ImageHelper.OpenFrame(destinationFile);

            Assert.AreEqual(destinationFile, "WP_000000 (Custom).jpg");
            Assert.AreEqual(96, destination.PixelWidth);
            Assert.AreEqual(72, destination.PixelHeight);
        }
    }
}
