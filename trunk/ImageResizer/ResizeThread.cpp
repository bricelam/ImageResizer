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

ResizeThread::ResizeThread(CPath pathSource, CPath pathDirectory, LONG * counter, LONG * shutdown)  
{		
	m_PathSource = pathSource;
	m_PathDirectory = pathDirectory;
	m_interlockedCounter = counter;
	m_interlockedShutdown = shutdown;
}
  
ResizeThread::~ResizeThread()
{
	
}
