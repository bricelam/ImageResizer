#pragma once

#include "Resource.h"
#include "PhotoToysClone_i.h"

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

class ATL_NO_VTABLE CPhotoToysShellExtension :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CPhotoToysShellExtension, &CLSID_PhotoToysShellExtension>,
	public IShellExtInit,
	public IContextMenu
{
public:
	CPhotoToysShellExtension();
	BEGIN_COM_MAP(CPhotoToysShellExtension)
		COM_INTERFACE_ENTRY(IShellExtInit)
		COM_INTERFACE_ENTRY(IContextMenu)
	END_COM_MAP()
	DECLARE_NOT_AGGREGATABLE(CPhotoToysShellExtension)
	DECLARE_REGISTRY_RESOURCEID(IDR_PHOTOTOYSSHELLEXTENSION)
	HRESULT STDMETHODCALLTYPE Initialize(PCIDLIST_ABSOLUTE pidlFolder, IDataObject *pdtobj, HKEY hkeyProgID);
	HRESULT STDMETHODCALLTYPE GetCommandString(UINT_PTR idCmd, UINT uFlags, UINT *pwReserved, LPSTR pszName, UINT cchMax);
	HRESULT STDMETHODCALLTYPE InvokeCommand(LPCMINVOKECOMMANDINFO pici);
	HRESULT STDMETHODCALLTYPE QueryContextMenu(HMENU hmenu, UINT indexMenu, UINT idCmdFirst, UINT idCmdLast, UINT uFlags);

private:
	UINT m_numFiles;
	PTCHAR *m_files;
	LPCSTR m_lpVerbA;
	LPCWSTR m_lpVerbW;
	HRESULT OnPhotoResize(LPCMINVOKECOMMANDINFO pici);
};

OBJECT_ENTRY_AUTO(__uuidof(PhotoToysShellExtension), CPhotoToysShellExtension)
