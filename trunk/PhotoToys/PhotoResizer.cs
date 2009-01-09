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
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace PhotoToys
{
	public static class PhotoResizer
	{
		private static string GetNewFileName(string filePath, string directoryName, string fileNameAppendage)
		{
			if (String.IsNullOrEmpty(fileNameAppendage))
			{
				return filePath;
			}

			string fileName = Path.GetFileNameWithoutExtension(filePath);
			string extension = Path.GetExtension(filePath);

			string path;
			int count = 1;

			do
			{
				path = Path.ChangeExtension(Path.Combine(directoryName, fileName + fileNameAppendage + ((count == 1) ? String.Empty : (" (" + count + ")"))), extension);
				++count;
			} while (File.Exists(path));

			return path;
		}

		public static void ResizePhoto(string sourceFile, string destinationDirectoryName, string fileNameAppendage, int width, int height, bool smallerOnly)
		{
			string destinationFile = GetNewFileName(sourceFile, destinationDirectoryName, fileNameAppendage);
			Image image = Image.FromFile(sourceFile);

			int sourceWidth = image.Width;
			int sourceHeight = image.Height;
			double widthRatio = width / (double)sourceWidth;
			double heightRatio = height / (double)sourceHeight;

			if (widthRatio > heightRatio)
			{
				width = (int)(heightRatio * sourceWidth);
			}
			else
			{
				height = (int)(widthRatio * sourceHeight);
			}

			if (width == sourceWidth || height == sourceHeight || (smallerOnly && (width > sourceWidth || height > sourceHeight)))
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
				Bitmap resizedImage = new Bitmap(image, width, height);

				foreach (PropertyItem propertyItem in image.PropertyItems)
				{
					resizedImage.SetPropertyItem(propertyItem);
				}

				ImageFormat format = image.RawFormat;

				image.Dispose();
				image = null;

				// TODO: The extention and type may get out of sync here.
				resizedImage.Save(destinationFile, format);

				resizedImage.Dispose();
				resizedImage = null;
			}
		}
	}
}
