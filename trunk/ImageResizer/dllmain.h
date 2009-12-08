using namespace ATL;

class CImageResizerModule :
	public CAtlDllModuleT<CImageResizerModule>
{
public :
	DECLARE_LIBID(LIBID_ImageResizerLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_IMAGERESIZER, "{204306FD-15A9-40AB-A450-3E355EDDDA75}")
};

extern class CImageResizerModule _AtlModule;
