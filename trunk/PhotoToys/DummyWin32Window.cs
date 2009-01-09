#region Common Public License Copyright Notice
/**************************************************************************\
* PhotoToys Clone                                                                   *
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
