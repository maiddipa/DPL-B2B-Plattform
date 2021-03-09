$dbsystem = "DEV-SQLSVR"			# Database System
$dbname = "LtmsWebApp_DPL"	# Database with Sample Data
$AttachmentPath = ".\\ReportSampleData.csv"
$sqlFile='.\\ReportSampleData.sql'

Invoke-Sqlcmd -ServerInstance $dbsystem -Database $dbname -InputFile $sqlFile | Export-CSV $AttachmentPath -Delimiter ';' -NoTypeInformation -Encoding UTF8

