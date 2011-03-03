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

// this class is now just used as a parameter for the thread pool tasks.
// probably should refactor this to a more meaningful name.

#pragma once

using namespace ATL;

class ResizeThread
{
public:
	ResizeThread(CPath pathSource, CPath pathDirectory, LONG * counter, LONG * shutdown);
	~ResizeThread();	
	CPath m_PathSource;
	CPath m_PathDirectory;	
	LONG * m_interlockedCounter;
	LONG * m_interlockedShutdown;
};
