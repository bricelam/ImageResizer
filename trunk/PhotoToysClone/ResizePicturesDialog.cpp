#include "stdafx.h"
#include "ResizePicturesDialog.h"
#include "Resource.h"

CResizePicturesDialog::CResizePicturesDialog(CWnd *pParentWnd)
	: CDialog(IDD_RESIZEPICTURES, pParentWnd)
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

BOOL CResizePicturesDialog::OnInitDialog()
{
	ResizeToBasic();

	GetSmallRadioButton()->SetCheck(BST_CHECKED);

	GetCustomLabel1()->EnableWindow(FALSE);
	GetWidthTextBox()->EnableWindow(FALSE);
	GetCustomLabel2()->EnableWindow(FALSE);
	GetHeightTextBox()->EnableWindow(FALSE);
	GetCustomLabel3()->EnableWindow(FALSE);

	GetWidthTextBox()->SetWindowText(_T("1200"));
	GetHeightTextBox()->SetWindowText(_T("1024"));

	return TRUE;
}

void CResizePicturesDialog::OnOK()
{
	m_original = (GetOriginalCheckBox()->GetCheck() == BST_CHECKED);

	// TODO: Resourcify these.
	if (GetSmallRadioButton()->GetCheck() == BST_CHECKED)
	{
		m_fileNameAppendage = _T(" (Small)");
	}
	else if (GetMediumRadioButton()->GetCheck() == BST_CHECKED)
	{
		m_fileNameAppendage = _T(" (Medium)");
	}
	else if (GetLargeRadioButton()->GetCheck() == BST_CHECKED)
	{
		m_fileNameAppendage = _T(" (Large)");
	}
	else if (GetWinCERadioButton()->GetCheck() == BST_CHECKED)
	{
		m_fileNameAppendage = _T(" (WinCE)");
	}
	else
	{
		m_fileNameAppendage = _T(" (Custom)");
	}

	if (GetSmallRadioButton()->GetCheck() == BST_CHECKED)
	{
		m_imageWidth = 640;
	}
	else if (GetMediumRadioButton()->GetCheck() == BST_CHECKED)
	{
		m_imageWidth = 800;
	}
	else if (GetLargeRadioButton()->GetCheck() == BST_CHECKED)
	{
		m_imageWidth = 1024;
	}
	else if (GetWinCERadioButton()->GetCheck() == BST_CHECKED)
	{
		m_imageWidth = 240;
	}
	else
	{
		CString width;
		GetWidthTextBox()->GetWindowText(width);

		// TODO: Validate.
		m_imageWidth = _ttoi((LPCTSTR)width);
	}

	if (GetSmallRadioButton()->GetCheck() == BST_CHECKED)
	{
		m_imageHeight = 480;
	}
	else if (GetMediumRadioButton()->GetCheck() == BST_CHECKED)
	{
		m_imageHeight = 600;
	}
	else if (GetLargeRadioButton()->GetCheck() == BST_CHECKED)
	{
		m_imageHeight = 768;
	}
	else if (GetWinCERadioButton()->GetCheck() == BST_CHECKED)
	{
		m_imageHeight = 320;
	}
	else
	{
		CString height;
		GetHeightTextBox()->GetWindowText(height);

		// TODO: Validate.
		m_imageHeight = _ttoi((LPCTSTR)height);
	}

	m_smallerOnly = (GetSmallerCheckBox()->GetCheck() == BST_CHECKED);

	CDialog::OnOK();
}

CButton *CResizePicturesDialog::GetSmallRadioButton()
{
	return (CButton *)GetDlgItem(IDC_SMALL);
}

CButton *CResizePicturesDialog::GetMediumRadioButton()
{
	return (CButton *)GetDlgItem(IDC_MEDIUM);
}

CButton *CResizePicturesDialog::GetLargeRadioButton()
{
	return (CButton *)GetDlgItem(IDC_LARGE);
}

CButton *CResizePicturesDialog::GetWinCERadioButton()
{
	return (CButton *)GetDlgItem(IDC_WINCE);
}

CButton *CResizePicturesDialog::GetAdvancedButton()
{
	return (CButton *)GetDlgItem(IDC_ADVANCED);
}

CButton *CResizePicturesDialog::GetOKButton()
{
	return (CButton *)GetDlgItem(IDOK);
}

CButton *CResizePicturesDialog::GetCancelButton()
{
	return (CButton *)GetDlgItem(IDCANCEL);
}

CButton *CResizePicturesDialog::GetCustomRadioButton()
{
	return (CButton *)GetDlgItem(IDC_CUSTOM);
}

CStatic *CResizePicturesDialog::GetCustomLabel1()
{
	return (CStatic *)GetDlgItem(IDC_CUSTOM1);
}

CEdit *CResizePicturesDialog::GetWidthTextBox()
{
	return (CEdit *)GetDlgItem(IDC_WIDTH);
}

CStatic *CResizePicturesDialog::GetCustomLabel2()
{
	return (CStatic *)GetDlgItem(IDC_CUSTOM2);
}

CEdit *CResizePicturesDialog::GetHeightTextBox()
{
	return (CEdit *)GetDlgItem(IDC_HEIGHT);
}

CStatic *CResizePicturesDialog::GetCustomLabel3()
{
	return (CStatic *)GetDlgItem(IDC_CUSTOM3);
}

CButton *CResizePicturesDialog::GetSmallerCheckBox()
{
	return (CButton *)GetDlgItem(IDC_SMALLER);
}

CButton *CResizePicturesDialog::GetOriginalCheckBox()
{
	return (CButton *)GetDlgItem(IDC_ORIGINAL);
}

CButton *CResizePicturesDialog::GetBasicButton()
{
	return (CButton *)GetDlgItem(IDC_BASIC);
}

CButton *CResizePicturesDialog::GetOKButton2()
{
	return (CButton *)GetDlgItem(IDC_OK);
}

CButton *CResizePicturesDialog::GetCancelButton2()
{
	return (CButton *)GetDlgItem(IDC_CANCEL);
}

void CResizePicturesDialog::ResizeToBasic()
{
	CRect rect = new CRect(0, 0, 276, 121);

	MapDialogRect(rect);
	CalcWindowRect(rect);

	SetWindowPos(NULL, rect.left, rect.top, rect.Width(), rect.Height(), SWP_NOMOVE | SWP_NOZORDER);
}

void CResizePicturesDialog::ResizeToAdvanced()
{
	CRect rect = new CRect(0, 0, 276, 167);

	MapDialogRect(rect);
	CalcWindowRect(rect);

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

IMPLEMENT_DYNAMIC(CResizePicturesDialog, CDialog)

BEGIN_MESSAGE_MAP(CResizePicturesDialog, CDialog)
	ON_BN_CLICKED(IDC_ADVANCED, &CResizePicturesDialog::OnBnClickedAdvancedButton)
	ON_BN_CLICKED(IDC_BASIC, &CResizePicturesDialog::OnBnClickedBasicButton)
	ON_BN_CLICKED(IDC_OK, &CDialog::OnOK)
	ON_BN_CLICKED(IDC_CANCEL, &CDialog::OnCancel)
	ON_BN_CLICKED(IDC_CUSTOM, &CResizePicturesDialog::OnBnClickedCustomRadioButton)
	ON_BN_CLICKED(IDC_SMALL, &CResizePicturesDialog::OnBnClickedNonCustomRadioButton)
	ON_BN_CLICKED(IDC_MEDIUM, &CResizePicturesDialog::OnBnClickedNonCustomRadioButton)
	ON_BN_CLICKED(IDC_LARGE, &CResizePicturesDialog::OnBnClickedNonCustomRadioButton)
	ON_BN_CLICKED(IDC_WINCE, &CResizePicturesDialog::OnBnClickedNonCustomRadioButton)
END_MESSAGE_MAP()

void CResizePicturesDialog::OnBnClickedAdvancedButton()
{
	ResizeToAdvanced();

	GetAdvancedButton()->ShowWindow(SW_HIDE);
	GetOKButton()->ShowWindow(SW_HIDE);
	GetCancelButton()->ShowWindow(SW_HIDE);

	GetCustomRadioButton()->ShowWindow(SW_SHOW);
	GetCustomLabel1()->ShowWindow(SW_SHOW);
	GetWidthTextBox()->ShowWindow(SW_SHOW);
	GetCustomLabel2()->ShowWindow(SW_SHOW);
	GetHeightTextBox()->ShowWindow(SW_SHOW);
	GetCustomLabel3()->ShowWindow(SW_SHOW);
	GetSmallerCheckBox()->ShowWindow(SW_SHOW);
	GetOriginalCheckBox()->ShowWindow(SW_SHOW);
	GetBasicButton()->ShowWindow(SW_SHOW);
	GetOKButton2()->ShowWindow(SW_SHOW);
	GetCancelButton2()->ShowWindow(SW_SHOW);
}

void CResizePicturesDialog::OnBnClickedBasicButton()
{
	ResizeToBasic();

	GetAdvancedButton()->ShowWindow(SW_SHOW);
	GetOKButton()->ShowWindow(SW_SHOW);
	GetCancelButton()->ShowWindow(SW_SHOW);

	GetCustomRadioButton()->ShowWindow(SW_HIDE);
	GetCustomLabel1()->ShowWindow(SW_HIDE);
	GetWidthTextBox()->ShowWindow(SW_HIDE);
	GetCustomLabel2()->ShowWindow(SW_HIDE);
	GetHeightTextBox()->ShowWindow(SW_HIDE);
	GetCustomLabel3()->ShowWindow(SW_HIDE);
	GetSmallerCheckBox()->ShowWindow(SW_HIDE);
	GetOriginalCheckBox()->ShowWindow(SW_HIDE);
	GetBasicButton()->ShowWindow(SW_HIDE);
	GetOKButton2()->ShowWindow(SW_HIDE);
	GetCancelButton2()->ShowWindow(SW_HIDE);

	if (GetCustomRadioButton()->GetCheck() == BST_CHECKED)
	{
		GetSmallRadioButton()->SetCheck(BST_CHECKED);

		GetCustomRadioButton()->SetCheck(BST_UNCHECKED);		
		OnBnClickedNonCustomRadioButton();
	}

	GetSmallerCheckBox()->SetCheck(BST_UNCHECKED);
	GetOriginalCheckBox()->SetCheck(BST_UNCHECKED);
}

void CResizePicturesDialog::OnBnClickedCustomRadioButton()
{
	GetCustomLabel1()->EnableWindow();
	GetWidthTextBox()->EnableWindow();
	GetCustomLabel2()->EnableWindow();
	GetHeightTextBox()->EnableWindow();
	GetCustomLabel3()->EnableWindow();
}

void CResizePicturesDialog::OnBnClickedNonCustomRadioButton()
{
	GetCustomLabel1()->EnableWindow(FALSE);
	GetWidthTextBox()->EnableWindow(FALSE);
	GetCustomLabel2()->EnableWindow(FALSE);
	GetHeightTextBox()->EnableWindow(FALSE);
	GetCustomLabel3()->EnableWindow(FALSE);
}
