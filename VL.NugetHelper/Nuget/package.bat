@echo off
%~d0
cd %~dp0
echo ����˵����
echo [pack=���������ļ������PackagesĿ¼]
echo [push=�ϴ�PackagesĿ¼�����а�]
echo [exit=�˳�]
:start
set /p var=����������:
if "%var%"=="pack" goto pack
if "%var%"=="push" goto push
if "%var%"=="exit" goto end
echo ����������������������
goto start

:pack
if not exist Packages mkdir Packages
@del Packages\*.nupkg /f /q
nuget pack -Build -Properties Configuration=Release -OutputDirectory Packages
goto start

:push 
nuget push Packages\*.nupkg -s http://localhost:88/nuget
goto start

:end