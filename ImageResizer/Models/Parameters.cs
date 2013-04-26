//------------------------------------------------------------------------------
// <copyright file="Parameters.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal class Parameters
    {
        private readonly ICollection<string> _selectedFiles = new Collection<string>();

        public string OutputDirectory { get; set; }

        public ICollection<string> SelectedFiles
        {
            get { return _selectedFiles; }
        }
    }
}