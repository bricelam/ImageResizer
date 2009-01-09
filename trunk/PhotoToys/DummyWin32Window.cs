using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PhotoToys
{
	/// <summary>
	/// Used to wrap the IntPtr returned by the Windows Shell into a usable type.
	/// </summary>
	class DummyWin32Window : IWin32Window
	{
		private IntPtr handle;

		/// <summary>
		/// Public no argument constructor.
		/// </summary>
		public DummyWin32Window()
		{
		}

		/// <summary>
		/// Constructor used to initialize the handle.
		/// </summary>
		/// <param name="handle"></param>
		public DummyWin32Window(IntPtr handle)
		{
			this.handle = handle;
		}

		/// <summary>
		/// Gets the handle to the window represented by the implementer.
		/// </summary>
		public IntPtr Handle
		{
			get { return handle; }
		}
	}
}
