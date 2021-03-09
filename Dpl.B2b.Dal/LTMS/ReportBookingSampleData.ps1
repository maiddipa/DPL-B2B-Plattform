$dbsystem = "DEV-SQLSVR"			# Database System
$dbname = "LtmsWebApp_DPL"	# Database with Sample Data
$AttachmentPath = ".\\ReportBookingSampleData.csv"
$sqlFile='.\\ReportBookingSampleData.sql'

Invoke-Sqlcmd -ServerInstance $dbsystem -Database $dbname -InputFile $sqlFile | Export-CSV $AttachmentPath -Delimiter ';' -NoTypeInformation -Encoding UTF8

