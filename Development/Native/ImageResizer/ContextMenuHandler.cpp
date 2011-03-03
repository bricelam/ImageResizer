/*******************************************************************************
 * Copyright (c) 2008, 2010 Brice Lambson and others. All rights reserved.
 *
 * This program and the accompanying materials are made available under the
 * terms of the Eclipse Public License v1.0 which accompanies this distribution,
 * and is available at http://www.eclipse.org/legal/epl-v10.html
 *
 * Contributors:
 *    Brice Lambson - initial API and implementation
 *    Thomas Bluemel - progress dialog
 ******************************************************************************/

#include "stdafx.h"
#include "ContextMenuHandler.h"
#include "PhotoResizeDlg.h"
#include "ImageHelper.h"
#include "ResizeThread.h"
#include "Worker.h"

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
		CString strResizeItem;
		strResizeItem.LoadString(IDS_RESIZEITEM);
		
		InsertMenu(hmenu, indexMenu, MF_STRING | MF_BYPOSITION, idCmdFirst + IDM_PHOTORESIZE, strResizeItem);
	}

	return MAKE_HRESULT(SEVERITY_SUCCESS, 0, IDM_PHOTORESIZE + 1);
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


DWORD WINAPI DlgThread(LPVOID lpParameter ); 

DWORD WINAPI DlgThread(LPVOID lpParameter)
{
	UINT i;
    int ret = 0;
	CPhotoResizeDlg waitDlg;
	CThreadInfo * threadInfo = (CThreadInfo *)lpParameter;
	CAtlArray<ResizeThread *> taskArray;

	::CoInitialize(NULL);

    if(IDOK == waitDlg.DoModal(threadInfo->m_hwnd))
	{
		CPath pathSource;
		CPath pathDirectory;
		CString strTitle, strCancel;
		size_t numFiles = threadInfo->m_aFiles.GetCount();
		CThreadPool<CWorker> m_threadPool;
		LONG interlockedCounter = 0;
		LONG interlockedShutdown = 0;
		ResizeThread *resizeThread;
		ImageHelperParam * pImageHelperParam;
		SYSTEM_INFO sysinfo;


		// create the progress dialog
		CComPtr<IProgressDialog> pProgressDialog;		
		pProgressDialog.CoCreateInstance(CLSID_ProgressDialog);			

		if (pProgressDialog != NULL)
		{
			strTitle.LoadString(IDS_RESIZING);
			strCancel.LoadString(IDS_CANCELING);

			pProgressDialog->SetTitle(strTitle.GetBuffer());
			// 0x020 is PROGDLG_MARQUEEPROGRESS, vc2008 couldn't find the definition
			// prog dlg doesn't list file names anymore since work is overlapped.
			pProgressDialog->StartProgressDialog(threadInfo->m_hwnd, NULL, 0x00000020, NULL);
			
			pProgressDialog->SetCancelMsg(strCancel.GetBuffer(), NULL);			
			pProgressDialog->SetLine(1, strTitle.GetBuffer(), FALSE, NULL);						
		}

		pImageHelperParam = new ImageHelperParam();
		pImageHelperParam->m_size = waitDlg.GetSize();
		pImageHelperParam->m_width = waitDlg.GetWidth();
		pImageHelperParam->m_height = waitDlg.GetHeight();
		pImageHelperParam->m_smallerOnly = waitDlg.IsSmallerOnly();
		pImageHelperParam->m_overwriteOriginal = waitDlg.IsOverwriteOriginal();
	
		// get number of cores/cpus on system. 
		// max number of threads is either number of cores or number of files
		GetSystemInfo(&sysinfo);		
		if(numFiles > sysinfo.dwNumberOfProcessors)
		{
			m_threadPool.Initialize((void *)pImageHelperParam, sysinfo.dwNumberOfProcessors);			
		}
		else
		{
			m_threadPool.Initialize((void *)pImageHelperParam, numFiles);
		}	

		// queue tasks to thread pool
		for (i = 0; i < numFiles; i++)
		{		
			pathSource = threadInfo->m_aFiles[i];

			if (CString(threadInfo->m_pathFolder).IsEmpty())
			{
				pathDirectory = pathSource;
				pathDirectory.RemoveFileSpec();
			}	
													
			resizeThread = new ResizeThread(pathSource, pathDirectory, &interlockedCounter, &interlockedShutdown);
			taskArray.Add(resizeThread);
			
			m_threadPool.QueueRequest((CWorker::RequestType)resizeThread);
			
			// increment counter for each task
			InterlockedIncrement(&interlockedCounter);
		}

		// worker threads decrement counter for each task they complete,
		// when the counter reaches zero, we're done.
		while(interlockedCounter > 0)
		{
			if(pProgressDialog != NULL && pProgressDialog->HasUserCancelled())
			{	
				// will cause worker thread to skip the actual resize tasks and just
				// mark themselves as completed.
				InterlockedExchange(&interlockedShutdown, 1);
				// wait for task count to go to zero
				while(interlockedCounter > 0)
				{
					Sleep(50);
				}
				m_threadPool.Shutdown(5 * 1000); // give it 5 seconds to shutdown, but all tasks should be done by now		
				// bug in cthreadpool, will throw an assertion in debug mode if tasks still exist after the timeout
				// http://groups.google.com/group/microsoft.public.vc.atl/browse_thread/thread/c3d77f272fde2816/93b94461766ded0f?lnk=st&amp;q=CThreadPool&amp;rnum=1#93b94461766ded0f
				// so, we don't want any current tasks when the shutdown times out
				// otherwise we could have resource/handle leaks
				// really should implement a thread pool that shuts down properly
				break;
			}
			Sleep(100); // wait a bit
		}				
		
		if (pProgressDialog != NULL)
		{
			pProgressDialog->StopProgressDialog();
			pProgressDialog.Release();
		}

		if(NULL != pImageHelperParam)
		{
			delete pImageHelperParam;
		}

		// delete tasks
		for(i = 0; i < taskArray.GetCount(); i++)
		{
			resizeThread = taskArray[i];
			if(NULL != resizeThread)
			{
				delete resizeThread;
			}
		}
		taskArray.RemoveAll();

		m_threadPool.Release();
	}

	if(NULL != threadInfo)
	{
		delete threadInfo;
		threadInfo = NULL;
	}

	::CoUninitialize();

    return ret;
}

HRESULT CContextMenuHandler::OnPhotoResize(LPCMINVOKECOMMANDINFO pici)
{
	DWORD threadId;	
	CThreadInfo * threadInfo;
	
	// params to thread
	threadInfo = new CThreadInfo();
	threadInfo->m_hwnd = pici->hwnd;
	threadInfo->m_aFiles.Copy(m_aFiles);
	threadInfo->m_pathFolder = m_pathFolder;

	// start thread for dialog so we don't block the ui.
	::CreateThread(NULL, 0, &DlgThread, (LPVOID)threadInfo, 0, &threadId);

	return S_OK;
}
