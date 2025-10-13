@echo off
cd /d "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server"
dotnet build --verbosity normal > build_output.txt 2>&1
type build_output.txt