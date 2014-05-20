//------------------------------------------------------------------------------
// <copyright file="ProgressPageViewModel.cs" company="Brice Lambson">
//     Copyright (c) 2011-2013 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BriceLambson.ImageResizer.Helpers;
    using BriceLambson.ImageResizer.Models;
    using BriceLambson.ImageResizer.Properties;
    using BriceLambson.ImageResizer.Services;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    internal class AdvancedPageViewModel : ViewModelBase, IDisposable
    {
        public AdvancedPageViewModel()
        {
      
        }

        public virtual void Dispose()
        {
           
        }

        protected virtual void OnCompleted()
        {

        }

        public static object[] ReplacementItemsExample = new object[]
        {
            //this values must be same as object[] "replacementItems" in method Rename
            "filename",
            "Mobile",
            "400",
            "300"
        };

        public static bool IsFileformatCorrect(char[] format)
        {
            for (int i = 0; i < format.Length; i++)
            {
                if (format[i].Equals('{'))
                {
                    if (i + 2 >= format.Length || !char.IsDigit(format[i + 1]) || Convert.ToInt32(format[i + 1].ToString()) >= AdvancedPageViewModel.ReplacementItemsExample.Length || !format[i + 2].Equals('}'))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}