//------------------------------------------------------------------------------
// <copyright file="ResizingServiceTests.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Test
{
    using System.IO;
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.Services;
    using BriceLambson.ImageResizer.Test.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("Content")]
    public class ResizingServiceTests
    {
        [TestMethod]
        public void Can_scale()
        {
            // Arrange
            var resizer
                = CreateResizer(
                    new CustomSize
                        {
                            Width = 96,
                            Height = 96,
                            Mode = Mode.Scale
                        });

            // Act
            var file = resizer.Resize("WP_000000.jpg");

            // Assert
            var image = ImageHelper.OpenFrame(file);

            Assert.AreEqual(96, image.PixelWidth);
            Assert.AreEqual(72, image.PixelHeight);
        }

        [TestMethod]
        public void Can_stretch()
        {
            // Arrange
            var resizer
                = CreateResizer(
                    new CustomSize
                    {
                        Width = 96,
                        Height = 96,
                        Mode = Mode.Stretch
                    });

            // Act
            var file = resizer.Resize("WP_000000.jpg");

            // Assert
            var image = ImageHelper.OpenFrame(file);

            Assert.AreEqual(96, image.PixelWidth);
            Assert.AreEqual(96, image.PixelHeight);
        }

        [TestMethod]
        public void Can_shrink_only()
        {
            // Arrange
            var resizer
                = CreateResizer(
                    new CustomSize
                    {
                        Width = 800,
                        Height = 600
                    },
                    shrinkOnly: true);

            // Act
            var file = resizer.Resize("WP_000000.jpg");

            // Assert
            var image = ImageHelper.OpenFrame(file);

            Assert.AreEqual(640, image.PixelWidth);
            Assert.AreEqual(480, image.PixelHeight);
        }

        [TestMethod]
        public void Can_set_quality_level()
        {
            // Arrange
            var size
                = new CustomSize
                    {
                        Width = 96,
                        Height = 96
                    };
            var sourceFile = "WP_000000.jpg";
            var resizer1 = CreateResizer(size, qualityLevel: 1);
            var resizer2 = CreateResizer(size, qualityLevel: 100);

            // Act
            var file1 = resizer1.Resize(sourceFile);
            var file2 = resizer2.Resize(sourceFile);

            // Assert
            var fileLength1 = new FileInfo(file1).Length;
            var fileLength2 = new FileInfo(file2).Length;

            Assert.IsTrue(fileLength1 < fileLength2);
        }

        private ResizingService CreateResizer(ResizeSize size, int qualityLevel = 75, bool shrinkOnly = false)
        {
            return new ResizingService(
                qualityLevel,
                shrinkOnly,
                size,
                new RenamingService(
                    "{0} ({1})",
                    null,
                    false,
                    size));
        }
    }
}
