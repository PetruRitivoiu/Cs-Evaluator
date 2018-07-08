@echo off

rem %~dp0 -- absolute path for ScanAndBuild.bat
rem %~1 -- dllFileFullPath

csc -target:library -out:"%~1" -warn:0 -nologo *.cs -reference:"nunit.framework.dll"

rem "C:\Utilitare\Avira Command Line Scanner\scancl" "%~1" --renameext=vir --quiet
