#include "stdafx.h"
#include "PhotoResizeDlg.h"
#include "Resource.h"

IMAGE_SIZE CPhotoResizeDlg::GetSize() const
{
	return m_size;
}

UINT CPhotoResizeDlg::GetHeight() const
{
	return m_nHeight;
}

UINT CPhotoResizeDlg::GetWidth() const
{
	return m_nWidth;
}

BOOL CPhotoResizeDlg::IsSmallerOnly() const
{
	return m_fSmallerOnly;
}

BOOL CPhotoResizeDlg::IsOverwriteOriginal() const
{
	return m_fOverwriteOriginal;
}

LRESULT CPhotoResizeDlg::OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL &bHandled)
{
	IMAGE_SIZE size = IMGSZ_SMALL;
	UINT nCustomWidth = 1280;
	UINT nCustomHeight = 720;
	BOOL fSmallerOnly = FALSE;
	BOOL fOverwriteOriginal = FALSE;

	SettingsHelper::LoadSettings(&size, &nCustomWidth, &nCustomHeight, &fSmallerOnly, &fOverwriteOriginal);

	switch (size)
	{
	case IMGSZ_SMALL:
		CheckDlgButton(IDC_SMALL, BST_CHECKED);
		break;
		
	case IMGSZ_MEDIUM:
		CheckDlgButton(IDC_MEDIUM, BST_CHECKED);
		break;
		
	case IMGSZ_LARGE:
		CheckDlgButton(IDC_LARGE, BST_CHECKED);
		break;
		
	case IMGSZ_MOBILE:
		CheckDlgButton(IDC_MOBILE, BST_CHECKED);
		break;
		
	case IMGSZ_CUSTOM:
		CheckDlgButton(IDC_CUSTOM, BST_CHECKED);
		break;
	}

	if (nCustomWidth != 0)
	{
		LPTSTR pszWidth = new TCHAR[33];
		_itot_s(nCustomWidth, pszWidth, 33, 10);
		SetDlgItemText(IDC_WIDTH, pszWidth);
		delete pszWidth;
	}

	if (nCustomHeight != 0)
	{
		LPTSTR pszHeight = new TCHAR[33];
		_itot_s(nCustomHeight, pszHeight, 33, 10);
		SetDlgItemText(IDC_HEIGHT, pszHeight);
		delete pszHeight;
	}

	if (size != IMGSZ_CUSTOM)
	{
		EnableCustom(FALSE);
	}

	if (fSmallerOnly)
	{
		CheckDlgButton(IDC_SMALLERONLY, BST_CHECKED);
	}

	if (fOverwriteOriginal)
	{
		CheckDlgButton(IDC_OVERWRITEORIGINAL, BST_CHECKED);
	}

	if (size == IMGSZ_CUSTOM ||
		fSmallerOnly ||
		fOverwriteOriginal)
	{
		ShowAdvanced();
	}
	else
	{
		ShowAdvanced(FALSE);
	}

	return TRUE;
}

LRESULT CPhotoResizeDlg::OnOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	IMAGE_SIZE size;
	UINT nWidth;
	UINT nHeight;
	BOOL fSmallerOnly = IsDlgButtonChecked(IDC_SMALLERONLY) == BST_CHECKED;
	BOOL fOverwriteOriginal = IsDlgButtonChecked(IDC_OVERWRITEORIGINAL) == BST_CHECKED;

	if (IsDlgButtonChecked(IDC_SMALL) == BST_CHECKED)
	{
		size = IMGSZ_SMALL;
	}
	else if (IsDlgButtonChecked(IDC_MEDIUM) == BST_CHECKED)
	{
		size = IMGSZ_MEDIUM;
	}
	else if (IsDlgButtonChecked(IDC_LARGE) == BST_CHECKED)
	{
		size = IMGSZ_LARGE;
	}
	else if (IsDlgButtonChecked(IDC_MOBILE) == BST_CHECKED)
	{
		size = IMGSZ_MOBILE;
	}
	else
	{
		size = IMGSZ_CUSTOM;
	}

	if (size == IMGSZ_CUSTOM)
	{
		CString strWidth;
		CString strHeight;
		LPTSTR pszWidthStop;
		LPTSTR pszHeightStop;

		GetDlgItemText(IDC_WIDTH, strWidth);
		GetDlgItemText(IDC_HEIGHT, strHeight);

		long lWidth = _tcstol(strWidth, &pszWidthStop, 10);
		long lHeight = _tcstol(strHeight, &pszHeightStop, 10);

		if (_tcslen(pszWidthStop) > 0 || _tcslen(pszHeightStop) > 0 || lWidth < 0 || lHeight < 0 || (lWidth == 0 && lHeight == 0))
		{
			CString strError;
			CString strCaption;

			strError.LoadString(IDS_INPUTERROR);
			GetWindowText(strCaption);

			MessageBox(strError, strCaption, MB_ICONERROR);

			return 1;
		}

		nWidth = lWidth;
		nHeight = lHeight;
	}
	else
	{
		SettingsHelper::GetDimmensionsForSize(size, nWidth, nHeight);
	}

	SettingsHelper::SaveSettings(size, nWidth, nHeight, fSmallerOnly, fOverwriteOriginal);

	m_size = size;
	m_nWidth = nWidth;
	m_nHeight = nHeight;
	m_fSmallerOnly = fSmallerOnly;
	m_fOverwriteOriginal = fOverwriteOriginal;

	EndDialog(IDOK);

	return 0;
}

LRESULT CPhotoResizeDlg::OnCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	EndDialog(IDCANCEL);

	return 0;
}

void CPhotoResizeDlg::ShowAdvanced(BOOL fShow)
{
	CRect rect;
	rect.left = 0;
	rect.top = 0;
	rect.right = 276;
	rect.bottom = fShow ? 167 : 121;

	MapDialogRect(rect);
	AdjustWindowRect(rect, GetStyle(), GetMenu() != NULL);

	SetWindowPos(NULL, rect.left, rect.top, rect.Width(), rect.Height(), SWP_NOMOVE | SWP_NOZORDER);

	int nBasicShow = fShow ? SW_HIDE : SW_SHOW;

	GetDlgItem(IDC_ADVANCED).ShowWindow(nBasicShow);
	GetDlgItem(IDOK).ShowWindow(nBasicShow);
	GetDlgItem(IDCANCEL).ShowWindow(nBasicShow);

	int nAdvancedShow = fShow ? SW_SHOW : SW_HIDE;

	GetDlgItem(IDC_CUSTOM).ShowWindow(nAdvancedShow);
	GetDlgItem(IDC_CUSTOM1).ShowWindow(nAdvancedShow);
	GetDlgItem(IDC_WIDTH).ShowWindow(nAdvancedShow);
	GetDlgItem(IDC_CUSTOM2).ShowWindow(nAdvancedShow);
	GetDlgItem(IDC_HEIGHT).ShowWindow(nAdvancedShow);
	GetDlgItem(IDC_CUSTOM3).ShowWindow(nAdvancedShow);
	GetDlgItem(IDC_SMALLERONLY).ShowWindow(nAdvancedShow);
	GetDlgItem(IDC_OVERWRITEORIGINAL).ShowWindow(nAdvancedShow);
	GetDlgItem(IDC_BASIC).ShowWindow(nAdvancedShow);
	GetDlgItem(IDC_OK).ShowWindow(nAdvancedShow);
	GetDlgItem(IDC_CANCEL).ShowWindow(nAdvancedShow);

	if (!fShow)
	{
		if (IsDlgButtonChecked(IDC_CUSTOM) == BST_CHECKED)
		{
			CheckRadioButton(IDC_SMALL, IDC_CUSTOM, IDC_MOBILE);
			EnableCustom(FALSE);
		}

		CheckDlgButton(IDC_SMALLERONLY, BST_UNCHECKED);
		CheckDlgButton(IDC_OVERWRITEORIGINAL, BST_UNCHECKED);
	}
}

void CPhotoResizeDlg::EnableCustom(BOOL fEnable)
{
	GetDlgItem(IDC_CUSTOM1).EnableWindow(fEnable);
	GetDlgItem(IDC_WIDTH).EnableWindow(fEnable);
	GetDlgItem(IDC_CUSTOM2).EnableWindow(fEnable);
	GetDlgItem(IDC_HEIGHT).EnableWindow(fEnable);
	GetDlgItem(IDC_CUSTOM3).EnableWindow(fEnable);
}

LRESULT CPhotoResizeDlg::OnClickedAdvanced(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	ShowAdvanced();

	return 0;
}

LRESULT CPhotoResizeDlg::OnClickedBasic(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	ShowAdvanced(FALSE);

	return 0;
}

LRESULT CPhotoResizeDlg::OnClickedCustom(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	EnableCustom();

	return 0;
}

LRESULT CPhotoResizeDlg::OnClickedOther(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	EnableCustom(FALSE);

	return 0;
}
