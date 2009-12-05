#include "stdafx.h"
#include "Resource.h"
#include "PhotoToysClone_i.h"
#include "dllmain.h"

STDAPI DllCanUnloadNow()
{
	if (_AtlModule.GetLockCount() != 0)
	{
		return S_FALSE;
	}

	return S_OK;
}

STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID *ppv)
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
	
STDAPI DllInstall(BOOL bInstall, LPCWSTR pszCmdLine)
{
    HRESULT hr = E_FAIL;
    static const wchar_t szUserSwitch[] = _T("user");

    if (pszCmdLine != NULL)
    {
    	if (_wcsnicmp(pszCmdLine, szUserSwitch, _countof(szUserSwitch)) == 0)
    	{
    		AtlSetPerUserRegistration(true);
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
