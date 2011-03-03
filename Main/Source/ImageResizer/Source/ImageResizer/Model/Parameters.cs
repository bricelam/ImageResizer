//------------------------------------------------------------------------------
// <copyright file="Parameters.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Model
{
    using System.Collections.Generic;

    internal class Parameters
    {
        public Parameters()
        {
            this.SelectedFiles = new List<string>();
        }

        public string OutputDirectory { get; set; }

        public IList<string> SelectedFiles { get; protected set; }
    }
}
