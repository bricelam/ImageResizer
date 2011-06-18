//------------------------------------------------------------------------------
// <copyright file="ResizeSize.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Model
{
    using System.ComponentModel;

    public class ResizeSize : INotifyPropertyChanged
    {
        private string name;
        private Mode mode;
        private double width;
        private double height;
        private Unit unit;

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name != value)
                {
                    this.name = value;

                    // TODO: Extract these from lambdas for compile-time checking
                    this.OnPropertyChanged("Name");
                }
            }
        }

        public Mode Mode
        {
            get
            {
                return this.mode;
            }

            set
            {
                if (this.mode != value)
                {
                    this.mode = value;
                    this.OnPropertyChanged("Mode");
                }
            }
        }

        public double Width
        {
            get
            {
                return this.width;
            }

            set
            {
                if (this.width != value)
                {
                    this.width = value;
                    this.OnPropertyChanged("Width");
                }
            }
        }

        public double Height
        {
            get
            {
                return this.height;
            }

            set
            {
                if (this.height != value)
                {
                    this.height = value;
                    this.OnPropertyChanged("Height");
                }
            }
        }

        public Unit Unit
        {
            get
            {
                return this.unit;
            }

            set
            {
                if (this.unit != value)
                {
                    this.unit = value;
                    this.OnPropertyChanged("Unit");
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
