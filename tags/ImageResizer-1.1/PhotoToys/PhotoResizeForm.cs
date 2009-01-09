//------------------------------------------------------------------------------
// <copyright file="PhotoResizeForm.cs" company="Brice Lambson">
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
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Gathers resizing options form the user.
    /// </summary>
    public partial class PhotoResizeForm : Form
    {
        /// <summary>
        /// Indicates weather the current view is the advanced view or not.
        /// </summary>
        private bool isAdvanced;

        /// <summary>
        /// Initializes a new instance of the PhotoResizeForm class.
        /// </summary>
        public PhotoResizeForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the new images' target width.
        /// </summary>
        public int PhotoWidth
        {
            get
            {
                if (smallRadioButton.Checked)
                {
                    return 640;
                }

                if (mediumRadioButton.Checked)
                {
                    return 800;
                }

                if (largeRadioButton.Checked)
                {
                    return 1024;
                }

                if (handheldRadioButton.Checked)
                {
                    return 240;
                }

                return Convert.ToInt32(customWidthTextBox.Text, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets the new images' target height.
        /// </summary>
        public int PhotoHeight
        {
            get
            {
                if (smallRadioButton.Checked)
                {
                    return 480;
                }

                if (mediumRadioButton.Checked)
                {
                    return 600;
                }

                if (largeRadioButton.Checked)
                {
                    return 768;
                }

                if (handheldRadioButton.Checked)
                {
                    return 320;
                }

                return Convert.ToInt32(customHeightTextBox.Text, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the the new images should only be made smaller.
        /// </summary>
        public bool SmallerOnly
        {
            get { return smallerOnlyCheckBox.Checked; }
        }

        /// <summary>
        /// Gets a string to append to the new images' file names.
        /// </summary>
        public string FileNameAppendage
        {
            get
            {
                if (resizeOriginalCheckBox.Checked)
                {
                    return String.Empty;
                }

                if (smallRadioButton.Checked)
                {
                    return " (Small)";
                }

                if (mediumRadioButton.Checked)
                {
                    return " (Medium)";
                }

                if (largeRadioButton.Checked)
                {
                    return " (Large)";
                }

                if (handheldRadioButton.Checked)
                {
                    return " (WinCE)";
                }

                return " (Custom)";
            }
        }

        /// <summary>
        /// Handles the form's FormClosing Event without using a delegate.
        /// </summary>
        /// <param name="e">The event's arguments.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            int dummy;

            if (this.DialogResult != DialogResult.Cancel && customRadioButton.Checked && !(Int32.TryParse(this.customWidthTextBox.Text, out dummy) && Int32.TryParse(this.customHeightTextBox.Text, out dummy)))
            {
                MessageBoxOptions options = 0;

                if (this.RightToLeft == RightToLeft.Yes)
                {
                    options = MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
                }

                MessageBox.Show("The custom size dimmensions must be numbers.\r\nPlease check that those text fields are numbers and try again.", "Picture Resizer", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, options);

                e.Cancel = true;
            }

            base.OnFormClosing(e);
        }

        /// <summary>
        /// Handels customRadioButton's CheckedChanged event.
        /// </summary>
        /// <param name="sender">The event's sender.</param>
        /// <param name="e">The event's arguements</param>
        private void HandleCustomRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            customLabel1.Enabled = customRadioButton.Checked;
            customWidthTextBox.Enabled = customRadioButton.Checked;
            customLabel2.Enabled = customRadioButton.Checked;
            customHeightTextBox.Enabled = customRadioButton.Checked;
            customLabel3.Enabled = customRadioButton.Checked;
        }

        /// <summary>
        /// Handels advancedButton's Click event.
        /// </summary>
        /// <param name="sender">The event's sender.</param>
        /// <param name="e">The event's arguements</param>
        private void HandleAdvancedButtonClick(object sender, EventArgs e)
        {
            this.isAdvanced = !this.isAdvanced;

            this.customRadioButton.Visible = this.isAdvanced;
            this.customLabel1.Visible = this.isAdvanced;
            this.customWidthTextBox.Visible = this.isAdvanced;
            this.customLabel2.Visible = this.isAdvanced;
            this.customHeightTextBox.Visible = this.isAdvanced;
            this.customLabel3.Visible = this.isAdvanced;
            this.smallerOnlyCheckBox.Visible = this.isAdvanced;
            this.resizeOriginalCheckBox.Visible = this.isAdvanced;

            if (this.isAdvanced)
            {
                this.Height = 299;
                this.advancedButton.Text = "<< &Basic";
            }
            else
            {
                if (this.customRadioButton.Checked)
                {
                    this.smallRadioButton.Checked = true;
                }

                this.customWidthTextBox.Text = "1200";
                this.customHeightTextBox.Text = "1024";
                this.smallerOnlyCheckBox.Checked = false;
                this.resizeOriginalCheckBox.Checked = false;

                this.Height = 227;
                this.advancedButton.Text = "&Advanced >>";
            }
        }
    }
}
