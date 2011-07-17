// ContextMenuHandler.cpp : Implementation of CContextMenuHandler

#include "stdafx.h"
#include "ContextMenuHandler.h"
#include "HDropIterator.h"

CContextMenuHandler::CContextMenuHandler()
{
	m_pidlFolder = NULL;
	m_pdtobj = NULL;
}

CContextMenuHandler::~CContextMenuHandler()
{
	Uninitialize();
}

void CContextMenuHandler::Uninitialize()
{
	CoTaskMemFree((LPVOID)m_pidlFolder);
	m_pidlFolder = NULL;

	if (m_pdtobj)
	{ 
		m_pdtobj->Release(); 
	}
}

HRESULT CContextMenuHandler::Initialize(PCIDLIST_ABSOLUTE pidlFolder, IDataObject *pdtobj, HKEY hkeyProgID)
{
	Uninitialize();

	if (pidlFolder)
	{
		m_pidlFolder = ILClone(pidlFolder);
	}

	if (pdtobj) 
	{ 
		m_pdtobj = pdtobj; 
		m_pdtobj->AddRef(); 
	}

	return S_OK;
}

HRESULT CContextMenuHandler::QueryContextMenu(HMENU hmenu, UINT indexMenu, UINT idCmdFirst, UINT idCmdLast, UINT uFlags)
{
	if (uFlags & CMF_DEFAULTONLY)
	{
		return MAKE_HRESULT(SEVERITY_SUCCESS, 0, 0);
	}

	INT idCmdMax = -1;
	
	HDropIterator i(m_pdtobj);
	i.First();

	PERCEIVED type;
	PERCEIVEDFLAG flag;
	LPTSTR pszPath = i.CurrentItem();
	LPTSTR pszExt = PathFindExtension(pszPath);

	AssocGetPerceivedType(pszExt, &type, &flag, NULL);

	free(pszPath);

	// If selected file is an image...
	if (type == PERCEIVED_TYPE_IMAGE)
	{		
		CString strResizePictures;

		// If handling drag-and-drop...
		if (m_pidlFolder)
		{			
			// Load 'Resize pictures here' string
			strResizePictures.LoadString(IDS_RESIZE_PICTURES_HERE);
		}
		else
		{
			// Load 'Resize pictures' string
			strResizePictures.LoadString(IDS_RESIZE_PICTURES);
		}

		// Add menu item
		InsertMenu(hmenu, indexMenu, MF_BYPOSITION, idCmdFirst + ID_RESIZE_PICTURES, strResizePictures);
		idCmdMax = ID_RESIZE_PICTURES;
	}	

	return MAKE_HRESULT(SEVERITY_SUCCESS, 0, idCmdMax + 1);
}

HRESULT CContextMenuHandler::GetCommandString(UINT_PTR idCmd, UINT uType, UINT *pReserved, LPSTR pszName, UINT cchMax)
{
	if (idCmd == ID_RESIZE_PICTURES)
	{
		if (uType == GCS_VERBW)
		{
			 wcscpy_s((LPWSTR)pszName, cchMax, RESIZE_PICTURES_VERBW);
		}
	}
	else
	{
		return E_INVALIDARG;
	}

	return S_OK;
}

HRESULT CContextMenuHandler::InvokeCommand(CMINVOKECOMMANDINFO *pici)
{
	if (HIWORD(pici->lpVerb))
	{
		if (pici->fMask & CMIC_MASK_UNICODE &&
			pici->cbSize == sizeof(CMINVOKECOMMANDINFOEX) &&
			wcscmp(((CMINVOKECOMMANDINFOEX *)pici)->lpVerbW, RESIZE_PICTURES_VERBW) == 0)
		{
			// TODO: CreateThread?
			return ResizePictures(pici);
		}
	}
	else if (LOWORD(pici->lpVerb) == ID_RESIZE_PICTURES)
	{
		return ResizePictures(pici);
	}

	return E_INVALIDARG;
}

// TODO: Error handling and memory management
HRESULT CContextMenuHandler::ResizePictures(CMINVOKECOMMANDINFO *pici)
{
	// Set the application path from the registry
	LPTSTR lpApplicationName = new TCHAR[MAX_PATH];
	ULONG nChars = MAX_PATH;

	// NOTE: The location is always read from the 32-bit key
	CRegKey regKey;
	regKey.Open(HKEY_LOCAL_MACHINE, _T("Software\\BriceLambson\\ImageResizer"), KEY_READ | KEY_WOW64_32KEY);
	regKey.QueryStringValue(NULL, lpApplicationName, &nChars);
	regKey.Close();

	CString commandLine;
	commandLine.Format(_T("\"%s\""), lpApplicationName);

	// Set the output directory
	if (m_pidlFolder)
	{
		TCHAR szFolder[MAX_PATH];
		SHGetPathFromIDList(m_pidlFolder, szFolder);

		commandLine.AppendFormat(_T(" /d \"%s\""), szFolder);
	}

	// Set the input files
	HDropIterator i(m_pdtobj);

	for (i.First(); !i.IsDone(); i.Next())
	{
		// TODO: Since command line arguments are limited, send using an anonymous pipe
		commandLine.AppendFormat(_T(" \"%s\""), i.CurrentItem());
	}

	LPTSTR lpszCommandLine = new TCHAR[commandLine.GetLength() + 1];
	_tcscpy(lpszCommandLine, commandLine);

	STARTUPINFO startupInfo;
	PROCESS_INFORMATION processInformation;

	ZeroMemory(&startupInfo, sizeof(STARTUPINFO));
	startupInfo.cb = sizeof(STARTUPINFO);
	startupInfo.dwFlags = STARTF_USESHOWWINDOW;
	startupInfo.wShowWindow = pici->nShow;

	// Start the resizer
	CreateProcess(NULL,
		lpszCommandLine,
		NULL,
		NULL,
		TRUE,
		0,
		NULL,
		NULL,
		&startupInfo,
		&processInformation);

	// Cleanup
	CloseHandle(processInformation.hProcess);
	CloseHandle(processInformation.hThread);
	delete[] lpszCommandLine;
	delete[] lpApplicationName;

	return S_OK;
}
