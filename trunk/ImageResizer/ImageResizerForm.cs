//------------------------------------------------------------------------------
// <copyright file="ImageResizerForm.cs" company="Brice Lambson">
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

namespace ImageResizer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.IO;
    using Microsoft.Win32;
    using PhotoToys;

    /// <summary>
    /// The application's main form.
    /// </summary>
    public partial class ImageResizerForm : Form
    {
        /// <summary>
        /// Form to show the user as a means of gathering resize options.
        /// </summary>
        private PhotoResizeForm photoResizeForm;

        /// <summary>
        /// No argument constructor.
        /// </summary>
        public ImageResizerForm()
        {
            this.InitializeComponent();

            ImageList imageList = new ImageList();
            imageList.ColorDepth = ColorDepth.Depth24Bit;
            imageList.ImageSize = new Size(96, 96);
            this.imagesListView.LargeImageList = imageList;

            this.photoResizeForm = new PhotoResizeForm();
        }

        /// <summary>
        /// Handels picturesListView's DragEnter event.
        /// </summary>
        /// <param name="sender">The event's sender.</param>
        /// <param name="e">The event's arguements</param>
        private void HandlePicturesListViewDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Handels picturesListView's DragDrop event.
        /// </summary>
        /// <param name="sender">The event's sender.</param>
        /// <param name="e">The event's arguements</param>
        private void HandlePicturesListViewDragDrop(object sender, DragEventArgs e)
        {
            string[] pathArray = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (pathArray != null)
            {
                this.AddPictures(pathArray);
            }
        }

        /// <summary>
        /// Handels browseButton's Click event.
        /// </summary>
        /// <param name="sender">The event's sender.</param>
        /// <param name="e">The event's arguements</param>
        private void HancleBrowseButtonClick(object sender, EventArgs e)
        {
            this.folderBrowserDialog.SelectedPath = this.outputTextBox.Text;

            if (this.folderBrowserDialog.ShowDialog() != DialogResult.Cancel)
            {
                this.outputTextBox.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Handels addButton's Click event.
        /// </summary>
        /// <param name="sender">The event's sender.</param>
        /// <param name="e">The event's arguements</param>
        private void HandleAddButtonClick(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                this.AddPictures(this.openFileDialog.FileNames);
            }
        }

        /// <summary>
        /// Handels imagesListView's KeyDown event.
        /// </summary>
        /// <param name="sender">The event's sender.</param>
        /// <param name="e">The event's arguements</param>
        private void HandleImagesListViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || (e.Control && e.KeyCode == Keys.D))
            {
                this.RemoveSelected();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                for (int i = 0; i < this.imagesListView.Items.Count; i++)
                {
                    this.imagesListView.SelectedIndices.Add(i);
                }
            }
        }

        /// <summary>
        /// Handels removeButton's Click event.
        /// </summary>
        /// <param name="sender">The event's sender.</param>
        /// <param name="e">The event's arguements</param>
        private void HandleRemoveButtonClick(object sender, EventArgs e)
        {
            this.RemoveSelected();
        }

        /// <summary>
        /// Handels resizeButton's Click event.
        /// </summary>
        /// <param name="sender">The event's sender.</param>
        /// <param name="e">The event's arguements</param>
        private void HandleResizeButtonClick(object sender, EventArgs e)
        {
            string outputPath = this.outputTextBox.Text;

            if (!Directory.Exists(outputPath))
            {
                MessageBoxOptions options = 0;

                if (this.RightToLeft == RightToLeft.Yes)
                {
                    options = MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
                }

                if (MessageBox.Show("The folder\r\n" + outputPath + "\r\ndoes not exist.\r\nWould you like to create it first?", "Image Resizer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, options) != DialogResult.No)
                {
                    Directory.CreateDirectory(outputPath);
                }
            }

            if (this.photoResizeForm.ShowDialog() != DialogResult.Cancel)
            {
                string fileNameAppendage = this.photoResizeForm.FileNameAppendage;
                int photoWidth = this.photoResizeForm.PhotoWidth;
                int photoHeight = this.photoResizeForm.PhotoHeight;
                bool smallerOnly = this.photoResizeForm.SmallerOnly;

                foreach (ListViewItem listViewItem in this.imagesListView.Items)
                {
                    PhotoResizer.ResizePhoto(listViewItem.Name, outputPath, fileNameAppendage, photoWidth, photoHeight, smallerOnly);
                }

                this.imagesListView.Clear();
            }
        }

        /// <summary>
        /// Adds pictures to the main list view.
        /// </summary>
        /// <param name="pathArray">An array of the images' paths.</param>
        private void AddPictures(string[] pathArray)
        {
            if (String.IsNullOrEmpty(this.outputTextBox.Text) && pathArray.Length > 0)
            {
                this.outputTextBox.Text = Path.GetDirectoryName(pathArray[0]);
            }

            ListView.ListViewItemCollection imagesListViewItems = this.imagesListView.Items;
            ImageList.ImageCollection imagesListViewLargeImageListImages = this.imagesListView.LargeImageList.Images;

            this.imagesListView.BeginUpdate();

            foreach (string path in pathArray)
            {
                if (imagesListViewItems.ContainsKey(path))
                {
                    continue;
                }

                try
                {
                    if (!imagesListViewLargeImageListImages.ContainsKey(path))
                    {
                        imagesListViewLargeImageListImages.Add(path, Image.FromFile(path));
                    }
                }
                catch (OutOfMemoryException)
                {
                    continue;
                }

                imagesListViewItems.Add(path, Path.GetFileNameWithoutExtension(path), path);
            }

            this.imagesListView.EndUpdate();
        }

        /// <summary>
        /// Removes the currently selected images from the main list view.
        /// </summary>
        private void RemoveSelected()
        {
            ListView.SelectedListViewItemCollection imagesListViewSelectedItems = this.imagesListView.SelectedItems;
            ListView.ListViewItemCollection imagesListViewItems = this.imagesListView.Items;

            for (int i = imagesListViewSelectedItems.Count - 1; i >= 0; i--)
            {
                imagesListViewItems.Remove(imagesListViewSelectedItems[i]);
            }
        }
    }
}
