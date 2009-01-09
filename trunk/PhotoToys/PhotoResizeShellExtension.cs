//------------------------------------------------------------------------------
// <copyright file="PhotoResizeShellExtension.cs" company="Brice Lambson">
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
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using Microsoft.Win32;
    using DialogResult = System.Windows.Forms.DialogResult;
    using System.Diagnostics;
    using System.IO;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The Resize Pictures shortcut menu extension.
    /// </summary>
    [ComVisible(true)]
    [Guid("4CF20B46-D006-4B90-a64b-DBAA9470EFBE")]
    public class PhotoResizeShellExtension : IShellExtInit, IContextMenu
    {
        /// <summary>
        /// The image files to resize.
        /// </summary>
        private string[] imageFiles = new string[0];

        /// <summary>
        /// Initializes the shortcut menu extension.
        /// </summary>
        /// <param name="pidlFolder">A pointer to an ITEMIDLIST structure that uniquely identifies a folder. For shortcut menu extensions, it is the item identifier list for the folder that contains the item whose shortcut menu is being displayed.</param>
        /// <param name="pdtobj">A pointer to an IDataObject interface object that can be used to retrieve the objects being acted upon.</param>
        /// <param name="hkeyProgID">The registry key for the file object or folder type.</param>
        public void Initialize(IntPtr pidlFolder, IDataObject pdtobj, IntPtr hkeyProgID)
        {
            if (pdtobj != null)
            {
                FORMATETC format = new FORMATETC();
                format.cfFormat = NativeMethods.CF_HDROP;
                format.ptd = IntPtr.Zero;
                format.dwAspect = DVASPECT.DVASPECT_CONTENT;
                format.lindex = -1;
                format.tymed = TYMED.TYMED_HGLOBAL;

                STGMEDIUM medium;

                pdtobj.GetData(ref format, out medium);

                uint fileCount = NativeMethods.DragQueryFile(medium.unionmember, 0xFFFFFFFF, IntPtr.Zero, 0);

                this.imageFiles = new string[fileCount];

                for (uint i = 0; i < fileCount; i++)
                {
                    uint bufferSize = NativeMethods.DragQueryFile(medium.unionmember, i, IntPtr.Zero, 0) + 1;

                    IntPtr file = Marshal.AllocHGlobal((int)bufferSize);
                    uint length = NativeMethods.DragQueryFile(medium.unionmember, i, file, bufferSize);

                    this.imageFiles[i] = Marshal.PtrToStringAnsi(file, (int)length);

                    Marshal.FreeHGlobal(file);
                }
            }
        }

        /// <summary>
        /// Adds commands to a shortcut menu.
        /// </summary>
        /// <param name="hmenu">A handle to the menu. The handler should specify this handle when adding menu items.</param>
        /// <param name="indexMenu">The zero-based position at which to insert the first menu item.</param>
        /// <param name="idCmdFirst">The minimum value that the handler can specify for a menu item identifier.</param>
        /// <param name="idCmdLast">The maximum value that the handler can specify for a menu item identifier.</param>
        /// <param name="uFlags">Optional flags specifying how the shortcut menu can be changed.</param>
        /// <returns>The offset of the largest command identifier that was assigned, plus one.</returns>
        [CLSCompliant(false)]
        public int QueryContextMenu(IntPtr hmenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, uint uFlags)
        {
            int count = 0;

            if ((uFlags & NativeMethods.CMF_DEFAULTONLY) == 0)
            {
                // Add our item to the menu.
                if (NativeMethods.InsertMenu(hmenu, indexMenu, (NativeMethods.MF_STRING | NativeMethods.MF_BYPOSITION), (IntPtr)(idCmdFirst + (count++)), "Resize Pictures") == 0)
                {
                    return Marshal.GetHRForLastWin32Error();
                }
            }

            return count;
        }

        /// <summary>
        /// Carries out the command associated with a shortcut menu item.
        /// </summary>
        /// <param name="pici">A pointer to a CMINVOKECOMMANDINFO or CMINVOKECOMMANDINFOEX structure containing information about the command.</param>
        public void InvokeCommand(IntPtr pici)
        {
            CMINVOKECOMMANDINFO ici = (CMINVOKECOMMANDINFO)Marshal.PtrToStructure(pici, typeof(CMINVOKECOMMANDINFO));
            CMINVOKECOMMANDINFOEX icix = new CMINVOKECOMMANDINFOEX();
            bool isUnicode = false;
            string verb = null;
            int idCmd = -1;

            if (ici.cbSize == 64)
            {
                if ((ici.fMask & NativeMethods.CMIC_MASK_UNICODE) != 0)
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
                showDialogState.ImageFiles = this.imageFiles;
                showDialogState.ShowDialogDelegate = showDialogDelegate;
                showDialogState.WorkingDirectory = ici.lpDirectory;

                // Show the Resize Pictures form.
                showDialogDelegate.BeginInvoke(new DummyWin32Window(ici.hwnd), this.ShowDialogCallback, showDialogState);
            }
            else
            {
                throw new ArgumentException("The specified command does not exist.", "pici");
            }
        }

        /// <summary>
        /// Gets information about a shortcut menu command, including the help string and the language-independent, or canonical, name for the command.
        /// </summary>
        /// <param name="idCmd">Menu command identifier offset.</param>
        /// <param name="uType">Flags specifying the information to return.</param>
        /// <param name="pReserved">Reserved. Handlers must ignore this parameter when called.</param>
        /// <param name="pszName">The address of the buffer to receive the null-terminated string being retrieved.</param>
        /// <param name="cchMax">Size of the buffer, in characters, to receive the null-terminated string.</param>
        [CLSCompliant(false)]
        public void GetCommandString(uint idCmd, uint uType, uint pReserved, IntPtr pszName, uint cchMax)
        {
            string name = null;

            if (idCmd == 0)
            {
                if ((uType & ~NativeMethods.GCS_UNICODE) == NativeMethods.GCS_VERB)
                {
                    name = "PhotoResize";
                }
            }
            else
            {
                throw new ArgumentException("The specified command does not exist.", "idCmd");
            }

            if (name != null)
            {
                byte[] bytes;

                name = name.Substring(0, Math.Min(name.Length, checked((int)cchMax - 1)));
                name += Char.ConvertFromUtf32(0);

                if ((uType & NativeMethods.GCS_UNICODE) != 0)
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
        /// <summary>
        /// Registers the shortcut menu.
        /// </summary>
        /// <param name="type">The type of the COM object to register</param>
        [ComRegisterFunction]
        private static void ComRegister(Type type)
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

        /// <summary>
        /// Unegisters the shortcut menu.
        /// </summary>
        /// <param name="type">The type of the COM object to unregister</param>
        [ComUnregisterFunction]
        private static void ComUnregister(Type type)
        {
            Registry.ClassesRoot.DeleteSubKey(@"SystemFileAssociations\image\ShellEx\ContextMenuHandlers\PhotoResize", false);

            using (RegistryKey approvedRegistryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true))
            {
                approvedRegistryKey.DeleteValue("{4CF20B46-D006-4B90-a64b-DBAA9470EFBE}");
            }
        }
#endif

        /// <summary>
        /// Callback function used to display a dialog asynchronously.
        /// </summary>
        /// <param name="result">The status of the asynchronous operation.</param>
        private void ShowDialogCallback(IAsyncResult result)
        {
            ShowDialogState showDialogState = (ShowDialogState)result.AsyncState;

            ShowDialogDelegate showDialogDelegate = showDialogState.ShowDialogDelegate;
            string[] imageFiles = showDialogState.ImageFiles;
            PhotoResizeForm photoResizeForm = showDialogState.PhotoResizeForm;

            if (showDialogDelegate.EndInvoke(result) != DialogResult.Cancel)
            {
                string outputPath = showDialogState.WorkingDirectory;
                string fileNameAppendage = photoResizeForm.FileNameAppendage;
                int photoWidth = photoResizeForm.PhotoWidth;
                int photoHeight = photoResizeForm.PhotoHeight;
                bool smallerOnly = photoResizeForm.SmallerOnly;

                foreach (string imageFile in imageFiles)
                {
                    PhotoResizer.ResizePhoto(imageFile, outputPath, fileNameAppendage, photoWidth, photoHeight, smallerOnly);
                }
            }

            photoResizeForm.Dispose();
            photoResizeForm = null;
        }
    }
}
