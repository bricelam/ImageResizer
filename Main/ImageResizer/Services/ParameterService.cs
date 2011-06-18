//------------------------------------------------------------------------------
// <copyright file="ParameterService.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Services
{
    using BriceLambson.ImageResizer.Model;

    internal class ParameterService
    {
        private string[] args;

        public ParameterService(string[] args)
        {
            this.args = args;
            this.Parameters = new Parameters();

            // TODO: Should this be done in the background?
            this.Parse();
        }

        public Parameters Parameters { get; private set; }

        private void Parse()
        {
            for (var i = 0; i < this.args.Length; i++)
            {
                var arg = this.args[i];

                if (arg[0] == '/' && arg[1] == 'd')
                {
                    this.Parameters.OutputDirectory = this.args[++i];
                }
                else
                {
                    this.Parameters.SelectedFiles.Add(arg);
                }
            }
        }
    }
}
