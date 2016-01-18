using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ImageResizer.Models;

namespace ImageResizer.Properties
{
    partial class Settings : IDataErrorInfo
    {
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (columnName != nameof(JpegQualityLevel))
                    return string.Empty;

                if (JpegQualityLevel < 1 || JpegQualityLevel > 100)
                    return string.Format(Resources.ValueMustBeBetween, 1, 100);

                return string.Empty;
            }
        }

        public IEnumerable<ResizeSize> AllSizes
            => Sizes.Append(CustomSize);

        public string Error => string.Empty;

        // TODO: Cache, use Regex
        public string FileNameFormat
            => FileName.Replace("%1", "{0}").Replace("%2", "{1}");

        public ResizeSize SelectedSize
        {
            get
            {
                return SelectedSizeIndex >= 0 && SelectedSizeIndex < Sizes.Count
                    ? Sizes[SelectedSizeIndex]
                    : CustomSize;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);

            if (e.PropertyName == nameof(Sizes) || e.PropertyName == nameof(CustomSize))
                OnPropertyChanged(sender, new PropertyChangedEventArgs(nameof(AllSizes)));
        }
    }
}
