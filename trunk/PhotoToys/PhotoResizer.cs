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
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace PhotoToys
{
	static class PhotoResizer
	{
		public static void ShowDialogCallback(IAsyncResult result)
		{
			ShowDialogState showDialogState = (ShowDialogState)result.AsyncState;

			ShowDialogDelegate showDialogDelegate = showDialogState.ShowDialogDelegate;
			string[] imageFiles = showDialogState.ImageFiles;
			PhotoResizeForm photoResizeForm = showDialogState.PhotoResizeForm;

			if (showDialogDelegate.EndInvoke(result) != DialogResult.Cancel)
			{
				foreach (string imageFile in imageFiles)
				{
					string destinationFile = GetNewFileName(imageFile, photoResizeForm.FileNameAppendage);
					int width = photoResizeForm.PhotoWidth;
					int height = photoResizeForm.PhotoHeight;
					bool smallerOnly = photoResizeForm.SmallerOnly;

					ResizePhoto(imageFile, destinationFile, width, height, smallerOnly);
				}
			}

			photoResizeForm.Dispose();
			photoResizeForm = null;
		}

		private static string GetNewFileName(string filePath, string fileNameAppendage)
		{
			if (fileNameAppendage == String.Empty)
			{
				return filePath;
			}

			string directoryName = Path.GetDirectoryName(filePath);
			string fileName = Path.GetFileNameWithoutExtension(filePath);
			string extension = Path.GetExtension(filePath);

			string path;
			int count = 1;

			do
			{
				path = BuildPath(directoryName, fileName + fileNameAppendage + ((count == 1) ? String.Empty : (" (" + count + ")")), extension);
				++count;
			} while (File.Exists(path));

			return path;
		}

		private static string BuildPath(string directoryName, string fileName, string extension)
		{
			return Path.ChangeExtension(Path.Combine(directoryName, fileName), extension);
		}

		// TODO: This method could use better exception handling.
		private static void ResizePhoto(string sourceFile, string destinationFile, int width, int height, bool smallerOnly)
		{
			Image image = Image.FromFile(sourceFile);

			if (width / (double)image.Width > height / (double)image.Height)
			{
				width = height * image.Width / image.Height;
			}
			else
			{
				height = width * image.Height / image.Width;
			}

			if (width == image.Width || height == image.Height || (smallerOnly && (width > image.Width || height > image.Height)))
			{
				image.Dispose();
				image = null;

				if (sourceFile != destinationFile)
				{
					File.Copy(sourceFile, destinationFile);
				}
			}
			else
			{
				ImageFormat format = image.RawFormat;

				Bitmap resizedImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);
				resizedImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
				
				Graphics graphics = Graphics.FromImage(resizedImage);
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

				Rectangle destRect = new Rectangle(0, 0, width, height);
				Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);

				graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);

				image.Dispose();
				image = null;

				// TODO: The extention and type may get out of synch here.
				resizedImage.Save(destinationFile, format);

				resizedImage.Dispose();
				resizedImage = null;
			}
		}
	}
}
