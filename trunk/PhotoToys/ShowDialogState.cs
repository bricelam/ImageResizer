//------------------------------------------------------------------------------
// <copyright file="ShowDialogState.cs" company="Brice Lambson">
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
    /// Used to asynchronously call Form.ShowDialog
    /// </summary>
    /// <param name="owner">Any object that implements IWin32Window that represents the top-level window that will own the modal dialog box.</param>
    /// <returns>One of the DialogResult values.</returns>
    internal delegate DialogResult ShowDialogDelegate(IWin32Window owner);

    /// <summary>
    /// Represents the state of an asynchronous call to ShowDialog.
    /// </summary>
    internal class ShowDialogState
    {
        /// <summary>
        /// The form that was shown on the asynchronous call.
        /// </summary>
        private PhotoResizeForm photoResizeForm;

        /// <summary>
        /// Tthe image files that are being resizing.
        /// </summary>
        private string[] imageFiles;

        /// <summary>
        /// The delegate used to make the asynchronous call.
        /// </summary>
        private ShowDialogDelegate showDialogDelegate;

        /// <summary>
        /// The direcotry in which to store resized images.
        /// </summary>
        private string workingDirectory;

        /// <summary>
        /// No argument constructor.
        /// </summary>
        public ShowDialogState()
        {
        }

        /// <summary>
        /// Gets or sets the form that was shown on the asynchronous call.
        /// </summary>
        public PhotoResizeForm PhotoResizeForm
        {
            get { return this.photoResizeForm; }
            set { this.photoResizeForm = value; }
        }

        /// <summary>
        /// Gets or sets the image files that are being resizing.
        /// </summary>
        public string[] ImageFiles
        {
            get { return this.imageFiles; }
            set { this.imageFiles = value; }
        }

        /// <summary>
        /// Gets or sets the delegate used to make the asynchronous call.
        /// </summary>
        public ShowDialogDelegate ShowDialogDelegate
        {
            get { return this.showDialogDelegate; }
            set { this.showDialogDelegate = value; }
        }

        /// <summary>
        /// Gets or sets the direcotry in which to store resized images.
        /// </summary>
        public string WorkingDirectory
        {
            get { return this.workingDirectory; }
            set { this.workingDirectory = value; }
        }
    }
}
