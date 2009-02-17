#include "stdafx.h"
#include "PhotoResizer.h"

using namespace Gdiplus;

CPhotoResizer::CPhotoResizer()
{
	GdiplusStartupInput input;
	GdiplusStartup(&m_token, &input, NULL);

	UINT size;
	GetImageEncodersSize(&m_numEncoders, &size);

	m_encoders = (ImageCodecInfo *)malloc(size);
	GetImageEncoders(m_numEncoders, size, m_encoders);
}

CPhotoResizer::~CPhotoResizer()
{
	free(m_encoders);

	GdiplusShutdown(m_token);
}

void CPhotoResizer::ResizePhoto(CString filename, CString dstFilename, UINT width, UINT height, BOOL smallerOnly)
{
	Image *image = Image::FromFile(filename);

	UINT srcWidth = image->GetWidth();
    UINT srcHeight = image->GetHeight();
    FLOAT widthRatio = width / (FLOAT)srcWidth;
    FLOAT heightRatio = height / (FLOAT)srcHeight;

	if (widthRatio > heightRatio)
    {
        width = (UINT)(heightRatio * srcWidth);
    }
    else
    {
        height = (UINT)(widthRatio * srcHeight);
    }

	if (width == srcWidth || height == srcHeight || (smallerOnly && (width > srcWidth || height > srcHeight)))
    {
        delete image;

		if (filename.CompareNoCase(dstFilename) != 0)
        {
			CopyFile(filename, dstFilename, TRUE);
        }
    }
	else
    {
        Image *dstImage = new Bitmap(width, height);

		Graphics *graphics = Graphics::FromImage(dstImage);
        
		graphics->SetCompositingQuality(CompositingQualityHighQuality);
		graphics->SetInterpolationMode(InterpolationModeHighQualityBicubic);
		graphics->Clear(Color::Transparent);
        graphics->DrawImage(image, 0, 0, width, height);
        
		delete graphics;

		UINT totalBufferSize;
		UINT numProperties;
		image->GetPropertySize(&totalBufferSize, &numProperties);

		PropertyItem *allItems = (PropertyItem *)malloc(totalBufferSize);
		image->GetAllPropertyItems(totalBufferSize, numProperties, allItems);

		for (UINT i = 0; i < numProperties; i++)
		{
			dstImage->SetPropertyItem(allItems + i);
		}

		free(allItems);

		CLSID clsidEncoder;
		GetImageEncoderClsid(image, &clsidEncoder);

        delete image;

        // TODO: The extention and type may get out of sync here.
		// TODO: Possibly use the encoderParams of image.
        dstImage->Save(dstFilename, &clsidEncoder);

        delete dstImage;
    }
}

void CPhotoResizer::GetImageEncoderClsid(Image *image, CLSID *clsidEncoder)
{
	CLSID format;
	image->GetRawFormat(&format);

	CLSID clsidEncoderBMP;
	BOOL found = FALSE;

	for (UINT i = 0; i < m_numEncoders && !found; i++)
	{
		ImageCodecInfo encoder = m_encoders[i];

		if (encoder.FormatID == format)
		{
			*clsidEncoder = encoder.Clsid;

			found = TRUE;
		}
		else if (encoder.FormatID == ImageFormatBMP)
		{
			clsidEncoderBMP = encoder.Clsid;
		}
	}

	if (!found)
	{
		// TODO: This default may not be ideal.
		*clsidEncoder = clsidEncoderBMP;
	}
}
