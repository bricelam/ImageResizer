#region Common Public License Copyright Notice
/**************************************************************************\
* PhotoToys Clone                                                          *
*                                                                          *
* Copyright © Brice Lambson. All rights reserved.                          *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl1.0.php)    *
* which can be found in the file CPL.txt at the root of this distribution. *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PhotoToys
{
	/// <summary>
	/// Used to asynchronously call Form.ShowDialog
	/// </summary>
	/// <param name="owner">Any object that implements IWin32Window that represents the top-level window that will own the modal dialog box.</param>
	/// <returns>One of the DialogResult values.</returns>
	delegate DialogResult ShowDialogDelegate(IWin32Window owner);

	/// <summary>
	/// Packages the state of an asynchronous call to ShowDialog.
	/// </summary>
	class ShowDialogState
	{
		private PhotoResizeForm photoResizeForm;
		private string[] imageFiles;
		private ShowDialogDelegate showDialogDelegate;
		private string workingDirectory;

		/// <summary>
		/// The form that was shown on the asynchronous call.
		/// </summary>
		public PhotoResizeForm PhotoResizeForm
		{
			get { return photoResizeForm; }
			set { photoResizeForm = value; }
		}

		/// <summary>
		/// The image files that we are resizing.
		/// </summary>
		public string[] ImageFiles
		{
			get { return imageFiles; }
			set { imageFiles = value; }
		}

		/// <summary>
		/// The delegate used to make the asynchronous call.
		/// </summary>
		internal ShowDialogDelegate ShowDialogDelegate
		{
			get { return showDialogDelegate; }
			set { showDialogDelegate = value; }
		}

		/// <summary>
		/// Direcotry in which to put resized images.
		/// </summary>
		public string WorkingDirectory
		{
			get { return workingDirectory; }
			set { workingDirectory = value; }
		}

		/// <summary>
		/// No argument constructor.
		/// </summary>
		public ShowDialogState()
		{
		}
	}
}
