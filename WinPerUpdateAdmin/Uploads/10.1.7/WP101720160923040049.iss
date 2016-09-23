; 
; Instalador winper version 10.1.7
; 

[Setup]
AppName=WinPer
AppVersion=10.1.7
DefaultDirName={pf}\WinPer
DefaultGroupName=WinPer
Compression=lzma2
SolidCompression=yes
OutputBaseFilename=WP101720160923040049

[Files]
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.7\reanti03.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.7\reanti04.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.7\reanti07.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.7\reanti08.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.7\recapa36.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.7\recapa37.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.7\recapa38.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.7\reprtX.exe"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.7\requeryX.exe"; DestDir: "{app}"

[Icons]
Name: "{group}\WinPer"; Filename: "{app}\reconect.exe"
