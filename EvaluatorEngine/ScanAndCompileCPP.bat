rem @echo off

rem %~dp0 -- absolute path for CompileAndExecute.bat
rem %~1 -- calea catre fisierul CPP care va fi compilat
rem %~2 -- numele fisierului exe

cd exes

rem echo "%~1" 

rem echo "%~2" 

rem "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\bin\vcvars32.bat"

"C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\bin\cl.exe" /EHsc /nologo "%~1" 

"C:\Utilitare\Avira Command Line Scanner\scancl" "%~2" --renameext=vir

cd..