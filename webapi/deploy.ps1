param (
    [string] $projectDir,
    [string] $outDir
)

function SyncSecretFiles {
    param (
        [string] $sourceFolder,
        [string] $destinationFolder
    )

    # Step 2: Check if there are .bin files in the folder
    if (Test-Path -Path (Join-Path $sourceFolder "*.bin")) {
        # Step 3: Copy .bin and .lock files to the project Data folder
        Write-Host "Copying .bin and .lock files to the project Data folder"
        Copy-Item -Path (Join-Path $sourceFolder "*.bin") -Destination $destinationFolder -Force
        Copy-Item -Path (Join-Path $sourceFolder "*.lock") -Destination $destinationFolder -Force
    }
    else {
        # Step 4: If no .bin files, copy .bin and .lock files from the project folder to the source folder
        Write-Host "Copying .bin and .lock files from the project folder to the source folder"
        Copy-Item -Path (Join-Path $projectFolder "*.bin") -Destination $sourceFolder -Force
        Copy-Item -Path (Join-Path $projectFolder "*.lock") -Destination $sourceFolder -Force
    }
}

# Step 1: Check if the Data folder in the out folder exists
if (Test-Path -Path $outDir -PathType Container) {
    Write-Host "Production Folder exists"
    SyncSecretFiles $outDir $projectDir
}
# Step 5: If both folders are empty, output error message
else {
    Write-Host "Error: No Data folder found in $outDir"
    exit 1
}

# pause the script so you can see the output
Read-Host -Prompt "Press Enter to exit"

