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
using System.Runtime.InteropServices;

namespace PhotoToys
{
	/// <summary>
	/// This structure defines the x- and y-coordinates of a point.
	/// </summary>
	public struct POINT
	{
		/// <summary>
		/// Specifies the x-coordinate of the point.
		/// </summary>
		public long x;

		/// <summary>
		/// Specifies the y-coordinate of the point.
		/// </summary>
		public long y;
	}

	/// <summary>
	/// Contains extended information about a shortcut menu command.
	/// </summary>
	public struct CMINVOKECOMMANDINFOEX
	{
		/// <summary>
		/// The size of this structure, in bytes.
		/// </summary>
		public uint cbSize;

		/// <summary>
		/// Indicate desired behavior and that other fields in the structure are to be used.
		/// </summary>
		public uint fMask;

		/// <summary>
		/// A handle to the window that is the owner of the shortcut menu.
		/// </summary>
		public IntPtr hwnd;

		/// <summary>
		/// A 32-bit value that contains zero in the high-order word and a menu-identifier offset of the command to carry out in the low-order word.
		/// If the high-order word is not zero, this member is the address of a null-terminated string specifying the language-independent name of the command to carry out.
		/// </summary>
		public IntPtr lpVerb;

		/// <summary>
		/// Optional parameters.
		/// </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string lpParameters;

		/// <summary>
		/// An optional working directory name.
		/// </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string lpDirectory;

		/// <summary>
		/// A set of SW_ values to pass to the ShowWindow function if the command displays a window or starts an application.
		/// </summary>
		public int nShow;

		/// <summary>
		/// An optional hot key to assign to any application activated by the command.
		/// </summary>
		public uint dwHotKey;

		/// <summary>
		/// An icon to use for any application activated by the command.
		/// </summary>
		public IntPtr hIcon;

		/// <summary>
		/// An ASCII title.
		/// </summary>
		[MarshalAs(UnmanagedType.LPStr)]
		public string lpTitle;

		/// <summary>
		/// A Unicode verb, for those commands that can use it.
		/// </summary>
		public IntPtr lpVerbW;

		/// <summary>
		/// A Unicode parameters, for those commands that can use it.
		/// </summary>
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpParametersW;

		/// <summary>
		/// A Unicode directory, for those commands that can use it.
		/// </summary>
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpDirectoryW;

		/// <summary>
		/// A Unicode title.
		/// </summary>
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpTitleW;

		/// <summary>
		/// The point where the command is invoked.
		/// </summary>
		public POINT ptInvoke;
	}
}
