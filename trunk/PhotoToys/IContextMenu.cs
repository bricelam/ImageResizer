//------------------------------------------------------------------------------
// <copyright file="IContextMenu.cs" company="Brice Lambson">
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

    /// <summary>
    /// Exposes methods that merge a shortcut menu associated with a Shell object.
    /// </summary>
    [ComImport]
    [Guid("000214e4-0000-0000-c000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IContextMenu
    {
        /// <summary>
        /// Adds commands to a shortcut menu.
        /// </summary>
        /// <param name="hmenu">A handle to the menu. The handler should specify this handle when adding menu items.</param>
        /// <param name="indexMenu">The zero-based position at which to insert the first menu item.</param>
        /// <param name="idCmdFirst">The minimum value that the handler can specify for a menu item identifier.</param>
        /// <param name="idCmdLast">The maximum value that the handler can specify for a menu item identifier.</param>
        /// <param name="uFlags">Optional flags specifying how the shortcut menu can be changed.</param>
        /// <returns>The offset of the largest command identifier that was assigned, plus one.</returns>
        [PreserveSig]
        int QueryContextMenu(IntPtr hmenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, uint uFlags);

        /// <summary>
        /// Carries out the command associated with a shortcut menu item.
        /// </summary>
        /// <param name="pici">A pointer to a CMINVOKECOMMANDINFO or CMINVOKECOMMANDINFOEX structure containing information about the command.</param>
        void InvokeCommand(IntPtr pici);

        /// <summary>
        /// Gets information about a shortcut menu command, including the help string and the language-independent, or canonical, name for the command.
        /// </summary>
        /// <param name="idCmd">Menu command identifier offset.</param>
        /// <param name="uType">Flags specifying the information to return.</param>
        /// <param name="pReserved">Reserved. Handlers must ignore this parameter when called.</param>
        /// <param name="pszName">The address of the buffer to receive the null-terminated string being retrieved.</param>
        /// <param name="cchMax">Size of the buffer, in characters, to receive the null-terminated string.</param>
        void GetCommandString(uint idCmd, uint uType, uint pReserved, [Out] IntPtr pszName, uint cchMax);
    }
}
