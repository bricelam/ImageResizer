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
	public interface IShellExtInit
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
