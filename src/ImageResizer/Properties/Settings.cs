using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ImageResizer.Models;

namespace ImageResizer.Properties
{
    partial class Settings : IDataErrorInfo
    {
        AllSizesCollection _allSizes;

        public IEnumerable<ResizeSize> AllSizes
            => _allSizes ?? (_allSizes = new AllSizesCollection(Sizes, CustomSize));

        public string Error
            => string.Empty;

        // TODO: Cache, use Regex
        public string FileNameFormat
            => FileName.Replace("{", "{{").Replace("}", "}}").Replace("%1", "{0}").Replace("%2", "{1}")
                .Replace("%3", "{2}").Replace("%4", "{3}");

        public ResizeSize SelectedSize
        {
            get
            {
                return SelectedSizeIndex >= 0 && SelectedSizeIndex < Sizes.Count
                    ? Sizes[SelectedSizeIndex]
                    : CustomSize;
            }
            set { throw new NotImplementedException(); }
        }

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

        class AllSizesCollection : IEnumerable<ResizeSize>, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable
        {
            readonly ObservableCollection<ResizeSize> _sizes;
            readonly CustomSize _customSize;

            public AllSizesCollection(ObservableCollection<ResizeSize> sizes, CustomSize customSize)
            {
                _sizes = sizes;
                _customSize = customSize;

                _sizes.CollectionChanged += HandleCollectionChanged;
                ((INotifyPropertyChanged)_sizes).PropertyChanged += HandlePropertyChanged;
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged;
            public event PropertyChangedEventHandler PropertyChanged;

            public int Count
                => _sizes.Count + 1;

            public ResizeSize this[int index]
                => index == _sizes.Count
                    ? _customSize
                    : _sizes[index];

            public IEnumerator<ResizeSize> GetEnumerator()
                => new AllSizesEnumerator(this);

            public void Dispose()
            {
                _sizes.CollectionChanged -= HandleCollectionChanged;
                ((INotifyPropertyChanged)_sizes).PropertyChanged -= HandlePropertyChanged;
            }

            void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
                => CollectionChanged?.Invoke(this, e);

            void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
                => PropertyChanged?.Invoke(this, e);

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            class AllSizesEnumerator : IEnumerator<ResizeSize>
            {
                readonly AllSizesCollection _list;

                int _index = -1;

                public AllSizesEnumerator(AllSizesCollection list)
                {
                    _list = list;
                }

                public ResizeSize Current
                    => _list[_index];

                object IEnumerator.Current
                    => Current;

                public void Dispose()
                {
                }

                public bool MoveNext()
                    => ++_index < _list.Count;

                public void Reset()
                    => _index = -1;
            }
        }
    }
}
