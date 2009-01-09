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
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Win32;

namespace PhotoToys
{
	[ComVisible(true)]
	[Guid("4CF20B46-D006-4B90-a64b-DBAA9470EFBE")]
	public class PhotoResizeShellExtention : IShellExtInit, IContextMenu
	{
		string[] imageFiles = new string[0];

		public void Initialize(IntPtr pidlFolder, IDataObject pdtobj, IntPtr hkeyProgID)
		{
			if (pdtobj != null)
			{
				FORMATETC format = new FORMATETC();
				format.cfFormat = Win32Api.CF_HDROP;
				format.ptd = IntPtr.Zero;
				format.dwAspect = DVASPECT.DVASPECT_CONTENT;
				format.lindex = -1;
				format.tymed = TYMED.TYMED_HGLOBAL;

				STGMEDIUM medium;

				pdtobj.GetData(ref format, out medium);

				uint fileCount = Win32Api.DragQueryFile(medium.unionmember, 0xFFFFFFFF, IntPtr.Zero, 0);

				imageFiles = new string[fileCount];

				for (uint i = 0; i < fileCount; i++)
				{
					uint bufferSize = Win32Api.DragQueryFile(medium.unionmember, i, IntPtr.Zero, 0) + 1;

					IntPtr file = Marshal.AllocHGlobal((int)bufferSize);
					Win32Api.DragQueryFile(medium.unionmember, i, file, bufferSize);

					imageFiles[i] = Marshal.PtrToStringAnsi(file);

					Marshal.FreeHGlobal(file);
				}
			}

		}

		public int QueryContextMenu(IntPtr hmenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, uint uFlags)
		{
			int count = 0;

			if ((uFlags & Win32Api.CMF_DEFAULTONLY) == 0)
			{
				// Add our item to the menu.
				if (Win32Api.InsertMenu(hmenu, indexMenu, (Win32Api.MF_STRING | Win32Api.MF_BYPOSITION), (IntPtr)(idCmdFirst + (count++)), "Resize Pictures") == 0) return Marshal.GetHRForLastWin32Error();
			}

			return count;
		}

		public void InvokeCommand(IntPtr pici)
		{
			CMINVOKECOMMANDINFO ici = (CMINVOKECOMMANDINFO)Marshal.PtrToStructure(pici, typeof(CMINVOKECOMMANDINFO));
			CMINVOKECOMMANDINFOEX icix = new CMINVOKECOMMANDINFOEX();
			bool isUnicode = false;
			string verb = null;
			int idCmd = -1;

			if (ici.cbSize == 64)
			{
				if ((ici.fMask & Win32Api.CMIC_MASK_UNICODE) != 0)
				{
					isUnicode = true;

					icix = (CMINVOKECOMMANDINFOEX)Marshal.PtrToStructure(pici, typeof(CMINVOKECOMMANDINFOEX));
				}
			}

			if (isUnicode && (((int)icix.lpVerbW >> 16) & 0xFFFF) != 0)
			{
				verb = Marshal.PtrToStringUni(icix.lpVerbW);
			}
			else if (!isUnicode && (((int)ici.lpVerb >> 16) & 0xFFFF) != 0)
			{
				verb = Marshal.PtrToStringAnsi(ici.lpVerb);
			}
			else
			{
				idCmd = ((int)ici.lpVerb & 0xFFFF);
			}

			if (verb == "PhotoResize" || idCmd == 0)
			{
				PhotoResizeForm photoResizeForm = new PhotoResizeForm();
				ShowDialogDelegate showDialogDelegate = photoResizeForm.ShowDialog;
				ShowDialogState showDialogState = new ShowDialogState();
				showDialogState.PhotoResizeForm = photoResizeForm;
				showDialogState.ImageFiles = imageFiles;
				showDialogState.ShowDialogDelegate = showDialogDelegate;

				// Show the Resize Pictures form.
				showDialogDelegate.BeginInvoke(new DummyWin32Window(ici.hwnd), PhotoResizer.ShowDialogCallback, showDialogState);
			}
			else
			{
				throw new ArgumentException();
			}
		}

		public void GetCommandString(uint idCmd, uint uType, uint pReserved, IntPtr pszName, uint cchMax)
		{
			string name = null;

			if (idCmd == 0)
			{
				if ((uType & ~(Win32Api.GCS_UNICODE)) == Win32Api.GCS_VERB)
				{
					name = "PhotoResize";
				}
			}
			else
			{
				throw new ArgumentException();
			}

			if (name != null)
			{
				byte[] bytes;

				name = name.Substring(0, Math.Min(name.Length, (int)cchMax - 1));
				name += Char.ConvertFromUtf32(0);

				if ((uType & Win32Api.GCS_UNICODE) != 0)
				{
					bytes = Encoding.Unicode.GetBytes(name);
				}
				else
				{
					bytes = Encoding.ASCII.GetBytes(name);
				}

				Marshal.Copy(bytes, 0, pszName, bytes.Length);
			}
		}

#if DEBUG
		[ComRegisterFunction]
		private static void Register(Type type)
		{
			using (RegistryKey photoResizeRegistryKey = Registry.ClassesRoot.CreateSubKey(@"SystemFileAssociations\image\ShellEx\ContextMenuHandlers\PhotoResize"))
			{
				photoResizeRegistryKey.SetValue(null, "{4CF20B46-D006-4B90-a64b-DBAA9470EFBE}");
			}

			using (RegistryKey approvedRegistryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
			{
				approvedRegistryKey.SetValue("{4CF20B46-D006-4B90-a64b-DBAA9470EFBE}", "PhotoToys");
			}
		}

		[ComUnregisterFunction]
		private static void Unregister(Type type)
		{
			Registry.ClassesRoot.DeleteSubKey(@"SystemFileAssociations\image\ShellEx\ContextMenuHandlers\PhotoResize", false);

			using (RegistryKey approvedRegistryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
			{
				approvedRegistryKey.DeleteValue("{4CF20B46-D006-4B90-a64b-DBAA9470EFBE}");
			}
		}
#endif
	}
}
