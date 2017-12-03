@ECHO OFF 
REM Written by zee 11thJan2017-User contributions licensed under cc-wiki with attribution required.

REM The following directory is for .NET 4.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319

echo Installing Custom Service...
echo ---------------------------------------------------
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /i C:\CustomService\CustomReminderService.exe
echo ---------------------------------------------------
pause
echo Done.