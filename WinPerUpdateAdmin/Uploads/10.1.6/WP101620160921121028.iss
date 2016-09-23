; 
; Instalador winper version 10.1.6
; 

[Setup]
AppName=WinPer
AppVersion=10.1.6
DefaultDirName={pf}\WinPer
DefaultGroupName=WinPer
Compression=lzma2
SolidCompression=yes
OutputBaseFilename=WP101620160921121028

[Files]
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.6\rehonoX.exe"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.6\reanti16.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.6\reanti18.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.6\reanti19.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.6\reanti20.qrp"; DestDir: "{app}"

[Icons]
Name: "{group}\WinPer"; Filename: "{app}\reconect.exe"
