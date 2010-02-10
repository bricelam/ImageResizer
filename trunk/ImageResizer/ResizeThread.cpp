/*******************************************************************************
 * Copyright (c) 2010 Thomas Bluemel. All rights reserved.
 *
 * This program and the accompanying materials are made available under the
 * terms of the Eclipse Public License v1.0 which accompanies this distribution,
 * and is available at http://www.eclipse.org/legal/epl-v10.html
 *
 * Contributors:
 *    Thomas Bluemel - initial API and implementation
 ******************************************************************************/

#include "stdafx.h"
#include "ResizeThread.h"

ResizeThread::ResizeThread(ImageHelper *pImageHelper, CPath &pathSource, 
						   CPath &pathDirectory, IMAGE_SIZE size, UINT width, 
						   UINT height, BOOL smallerOnly, BOOL overwriteOriginal)
	: m_PathSource(pathSource), m_PathDirectory(pathDirectory)
{
	m_Size = size;
	m_Width = width;
	m_Height = height;
	m_SmallerOnly = smallerOnly;
	m_OverwriteOriginal = overwriteOriginal;
	m_pImageHelper = pImageHelper;

	m_Thread = NULL;
}

ResizeThread::~ResizeThread()
{
	if (m_Thread != NULL)
		CloseHandle (m_Thread);
}

DWORD ResizeThread::ThreadStart(LPVOID lpParameter)
{
	ResizeThread *pResize = (ResizeThread *)lpParameter;
	pResize->Resize();
	return 0;
}

void ResizeThread::Resize()
{
	m_pImageHelper->Resize(m_PathSource, m_PathDirectory, m_Size, 
		m_Width, m_Height, m_SmallerOnly, m_OverwriteOriginal);
}

bool ResizeThread::Run()
{
	DWORD dwThreadId;

	if (m_Thread != NULL)
		CloseHandle (m_Thread);

	m_Thread = CreateThread(NULL, 0, ThreadStart, (LPVOID)this, 0, &dwThreadId);
	if (m_Thread != NULL)
		return true;

	return false;
}

void ResizeThread::Wait()
{
	MSG msg;
	while (true)
	{
		switch (MsgWaitForMultipleObjectsEx(1, &m_Thread, INFINITE, QS_ALLINPUT, MWMO_INPUTAVAILABLE))
		{
		case WAIT_OBJECT_0: // Thread terminated
			CloseHandle(m_Thread);
			m_Thread = NULL;
			return;
		case WAIT_OBJECT_0 + 1: // Process message
			while (PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
			{
				TranslateMessage(&msg);
				DispatchMessage(&msg);
			}
			break;
		default:
			return;
		}
	}
}
