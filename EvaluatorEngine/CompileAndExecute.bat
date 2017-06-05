@echo off

rem %~dp0 -- absolute path for CompileAndExecute.bat
rem %~1 -- calea catre fisierul CS care va fi compilat
rem %~2 -- numele fisierului executabil
rem %~3 -- fisierul de validare

rem set exeName=execute_%~1.exe

rem C:\WINDOWS\Microsoft.NET\Framework\v3.5\csc /out:exes/"%~2" /nologo "%~1"
rem "C:\Utilitare\Avira Command Line Scanner\scancl" [FILE_PATH] --renameext=vir
exes\"%~2" "%~3"