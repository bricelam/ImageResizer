#include "stdafx.h"
#include "ImageHelper.h"

ImageHelper::ImageHelper()
{
	GdiplusStartupInput input;
	GdiplusStartup(&m_token, &input, NULL);

	// Load the list of encoders.
	UINT size;
	UINT numEncoders;
	GetImageEncodersSize(&numEncoders, &size);

	ImageCodecInfo *encoders = (ImageCodecInfo *)malloc(size);
	GetImageEncoders(numEncoders, size, encoders);

	for (UINT i = 0; i < numEncoders; i++)
	{
		ImageCodecInfo encoder = encoders[i];

		m_aEncoders.Add(encoder);

		// Find a default encoder to use.
		if (encoder.FormatID == ImageFormatPNG)
		{
			m_defaultEncoder = encoder;
		}
	}

	free(encoders);	
}

ImageHelper::~ImageHelper()
{
	GdiplusShutdown(m_token);
}

void ImageHelper::Resize(const CPath &pathSource, const CPath &pathDirectory, IMAGE_SIZE size, UINT nWidth, UINT nHeight, BOOL fSmallerOnly, BOOL fOverwriteOriginal)
{
	CPath pathDestination = GetDestinationPath(pathSource, pathDirectory, size, fOverwriteOriginal);
	Image *pImage = Image::FromFile(CT2W(pathSource));

	AdjustSize(nWidth, nHeight, pImage->GetWidth(), pImage->GetHeight(), fSmallerOnly);

	Image *pImage2 = new Bitmap(nWidth, nHeight);
	Graphics *pGraphics = Graphics::FromImage(pImage2);

	// Draw new image.
	pGraphics->SetCompositingQuality(CompositingQualityHighQuality);
	pGraphics->SetInterpolationMode(InterpolationModeHighQualityBicubic);
	pGraphics->Clear(Color::Transparent);
	pGraphics->DrawImage(pImage, 0, 0, nWidth, nHeight);

	delete pGraphics;

	// Copy metadata.
	UINT totalBufferSize;
	UINT numProperties;
	pImage->GetPropertySize(&totalBufferSize, &numProperties);

	PropertyItem *allItems = (PropertyItem *)malloc(totalBufferSize);
	pImage->GetAllPropertyItems(totalBufferSize, numProperties, allItems);

	for (UINT i = 0; i < numProperties; i++)
	{
		pImage2->SetPropertyItem(allItems + i);
	}

	free(allItems);

	const ImageCodecInfo *pEncoder = GetEncoderForImage(pImage);

	delete pImage;

	if (!pEncoder)
	{
		pEncoder = &m_defaultEncoder;
		pathDestination.RenameExtension(_T(".png"));
	}

	// Write new image.
	pImage2->Save(CT2W(pathDestination), &pEncoder->Clsid);

	delete pImage2;
}

CPath ImageHelper::GetDestinationPath(const CPath &pathSource, const CPath &pathDirecotry, IMAGE_SIZE size, BOOL fOverwriteOriginal)
{
	if (fOverwriteOriginal)
	{
		return pathSource;
	}

	CString strAppendage = SettingsHelper::GetAppendageForSize(size);
	
	int iFileName = pathSource.FindFileName();
	int iExtension = pathSource.FindExtension();

	CString strFileName = CString(pathSource).Mid(iFileName, iExtension - iFileName);
	CString strExtension = CString(pathSource).Mid(iExtension);
	
	CPath path;
	UINT count = 1;
	
	do
	{
		CString strNewFileName;

		if (count == 1)
		{
			strNewFileName.FormatMessage(IDS_FILENAME, strFileName, strAppendage);
		}
		else
		{
			strNewFileName.FormatMessage(IDS_FILENAMEEX, strFileName, strAppendage, count);
		}

		path.Combine(pathDirecotry, strNewFileName);
		path.RenameExtension(strExtension);

		count++;
	} while (path.FileExists());

	return path;
}

void ImageHelper::AdjustSize(UINT &nWidth, UINT &nHeight, UINT srcWidth, UINT srcHeight, BOOL fSmallerOnly)
{
	if (nHeight == 0)
	{
		nHeight = srcWidth * srcHeight / nWidth;
	}
	else if (nWidth == 0)
	{
		nWidth = srcHeight * srcWidth / nHeight;
	}

	FLOAT widthRatio = nWidth / (FLOAT)srcWidth;
	FLOAT heightRatio = nHeight / (FLOAT)srcHeight;

	if (widthRatio > heightRatio)
	{
		nWidth = (UINT)(heightRatio * srcWidth);
	}
	else
	{
		nHeight = (UINT)(widthRatio * srcHeight);
	}

	if (nWidth == srcWidth || nHeight == srcHeight || (fSmallerOnly && (nWidth > srcWidth || nHeight > srcHeight)))
	{
		nWidth = srcWidth;
		nHeight = srcHeight;
	}
}

const ImageCodecInfo *ImageHelper::GetEncoderForImage(Image *pImage)
{
	GUID format;
	pImage->GetRawFormat(&format);

	size_t numEncoders = m_aEncoders.GetCount();

	for (UINT i = 0; i < numEncoders; i++)
	{
		if (m_aEncoders[i].FormatID == format)
		{
			return &m_aEncoders[i];
		}
	}

	return NULL;
}
