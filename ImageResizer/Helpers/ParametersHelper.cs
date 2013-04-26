//------------------------------------------------------------------------------
// <copyright file="ParametersHelper.cs" company="Brice Lambson">
//     Copyright (c) 2011-2012 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Helpers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.IO.Pipes;
    using System.Text;
    using System.Threading.Tasks;
    using BriceLambson.ImageResizer.Extensions;
    using BriceLambson.ImageResizer.Models;

    internal static class ParametersHelper
    {
        public static async Task<Parameters> ParseAsync(string[] args)
        {
            Contract.Requires(args != null);

            var parameters = new Parameters();

            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                if (arg[0] == '/' && arg[1] == 'd')
                {
                    parameters.OutputDirectory = args[++i];
                }
                else if (arg[0] == '/' && arg[1] == 'p')
                {
                    var pipeHandleAsString = args[++i];

                    // NOTE: In theory, this isn't always Unicode, but for all modern platforms, we
                    //       can safely make that assumption
                    using (var readPipe = new StreamReader(new AnonymousPipeClientStream(pipeHandleAsString), Encoding.Unicode))
                    {
                        // TODO: Set a timeout for the first read?
                        var selectedFile = await readPipe.ReadLineAsync();

                        while (selectedFile != String.Empty)
                        {
                            parameters.SelectedFiles.Add(selectedFile);

                            selectedFile = await readPipe.ReadLineAsync();
                        }
                    }
                }
                else
                {
                    parameters.SelectedFiles.Add(arg);
                }
            }

            return parameters;
        }
    }
}