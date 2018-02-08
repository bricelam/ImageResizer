﻿using System.Collections.Generic;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using ImageResizer.Properties;

namespace ImageResizer.Models
{
    public class ResizeSize : ObservableObject
    {
        static readonly IDictionary<string, string> _tokens;

        string _name;
        ResizeFit _fit = ResizeFit.Fit;
        double _width;
        double _height;
        ResizeUnit _unit = ResizeUnit.Pixel;

        static ResizeSize()
            => _tokens = new Dictionary<string, string>
            {
                ["$small$"] = Resources.Small,
                ["$medium$"] = Resources.Medium,
                ["$large$"] = Resources.Large,
                ["$phone$"] = Resources.Phone
            };

        public virtual string Name
        {
            get => _name;
            set => Set(nameof(Name), ref _name, ReplaceTokens(value));
        }

        public ResizeFit Fit
        {
            get => _fit;
            set => Set(nameof(Fit), ref _fit, value);
        }

        public double Width
        {
            get => _width;
            set => Set(nameof(Width), ref _width, value);
        }

        public double Height
        {
            get => _height;
            set => Set(nameof(Height), ref _height, value);
        }

        public ResizeUnit Unit
        {
            get => _unit;
            set => Set(nameof(Unit), ref _unit, value);
        }

        public double GetPixelWidth(int originalWidth, double dpi)
            => ConvertToPixels(Width, Unit, originalWidth, dpi);

        public double GetPixelHeight(int originalHeight, double dpi)
            => ConvertToPixels(Height, Unit, originalHeight, dpi);

        string ReplaceTokens(string text)
            => (text != null && _tokens.TryGetValue(text, out var result))
                ? result
                : text;

        double ConvertToPixels(double value, ResizeUnit unit, int originalValue, double dpi)
        {
            if (value == 0)
            {
                if (Fit == ResizeFit.Fit)
                    return double.PositiveInfinity;

                Debug.Assert(Fit == ResizeFit.Fill || Fit == ResizeFit.Stretch, "Unexpected ResizeFit value: " + Fit);

                return originalValue;
            }

            switch (unit)
            {
                case ResizeUnit.Inch:
                    return value * dpi;

                case ResizeUnit.Centimeter:
                    return value * dpi * 2.54;

                case ResizeUnit.Percent:
                    return value / 100 * originalValue;

                default:
                    Debug.Assert(unit == ResizeUnit.Pixel, "Unexpected unit value: " + unit);
                    return value;
            }
        }
    }
}
