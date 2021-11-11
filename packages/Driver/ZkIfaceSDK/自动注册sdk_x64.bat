cd /d %~dp0
copy .\*.dll %windir%\syswow64\
regsvr32 %windir%\syswow64\zkemkeeper.dll