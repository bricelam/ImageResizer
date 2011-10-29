//------------------------------------------------------------------------------
// <copyright file="UpdateAvailableEvent.cs" company="Brice Lambson">
//     Copyright (c) 2011 Brice Lambson. All rights reserved.
//
//     The use of this software is governed by the Microsoft Public License
//     which is included with this distribution.
// </copyright>
//------------------------------------------------------------------------------

namespace BriceLambson.ImageResizer.Models
{
    using BriceLambson.ImageResizer.ViewModels;
    using Microsoft.Practices.Prism.Events;

    internal class UpdateAvailableEvent : CompositePresentationEvent<UpdateAvailableViewModel>
    {
    }
}