cd e:/CodeRepository/SynologyDrive/RUINORERP
dotnet build RUINORERP.UI/RUINORERP.UI.csproj --verbosity minimal --no-restore > ui_build_output_new.txt 2>&1
echo Build completed with exit code %ERRORLEVEL%