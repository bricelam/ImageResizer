@ECHO OFF

CALL :GetTemplateIncludeDir
IF "%TemplateInclude%"=="" GOTO Error

IF "%CommonProgramFiles(x86)%"=="" (
    SET "TextTransform=%CommonProgramFiles%\microsoft shared\TextTemplating\10.0\TextTransform.exe"
) ELSE (
    SET "TextTransform=%CommonProgramFiles(x86)%\microsoft shared\TextTemplating\10.0\TextTransform.exe"
)

"%TextTransform%" -I "%TemplateInclude%" Version.tt
IF ERRORLEVEL 1 GOTO Error

GOTO End

:GetTemplateIncludeDir
SET TemplateInclude=
CALL :GetTemplateIncludeDirHelper32 > nul 2>&1
IF ERRORLEVEL 1 CALL :GetTemplateIncludeDirHelper64 > nul 2>&1
IF "%TemplateInclude:~-1%"=="\" SET "TemplateInclude=%TemplateInclude:~0,-1%"
EXIT /B 0

:GetTemplateIncludeDirHelper32
FOR /F "tokens=1,2*" %%i IN ('REG QUERY "HKLM\SOFTWARE\Microsoft\VisualStudio\10.0\TextTemplating\IncludeFolders\.tt" /v "Include67826F5E-E1F5-4618-B91C-957E4A34F0D9"') DO (
    IF "%%i"=="Include67826F5E-E1F5-4618-B91C-957E4A34F0D9" (
        SET "TemplateInclude=%%k"
    )
)
IF "%TemplateInclude%"=="" EXIT /B 1
EXIT /B 0

:GetTemplateIncludeDirHelper64
FOR /F "tokens=1,2*" %%i IN ('REG QUERY "HKLM\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\10.0\TextTemplating\IncludeFolders\.tt" /v "Include67826F5E-E1F5-4618-B91C-957E4A34F0D9"') DO (
    IF "%%i"=="Include67826F5E-E1F5-4618-B91C-957E4A34F0D9" (
        SET "TemplateInclude=%%k"
    )
)
IF "%TemplateInclude%"=="" EXIT /B 1
EXIT /B 0

:Error
ECHO ERROR: Something somewhere whent terribly wrong.
PAUSE
GOTO End

:End
