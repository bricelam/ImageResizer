#include "stdafx.h"
#include "Resource.h"
#include "PhotoToysClone_i.h"
#include "dllmain.h"

CPhotoToysCloneModule _AtlModule;

class CPhotoToysCloneApp : public CWinApp
{
public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();
	DECLARE_MESSAGE_MAP()
};

BEGIN_MESSAGE_MAP(CPhotoToysCloneApp, CWinApp)
END_MESSAGE_MAP()

CPhotoToysCloneApp theApp;

BOOL CPhotoToysCloneApp::InitInstance()
{
	return CWinApp::InitInstance();
}

int CPhotoToysCloneApp::ExitInstance()
{
	return CWinApp::ExitInstance();
}
