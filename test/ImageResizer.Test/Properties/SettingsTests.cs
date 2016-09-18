using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ImageResizer.Models;
using ImageResizer.Test;
using Xunit;

namespace ImageResizer.Properties
{
    public class SettingsTests
    {
        [Fact]
        public void AllSizes_propagates_Sizes_collection_events()
        {
            var settings = new Settings
            {
                Sizes = new ObservableCollection<ResizeSize>(),
                CustomSize = new CustomSize()
            };
            var ncc = (INotifyCollectionChanged)settings.AllSizes;

            var result = AssertEx.Raises<NotifyCollectionChangedEventArgs>(
                h => ncc.CollectionChanged += h,
                h => ncc.CollectionChanged -= h,
                () => settings.Sizes.Add(new ResizeSize()));

            Assert.Equal(NotifyCollectionChangedAction.Add, result.Arguments.Action);
        }

        [Fact]
        public void AllSizes_propagates_Sizes_property_events()
        {
            var settings = new Settings
            {
                Sizes = new ObservableCollection<ResizeSize>(),
                CustomSize = new CustomSize()
            };
            var npc = (INotifyPropertyChanged)settings.AllSizes;

            var result = AssertEx.Raises<PropertyChangedEventArgs>(
                h => npc.PropertyChanged += h,
                h => npc.PropertyChanged -= h,
                () => settings.Sizes.Add(new ResizeSize()));

            Assert.Equal("Item[]", result.Arguments.PropertyName);
        }

        [Fact]
        public void AllSizes_contains_Sizes()
        {
            var settings = new Settings
            {
                Sizes = new ObservableCollection<ResizeSize> { new ResizeSize() },
                CustomSize = new CustomSize()
            };

            Assert.Contains(settings.Sizes[0], settings.AllSizes);
        }

        [Fact]
        public void AllSizes_contains_CustomSize()
        {
            var settings = new Settings
            {
                Sizes = new ObservableCollection<ResizeSize>(),
                CustomSize = new CustomSize()
            };

            Assert.Contains(settings.CustomSize, settings.AllSizes);
        }

        [Fact]
        public void AllSizes_handles_property_events_for_CustomSize()
        {
            var originalCustomSize = new CustomSize();
            var settings = new Settings
            {
                Sizes = new ObservableCollection<ResizeSize>(),
                CustomSize = originalCustomSize
            };
            var ncc = (INotifyCollectionChanged)settings.AllSizes;

            var result = AssertEx.Raises<NotifyCollectionChangedEventArgs>(
                h => ncc.CollectionChanged += h,
                h => ncc.CollectionChanged -= h,
                () => settings.CustomSize = new CustomSize());

            Assert.Equal(NotifyCollectionChangedAction.Replace, result.Arguments.Action);
            Assert.Equal(1, result.Arguments.NewItems.Count);
            Assert.Equal(settings.CustomSize, result.Arguments.NewItems[0]);
            Assert.Equal(0, result.Arguments.NewStartingIndex);
            Assert.Equal(1, result.Arguments.OldItems.Count);
            Assert.Equal(originalCustomSize, result.Arguments.OldItems[0]);
            Assert.Equal(0, result.Arguments.OldStartingIndex);
        }
    }
}
