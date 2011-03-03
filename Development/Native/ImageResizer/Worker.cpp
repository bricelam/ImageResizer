#include "StdAfx.h"
#include "Worker.h"
#include "ResizeThread.h"


BOOL CWorker::Initialize(void * pvWorkerParam)
{
	ImageHelperParam * pIHP;

	//Initialize COM.
	CoInitialize(NULL);	

	// get the params for the image helper class
	pIHP = (ImageHelperParam *)pvWorkerParam;

	m_imageHelper = new ImageHelper(pIHP->m_size, pIHP->m_width, pIHP->m_height, 
		pIHP->m_smallerOnly, pIHP->m_overwriteOriginal);

	if(NULL == m_imageHelper)
	{
		return FALSE;
	}
	else
	{
		m_imageHelper->piFactory = NULL;

		// Create the COM imaging factory.
		HRESULT hr = CoCreateInstance(
			CLSID_WICImagingFactory,
			NULL,
			CLSCTX_INPROC_SERVER,
			IID_IWICImagingFactory,
			(LPVOID*)&m_imageHelper->piFactory);
	
		if (SUCCEEDED(hr))
		{
			return TRUE;
		}
		else
		{
			return FALSE;
		}
	}
}

void CWorker::Execute(RequestType request, void * pvWorkerParam, 
					  OVERLAPPED* pOverlapped)
{
	// get params for this task
	ResizeThread * resizeThread = (ResizeThread *)(DWORD_PTR)request;

	// do the resize, unless we're signaled to exit, then just
	// get through the tasks as fast as possible
	if(0 == *(resizeThread->m_interlockedShutdown))
	{
		m_imageHelper->Resize(resizeThread->m_PathSource, resizeThread->m_PathDirectory);	
	}

	// decrement task counter
	InterlockedDecrement(resizeThread->m_interlockedCounter);
}

void CWorker::Terminate(void * pvWorkerParam)
{
	delete m_imageHelper;

	//Uninitialize COM.
	CoUninitialize();
}
