//------------------------------------------------------------------------------
// <copyright file="ResizingServiceTest.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer
{
    using System.IO;
    using System.Linq;
    using System.Windows.Media.Imaging;
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.Services;
    using Xunit;

    public class ResizingServiceTest
    {
        [Fact]
        public void CanScale()
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
            var image = LoadBitmap(file);

            Assert.Equal(96, image.PixelWidth);
            Assert.Equal(72, image.PixelHeight);
        }

        [Fact]
        public void CanScaleWithAutoWidth()
        {
            // Arrange
            var resizer
                = CreateResizer(
                    new CustomSize
                    {
                        Width = 0,
                        Height = 72,
                        Mode = Mode.Scale
                    });

            // Act
            var file = resizer.Resize("WP_000000.jpg");

            // Assert
            var image = LoadBitmap(file);

            Assert.Equal(96, image.PixelWidth);
            Assert.Equal(72, image.PixelHeight);
        }

        [Fact]
        public void CanScaleWithAutoHeight()
        {
            // Arrange
            var resizer
                = CreateResizer(
                    new CustomSize
                    {
                        Width = 96,
                        Height = 0,
                        Mode = Mode.Scale
                    });

            // Act
            var file = resizer.Resize("WP_000000.jpg");

            // Assert
            var image = LoadBitmap(file);

            Assert.Equal(96, image.PixelWidth);
            Assert.Equal(72, image.PixelHeight);
        }

        [Fact]
        public void CanStretch()
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
            var image = LoadBitmap(file);

            Assert.Equal(96, image.PixelWidth);
            Assert.Equal(96, image.PixelHeight);
        }

        [Fact]
        public void CanStretchWhileIgnoringRotations()
        {
            // Arrange
            var resizer
                = CreateResizer(
                    new CustomSize
                    {
                        Width = 72,
                        Height = 96,
                        Mode = Mode.Stretch
                    },
                    ignoreRotations: true);

            // Act
            var file = resizer.Resize("WP_000000.jpg");

            // Assert
            var image = LoadBitmap(file);

            Assert.Equal(72, image.PixelWidth);
            Assert.Equal(96, image.PixelHeight);
        }

        [Fact]
        public void CanShrinkOnly()
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
            var image = LoadBitmap(file);

            Assert.Equal(640, image.PixelWidth);
            Assert.Equal(480, image.PixelHeight);
        }

        [Fact]
        public void CanSetQualityLevel()
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

            Assert.True(fileLength1 < fileLength2);
        }

        [Fact]
        public void CanIgnoreRotations()
        {
            // Arrange
            var resizer
                = CreateResizer(
                    new CustomSize
                    {
                        Width = 72,
                        Height = 96,
                        Mode = Mode.Scale
                    });

            // Act
            var file = resizer.Resize("WP_000000.jpg");

            // Assert
            var image = LoadBitmap(file);

            Assert.Equal(96, image.PixelWidth);
            Assert.Equal(72, image.PixelHeight);
        }

        [Fact]
        public void CanHonorRotations()
        {
            // Arrange
            var resizer
                = CreateResizer(
                    new CustomSize
                    {
                        Width = 72,
                        Height = 96,
                        Mode = Mode.Scale
                    },
                    ignoreRotations: false);

            // Act
            var file = resizer.Resize("WP_000000.jpg");

            // Assert
            var image = LoadBitmap(file);

            Assert.Equal(72, image.PixelWidth);
            Assert.Equal(54, image.PixelHeight);
        }

        private ResizingService CreateResizer(ResizeSize size, int qualityLevel = 75, bool shrinkOnly = false, bool ignoreRotations = true)
        {
            return new ResizingService(
                qualityLevel,
                shrinkOnly,
                ignoreRotations,
                size,
                new RenamingService(
                    "{0} ({1})",
                    null,
                    false,
                    size));
        }

        private BitmapSource LoadBitmap(string file)
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
