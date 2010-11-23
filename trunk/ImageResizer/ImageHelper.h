#pragma once

#include "SettingsHelper.h"
#include "Wincodec.h"
#include "Wincodecsdk.h"

using namespace ATL;

class ImageHelperParam
{
public:
	IMAGE_SIZE m_size;
	UINT m_width;
	UINT m_height;
	BOOL m_smallerOnly;
	BOOL m_overwriteOriginal;
};

class ImageHelper sealed
{
public:
	ImageHelper(IMAGE_SIZE size, UINT nWidth, UINT nHeight, BOOL fSmallerOnly, BOOL fOverwriteOriginal);
	~ImageHelper();

	void Resize(const CPath &pathSource, const CPath &pathDirectory);

	IWICImagingFactory *piFactory;

private:	

	CPath GetDestinationPath(const CPath &pathSource, const CPath &pathDirecotry, IMAGE_SIZE size, BOOL fOverwriteOriginal);
	void AdjustSize(UINT &nWidth, UINT &nHeight, UINT srcWidth, UINT srcHeight, BOOL fSmallerOnly);

	IMAGE_SIZE m_size;
	UINT m_width;
	UINT m_height;
	BOOL m_smallerOnly;
	BOOL m_overwriteOriginal;

};
