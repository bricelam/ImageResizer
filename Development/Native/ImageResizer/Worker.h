#pragma once
#include "ImageHelper.h"

class CWorker
{
public:
	typedef DWORD_PTR RequestType;

	BOOL Initialize(void * pvWorkerParam);

	void Execute(
			RequestType request,
			void * pvWorkerParam,
			OVERLAPPED* pOverlapped
		);

	void Terminate(void * pvWorkerParam);

private:
	ImageHelper * m_imageHelper;

};
