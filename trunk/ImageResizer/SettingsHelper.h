#pragma once

using namespace ATL;

enum IMAGE_SIZE
{
	IMGSZ_SMALL,
	IMGSZ_MEDIUM,
	IMGSZ_LARGE,
	IMGSZ_MOBILE,
	IMGSZ_CUSTOM
};

class SettingsHelper
{
public:
	static CString GetAppendageForSize(IMAGE_SIZE size);
	static void GetDimmensionsForSize(IMAGE_SIZE size, UINT &nWidth, UINT &nHeight);
	static void LoadSettings(IMAGE_SIZE *pSize, UINT *pCustomWidth, UINT *pCustomHeight, BOOL *pSmallerOnly, BOOL *pOverwriteOriginal);
	static void SaveSettings(IMAGE_SIZE size, UINT nCustomWidth, UINT nCustomHeight, BOOL fSmallerOnly, BOOL fOverwriteOriginal);
};
