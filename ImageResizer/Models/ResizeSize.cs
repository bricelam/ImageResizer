//------------------------------------------------------------------------------
// <copyright file="ResizeSize.cs" company="Brice Lambson">
//     Copyright (c) 2011-2013 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Models
{
    using System.Collections.Generic;
    using BriceLambson.ImageResizer.Helpers;
    using BriceLambson.ImageResizer.Properties;
    using GalaSoft.MvvmLight;

    public class ResizeSize : ObservableObject
    {
        private static readonly IDictionary<string, string> _nameTokens = new Dictionary<string, string>
            {
                { "Small", Resources.Small },
                { "Medium", Resources.Medium },
                { "Large", Resources.Large },
                { "Mobile", Resources.Mobile }
            };
        private string _name;
        private Mode _mode;
        private double _width;
        private double _height;
        private Unit _unit;

        public virtual string Name
        {
            get { return TemplatingEngine.ReplaceTokens(_name, _nameTokens); }
            set { Set(() => Name, ref _name, value); }
        }

        public Mode Mode
        {
            get { return _mode; }
            set { Set(() => Mode, ref _mode, value); }
        }

        public double Width
        {
            get { return _width; }
            set { Set(() => Width, ref _width, value); }
        }

        public double Height
        {
            get { return _height; }
            set { Set(() => Height, ref _height, value); }
        }

        public Unit Unit
        {
            get { return _unit; }
            set { Set(() => Unit, ref _unit, value); }
        }
    }
}