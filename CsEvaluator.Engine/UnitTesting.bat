rem %~dp0 -- absolute path for ScanAndBuild.bat
rem %~1 -- dllFillFullName

nunit3-console --dispose-runners "%~1"
