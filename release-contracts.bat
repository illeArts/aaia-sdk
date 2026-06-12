@echo off
:: AAIA Contracts Release — Doppelklick-Starter
:: Fragt nach Bump-Typ und startet das PowerShell-Skript.

cd /d "%~dp0"

echo.
echo  AAIA.Shared.Contracts Release
echo  ================================
echo.
echo  [1] patch   2.0.0 -^> 2.0.1   Bugfix / optionale Property
echo  [2] minor   2.0.0 -^> 2.1.0   Neues Interface / DTO
echo  [3] major   2.0.0 -^> 3.0.0   Breaking Change
echo.
set /p CHOICE="  Auswahl (1/2/3, Standard=1): "

if "%CHOICE%"=="2" set BUMP=minor
if "%CHOICE%"=="3" set BUMP=major
if not defined BUMP set BUMP=patch

echo.
echo  Starte Release mit Bump-Typ: %BUMP%
echo.

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0release-contracts.ps1" -Bump %BUMP%

echo.
pause
