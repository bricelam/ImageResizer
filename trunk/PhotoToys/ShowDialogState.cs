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
		/// No argument constructor.
		/// </summary>
		public ShowDialogState()
		{
		}
	}
}
