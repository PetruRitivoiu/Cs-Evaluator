@echo off

rem %~dp0 -- absolute path for CompileAndExecute.bat

C:\WINDOWS\Microsoft.NET\Framework\v3.5\csc /out:exes/execute.exe /nologo "%~1"  
exes\execute.exe "%~2"