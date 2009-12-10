#include "stdafx.h"
#include "ContextMenuHandler.h"
#include "PhotoResizeDlg.h"
#include "ImageHelper.h"

#define IDM_PHOTORESIZE 0
#define VERB_PHOTORESIZE _T("PhotoResize")

HRESULT CContextMenuHandler::Initialize(PCIDLIST_ABSOLUTE pidlFolder, IDataObject *pdtobj, HKEY hkeyProgID)
{
	// ATL-ify parameters.
	CString strFolder;

	SHGetPathFromIDList(pidlFolder, strFolder.GetBuffer(MAX_PATH));
	strFolder.ReleaseBuffer();

	// Set drop folder.
	m_pathFolder.m_strPath = strFolder;

	// Get selected files.
	STGMEDIUM medium;
	FORMATETC formatetc;
	
	formatetc.cfFormat = CF_HDROP;
	formatetc.ptd = NULL;
	formatetc.dwAspect = DVASPECT_CONTENT;
	formatetc.lindex = -1;
	formatetc.tymed = TYMED_HGLOBAL;

	pdtobj->GetData(&formatetc, &medium);

	UINT numFiles = DragQueryFile((HDROP)medium.hGlobal, 0xFFFFFFFF, NULL, 0);

	for (UINT i = 0; i < numFiles; i++)
	{
		CString strFile;

		DragQueryFile((HDROP)medium.hGlobal, i, strFile.GetBuffer(MAX_PATH), MAX_PATH);
		strFile.ReleaseBuffer();

		m_aFiles.Add(CPath(strFile));
	}

	// Cleanup.
	ReleaseStgMedium(&medium);

	return S_OK;
}

HRESULT CContextMenuHandler::QueryContextMenu(HMENU hmenu, UINT indexMenu, UINT idCmdFirst, UINT idCmdLast, UINT uFlags)
{
	if (!(uFlags & CMF_DEFAULTONLY))
	{
		InsertMenu(hmenu, indexMenu, MF_STRING | MF_BYPOSITION, idCmdFirst + IDM_PHOTORESIZE, _T("Resize Pictures"));
	}

	return MAKE_HRESULT(SEVERITY_SUCCESS, 0, idCmdFirst + IDM_PHOTORESIZE + 1);
}

HRESULT CContextMenuHandler::GetCommandString(UINT_PTR idCmd, UINT uFlags, UINT *pwReserved, LPSTR pszName, UINT cchMax)
{
	if (uFlags == GCS_VERBA)
	{
		if (idCmd == IDM_PHOTORESIZE)
		{
			CStringA::CopyChars(pszName, cchMax, CT2CA(VERB_PHOTORESIZE), CString(VERB_PHOTORESIZE).GetLength());
			
			return S_OK;
		}
	}
	else if (uFlags == GCS_VERBW)
	{
		if (idCmd == IDM_PHOTORESIZE)
		{
			CStringW::CopyChars((LPWSTR)pszName, cchMax, CT2CW(VERB_PHOTORESIZE), CString(VERB_PHOTORESIZE).GetLength());

			return S_OK;
		}
	}

	return E_INVALIDARG;
}

HRESULT CContextMenuHandler::InvokeCommand(LPCMINVOKECOMMANDINFO pici)
{
	BOOL fUnicode = pici->cbSize == sizeof(CMINVOKECOMMANDINFOEX) &&
					pici->fMask & CMIC_MASK_UNICODE;

	if (!fUnicode && HIWORD(pici->lpVerb))
	{
		if (CString(pici->lpVerb).CompareNoCase(VERB_PHOTORESIZE) == 0)
		{
			return OnPhotoResize(pici);
		}
	}
	else if (fUnicode && HIWORD(((LPCMINVOKECOMMANDINFOEX)pici)->lpVerbW))
	{
		if (CString(((LPCMINVOKECOMMANDINFOEX)pici)->lpVerbW).CompareNoCase(VERB_PHOTORESIZE) == 0)
		{
			return OnPhotoResize(pici);
		}
	}
	else
	{
		if (LOWORD(pici->lpVerb) == IDM_PHOTORESIZE)
		{
			return OnPhotoResize(pici);
		}
	}
		
	return E_FAIL;
}

HRESULT CContextMenuHandler::OnPhotoResize(LPCMINVOKECOMMANDINFO pici)
{
	CPhotoResizeDlg dlgPhotoResize;

	// TODO: Don't block.
	if (dlgPhotoResize.DoModal(pici->hwnd) == IDOK)
	{
		CPath pathSource;
		ImageHelper imageHelper;
		size_t numFiles = m_aFiles.GetCount();
		CPath pathDirectory;

		IMAGE_SIZE size = dlgPhotoResize.GetSize();
		UINT nWidth = dlgPhotoResize.GetWidth();
		UINT nHeight = dlgPhotoResize.GetHeight();
		BOOL fSmallerOnly = dlgPhotoResize.IsSmallerOnly();
		BOOL fOverwriteOriginal = dlgPhotoResize.IsOverwriteOriginal();

		if (!CString(m_pathFolder).IsEmpty())
		{
			pathDirectory = m_pathFolder;
		}

		// TODO: Multi-thread.
		for (UINT i = 0; i < numFiles; i++)
		{
			pathSource = m_aFiles[i];
			
			if (CString(m_pathFolder).IsEmpty())
			{
				pathDirectory = pathSource;
				pathDirectory.RemoveFileSpec();
			}

			imageHelper.Resize(pathSource, pathDirectory, size, nWidth, nHeight, fSmallerOnly, fOverwriteOriginal);
		}
	}

	return S_OK;
}
