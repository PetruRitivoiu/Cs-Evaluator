@echo off

rem %~dp0 -- absolute path for CompileAndExecute.bat
rem %~1 -- calea catre fisierul CS care va fi compilat
rem %~2 -- numele fisierului executabil

C:\WINDOWS\Microsoft.NET\Framework\v3.5\csc /out:exes/"%~2" /nologo "%~1"
"C:\Utilitare\Avira Command Line Scanner\scancl" %~2 --renameext=vir