#pragma once

#include "SettingsHelper.h"

using namespace ATL;
using namespace Gdiplus;

class ImageHelper sealed
{
public:
	ImageHelper();
	~ImageHelper();
	void Resize(const CPath &pathSource, const CPath &pathDirectory, IMAGE_SIZE size, UINT nWidth, UINT nHeight, BOOL fSmaller, BOOL fOriginal);

private:
	ULONG_PTR m_token;
	CAtlArray<ImageCodecInfo> m_aEncoders;
	ImageCodecInfo m_defaultEncoder;
	CPath GetDestinationPath(const CPath &pathSource, const CPath &pathDirecotry, IMAGE_SIZE size, BOOL fOriginal);
	void AdjustSize(UINT &nWidth, UINT &nHeight, UINT srcWidth, UINT srcHeight, BOOL fSmaller);
	const ImageCodecInfo *GetEncoderForImage(Image *pImage);
};
