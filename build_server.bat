@echo off
cd /d "E:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Server"
dotnet build > server_build_output.txt 2>&1
type server_build_output.txt