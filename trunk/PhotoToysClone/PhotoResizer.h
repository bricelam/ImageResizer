#pragma once

using namespace Gdiplus;

class CPhotoResizer sealed
{
public:
	CPhotoResizer();
	~CPhotoResizer();
	void ResizePhoto(CString srcFilename, CString dstFilename, UINT width, UINT height, BOOL smallerOnly);

private:
	ULONG_PTR m_token;
	UINT m_numEncoders;
	ImageCodecInfo *m_encoders;
	void GetImageEncoderClsid(Image *image, CLSID *clsidEncoder);
};
