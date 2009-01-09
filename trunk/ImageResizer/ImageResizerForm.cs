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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using PhotoToys;

namespace ImageResizer
{
	public partial class ImageResizerForm : Form
	{
		PhotoResizeForm photoResizeForm;

		public ImageResizerForm()
		{
			InitializeComponent();

			ImageList imageList = new ImageList();
			imageList.ColorDepth = ColorDepth.Depth24Bit;
			imageList.ImageSize = new Size(96, 96);
			imagesListView.LargeImageList = imageList;

			photoResizeForm = new PhotoResizeForm();
		}

		private void picturesListView_DragEnter(object sender, DragEventArgs e)
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

		private void picturesListView_DragDrop(object sender, DragEventArgs e)
		{
			string[] pathArray = e.Data.GetData(DataFormats.FileDrop) as string[];

			if (pathArray != null)
			{
				AddPictures(pathArray);
			}
		}

		private void browseButton_Click(object sender, EventArgs e)
		{
			folderBrowserDialog.SelectedPath = outputTextBox.Text;

			if (folderBrowserDialog.ShowDialog() != DialogResult.Cancel)
			{
				outputTextBox.Text = folderBrowserDialog.SelectedPath;
			}
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			if (openFileDialog.ShowDialog() != DialogResult.Cancel)
			{
				AddPictures(openFileDialog.FileNames);
			}
		}

		private void imagesListView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete || (e.Control && e.KeyCode == Keys.D))
			{
				RemoveSelected();
			}
			else if (e.Control && e.KeyCode == Keys.A)
			{
				for (int i = 0; i < imagesListView.Items.Count; i++)
				{
					imagesListView.SelectedIndices.Add(i);
				}
			}
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			RemoveSelected();
		}

		private void resizeButton_Click(object sender, EventArgs e)
		{
			string outputPath = outputTextBox.Text;

			if (!Directory.Exists(outputPath))
			{
				MessageBoxOptions options = 0;

				if (RightToLeft == RightToLeft.Yes)
				{
					options = MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
				}

				if (MessageBox.Show("The folder\r\n" + outputPath + "\r\ndoes not exist.\r\nWould you like to create it first?", "Image Resizer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, options) != DialogResult.No)
				{
					Directory.CreateDirectory(outputPath);
				}
			}

			if (photoResizeForm.ShowDialog() != DialogResult.Cancel)
			{
				string fileNameAppendage = photoResizeForm.FileNameAppendage;
				int photoWidth = photoResizeForm.PhotoWidth;
				int photoHeight = photoResizeForm.PhotoHeight;
				bool smallerOnly = photoResizeForm.SmallerOnly;

				foreach (ListViewItem listViewItem in imagesListView.Items)
				{
					PhotoResizer.ResizePhoto(listViewItem.Name, outputPath, fileNameAppendage, photoWidth, photoHeight, smallerOnly);
				}

				imagesListView.Clear();
			}
		}

		private void AddPictures(string[] pathArray)
		{
			if (String.IsNullOrEmpty(outputTextBox.Text) && pathArray.Length > 0)
			{
				outputTextBox.Text = Path.GetDirectoryName(pathArray[0]);
			}

			imagesListView.BeginUpdate();

			foreach (string path in pathArray)
			{
				if (imagesListView.Items.ContainsKey(path))
				{
					continue;
				}

				try
				{
					if (!imagesListView.LargeImageList.Images.ContainsKey(path))
					{
						imagesListView.LargeImageList.Images.Add(path, Image.FromFile(path));
					}
				}
				catch (OutOfMemoryException)
				{
					continue;
				}

				imagesListView.Items.Add(path, Path.GetFileNameWithoutExtension(path), path);
			}

			imagesListView.EndUpdate();
		}

		private void RemoveSelected()
		{
			for (int i = imagesListView.SelectedItems.Count - 1; i >= 0; i--)
			{
				ListViewItem selectedItem = imagesListView.SelectedItems[i];

				imagesListView.Items.Remove(selectedItem);
			}
		}
	}
}
