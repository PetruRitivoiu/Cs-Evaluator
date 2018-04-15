@echo off

rem %~dp0 -- absolute path for ScanAndBuild.bat
rem %~1 -- csFileFullPath
rem %~2 -- dllFileFullPath

"C:\WINDOWS\Microsoft.NET\Framework\v3.5\csc" -nologo -out:"%~2" "%~1"  -warn:0

"C:\Utilitare\Avira Command Line Scanner\scancl" "%~2" --renameext=vir --quiet
