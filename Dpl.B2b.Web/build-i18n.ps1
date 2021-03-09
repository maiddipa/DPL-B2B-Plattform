$cwd = $PSScriptRoot #Get-Location
Get-ChildItem "./apps/dpl-live/src/locale" -Filter *.xlf | 
Foreach-Object {
    $fileName = $_.Name;
    $locale = $fileName.Split(".")[1];   

    $RunBuildScriptBlock = {
        Param (
            [string] [Parameter(Mandatory=$true)] $cwd,
            [string] [Parameter(Mandatory=$true)] $locale
        )
        Set-Location $cwd

        ng build --prod --output-path ./dist/apps/dpl-live/$locale --base-href /$locale/ --i18n-file apps/dpl-live/src/locale/messages.$locale.xlf --i18n-format xlf --i18n-locale $locale --i18n-missing-translation error        
    }

    Start-Job  $RunBuildScriptBlock -Name $locale -ArgumentList $cwd, $locale 
}

Get-Job | Wait-Job | Receive-Job
"done"