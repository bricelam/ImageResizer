#include "stdafx.h"
#include "PhotoToysShellExtension.h"
#include "ResizePicturesDialog.h"
#include "PhotoResizer.h"

#define IDM_PHOTORESIZE 0

CPhotoToysShellExtension::CPhotoToysShellExtension()
{
	m_files = NULL;
	m_lpVerbA = "PhotoResize";
	m_lpVerbW = _T("PhotoResize");
}

HRESULT CPhotoToysShellExtension::Initialize(PCIDLIST_ABSOLUTE pidlFolder, IDataObject *pdtobj, HKEY hkeyProgID)
{
	if (pdtobj != NULL)
	{
		STGMEDIUM medium;

		FORMATETC formatetc;
		formatetc.cfFormat = CF_HDROP;
		formatetc.ptd = NULL;
		formatetc.dwAspect = DVASPECT_CONTENT;
		formatetc.lindex = -1;
		formatetc.tymed = TYMED_HGLOBAL;

		pdtobj->GetData(&formatetc, &medium);

		m_numFiles = DragQueryFile((HDROP)medium.hGlobal, 0xFFFFFFFF, NULL, 0);
		m_files = new PTCHAR[m_numFiles];

		for (UINT i = 0; i < m_numFiles; i++)
		{
			m_files[i] = new TCHAR[MAX_PATH];
			DragQueryFile((HDROP)medium.hGlobal, i, m_files[i], MAX_PATH);
		}

		ReleaseStgMedium(&medium);
	}

	return S_OK;
}

HRESULT CPhotoToysShellExtension::QueryContextMenu(HMENU hmenu, UINT indexMenu, UINT idCmdFirst, UINT idCmdLast, UINT uFlags)
{
	if (!(uFlags & CMF_DEFAULTONLY))
	{
		CMenu *pMenu = CMenu::FromHandle(hmenu);

		pMenu->InsertMenu(indexMenu, MF_STRING | MF_BYPOSITION, idCmdFirst + IDM_PHOTORESIZE, _T("Resize Pictures"));

		// UNDONE: "Sometimes the hardest thing and the right thing are the same." --The Fray, "All At Once"
		//delete pMenu;
	}

	return MAKE_HRESULT(SEVERITY_SUCCESS, 0, IDM_PHOTORESIZE + 1);
}

HRESULT CPhotoToysShellExtension::GetCommandString(UINT_PTR idCmd, UINT uFlags, UINT *pwReserved, LPSTR pszName, UINT cchMax)
{
	HRESULT hResult = E_INVALIDARG;

	if (idCmd == 0)
	{
		switch (uFlags)
		{
		case GCS_VERBA:
			hResult = StringCchCopyNA(pszName, 12, m_lpVerbA, cchMax);
			break;

		case GCS_VERBW:
			hResult = StringCchCopyNW((LPWSTR)pszName, 12, m_lpVerbW, cchMax);
			break;

		default:
			hResult = S_OK;
			break;
		}
	}

	return hResult;
}

HRESULT CPhotoToysShellExtension::InvokeCommand(LPCMINVOKECOMMANDINFO pici)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	BOOL fEx = FALSE;
	BOOL fUnicode = FALSE;

	if (pici->cbSize == sizeof(CMINVOKECOMMANDINFOEX))
	{
		fEx = TRUE;

		if (pici->fMask & CMIC_MASK_UNICODE)
		{
			fUnicode = TRUE;
		}
	}

	if (!fUnicode && HIWORD(pici->lpVerb))
	{
		if (StrCmpIA(pici->lpVerb, m_lpVerbA) == 0)
		{
			return OnPhotoResize(pici);
		}
	}
	else if (fUnicode && HIWORD(((CMINVOKECOMMANDINFOEX *)pici)->lpVerbW))
	{
		if (StrCmpIW(((CMINVOKECOMMANDINFOEX *)pici)->lpVerbW, m_lpVerbW) == 0)
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

HRESULT CPhotoToysShellExtension::OnPhotoResize(LPCMINVOKECOMMANDINFO pici)
{	
	CResizePicturesDialog resizePicturesDialog(CWnd::FromHandle(pici->hwnd));

	// TODO: This maybe ought to be executed on a new thread.
	if (resizePicturesDialog.DoModal() == IDOK)
	{
		CPath sourceFile;
		CPath destinationFile;
		UINT width = resizePicturesDialog.GetImageWidth();
		UINT height = resizePicturesDialog.GetImageHeight();
		BOOL smallerOnly = resizePicturesDialog.GetSmallerOnly();

		CPhotoResizer photoResizer;
		
		for (UINT i = 0; i < m_numFiles; i++)
		{
			sourceFile = m_files[i];
			destinationFile = resizePicturesDialog.GetDestinationFile((CPath)sourceFile);
			
			photoResizer.ResizePhoto(sourceFile, destinationFile, width, height, smallerOnly);
		}
	}

	return S_OK;
}