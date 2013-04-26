//------------------------------------------------------------------------------
// <copyright file="ResizeSize.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Models
{
    using GalaSoft.MvvmLight;

    public class ResizeSize : ObservableObject
    {
        private string _name;
        private Mode _mode;
        private double _width;
        private double _height;
        private Unit _unit;

        public virtual string Name
        {
            get { return _name; }
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