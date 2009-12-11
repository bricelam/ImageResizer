#include "stdafx.h"
#include "ImageResizer_i.h"
#include "dllmain.h"

CImageResizerModule _AtlModule;

extern "C" BOOL WINAPI DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{
	hInstance;

	return _AtlModule.DllMain(dwReason, lpReserved); 
}
