#include "stdafx.h"
#include "SettingsHelper.h"

using namespace ATL;

CString SettingsHelper::GetAppendageForSize(IMAGE_SIZE size)
{
	CString strAppendage;

	switch (size)
	{
	case IMGSZ_SMALL:
		strAppendage.LoadString(IDS_SMALL);
		break;

	case IMGSZ_MEDIUM:
		strAppendage.LoadString(IDS_MEDIUM);
		break;

	case IMGSZ_LARGE:
		strAppendage.LoadString(IDS_LARGE);
		break;

	case IMGSZ_MOBILE:
		strAppendage.LoadString(IDS_MOBILE);
		break;

	default:
		strAppendage.LoadString(IDS_CUSTOM);
		break;
	}

	return strAppendage;
}

void SettingsHelper::GetDimmensionsForSize(IMAGE_SIZE size, UINT &nWidth, UINT &nHeight)
{
	switch (size)
	{
	case IMGSZ_SMALL:
		nWidth = 854;
		nHeight = 480;
		break;

	case IMGSZ_MEDIUM:
		nWidth = 1366;
		nHeight = 768;
		break;

	case IMGSZ_LARGE:
		nWidth = 1920;
		nHeight = 1080;
		break;

	case IMGSZ_MOBILE:
		nWidth = 320;
		nHeight = 480;
		break;
	}
}

void SettingsHelper::LoadSettings(IMAGE_SIZE *pSize, UINT *pCustomWidth, UINT *pCustomHeight, BOOL *pSmallerOnly, BOOL *pOverwriteOriginal)
{
	CRegKey key;
	DWORD type;
	ULONG bytes;

	key.Create(HKEY_CURRENT_USER, _T("Software\\Brice Lambson\\Image Resizer"));

	type = REG_DWORD;
	bytes = sizeof(IMAGE_SIZE);
	key.QueryValue(_T("Size"), &type, pSize, &bytes);

	type = REG_DWORD;
	bytes = sizeof(UINT);
	key.QueryValue(_T("CustomWidth"), &type, pCustomWidth, &bytes);

	type = REG_DWORD;
	bytes = sizeof(UINT);
	key.QueryValue(_T("CustomHeight"), &type, pCustomHeight, &bytes);

	type = REG_DWORD;
	bytes = sizeof(BOOL);
	key.QueryValue(_T("SmallerOnly"), &type, pSmallerOnly, &bytes);

	type = REG_DWORD;
	bytes = sizeof(BOOL);
	key.QueryValue(_T("OverwriteOriginal"), &type, pOverwriteOriginal, &bytes);

	key.Close();
}

void SettingsHelper::SaveSettings(IMAGE_SIZE size, UINT nWidth, UINT nHeight, BOOL fSmallerOnly, BOOL fOverwriteOriginal)
{
	CRegKey key;

	key.Create(HKEY_CURRENT_USER, _T("Software\\Brice Lambson\\Image Resizer"));

	key.SetValue(_T("Size"), REG_DWORD, &size, sizeof(IMAGE_SIZE));
	key.SetValue(_T("SmallerOnly"), REG_DWORD, &fSmallerOnly, sizeof(BOOL));
	key.SetValue(_T("OverwriteOriginal"), REG_DWORD, &fOverwriteOriginal, sizeof(BOOL));
	
	// Only save the last USED custom Width and Height.
	if (size == IMGSZ_CUSTOM)
	{
		key.SetValue(_T("CustomWidth"), REG_DWORD, &nWidth, sizeof(UINT));
		key.SetValue(_T("CustomHeight"), REG_DWORD, &nHeight, sizeof(UINT));
	}

	key.Close();
}
