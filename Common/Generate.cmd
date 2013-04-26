@ECHO OFF

CALL :GetTemplateIncludeDir
IF "%TemplateInclude%"=="" GOTO Error

IF "%CommonProgramFiles(x86)%"=="" (
    SET "TextTransform=%CommonProgramFiles%\Microsoft Shared\TextTemplating\11.0\TextTransform.exe"
) ELSE (
    SET "TextTransform=%CommonProgramFiles(x86)%\Microsoft Shared\TextTemplating\11.0\TextTransform.exe"
)

"%TextTransform%" -I "%TemplateInclude%" Common.tt
IF ERRORLEVEL 1 GOTO Error

GOTO :EOF

:GetTemplateIncludeDir
SET TemplateInclude=
CALL :GetTemplateIncludeDirHelper > nul 2>&1
IF "%TemplateInclude:~-1%"=="\" SET "TemplateInclude=%TemplateInclude:~0,-1%"
EXIT /B 0

:GetTemplateIncludeDirHelper
FOR /F "tokens=1,2*" %%i IN ('REG QUERY "HKEY_CURRENT_USER\Software\Microsoft\VisualStudio\11.0_Config\TextTemplating\IncludeFolders\.tt" /v "Include67826F5E-E1F5-4618-B91C-957E4A34F0D9"') DO (
    IF "%%i"=="Include67826F5E-E1F5-4618-B91C-957E4A34F0D9" (
        SET "TemplateInclude=%%k"
    )
)
IF "%TemplateInclude%"=="" EXIT /B 1
EXIT /B 0

:Error
ECHO ERROR: Something somewhere whent terribly wrong.
PAUSE
GOTO :EOF
