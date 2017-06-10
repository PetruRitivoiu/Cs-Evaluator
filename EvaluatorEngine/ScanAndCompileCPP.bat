@echo off

rem %~dp0 -- absolute path for CompileAndExecute.bat
rem %~1 -- calea catre fisierul CPP care va fi compilat
rem %~2 -- numele fisierului exe

cd exes

"C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\bin\cl.exe" /EHsc /nologo "%~1" 


"C:\Utilitare\Avira Command Line Scanner\scancl" %~2 --renameext=vir

cd..