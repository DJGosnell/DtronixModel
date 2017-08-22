if (-Not (Test-Path nuget.exe)){
	"Downloading newest nuget.exe."
	(New-Object Net.WebClient).DownloadFile('https://www.nuget.org/nuget.exe', 'nuget.exe')
	"Finished downloading nuget.exe."
}
.\nuget.exe restore DtronixModel.sln

if ([System.IntPtr]::Size -eq 4) { 
	& "C:\Program Files\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.com" DtronixModel.sln /rebuild Install
} else { 
	& "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.com" DtronixModel.sln /rebuild Install 
}
