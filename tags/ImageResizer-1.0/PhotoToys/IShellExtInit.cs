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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace PhotoToys
{
	/// <summary>
	/// Exposes a method that initializes Shell extensions for shortcut menus.
	/// </summary>
	[ComImport]
	[Guid("000214E8-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IShellExtInit
	{
		/// <summary>
		/// Initializes a shortcut menu extension.
		/// </summary>
		/// <param name="pidlFolder">A pointer to an ITEMIDLIST structure that uniquely identifies a folder. For shortcut menu extensions, it is the item identifier list for the folder that contains the item whose shortcut menu is being displayed.</param>
		/// <param name="pdtobj">A pointer to an IDataObject interface object that can be used to retrieve the objects being acted upon.</param>
		/// <param name="hkeyProgID">The registry key for the file object or folder type.</param>
		void Initialize(IntPtr pidlFolder, [In] IDataObject pdtobj, IntPtr hkeyProgID);
	}
}
