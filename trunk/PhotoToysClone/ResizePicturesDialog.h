#pragma once

class CResizePicturesDialog sealed : public CDialog
{
public:
	CResizePicturesDialog(CWnd *pParentWnd = NULL);
	UINT GetImageWidth();
	UINT GetImageHeight();
	BOOL GetSmallerOnly();
	BOOL OnInitDialog();
	void OnOK();
	CPath GetDestinationFile(CPath sourceFile);

private:
	CString m_fileNameAppendage;
	UINT m_imageWidth;
	UINT m_imageHeight;
	BOOL m_smallerOnly;
	BOOL m_original;
	CButton *GetSmallRadioButton();
	CButton *GetMediumRadioButton();
	CButton *GetLargeRadioButton();
	CButton *GetWinCERadioButton();
	CButton *GetAdvancedButton();
	CButton *GetOKButton();
	CButton *GetCancelButton();
	CButton *GetCustomRadioButton();
	CStatic *GetCustomLabel1();
	CEdit *GetWidthTextBox();
	CStatic *GetCustomLabel2();
	CEdit *GetHeightTextBox();
	CStatic *GetCustomLabel3();
	CButton *GetSmallerCheckBox();
	CButton *GetOriginalCheckBox();
	CButton *GetBasicButton();
	CButton *GetOKButton2();
	CButton *GetCancelButton2();	
	void ResizeToBasic();
	void ResizeToAdvanced();
	CPath GetDestinationFile(CPath sourceFile, CPath direcotryName);	
	DECLARE_DYNAMIC(CResizePicturesDialog)
	DECLARE_MESSAGE_MAP()
	afx_msg void OnBnClickedAdvancedButton();
	afx_msg void OnBnClickedBasicButton();
	afx_msg void OnBnClickedOkButton();
	afx_msg void OnBnClickedCancelButton();
	afx_msg void OnBnClickedCustomRadioButton();
	afx_msg void OnBnClickedNonCustomRadioButton();
};
