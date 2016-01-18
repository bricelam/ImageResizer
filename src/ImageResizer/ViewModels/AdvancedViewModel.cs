using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using ImageResizer.Properties;

namespace ImageResizer.ViewModels
{
    public class AdvancedViewModel : ViewModelBase
    {
        static readonly IDictionary<Guid, string> _encoderMap;
        static readonly object[] _args = new[]
        {
            "1607281400a",
            Resources.Large
        };

        static AdvancedViewModel()
        {
            var bmpCodec = new BmpBitmapEncoder().CodecInfo;
            var gifCodec = new GifBitmapEncoder().CodecInfo;
            var jpegCodec = new JpegBitmapEncoder().CodecInfo;
            var pngCodec = new PngBitmapEncoder().CodecInfo;
            var tiffCodec = new TiffBitmapEncoder().CodecInfo;
            var wmpCodec = new WmpBitmapEncoder().CodecInfo;

            // TODO: Add WIC codecs
            _encoderMap = new Dictionary<Guid, string>
            {
                [bmpCodec.ContainerFormat] = bmpCodec.FriendlyName,
                [gifCodec.ContainerFormat] = gifCodec.FriendlyName,
                [jpegCodec.ContainerFormat] = jpegCodec.FriendlyName,
                [pngCodec.ContainerFormat] = pngCodec.FriendlyName,
                [tiffCodec.ContainerFormat] = tiffCodec.FriendlyName,
                [wmpCodec.ContainerFormat] = wmpCodec.FriendlyName,
            };
        }

        public AdvancedViewModel(Settings settings)
        {
            Settings = settings;
        }

        public static IDictionary<Guid, string> EncoderMap
            => _encoderMap;

        public Settings Settings { get; }

        public string Version
            => typeof(AdvancedViewModel).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;

        public IEnumerable<Guid> Encoders
            => _encoderMap.Keys;

        public void Close(bool accepted)
        {
            // TODO: Sort out interaction with input page
            if (accepted)
                Settings.Save();
            else
                Settings.Reload();
        }
    }
}
