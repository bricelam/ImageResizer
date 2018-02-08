#pragma once

#define ID_RESIZE_PICTURES 0
#define RESIZE_PICTURES_VERBW L"resize"

#include "resource.h"
#include "ShellExtensions_i.h"

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;

class ATL_NO_VTABLE CContextMenuHandler :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CContextMenuHandler, &CLSID_ContextMenuHandler>,
	public IShellExtInit,
	public IContextMenu
{
	BEGIN_COM_MAP(CContextMenuHandler)
		COM_INTERFACE_ENTRY(IShellExtInit)
		COM_INTERFACE_ENTRY(IContextMenu)
	END_COM_MAP()
	DECLARE_REGISTRY_RESOURCEID(IDR_CONTEXTMENUHANDLER)
	DECLARE_NOT_AGGREGATABLE(CContextMenuHandler)

public:
	CContextMenuHandler();
	~CContextMenuHandler();
	HRESULT STDMETHODCALLTYPE Initialize(_In_opt_ PCIDLIST_ABSOLUTE pidlFolder, _In_opt_ IDataObject *pdtobj, _In_opt_ HKEY hkeyProgID);
	HRESULT STDMETHODCALLTYPE QueryContextMenu(_In_ HMENU hmenu, UINT indexMenu, UINT idCmdFirst, UINT idCmdLast, UINT uFlags);
	HRESULT STDMETHODCALLTYPE GetCommandString(UINT_PTR idCmd, UINT uType, _In_ UINT *pReserved, LPSTR pszName, UINT cchMax);
	HRESULT STDMETHODCALLTYPE InvokeCommand(_In_ CMINVOKECOMMANDINFO *pici);

private:
	void Uninitialize();
	HRESULT ResizePictures(CMINVOKECOMMANDINFO *pici);
	PCIDLIST_ABSOLUTE m_pidlFolder;
	IDataObject *m_pdtobj;
};

OBJECT_ENTRY_AUTO(__uuidof(ContextMenuHandler), CContextMenuHandler)
