@echo off
setlocal

rem Set the source directories for the .bin and .lock files
set "debugFolder=\bin\Debug\net7.0\Data"
set "productionFolder=\Production\Data"

rem Set the destination directory in your project
set "projectFolder=\Data"

rem Step 1: Check if the Data folder in the production folder exists
if exist "%productionFolder%" (
    call :SyncSecretFiles "%productionFolder%" "%projectFolder%"
)
rem Step 2: Check if the Data folder in the Debug folder exists
else if exist "%debugFolder%" (
    call :SyncSecretFiles "%debugFolder%" "%projectFolder%"
)
rem Step 6: If both folders are empty, output error message
else (
    echo "Error: No Data folder found in either the Debug or Production folder."
    exit /b 1
)

exit /b 0

:SyncSecretFiles
    rem Step 3: check there are bin and lock files in the folder
    dir /b "%~1\*.bin" >nul 2>&1 && (
        rem Step 4: Copy .bin and .lock files to the project Data folder
        robocopy "%~1" "%~2" *.bin *.lock /XO

        echo "Success: .bin and .lock files copied to the project folder."
    ) || (
        rem Step 5: If no bin and lock files, copy bin and lock files from the project folder to the production folder
        robocopy "%~2" "%~1" *.bin *.lock /XO

        echo "Success: .bin and .lock files copied to the target build folder."
    )

endlocal

pause

exit /b 0
```