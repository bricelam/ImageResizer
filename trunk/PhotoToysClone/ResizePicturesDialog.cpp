#include "stdafx.h"
#include "ResizePicturesDialog.h"
#include "Resource.h"

CResizePicturesDialog::CResizePicturesDialog()
{
	m_imageWidth = 0;
	m_imageHeight = 0;
	m_smallerOnly = FALSE;
	m_original = FALSE;
}

CPath CResizePicturesDialog::GetDestinationFile(CPath sourceFile)
{
	CPath directoryName = sourceFile;
	directoryName.RemoveFileSpec();	
	
	return GetDestinationFile(sourceFile, directoryName);
}

UINT CResizePicturesDialog::GetImageWidth()
{
	return m_imageWidth;
}

UINT CResizePicturesDialog::GetImageHeight()
{
	return m_imageHeight;
}

BOOL CResizePicturesDialog::GetSmallerOnly()
{	
	return m_smallerOnly;
}

LRESULT CResizePicturesDialog::OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL &bHandled)
{
	ResizeToBasic();

	CheckDlgButton(IDC_SMALL, BST_CHECKED);

	GetDlgItem(IDC_CUSTOM1).EnableWindow(FALSE);
	GetDlgItem(IDC_WIDTH).EnableWindow(FALSE);
	GetDlgItem(IDC_CUSTOM2).EnableWindow(FALSE);
	GetDlgItem(IDC_HEIGHT).EnableWindow(FALSE);
	GetDlgItem(IDC_CUSTOM3).EnableWindow(FALSE);

	SetDlgItemText(IDC_WIDTH, _T("1200"));
	SetDlgItemText(IDC_HEIGHT, _T("1024"));

	return 1;
}

LRESULT CResizePicturesDialog::OnOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	m_original = (IsDlgButtonChecked(IDC_ORIGINAL) == BST_CHECKED);

	// TODO: Resourcify appendages.
	if (IsDlgButtonChecked(IDC_SMALL) == BST_CHECKED)
	{
		m_fileNameAppendage = _T(" (Small)");
		m_imageWidth = 640;
		m_imageHeight = 480;
	}
	else if (IsDlgButtonChecked(IDC_MEDIUM) == BST_CHECKED)
	{
		m_fileNameAppendage = _T(" (Medium)");
		m_imageWidth = 800;
		m_imageHeight = 600;
	}
	else if (IsDlgButtonChecked(IDC_LARGE) == BST_CHECKED)
	{
		m_fileNameAppendage = _T(" (Large)");
		m_imageWidth = 1024;
		m_imageHeight = 768;
	}
	else if (IsDlgButtonChecked(IDC_WINCE) == BST_CHECKED)
	{
		m_fileNameAppendage = _T(" (WinCE)");
		m_imageWidth = 240;
		m_imageHeight = 320;
	}
	else
	{
		CString width;
		CString height;

		m_fileNameAppendage = _T(" (Custom)");

		GetDlgItemText(IDC_WIDTH, width);
		GetDlgItemText(IDC_HEIGHT, height);

		// TODO: Validate.
		m_imageWidth = _ttoi((LPCTSTR)width);
		m_imageHeight = _ttoi((LPCTSTR)height);
	}

	m_smallerOnly = (IsDlgButtonChecked(IDC_SMALLER) == BST_CHECKED);

	EndDialog(IDOK);

	return 1;
}

LRESULT CResizePicturesDialog::OnCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	EndDialog(IDCANCEL);

	return 1;
}

void CResizePicturesDialog::ResizeToBasic()
{
	CRect rect = new CRect(0, 0, 276, 121);

	MapDialogRect(rect);
	AdjustWindowRect(rect, GetStyle(), GetMenu() != NULL);

	SetWindowPos(NULL, rect.left, rect.top, rect.Width(), rect.Height(), SWP_NOMOVE | SWP_NOZORDER);
}

void CResizePicturesDialog::ResizeToAdvanced()
{
	CRect rect = new CRect(0, 0, 276, 167);

	MapDialogRect(rect);
	AdjustWindowRect(rect, GetStyle(), GetMenu() != NULL);

	SetWindowPos(NULL, rect.left, rect.top, rect.Width(), rect.Height(), SWP_NOMOVE | SWP_NOZORDER);
}

CPath CResizePicturesDialog::GetDestinationFile(CPath sourceFile, CPath direcotryName)
{
	if (m_original)
	{
		return sourceFile;
	}
	
	CPath fileName = sourceFile.m_strPath.Mid(sourceFile.FindFileName());
	fileName.RemoveExtension();
	CString extension = sourceFile.m_strPath.Mid(sourceFile.FindExtension());
	
	CPath path;
	UINT count = 1;
	
	do
	{
		CString countString;
		countString.Format(_T(" (%d)"), count);

		path.Combine(direcotryName, fileName.m_strPath + m_fileNameAppendage + (count == 1 ? _T("") : countString));
		path.RenameExtension(extension);

		count++;
	} while (path.FileExists());

	return path;
}

LRESULT CResizePicturesDialog::OnBnClickedAdvancedButton(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	ResizeToAdvanced();

	GetDlgItem(IDC_ADVANCED).ShowWindow(SW_HIDE);
	GetDlgItem(IDOK).ShowWindow(SW_HIDE);
	GetDlgItem(IDCANCEL).ShowWindow(SW_HIDE);

	GetDlgItem(IDC_CUSTOM).ShowWindow(SW_SHOW);
	GetDlgItem(IDC_CUSTOM1).ShowWindow(SW_SHOW);
	GetDlgItem(IDC_WIDTH).ShowWindow(SW_SHOW);
	GetDlgItem(IDC_CUSTOM2).ShowWindow(SW_SHOW);
	GetDlgItem(IDC_HEIGHT).ShowWindow(SW_SHOW);
	GetDlgItem(IDC_CUSTOM3).ShowWindow(SW_SHOW);
	GetDlgItem(IDC_SMALLER).ShowWindow(SW_SHOW);
	GetDlgItem(IDC_ORIGINAL).ShowWindow(SW_SHOW);
	GetDlgItem(IDC_BASIC).ShowWindow(SW_SHOW);
	GetDlgItem(IDC_OK).ShowWindow(SW_SHOW);
	GetDlgItem(IDC_CANCEL).ShowWindow(SW_SHOW);

	return 1;
}

LRESULT CResizePicturesDialog::OnBnClickedBasicButton(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	ResizeToBasic();

	GetDlgItem(IDC_ADVANCED).ShowWindow(SW_SHOW);
	GetDlgItem(IDOK).ShowWindow(SW_SHOW);
	GetDlgItem(IDCANCEL).ShowWindow(SW_SHOW);

	GetDlgItem(IDC_CUSTOM).ShowWindow(SW_HIDE);
	GetDlgItem(IDC_CUSTOM1).ShowWindow(SW_HIDE);
	GetDlgItem(IDC_WIDTH).ShowWindow(SW_HIDE);
	GetDlgItem(IDC_CUSTOM2).ShowWindow(SW_HIDE);
	GetDlgItem(IDC_HEIGHT).ShowWindow(SW_HIDE);
	GetDlgItem(IDC_CUSTOM3).ShowWindow(SW_HIDE);
	GetDlgItem(IDC_SMALLER).ShowWindow(SW_HIDE);
	GetDlgItem(IDC_ORIGINAL).ShowWindow(SW_HIDE);
	GetDlgItem(IDC_BASIC).ShowWindow(SW_HIDE);
	GetDlgItem(IDC_OK).ShowWindow(SW_HIDE);
	GetDlgItem(IDC_CANCEL).ShowWindow(SW_HIDE);

	if (IsDlgButtonChecked(IDC_CUSTOM) == BST_CHECKED)
	{
		CheckDlgButton(IDC_SMALL, BST_CHECKED);

		CheckDlgButton(IDC_CUSTOM, BST_UNCHECKED);		
		OnBnClickedNonCustomRadioButton(wNotifyCode, wID, hWndCtl, bHandled);
	}

	CheckDlgButton(IDC_SMALLER, BST_UNCHECKED);
	CheckDlgButton(IDC_ORIGINAL, BST_UNCHECKED);

	return 1;
}

LRESULT CResizePicturesDialog::OnBnClickedCustomRadioButton(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	GetDlgItem(IDC_CUSTOM1).EnableWindow();
	GetDlgItem(IDC_WIDTH).EnableWindow();
	GetDlgItem(IDC_CUSTOM2).EnableWindow();
	GetDlgItem(IDC_HEIGHT).EnableWindow();
	GetDlgItem(IDC_CUSTOM3).EnableWindow();

	return 1;
}

LRESULT CResizePicturesDialog::OnBnClickedNonCustomRadioButton(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL &bHandled)
{
	GetDlgItem(IDC_CUSTOM1).EnableWindow(FALSE);
	GetDlgItem(IDC_WIDTH).EnableWindow(FALSE);
	GetDlgItem(IDC_CUSTOM2).EnableWindow(FALSE);
	GetDlgItem(IDC_HEIGHT).EnableWindow(FALSE);
	GetDlgItem(IDC_CUSTOM3).EnableWindow(FALSE);

	return 1;
}
