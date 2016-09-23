; 
; Instalador winper version 10.1.5
; 

[Setup]
AppName=WinPer
AppVersion=10.1.5
DefaultDirName={pf}\WinPer
DefaultGroupName=WinPer
Compression=lzma2
SolidCompression=yes
OutputBaseFilename=WP101520160914105128

[Files]
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\libro_csvtt_corte.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\libro4filas10columnas.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\libro4filas10columnas_corte.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\libro4filas10columnas_matriz.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\libro4filas10columnas_matriz_corte.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\libro5filas4columnas.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\libro5filas4columnas_corte.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\libro5filas4columnas_matriz.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\reanti03.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\reanti04.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\reanti05.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\reanti07.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\reasisX.exe"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\rehyd01.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\rehyd02.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\rehyd03.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\rehyd04.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\rehyd05.qrp"; DestDir: "{app}"
Source: "C:\Users\mariol\Documents\Visual Studio 2015\Projects\WinperUpdateServer\WinperUpdateAdmin\Uploads\10.1.5\reliqX.exe"; DestDir: "{app}"

[Icons]
Name: "{group}\WinPer"; Filename: "{app}\reconect.exe"
