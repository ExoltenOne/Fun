:: Chocolatey install script from earlier

@powershell -NoProfile -ExecutionPolicy Bypass -Command "iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))" && SET "PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin"


:: Install all the packages
:: -y confirm yes for any prompt during the install process ﻿

:: Browsers
choco install googlechrome -fy
choco install firefox -fy

:: DevTools
choco install notepadplusplus.install -fy
choco install git.install -fy
choco install nodejs.install -fy
choco install sqlite -fy

choco install sql-server-management-studio -fy
choco install vscode -fy
choco install visualstudio2017community -fy
choco install visualstudio2017enterprise -fy

:: Tools
choco install autohotkey.portable -fy
choco install 7zip -fy
choco install skype -fy
choco install ccleaner -fy
choco install sysinternals -fy


:: choco install <package_name> repeats for all the packages you want to install