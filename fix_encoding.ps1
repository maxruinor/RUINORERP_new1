# Simple file encoding fix script
Write-Host "Starting file encoding fix process"

# Check if file exists
$file_path = "e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Helper\系统简介.htm"
$output_path = "e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Helper\系统简介_fixed.htm"

if (Test-Path $file_path) {
    Write-Host "File exists, processing..."
    
    # Read file content
    $content = Get-Content -Path $file_path
    
    # Save file in UTF-8 format
    $content | Out-File -FilePath $output_path -Encoding UTF8
    
    Write-Host "File processing complete, saved to: $output_path"
} else {
    Write-Host "File does not exist: $file_path"
}