# Set the source directories for the .bin and .lock files
$debugFolder = "bin\Debug\net7.0\Data"
$productionFolder = "Production\Data"

# Set the destination directory in your project
$projectFolder = "Data"

# Step 1: Check if the Data folder in the production folder exists
if (Test-Path -Path $productionFolder -PathType Container) {
    Write-Host "Production Folder exists"
    SyncSecretFiles $productionFolder $projectFolder
}
# Step 2: Check if the Data folder in the Debug folder exists
elseif (Test-Path -Path $debugFolder -PathType Container) {
    Write-Host "Debug Folder exists"
    SyncSecretFiles $debugFolder $projectFolder
}
# Step 6: If both folders are empty, output error message
else {
    Write-Host "Error: No Data folder found in either the Debug or Production folder."
    exit 1
}

# pause the script so you can see the output
Read-Host -Prompt "Press Enter to exit"

exit 0

function SyncSecretFiles {
    param (
        [string] $sourceFolder,
        [string] $destinationFolder
    )

    # Step 3: Check if there are .bin files in the folder
    if (Test-Path -Path (Join-Path $sourceFolder "*.bin")) {
        # Step 4: Copy .bin and .lock files to the project Data folder
        Write-Host "Copying .bin and .lock files to the project Data folder"
        Copy-Item -Path (Join-Path $sourceFolder "*.bin") -Destination $destinationFolder -Force
    }
    else {
        # Step 5: If no .bin files, copy .bin and .lock files from the project folder to the source folder
        Write-Host "Copying .bin and .lock files from the project folder to the source folder"
        Copy-Item -Path (Join-Path $projectFolder "*.bin") -Destination $sourceFolder -Force
    }
}