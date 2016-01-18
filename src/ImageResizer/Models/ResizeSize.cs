using System.Diagnostics;
using GalaSoft.MvvmLight;

namespace ImageResizer.Models
{
    public class ResizeSize : ObservableObject
    {
        string _name;
        ResizeFit _fit = ResizeFit.Fit;
        double _width;
        double _height;
        ResizeUnit _unit = ResizeUnit.Pixel;

        public virtual string Name
        {
            get { return _name; }
            set { Set(nameof(Name), ref _name, value); }
        }

        public string LocalizedName
            // TODO: Cache, inline ResourceTemplateConverter
            => (string)new Views.ResourceTemplateConverter().Convert(Name, null, null, null);

        public string CleanName
            // TODO: Cache, honor escapes
            => LocalizedName.Replace("_", "");

        public ResizeFit Fit
        {
            get { return _fit; }
            set { Set(nameof(Fit), ref _fit, value); }
        }

        public double Width
        {
            get { return _width; }
            set { Set(nameof(Width), ref _width, value); }
        }

        public double Height
        {
            get { return _height; }
            set { Set(nameof(Height), ref _height, value); }
        }

        public ResizeUnit Unit
        {
            get { return _unit; }
            set { Set(nameof(Unit), ref _unit, value); }
        }

        public double GetPixelWidth(int originalWidth, double dpi)
            => ConvertToPixels(Width, Unit, originalWidth, dpi);

        public double GetPixelHeight(int originalHeight, double dpi)
            => ConvertToPixels(Height, Unit, originalHeight, dpi);

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
