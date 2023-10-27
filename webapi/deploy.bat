@echo off
setlocal

:: Set the project directory and output directory
set "projectDir=%~1\Data"
set "outDir=%~1\%~2Data"

:: Check if the Data folder in the out folder does not exist
if not exist "%outDir%" (
    :: Copy the Data folder from the project folder to the out folder
    echo Copying the Data folder from the project folder to the out folder
    xcopy /E /I /Y "%projectDir%" "%outDir%"
) else (
    :: Sync the secret files
    echo Syncing the secret files
    call :SyncSecretFiles "%outDir%" "%projectDir%"
)

endlocal
exit /b 0

:: Function to synchronize secret files
:SyncSecretFiles
    set "source=%~1"
    set "destination=%~2"

    :: Sync the secret files
    robocopy "%source%" "%destination%" *.bin *.lock /XO
    robocopy "%destination%" "%source%" *.bin *.lock /XO

