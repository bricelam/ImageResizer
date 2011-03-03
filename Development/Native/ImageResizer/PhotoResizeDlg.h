#pragma once

#include "SettingsHelper.h"

using namespace ATL;

class CPhotoResizeDlg sealed : public CDialogImpl<CPhotoResizeDlg>
{
public:
	enum { IDD = IDD_PHOTORESIZEDLG };
	IMAGE_SIZE GetSize() const;
	UINT GetHeight() const;
	UINT GetWidth() const;
	BOOL IsSmallerOnly() const;
	BOOL IsOverwriteOriginal() const;
	BEGIN_MSG_MAP(CPhotoResizeDlg)
		MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
		COMMAND_HANDLER(IDOK, BN_CLICKED, OnOK)
		COMMAND_HANDLER(IDCANCEL, BN_CLICKED, OnCancel)
		COMMAND_HANDLER(IDC_ADVANCED, BN_CLICKED, OnClickedAdvanced)
		COMMAND_HANDLER(IDC_BASIC, BN_CLICKED, OnClickedBasic)
		COMMAND_HANDLER(IDC_OK, BN_CLICKED, OnOK)
		COMMAND_HANDLER(IDC_CANCEL, BN_CLICKED, OnCancel)
		COMMAND_HANDLER(IDC_CUSTOM, BN_CLICKED, OnClickedCustom)
		COMMAND_HANDLER(IDC_SMALL, BN_CLICKED, OnClickedOther)
		COMMAND_HANDLER(IDC_MEDIUM, BN_CLICKED, OnClickedOther)
		COMMAND_HANDLER(IDC_LARGE, BN_CLICKED, OnClickedOther)
		COMMAND_HANDLER(IDC_MOBILE, BN_CLICKED, OnClickedOther)
	END_MSG_MAP()

private:
	IMAGE_SIZE m_size;
	UINT m_nHeight;
	UINT m_nWidth;
	BOOL m_fSmallerOnly;
	BOOL m_fOverwriteOriginal;
	void ShowAdvanced(BOOL fShow = TRUE);
	void EnableCustom(BOOL fEnable = TRUE);
	LRESULT OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL &bHandled);
	LRESULT OnOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnClickedAdvanced(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnClickedBasic(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnClickedCustom(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
	LRESULT OnClickedOther(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled);
};
