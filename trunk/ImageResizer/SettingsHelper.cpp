#include "StdAfx.h"
#include "SettingsHelper.h"

using namespace ATL;

CString SettingsHelper::GetAppendageForSize(IMAGE_SIZE size)
{
	// TODO: Resourcify.
	switch (size)
	{
	case IMGSZ_SMALL:
		return _T("Small");

	case IMGSZ_MEDIUM:
		return _T("Medium");

	case IMGSZ_LARGE:
		return _T("Large");

	case IMGSZ_MOBILE:
		return _T("Mobile");
	}

	return _T("Custom");;
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

void SettingsHelper::LoadSettings(IMAGE_SIZE *pSize, UINT *pCustomWidth, UINT *pCustomHeight, BOOL *pSmaller, BOOL *pOriginal)
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
	key.QueryValue(_T("Smaller"), &type, pSmaller, &bytes);

	type = REG_DWORD;
	bytes = sizeof(BOOL);
	key.QueryValue(_T("Original"), &type, pOriginal, &bytes);

	key.Close();
}

void SettingsHelper::SaveSettings(IMAGE_SIZE size, UINT nWidth, UINT nHeight, BOOL fSmaller, BOOL fOriginal)
{
	CRegKey key;

	key.Create(HKEY_CURRENT_USER, _T("Software\\Brice Lambson\\Image Resizer"));

	key.SetValue(_T("Size"), REG_DWORD, &size, sizeof(IMAGE_SIZE));
	key.SetValue(_T("Smaller"), REG_DWORD, &fSmaller, sizeof(BOOL));
	key.SetValue(_T("Original"), REG_DWORD, &fOriginal, sizeof(BOOL));
	
	// Only save the last USED custom Width and Height.
	if (size == IMGSZ_CUSTOM)
	{
		key.SetValue(_T("CustomWidth"), REG_DWORD, &nWidth, sizeof(UINT));
		key.SetValue(_T("CustomHeight"), REG_DWORD, &nHeight, sizeof(UINT));
	}

	key.Close();
}
