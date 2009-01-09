//------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Brice Lambson">
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
    using System.Runtime.InteropServices;

    /// <summary>
    /// This class acts as a facad to the WIN32 API.
    /// </summary>
    internal static class NativeMethods
    {
        public const short CF_HDROP = 0xF;
        public const uint CMF_DEFAULTONLY = 1;
        public const uint MF_STRING = 0;
        public const uint MF_BYPOSITION = 0x400;
        public const uint GCS_VERB = 0;
        public const uint GCS_UNICODE = 4;
        public const uint CMIC_MASK_UNICODE = 0x4000;

        /// <summary>
        /// Inserts a new menu item into a menu, moving other items down the menu.
        /// </summary>
        /// <param name="hMenu">Handle to the menu to be changed.</param>
        /// <param name="uPosition">Specifies the menu item before which the new menu item is to be inserted, as determined by the uFlags parameter.</param>
        /// <param name="uFlags">Specifies flags that control the interpretation of the uPosition parameter and the content, appearance, and behavior of the new menu item.</param>
        /// <param name="uIDNewItem">Specifies either the identifier of the new menu item or, if the uFlags parameter has the MF_POPUP flag set, a handle to the drop-down menu or submenu.</param>
        /// <param name="lpNewItem">Specifies the content of the new menu item.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, BestFitMapping = false)]
        public static extern int InsertMenu(IntPtr hMenu, uint uPosition, uint uFlags, IntPtr uIDNewItem, string lpNewItem);

        /// <summary>
        /// Retrieves the names of dropped files that result from a successful drag-and-drop operation.
        /// </summary>
        /// <param name="hDrop">Identifier of the structure containing the file names of the dropped files.</param>
        /// <param name="iFile">Index of the file to query.</param>
        /// <param name="lpszFile">The address of a buffer to receive the file name of a dropped file when the function returns.</param>
        /// <param name="cch">Size, in characters, of the lpszFile buffer.</param>
        /// <returns>A count of the characters copied, not including the terminating null character.</returns>
        [DllImport("shell32.dll")]
        public static extern uint DragQueryFile(IntPtr hDrop, uint iFile, IntPtr lpszFile, uint cch);
    }
}
