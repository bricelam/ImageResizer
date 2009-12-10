#pragma once

#include "SettingsHelper.h"

using namespace ATL;
using namespace Gdiplus;

class ImageHelper sealed
{
public:
	ImageHelper();
	~ImageHelper();
	void Resize(const CPath &pathSource, const CPath &pathDirectory, IMAGE_SIZE size, UINT nWidth, UINT nHeight, BOOL fSmallerOnly, BOOL fOverwriteOriginal);

private:
	ULONG_PTR m_token;
	CAtlArray<ImageCodecInfo> m_aEncoders;
	ImageCodecInfo m_defaultEncoder;
	CPath GetDestinationPath(const CPath &pathSource, const CPath &pathDirecotry, IMAGE_SIZE size, BOOL fOverwriteOriginal);
	void AdjustSize(UINT &nWidth, UINT &nHeight, UINT srcWidth, UINT srcHeight, BOOL fSmallerOnly);
	const ImageCodecInfo *GetEncoderForImage(Image *pImage);
};
