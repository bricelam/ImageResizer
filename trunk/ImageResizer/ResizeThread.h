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

#pragma once

#include "ImageHelper.h"

using namespace ATL;

class ResizeThread sealed
{
private:
	CPath &m_PathSource;
	CPath &m_PathDirectory;
	IMAGE_SIZE m_Size;
	UINT m_Width;
	UINT m_Height;
	BOOL m_SmallerOnly;
	BOOL m_OverwriteOriginal;
	ImageHelper *m_pImageHelper;
	HANDLE m_Thread;

	static DWORD WINAPI ThreadStart(LPVOID lpParameter);
	void Resize();

public:
	ResizeThread(ImageHelper *pImageHelper, CPath &pathSource, 
		CPath &pathDirectory, IMAGE_SIZE size, UINT width, UINT height, 
		BOOL smallerOnly, BOOL overwriteOriginal);
	~ResizeThread();

	bool Run();
	void Wait();
};
