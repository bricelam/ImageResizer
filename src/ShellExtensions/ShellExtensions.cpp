#include "stdafx.h"
#include "resource.h"
#include "ShellExtensions_i.h"
#include "dllmain.h"

STDAPI DllCanUnloadNow()
{
	return _AtlModule.DllCanUnloadNow();
}

STDAPI DllGetClassObject(_In_ REFCLSID rclsid, _In_ REFIID riid, _Outptr_ LPVOID* ppv)
{
	return _AtlModule.DllGetClassObject(rclsid, riid, ppv);
}

STDAPI DllRegisterServer()
{
	return _AtlModule.DllRegisterServer();
}

STDAPI DllUnregisterServer()
{
	return _AtlModule.DllUnregisterServer();
}

STDAPI DllInstall(BOOL bInstall, _In_opt_ LPCWSTR pszCmdLine)
{
	HRESULT hr = E_FAIL;
	static const wchar_t szUserSwitch[] = L"user";

	if (pszCmdLine != NULL)
	{
		if (_wcsnicmp(pszCmdLine, szUserSwitch, _countof(szUserSwitch)) == 0)
		{
			ATL::AtlSetPerUserRegistration(true);
		}
	}

	if (bInstall)
	{
		hr = DllRegisterServer();

		if (FAILED(hr))
		{
			DllUnregisterServer();
		}
	}
	else
	{
		hr = DllUnregisterServer();
	}

	return hr;
}
