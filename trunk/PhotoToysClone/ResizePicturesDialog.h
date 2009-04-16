#pragma once

#include "Resource.h"

class CResizePicturesDialog sealed : public CDialogImpl<CResizePicturesDialog>
{
public:
	enum { IDD = IDD_RESIZEPICTURES };
	CResizePicturesDialog();
	UINT GetImageWidth();
	UINT GetImageHeight();
	BOOL GetSmallerOnly();
	CPath GetDestinationFile(CPath sourceFile);
	BEGIN_MSG_MAP(CResizePicturesDialog)
		MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
		COMMAND_HANDLER(IDOK, BN_CLICKED, OnOK)
		COMMAND_HANDLER(IDCANCEL, BN_CLICKED, OnCancel)
		COMMAND_HANDLER(IDC_ADVANCED, BN_CLICKED, OnBnClickedAdvancedButton)
		COMMAND_HANDLER(IDC_BASIC, BN_CLICKED, OnBnClickedBasicButton)
		COMMAND_HANDLER(IDC_OK, BN_CLICKED, OnOK)
		COMMAND_HANDLER(IDC_CANCEL, BN_CLICKED, OnCancel)
		COMMAND_HANDLER(IDC_CUSTOM, BN_CLICKED, OnBnClickedCustomRadioButton)
		COMMAND_HANDLER(IDC_SMALL, BN_CLICKED, OnBnClickedNonCustomRadioButton)
		COMMAND_HANDLER(IDC_MEDIUM, BN_CLICKED, OnBnClickedNonCustomRadioButton)
		COMMAND_HANDLER(IDC_LARGE, BN_CLICKED, OnBnClickedNonCustomRadioButton)
		COMMAND_HANDLER(IDC_WINCE, BN_CLICKED, OnBnClickedNonCustomRadioButton)
	END_MSG_MAP()

private:
	CString m_fileNameAppendage;
	UINT m_imageWidth;
	UINT m_imageHeight;
	BOOL m_smallerOnly;
	BOOL m_original;
	void ResizeToBasic();
	void ResizeToAdvanced();
	CPath GetDestinationFile(CPath sourceFile, CPath direcotryName);
	LRESULT OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL &bHandled);
	LRESULT OnOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnBnClickedAdvancedButton(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnBnClickedBasicButton(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnBnClickedOkButton(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnBnClickedCancelButton(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnBnClickedCustomRadioButton(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnBnClickedNonCustomRadioButton(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
};
