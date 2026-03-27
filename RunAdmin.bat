@echo off
REM Chay cua so Admin de test - khong can dang nhap
cd /d "%~dp0"
if exist "MoneyFlowApp\bin\Debug\net8.0-windows\MoneyFlowApp.exe" (
    start "" "MoneyFlowApp\bin\Debug\net8.0-windows\MoneyFlowApp.exe" --admin
) else (
    echo Chua build. Chay: dotnet build
    pause
)
