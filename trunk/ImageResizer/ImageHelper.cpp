#include "stdafx.h"
#include "ImageHelper.h"

ImageHelper::ImageHelper(IMAGE_SIZE nsize, UINT nWidth, UINT nHeight, BOOL fSmallerOnly, BOOL fOverwriteOriginal)
{	
	m_size = nsize;
	m_width = nWidth;
	m_height = nHeight;
	m_smallerOnly = fSmallerOnly;
	m_overwriteOriginal = fOverwriteOriginal;
	
}

ImageHelper::~ImageHelper()
{
}

void ImageHelper::Resize(const CPath &pathSource, const CPath &pathDirectory)
{	
	UINT width = 0;
	UINT height = 0;
	UINT newwidth = m_width;
	UINT newheight = m_height;
	CPath pathDestination;
	HRESULT hr;		
	IWICBitmapDecoder *piDecoder = NULL;
		

	// Create the decoder.
	hr = piFactory->CreateDecoderFromFilename(
		pathSource,
		NULL,
		GENERIC_READ,
		WICDecodeMetadataCacheOnDemand, //For JPEG lossless decoding/encoding.
		&piDecoder);

	// get target filename
	pathDestination = GetDestinationPath(pathSource, pathDirectory, m_size, m_overwriteOriginal);	
	
	// open source file	
	// Variables used for encoding.
	IWICStream *piFileStream = NULL;
	IWICBitmapEncoder *piEncoder = NULL;
	IWICMetadataBlockWriter *piBlockWriter = NULL;
	IWICMetadataBlockReader *piBlockReader = NULL;
	IWICBitmapScaler * piBitmapScaler = NULL;
	WICPixelFormatGUID pixelFormat = { 0 };
	UINT count = 0;
	double dpiX, dpiY = 0.0;

	// Create a bitmap scaler
	if (SUCCEEDED(hr))
	{
		hr = piFactory->CreateBitmapScaler(&piBitmapScaler);
	}

	// Create a file stream.
	if (SUCCEEDED(hr))
	{
		hr = piFactory->CreateStream(&piFileStream);
	}

	// Initialize our new file stream.
	if (SUCCEEDED(hr))
	{
		hr = piFileStream->InitializeFromFilename(pathDestination, GENERIC_WRITE);
	}

	// Create the encoder.
	if (SUCCEEDED(hr))
	{
		hr = piFactory->CreateEncoder(GUID_ContainerFormatJpeg, NULL, &piEncoder);
	}
	// Initialize the encoder
	if (SUCCEEDED(hr))
	{
		hr = piEncoder->Initialize(piFileStream,WICBitmapEncoderNoCache);
	}
	if (SUCCEEDED(hr))
	{
		piDecoder->GetFrameCount(&count);
	}

	//Process each frame of the image.
	for (UINT i = 0; i < count && SUCCEEDED(hr); i++)
	{
		//Frame variables.
		IWICBitmapFrameDecode *piFrameDecode = NULL;
		IWICBitmapFrameEncode *piFrameEncode = NULL;
		IWICMetadataQueryReader *piFrameQReader = NULL;
		IWICMetadataQueryWriter *piFrameQWriter = NULL;

		//Get and create image frame.
		if (SUCCEEDED(hr))
		{
			hr = piDecoder->GetFrame(i, &piFrameDecode);			
		}
		if (SUCCEEDED(hr))
		{
			hr = piEncoder->CreateNewFrame(&piFrameEncode, NULL);
		}

		//Initialize the encoder.
		if (SUCCEEDED(hr))
		{
			hr = piFrameEncode->Initialize(NULL);
		}
		//Get and set size.
		if (SUCCEEDED(hr))
		{
			piFrameDecode->GetSize(&width, &height);
		}
		if (SUCCEEDED(hr))
		{
			// todo, need to transform the bitmap
			AdjustSize(newwidth, newheight, width, height, m_smallerOnly);
			piBitmapScaler->Initialize(static_cast<IWICBitmapSource*> (piFrameDecode), newwidth, newheight, 
				WICBitmapInterpolationModeFant);
			hr = piFrameEncode->SetSize(newwidth, newheight);
		}
		//Get and set resolution.
		if (SUCCEEDED(hr))
		{
			piFrameDecode->GetResolution(&dpiX, &dpiY);
		}
		if (SUCCEEDED(hr))
		{
			hr = piFrameEncode->SetResolution(dpiX, dpiY);
		}
		//Set pixel format.
		if (SUCCEEDED(hr))
		{
			piFrameDecode->GetPixelFormat(&pixelFormat);
		}
		if (SUCCEEDED(hr))
		{
			hr = piFrameEncode->SetPixelFormat(&pixelFormat);
		}

		//Check that the destination format and source formats are 
		// the same.
		bool formatsEqual = FALSE;

		if (SUCCEEDED(hr))
		{
			GUID srcFormat;
			GUID destFormat;

			hr = piDecoder->GetContainerFormat(&srcFormat);
			if (SUCCEEDED(hr))
			{
				hr = piEncoder->GetContainerFormat(&destFormat);
			}
			if (SUCCEEDED(hr))
			{
				if (srcFormat == destFormat)
					formatsEqual = true;
				else
					formatsEqual = false;
			}
		}

		if (SUCCEEDED(hr) && formatsEqual)
		{
			//Copy metadata using metadata block reader/writer.
			if (SUCCEEDED(hr))
			{
				piFrameDecode->QueryInterface(
					IID_IWICMetadataBlockReader,
					(LPVOID*)&piBlockReader);
			}
			if (SUCCEEDED(hr))
			{
				piFrameEncode->QueryInterface(
					IID_IWICMetadataBlockWriter,
					(LPVOID*)&piBlockWriter);
			}
			if (SUCCEEDED(hr))
			{
				piBlockWriter->InitializeFromBlockReader(
					piBlockReader);
			}
		}

		if (SUCCEEDED(hr))
		{
			hr = piFrameEncode->WriteSource(
				static_cast<IWICBitmapSource*> (piBitmapScaler),
				NULL); // Using NULL enables JPEG loss-less encoding.
		}

		//Commit the frame.
		if (SUCCEEDED(hr))
		{
			hr = piFrameEncode->Commit();
		}

		if (piFrameQReader)
			piFrameQReader->Release();
		if (piFrameQWriter)
			piFrameQWriter->Release();
		if (piFrameEncode)
			piFrameEncode->Release();
		if (piFrameDecode)
			piFrameDecode->Release();
	}

	if (SUCCEEDED(hr))
	{
		piEncoder->Commit();
	}

	if (SUCCEEDED(hr))
	{
		piFileStream->Commit(STGC_DEFAULT);
	}
	
	if (piBlockWriter)
		piBlockWriter->Release();	
	if (piEncoder)
		piEncoder->Release();	
	if (piBlockWriter)
		piBlockWriter->Release();
	if (piBlockReader)
		piBlockReader->Release();
	if (piBitmapScaler)
		piBitmapScaler->Release();	
	if (piDecoder)
		piDecoder->Release();
	if (piFileStream)
	{
		piFileStream->Release();
	}
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

// nWidth and nHeight are in/out, in is target size, out is adjusted target size
void ImageHelper::AdjustSize(UINT &nWidth, UINT &nHeight, UINT srcWidth, UINT srcHeight, BOOL fSmallerOnly)
{
	UINT temp;
	BOOL flip = FALSE;
	FLOAT srcAspectRatio; // src aspect ratio
	FLOAT targetAspectRatio; // target aspect ratio
	FLOAT resizeRatio; // ratio to use for resize


	// none of the src dimensions should be zero, only one dest dimension can be zero
	ATLASSERT(nWidth || nHeight);
	ATLASSERT(srcWidth);
	ATLASSERT(srcHeight);
	
	if(srcWidth != 0 && srcHeight != 0)
	{
		if (nHeight == 0)
		{
			nHeight = srcWidth * srcHeight / nWidth;
		}
		else if (nWidth == 0)
		{
			nWidth = srcHeight * srcWidth / nHeight;
		}

		srcAspectRatio = srcWidth / (FLOAT)srcHeight;
		targetAspectRatio = nWidth / (FLOAT)nHeight;

		// if the aspect ratios match, no adjustment to nWidth or nHeight is required, 
		if(srcAspectRatio != targetAspectRatio)
		{
			if(srcAspectRatio < 1)
			{
				// image is taller than wide
				// flip dimensions before calculation adjustment
				flip = TRUE;
				temp = srcWidth;
				srcWidth = srcHeight;
				srcHeight = temp;
				srcAspectRatio = srcWidth / (FLOAT)srcHeight;
			}
			
			if(srcAspectRatio < targetAspectRatio)
			{
				// height is limiting, use height for resize ratio
				resizeRatio = nHeight / (FLOAT)srcHeight;					
			}
			else
			{
				// width is limiting, use width for resize ratio
				resizeRatio = nWidth / (FLOAT)srcWidth;
			}	

			// adjust nWidth and nHeight using resizeRatio from above
			nWidth = UINT(srcWidth * resizeRatio);
			nHeight = UINT(srcHeight * resizeRatio);
		}		

		if(fSmallerOnly && (nWidth > srcWidth || nHeight > srcHeight))
		{
			nWidth = srcWidth;
			nHeight = srcHeight;
		}

		if(flip)
		{
			// if image was taller than wide, flip adjusted dimensions
			temp = nWidth;
			nWidth = nHeight;
			nHeight = temp;
		}
	}			
}

