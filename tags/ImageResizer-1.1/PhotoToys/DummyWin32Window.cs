//------------------------------------------------------------------------------
// <copyright file="DummyWin32Window.cs" company="Brice Lambson">
//     PhotoToys Clone
//
//     Copyright © Brice Lambson. All rights reserved.
//
//     The use and distribution terms for this software are covered by the
//     Common Public License 1.0 (http://opensource.org/licenses/cpl1.0.php)
//     which can be found in the file CPL.txt at the root of this distribution.
//     By using this software in any fashion, you are agreeing to be bound by
//     the terms of this license.
//
//     You must not remove this notice, or any other, from this software.
// </copyright>
//------------------------------------------------------------------------------

namespace PhotoToys
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Used to wrap the IntPtr returned by the Windows Shell into a usable type.
    /// </summary>
    internal class DummyWin32Window : IWin32Window
    {
        /// <summary>
        /// The handle to the window.
        /// </summary>
        private IntPtr handle;

        /// <summary>
        /// Initializes a new instance of the DummyWin32Window class.
        /// </summary>
        public DummyWin32Window()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DummyWin32Window class with the given handle.
        /// </summary>
        /// <param name="handle">The handle to the window.</param>
        public DummyWin32Window(IntPtr handle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// Gets the handle to the window.
        /// </summary>
        public IntPtr Handle
        {
            get { return this.handle; }
        }
    }
}
